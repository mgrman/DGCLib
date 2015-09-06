using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Misc
{
    public class DualGraph_VoronoiDiagram : IAlgorithm<Polygon>
    {
        [BindInGUI]
        public double InfiniteMultiplier { get; set; }

        public IEnumerable<Vector> ClipPoints { get; set; }

        [BindInGUI]
        public IEnumerable<Point> Points { get; set; }

        private class CellPoint : IEquatable<CellPoint>
        {
            public Point Pos { get; private set; }
            public List<Triangle> Triangles { get; private set; }

            public CellPoint(Point pos)
            {
                Pos = pos;
                Triangles = new List<Triangle>();
            }

            public CellPoint(Point pos, Triangle tri)
            {
                Pos = pos;
                Triangles = new List<Triangle>() { tri };
            }

            bool IEquatable<CellPoint>.Equals(CellPoint v)
            {
                return Pos == v.Pos;
            }
        }

        public DualGraph_VoronoiDiagram()
        {
            InfiniteMultiplier = 3;
        }

        private List<CellPoint> _points;

        //public List<List<Vector>> Compute(List<Triangle> triangles, TriReporter reporter = null)
        //{
        //    var engine = new DualGraph();
        //    return engine.FromTriangles(triangles,reporter);
        //}

        public List<Polygon> Compute()
        {
            var delaunay = new DelaunayTriangulation();
            delaunay.Points = Points;

            var triangles = delaunay.Compute();

            return FromTriangles(triangles);
        }

        public List<Polygon> FromTriangles(List<Triangle> triangles, TriReporter reporter = null)
        {
            _points = new List<CellPoint>();

            foreach (var tri in triangles)
            {
                foreach (var p in tri.ToList())
                {
                    int i = _points.IndexOf(new CellPoint(p));
                    if (i >= 0)
                    {
                        _points[i].Triangles.Add(tri);
                    }
                    else
                    {
                        _points.Add(new CellPoint(p, tri));
                    }
                }
            }

            List<Polygon> cells = new List<Polygon>();
            foreach (var p in _points)
            {
                var tri = p.Triangles;

                List<double> angles = new List<double>();

                Point pointsCenter = p.Pos;

                Point handle = new Point(0, 1);
                tri.Sort((a, b) =>
                {
                    double angleA = Vector.AngleOriented(handle - pointsCenter, a.CircumcircleCenter - pointsCenter);
                    double angleB = Vector.AngleOriented(handle - pointsCenter, b.CircumcircleCenter - pointsCenter);

                    if (angleA < angleB)
                        return -1;
                    if (angleA > angleB)
                        return 1;

                    return 0;
                });

                var cell = tri.Select((o) => { return o.CircumcircleCenter; }).ToList();

                //bool isBorder = false;
                Point startPoint = new Point(0, 0);
                Point endPoint = new Point(0, 0);
                for (int i = 0; i < tri.Count; i++)
                {
                    var t1 = tri[i];
                    var e = t1.GetEdge(p.Pos);
                    var nStart = t1.GetNeighbourByEdge(e);
                    var nEnd = t1.GetNeighbourByEdge(e.Previous);
                    if (nStart == null)
                    {
                        startPoint = StartPoint(t1, p.Pos);
                        cell.Add(startPoint);
                        //isBorder = true;
                    }
                    if (nEnd == null)
                    {
                        endPoint = EndPoint(t1, p.Pos);
                        cell.Add(endPoint);
                        //isBorder = true;
                    }
                }

                //var clipPoints = ClipPoints!=null ? ClipPoints.ToArray(): new Vector[]{
                //    new Vector(-1, -1),
                //    new Vector(-1, 1),
                //    new Vector(1, 1),
                //    new Vector(1, -1)};

                cell = (cell.SortAngleCCW()).ToList();

                ////check which border point should be added to dual graph
                ////must be added after clipping beacuse the cell could become concave and clipping wouldnt work
                //List<Vector> toAdd = new List<Vector>();
                //if (isBorder)
                //{
                //    foreach(var c in clipPoints)
                //    {
                //        if(IsInsideInfiniteConvex(c,cell,new SimpleEdge(endPoint,startPoint)))
                //        {
                //            toAdd.Add(c);
                //        }
                //    }
                //}

                //for (int i = 0; i < clipPoints.Length; i++)
                //{
                //    var c1 = clipPoints[i];
                //    var c2 = clipPoints[(i + 1) % clipPoints.Length];
                //    cell = GeometryUtils.IntersectHalfPlaneConvex(c1, c2, cell);
                //}

                //cell.AddRange(toAdd);
                //cell = GeometryUtils.SortAngleCCW(cell).ToList();

                cells.Add(new Polygon(cell));
                //if (reporter != null) reporter(cells);
            }
            return cells;
        }

        private Point StartPoint(Triangle t, Point p)
        {
            var e = t.GetEdge(p);

            var start = t.CircumcircleCenter;
            var edgeVec = (e.End - e.Start);
            var perp = edgeVec.Perpendicular.GetNormalized();
            var vec = perp * (start.DistanceFromZero * (1 + InfiniteMultiplier) + InfiniteMultiplier);
            return start + vec;
        }

        private Point EndPoint(Triangle t, Point p)
        {
            var e = t.GetEdge(p).Previous;

            var start = t.CircumcircleCenter;
            var edgeVec = (e.End - e.Start);
            var perp = edgeVec.Perpendicular.GetNormalized();
            var vec = perp * (start.DistanceFromZero * (1 + InfiniteMultiplier) + InfiniteMultiplier);
            return start + vec;
        }

        private bool IsInsideInfiniteConvex(Vector point, List<Vector> ngon, SimpleEdge omittedEdge)
        {
            for (int i = 0; i < ngon.Count; i++)
            {
                Vector a = ngon[i];
                Vector b = ngon[(i + 1) % ngon.Count];

                if (a == omittedEdge.Begin && b == omittedEdge.End)
                    continue;

                Vector vecEdge = b - a;
                Vector vecPoint = point - a;

                if (Vector.Determinant(vecPoint, vecEdge) > 0)
                    return false;
            }
            return true;
        }

        private class SimpleEdge
        {
            public Vector Begin { get; set; }
            public Vector End { get; set; }

            public SimpleEdge(Vector begin, Vector end)
            {
                Begin = begin;
                End = end;
            }
        }

        IEnumerable<Polygon> IAlgorithm<Polygon>.Compute()
        {
            return Compute();
        }
    }
}