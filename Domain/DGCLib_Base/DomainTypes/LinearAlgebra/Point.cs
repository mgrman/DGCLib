using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DGCLib_Base.DomainTypes.LinearAlgebra
{
    public struct Point : IGeoStructure
    {
        [DataMember]
        public double X;

        [DataMember]
        public double Y;

        public readonly static Point Zero = new Point(0, 0);

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region Static methods

        public static Vector operator -(Point a, Point b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator +(Point a, Vector b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator +(Vector a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Vector b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator -(Vector a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator +(Point a, Size b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator +(Size a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Point))
                return false;

            return this == (Point)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Static methods

        public Point Scale(Size scale)
        {
            return new Point(this.X * scale.X, this.Y * scale.Y);
        }

        public static Point Mix(Point a, Point b, double alfa)
        {
            return (b - a) * alfa + a;
        }

        public Vector ToVector()
        {
            return new Vector(X, Y);
        }

        public Size ToSize()
        {
            return new Size(X, Y);
        }

        public double DistanceFromZero
        {
            get
            {
                return Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
            }
        }

        public void Translate(Vector offset)
        {
            X += offset.X;
            Y += offset.Y;
        }

        public Point GetRotated(double alfa)
        {
            return new Point(Math.Cos(alfa) * X - Math.Sin(alfa) * Y, Math.Sin(alfa) * X + Math.Cos(alfa) * Y);
        }

        public void Rotate(double alfa)
        {
            var x = X;
            var y = Y;

            X = Math.Cos(alfa) * x - Math.Sin(alfa) * y;
            Y = Math.Sin(alfa) * x + Math.Cos(alfa) * y;
        }

        public static Point? TryParse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var items = value.Split(';');

            double x;
            double y;
            if (!double.TryParse(items[0].Trim(), out x))
                return null;
            if (!double.TryParse(items[1].Trim(), out y))
                return null;

            return new Point(x, y);
        }
    }

    public static class PointExtensions
    {
        public static Point GetAverage(this IEnumerable<Point> points)
        {
            var sum = new Point();
            int count = 0;
            foreach (var p in points ?? new Point[0])
            {
                sum.X += p.X;
                sum.Y += p.Y;
                count++;
            }

            sum.X /= count;
            sum.Y /= count;
            return sum;
        }

        public static Rectangle GetBB(this IEnumerable<Point> points)
        {
            if (points == null)
                return new Rectangle();

            Point min = new Point(double.MaxValue, double.MaxValue);
            Point max = new Point(double.MinValue, double.MinValue);

            foreach (var p in points)
            {
                if (p.X < min.X)
                    min.X = p.X;
                if (p.X > max.X)
                    max.X = p.X;

                if (p.Y < min.Y)
                    min.Y = p.Y;
                if (p.Y > max.Y)
                    max.Y = p.Y;
            }

            return new Rectangle(min, max);
        }

        public static bool ContainsConvex(this IList<Point> ngon, Point point)
        {
            for (int i = 0; i < ngon.Count; i++)
            {
                Point a = ngon[i];
                Point b = ngon[(i + 1) % ngon.Count];
                Vector vecEdge = b - a;
                Vector vecPoint = point - a;

                if (Vector.Determinant(vecPoint, vecEdge) > 0)
                    return false;
            }
            return true;
        }

        public static IEnumerable<Point> SortAngleCCW(this IEnumerable<Point> points)
        {
            List<double> angles = new List<double>();

            Point pointsCenter = points.GetAverage();

            var rand = new Random();

            Point handle = new Point(rand.NextDouble() * 2 - 1, rand.NextDouble() * 2 - 1);

            return points.OrderBy((a) =>
            {
                double angleA = Vector.AngleOriented(handle - pointsCenter, a - pointsCenter);
                return angleA;
            });
        }
    }
}