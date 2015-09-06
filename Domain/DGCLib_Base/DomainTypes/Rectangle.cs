using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DGCLib_Base.DomainTypes
{
    [DataContract]
    public class Rectangle : IGeoStructure
    {
        [DataMember]
        public Point Min;

        [DataMember]
        public Point Max;

        public Rectangle(Point min, Point max)
        {
            Min = min;
            Max = max;
        }

        public Rectangle()
        {
            Min = new Point();
            Max = new Point();
        }

        public IEnumerable<Line> EdgeLines
        {
            get
            {
                yield return new Line(new Point(Min.X, Min.Y), new Point(Max.X, Min.Y));
                yield return new Line(new Point(Max.X, Min.Y), new Point(Max.X, Max.Y));
                yield return new Line(new Point(Max.X, Max.Y), new Point(Min.X, Max.Y));
                yield return new Line(new Point(Min.X, Max.Y), new Point(Min.X, Min.Y));
            }
        }

        public Rectangle Clone()
        {
            return new Rectangle(Min, Max);
        }

        public Size Size
        {
            get
            {
                return (Max - Min).ToSize();
            }
        }

        public Point Center
        {
            get
            {
                return Min + Size / 2;
            }
        }
    }
}