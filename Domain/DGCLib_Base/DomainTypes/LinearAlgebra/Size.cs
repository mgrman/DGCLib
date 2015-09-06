using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace DGCLib_Base.DomainTypes.LinearAlgebra
{
    [DataContract]
    public struct Size
    {
        [DataMember]
        public double X;

        [DataMember]
        public double Y;

        public Size(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region Static Methods

        public static Size operator *(Size a, double b)
        {
            return new Size(a.X * b, a.Y * b);
        }

        public static Size operator *(double b, Size a)
        {
            return new Size(a.X * b, a.Y * b);
        }

        public static Size operator /(Size a, double b)
        {
            return new Size(a.X / b, a.Y / b);
        }

        public static bool operator ==(Size a, Size b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Size a, Size b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Size))
                return false;

            return this == (Size)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} x {1}", X, Y); ;
        }

        public static double DiagonalSize(Size a, Size b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        #endregion Static Methods

        public double Surface { get { return X * Y; } }

        public Size GetRounded(double precision)
        {
            return new Size(Math.Round(X / precision) * precision, Math.Round(Y / precision) * precision);
        }

        public static Size? TryParse(string value)
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

            return new Size(x, y);
        }
    }
}