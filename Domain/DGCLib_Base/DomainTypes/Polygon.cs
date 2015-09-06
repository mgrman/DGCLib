using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Collections;
using System.Collections.Generic;

namespace DGCLib_Base.DomainTypes
{
    public class Polygon : IEnumerable<Point>, IGeoStructure
    {
        public List<Point> Points { get; private set; }

        public Polygon()
        {
            Points = new List<Point>();
        }

        public Polygon(IEnumerable<Point> points)
            : this()
        {
            Points.AddRange(points);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return Points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Points.GetEnumerator();
        }

        public IEnumerable<Line> EdgeLines
        {
            get
            {
                for (int i = 0; i < Points.Count; i++)
                    yield return new Line(Points[i], Points[(i + 1) % Points.Count]);
            }
        }

        public void Add(Point p)
        {
            Points.Add(p);
        }

        public object Clone()
        {
            return new Polygon(Points);
        }
    }
}