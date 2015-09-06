using System;
using System.Runtime.Serialization;

namespace DGCLib_Base.DomainTypes.LinearAlgebra
{
    [DataContract]
    public struct Vector
    {
        [DataMember]
        public double X;

        [DataMember]
        public double Y;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region Static Methods

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.X, -a.Y);
        }

        //public static Vector operator -(Vector a, double b)
        //{
        //    return new Vector(a.X - b, a.Y - b);
        //}
        //public static Vector operator -(double a, Vector b)
        //{
        //    return new Vector(a - b.X, a - b.Y);
        //}
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(double b, Vector a)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(Vector a, Vector b)
        {
            return new Vector(a.X * b.X, a.Y * b.Y);
        }

        public static Vector operator /(Vector a, double b)
        {
            return new Vector(a.X / b, a.Y / b);
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector))
                return false;

            return this == (Vector)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static double DotProduct(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static double Determinant(Vector a, Vector b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static double Angle(Vector a, Vector b)
        {
            double angle = Math.Acos(Vector.DotProduct(a, b) / (a.Magnitude * b.Magnitude));
            return angle;
        }

        public static double AngleOriented(Vector a, Vector b)
        {
            var angle = Vector.Angle(a, b);
            if (double.IsNaN(angle))
                return Math.PI;
            double deter = Vector.Determinant(a, b);

            if (deter < 0)
                return Math.PI * 2 - angle;
            if (deter > 0)
                return angle;

            if (deter == 0)
            {
                var ratio = VectorSizeRatio(a, b);
                if (ratio < 0)
                    return Math.PI;

                if (ratio > 0)

                    return 0;
            }
            return 0;
        }

        private static double VectorSizeRatio(Vector a, Vector b)
        {
            if (Determinant(a, b) != 0)
                throw new ArgumentException("Determinant of vectors must be zero.");

            double ratioX = double.NaN;
            double ratioY = double.NaN;
            if (b.X != 0)
            {
                ratioX = a.X / b.X;
            }

            if (b.Y != 0)
            {
                ratioY = a.Y / b.Y;
            }

            if (double.IsNaN(ratioX) && double.IsNaN(ratioY))
                return 0;

            if (double.IsNaN(ratioX))
                return ratioY;

            if (double.IsNaN(ratioY))
                return ratioX;

            return 0;
        }

        public static Vector Project(Vector a, Vector b)
        {
            return b * Vector.DotProduct(a, b) / Vector.DotProduct(b, b);
        }

        public static double ComputeDistance(Vector a, Vector b)
        {
            return (a - b).Magnitude;
        }

        #endregion Static Methods

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public Size ToSize()
        {
            return new Size(X, Y);
        }

        public double Magnitude { get { return Math.Sqrt(Vector.DotProduct(this, this)); } }

        public double Magnitude2 { get { return Vector.DotProduct(this, this); } }

        public Vector Perpendicular { get { return new Vector(Y, -X); } }

        public Vector Clone()
        {
            return new Vector(X, Y);
        }

        public void Normalize()
        {
            var mag = Magnitude;
            X /= mag;
            Y /= mag;
        }

        public Vector GetNormalized()
        {
            var mag = Magnitude;
            return new Vector(X / mag, Y / mag);
        }

        public Vector GetRounded(double precision)
        {
            return new Vector(Math.Round(X / precision) * precision, Math.Round(Y / precision) * precision);
        }

        public Vector GetRotated(double alfa)
        {
            return new Vector(Math.Cos(alfa) * X - Math.Sin(alfa) * Y, Math.Sin(alfa) * X + Math.Cos(alfa) * Y);
        }

        public void Rotate(double alfa)
        {
            var x = X;
            var y = Y;

            X = Math.Cos(alfa) * x - Math.Sin(alfa) * y;
            Y = Math.Sin(alfa) * x + Math.Cos(alfa) * y;
        }

        public static Vector? TryParse(string value)
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

            return new Vector(x, y);
        }
    }
}