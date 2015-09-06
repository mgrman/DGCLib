using DGCLib_Base;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Linq;

namespace DGCLib_IK
{
    internal static class IKUtils
    {
        public static Point[] GetTriangleIKPositions(Point target, double adjecent, double opposite, bool invertSides)
        {
            Point startPoint = Point.Zero;

            Vector offset = Point.Zero - startPoint;
            target.Translate(offset);

            double hypotenuse = target.DistanceFromZero;
            double alfa = Vector.AngleOriented(target.ToVector(), new Vector(1, 0));

            Point[] points = new Point[0];

            if (hypotenuse > (adjecent + opposite)) //too far away
            {
                points = new Point[] { new Point(0, 0), new Point(adjecent, 0), new Point(adjecent + opposite, 0) };

#if DEBUG
                Reporting.LogMessage("Too far away");
#endif
            }
            if ((adjecent + opposite) > hypotenuse) //bending area
            {
                double angle = Math.Acos((Math.Pow(adjecent, 2) - Math.Pow(opposite, 2) + Math.Pow(hypotenuse, 2)) / (2 * adjecent * hypotenuse));
#if DEBUG
                Reporting.LogMessage(angle.ToString());
#endif
                double y = Math.Abs(adjecent * Math.Sin(angle));
                double x = Math.Sqrt(Math.Pow(adjecent, 2) - Math.Pow(y, 2)) * (angle > Math.PI / 2 ? -1 : 1);

                if (invertSides)
                {
                    y = -y;
                }

                var mPos = new Point(x, y);

                points = new Point[] { new Point(), mPos, new Point(hypotenuse, 0) };
#if DEBUG
                Reporting.LogMessage("Bending area");
#endif
            }

            if (hypotenuse < (adjecent - opposite)) //too close
            {
                points = new Point[] { new Point(), new Point(adjecent, 0), new Point(adjecent - opposite, 0) };
#if DEBUG
                Reporting.LogMessage("Too close (first bigger)");
#endif
            }

            if (hypotenuse < (-adjecent + opposite)) //too close
            {
                points = new Point[] { new Point(), new Point(-adjecent, 0), new Point(-adjecent + opposite, 0) };
#if DEBUG
                Reporting.LogMessage("Too close (second bigger)");
#endif
            }

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = points[i].GetRotated(-alfa);
            }

            for (int i = 0; i < points.Length; i++)
            {
                points[i].Translate(-offset);
            }
            return points;
        }

        internal static Point CalculateNewCenter(Point target, double firstLength, double secondLength)
        {
            var positions = GetTriangleIKPositions(target, firstLength, secondLength, true);

            return positions.Skip(1).FirstOrDefault();
        }

        internal static void ApplyTargetCorrections(Point[] points, double[] lengths, Point target)
        {
            var count = points.Length;

            points[count - 1] = target;
            for (int i = count - 2; i >= 1; i--) //reversed order (including naming)
            {
                var prev = points[i + 1];
                var cur = points[i];
                var next = points[i - 1];

                var prevLength = lengths[i];
                var nextLength = lengths[i - 1];

                var relTarget = next - prev;

                //conversion toPoint and toVector is because of translation of coordinates to local ones
                var newCur = CalculateNewCenter(relTarget.ToPoint(), prevLength, nextLength);
                if (newCur != null)
                    points[i] = prev + newCur.ToVector();
            }
        }

        internal static void ApplyConvexityCorrections(Point[] points, double[] lengths, Point target)
        {
            var count = points.Length;

            for (int j = 1; j < count - 1; j++)
            {
                bool correctedSometing = false;
                for (int i = 1; i < count - 1; i++)
                {
                    var prev = points[i - 1];
                    var cur = points[i];
                    var next = points[i + 1];

                    var dirBefore = cur - prev;
                    var dirAfter = next - cur;

                    if (Vector.Determinant(dirBefore, dirAfter) > 0)
                    {
                        var dirWhole = next - prev;

                        var proj = Vector.Project(dirBefore, dirWhole);

                        var dif = dirBefore - proj;

                        var newDirBefore = dirBefore - 2 * dif;
                        var newCur = prev + newDirBefore;

                        points[i] = newCur;
                        correctedSometing = true;
                    }
                }
                if (!correctedSometing)
                    break;
            }
        }
    }
}