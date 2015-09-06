using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Runtime.Serialization;

namespace DGCLib_Base.DomainTypes
{
    [DataContract]
    public class Line : IGeoStructure
    {
        [DataMember]
        public Point Start { get; set; }

        [DataMember]
        public Point End { get; set; }

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public Line()
        {
            Start = new Point();
            End = new Point();
        }

        public Line Clone()
        {
            return new Line(Start, End);
        }

        public double Length
        {
            get
            {
                return (Start - End).Magnitude;
            }
        }

        public Point Center
        {
            get
            {
                return Point.Mix(Start, End, 0.5);
            }
        }
    }
}