using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Misc
{
    public class Incremental_ConvexHull : IAlgorithm<Polygon>
    {
        [BindInGUI]
        public IEnumerable<Point> Points { get; set; }

        public bool LineClosed { get { return true; } }

        public Incremental_ConvexHull()
        {
        }

        public Polygon Compute()
        {
            if (!Points.HasAtLeast(3))
                return new Polygon();

            List<Point> hull = Points.Take(3).SortAngleCCW().ToList();

            int insertIndex = 0;

            foreach (var p in Points.Skip(3))
            {
                if (hull.ContainsConvex(p))
                    continue;

                List<int> toRemove = new List<int>();
                insertIndex = -1;
                for (int i = 0; i < hull.Count; i++)
                {
                    Point previous = hull[i == 0 ? hull.Count - 1 : (i - 1)];
                    Point current = hull[i];
                    Point next = hull[(i + 1) % hull.Count];

                    bool canSeeCurEdge = GeometryUtils.CanSee(current, next, p);
                    bool canSeePrevEdge = GeometryUtils.CanSee(previous, current, p);

                    //find out if any point should be removed
                    if (!canSeePrevEdge && !canSeeCurEdge)
                    {
                        toRemove.Add(i);
                    }
                    //set index of first not seen edge as index where point will be added
                    if (!canSeePrevEdge && insertIndex == -1)
                    {
                        insertIndex = i;
                    }
                }

                toRemove.Reverse();
                foreach (int i in toRemove)
                {
                    hull.RemoveAt(i);
                }
                hull.Insert(insertIndex, p);
            }

            return new Polygon(hull);
        }

        IEnumerable<Polygon> IAlgorithm<Polygon>.Compute()
        {
            yield return Compute();
        }
    }
}