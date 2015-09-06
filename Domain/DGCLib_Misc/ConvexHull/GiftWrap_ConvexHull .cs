using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Misc
{
    public class GiftWrap_ConvexHull : IAlgorithm<Polygon>
    {
        [BindInGUI]
        public IEnumerable<Point> Points { get; set; }

        public bool LineClosed { get { return true; } }

        public GiftWrap_ConvexHull()
        {
        }

        public Polygon Compute()
        {
            if (!Points.HasAtLeast(3))
                return new Polygon();

            List<Point> hull = new List<Point>();

            Point start = GeometryUtils.RightMost(Points);

            Point currPoint;
            Point endPoint = start;
            List<Point> points = Points.ToList();
            do
            {
                currPoint = endPoint;
                hull.Add(endPoint);
                if (endPoint != start)
                    points.Remove(endPoint);

                endPoint = points[0];
                double alfa = 2 * Math.PI;
                Vector current = hull.Count == 1 ? new Vector(0, 1) : hull[hull.Count - 1] - hull[hull.Count - 2];
                foreach (var p in points)
                {
                    if (p == currPoint)
                        continue;

                    Vector processed = p - currPoint;
                    double angle = Vector.Angle(processed, current);
                    if (angle < alfa)
                    {
                        endPoint = p;
                        alfa = angle;
                    }
                }
            }
            while (endPoint != start);

            return new Polygon(hull);
        }

        IEnumerable<Polygon> IAlgorithm<Polygon>.Compute()
        {
            yield return Compute();
        }
    }
}