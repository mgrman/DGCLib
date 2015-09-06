using DGCLib_Base;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_IK
{
    /// <summary>
    /// WORKING
    /// </summary>
    public class NgonIK_Incremental : BaseIK
    {
        public enum MixTypes
        {
            Forward,
            Backward
        }

        [BindInGUI]
        public MixTypes MixType { get; set; }

        public NgonIK_Incremental()
            : base()
        {
            Lengths = new double[] { 0.1, 0.11, 0.05, 0.15, 0.08 };
        }

        protected override Point[] GetPositionsImpl()
        {
            switch (MixType)
            {
                case MixTypes.Forward:
                    return ForwardIncrements(Lengths, Target);

                case MixTypes.Backward:
                    return BackwardIncrements(Lengths, Target);
            }
            return new Point[0];
        }

        private Point[] ForwardIncrements(double[] lengths, Point target)
        {
            //TODO
            //dodat lubovolny pociatok
            //tak nejak imnplicitne sa berie (0,0)

            List<Point> calculatedPoints = new List<Point>();
            double reachedDistance = lengths[0];
            double angle = 0;

            calculatedPoints.Add(new Point());

            Point[] trianglePoints = null;
            for (int i = 0; i < lengths.Length - 1; i++)
            {
                trianglePoints = CalculateTriangleIK(target, reachedDistance, lengths[i + 1]);
                reachedDistance = trianglePoints.Last().DistanceFromZero;

                angle = Vector.Angle(target.ToVector(), trianglePoints[1].ToVector());
                angle = double.IsNaN(angle) ? 0 : angle;
                RotatePoints(calculatedPoints, angle);
                calculatedPoints.Add(trianglePoints[1]);
            }

            calculatedPoints.Add(trianglePoints.Last());

            return calculatedPoints.ToArray();
        }

        private Point[] BackwardIncrements(double[] lengths, Point target)
        {
            target = target.Scale(new Size(-1, -1));

            lengths = lengths.Reverse().ToArray();

            List<Point> calculatedPoints = new List<Point>();
            double reachedDistance = lengths[0];
            double angle = 0;

            calculatedPoints.Add(new Point());

            Point[] trianglePoints = null;
            for (int i = 0; i < lengths.Length - 1; i++)
            {
                trianglePoints = CalculateTriangleIK(target, reachedDistance, lengths[i + 1]);
                reachedDistance = trianglePoints.Last().DistanceFromZero;

                angle = Vector.Angle(target.ToVector(), trianglePoints[1].ToVector());
                angle = double.IsNaN(angle) ? 0 : angle;

                RotatePoints(calculatedPoints, angle);
                calculatedPoints.Add(trianglePoints[1]);
            }

            var lastPoint = trianglePoints.Last();
            calculatedPoints.Add(lastPoint);

            var yAxisAngle = -Vector.AngleOriented(new Vector(0, 1), lastPoint.ToVector());

            return calculatedPoints
                .Select(o => { return o - lastPoint; })
                .Reverse()
                .Select(o => { return o.GetRotated(yAxisAngle); })
                .Select(o => { return new Point(-o.X, o.Y); })
                .Select(o => { return o.GetRotated(-yAxisAngle); })
                .ToArray();
        }

        private Point[] CalculateTriangleIK(Point target, double firstLength, double secondLength)
        {
            return IKUtils.GetTriangleIKPositions(target, firstLength, secondLength, false);
        }

        private void RotatePoints(List<Point> points, double angle)
        {
            points.ForEach(o =>
            {
                o.Rotate(angle);
            });
        }
    }
}