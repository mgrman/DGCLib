using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Misc
{
    public class DelaunayTriangulation : IAlgorithm<Triangle>
    {
        private List<Triangle> _triangles;
        private List<Point> _points;
        private List<Point> _bbPoints;
        private TriReporter _reporter;

        [BindInGUI]
        public IEnumerable<Point> Points { get; set; }

        public TriReporter Reporter { get { return _reporter; } set { _reporter = value; } }

        public DelaunayTriangulation()
        {
        }

        public List<Triangle> Compute()
        {
            var delTriang = new DelaunayTriangulation();

            return delTriang.ComputeTriangles(Points);
        }

        private List<Triangle> ComputeTriangles(IEnumerable<Point> points, TriReporter reporter = null)
        {
            double precision = 0.00000001;
            _points = new List<Point>();
            foreach (var p in points)
            {
                bool contains = false;
                foreach (var q in _points)
                {
                    if ((q - p).Magnitude < precision)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    _points.Add(p);
                }
            }

            _triangles = new List<Triangle>();
            try
            {
                _reporter = reporter;
                CreateBoundingTriangle();
                Triangulate();
                RemoveBoundingTriangle();
            }
            catch { }

            return _triangles;
        }

        private void CreateBoundingTriangle()
        {
            var bb = _points.GetBB();

            double offset = 0.1;
            var start = bb.Min - new Vector(offset, offset);
            double distX = bb.Max.X - bb.Min.X;
            double distY = bb.Max.Y - bb.Min.Y;
            var x = new Point(bb.Min.X + 2 * distX + offset * 2, bb.Min.Y - offset);
            var y = new Point(bb.Min.X - offset, bb.Min.Y + 2 * distY + offset * 2);
            _bbPoints = new List<Point>() { start, x, y };
            var tri = new Triangle(start, x, y);
            _triangles.Add(tri);
        }

        private void Triangulate()
        {
            foreach (var p in _points)
            {
                //for animation
                Report();
                var hitTri = GetHitTriangle(p);

                if (!hitTri.IsEdgeHit)//no edge hit
                {
                    var tri = hitTri.HitTriangle;
                    _triangles.Remove(tri);

                    var triAB = new Triangle(p, tri.A, tri.B);
                    var triBC = new Triangle(p, tri.B, tri.C);
                    var triCA = new Triangle(p, tri.C, tri.A);

                    triAB.NeiAB = triCA;
                    triAB.NeiBC = tri.NeiAB;
                    triAB.NeiCA = triBC;

                    triBC.NeiAB = triAB;
                    triBC.NeiBC = tri.NeiBC;
                    triBC.NeiCA = triCA;

                    triCA.NeiAB = triBC;
                    triCA.NeiBC = tri.NeiCA;
                    triCA.NeiCA = triAB;

                    if (tri.NeiAB != null)
                        tri.NeiAB.ReplaceNeighbour(tri, triAB);
                    if (tri.NeiBC != null)
                        tri.NeiBC.ReplaceNeighbour(tri, triBC);
                    if (tri.NeiCA != null)
                        tri.NeiCA.ReplaceNeighbour(tri, triCA);
                    _triangles.Add(triAB);
                    _triangles.Add(triBC);
                    _triangles.Add(triCA);

                    Legalize(triAB.GetEdge(TriEdge.BC));
                    Legalize(triBC.GetEdge(TriEdge.BC));
                    Legalize(triCA.GetEdge(TriEdge.BC));
                }
                else //edge hit
                {
                    Edge edge = hitTri.Edge;
                    Triangle cur = edge.Triangle;
                    Point pointOppositeCur = edge.Next.End;

                    Triangle nei = cur.GetNeighbourByEdge(edge);
                    Edge twin = nei.GetEdge(edge.End);
                    Point pointOppositeNei = twin.Next.End;

                    _triangles.Remove(cur);
                    _triangles.Remove(nei);

                    var triCurA = new Triangle(p, edge.End, pointOppositeCur);
                    var triCurB = new Triangle(pointOppositeCur, edge.Start, p);
                    var triNeiA = new Triangle(p, twin.End, pointOppositeNei);
                    var triNeiB = new Triangle(pointOppositeNei, twin.Start, p);

                    triCurA.NeiAB = triNeiB;
                    triCurA.NeiBC = cur.GetNeighbourByEdge(edge.Next);
                    triCurA.NeiCA = triCurB;

                    triCurB.NeiAB = cur.GetNeighbourByEdge(edge.Previous);
                    triCurB.NeiBC = triNeiA;
                    triCurB.NeiCA = triCurA;

                    triNeiA.NeiAB = triCurB;
                    triNeiA.NeiBC = nei.GetNeighbourByEdge(twin.Next);
                    triNeiA.NeiCA = triNeiB;

                    triCurB.NeiAB = nei.GetNeighbourByEdge(twin.Previous);
                    triCurB.NeiBC = triCurA;
                    triCurB.NeiCA = triNeiA;

                    if (cur.GetNeighbourByEdge(edge.Next) != null)
                        cur.GetNeighbourByEdge(edge.Next).ReplaceNeighbour(cur, triCurA);
                    if (cur.GetNeighbourByEdge(edge.Previous) != null)
                        cur.GetNeighbourByEdge(edge.Previous).ReplaceNeighbour(cur, triCurB);

                    if (nei.GetNeighbourByEdge(twin.Next) != null)
                        nei.GetNeighbourByEdge(twin.Next).ReplaceNeighbour(nei, triNeiA);
                    if (nei.GetNeighbourByEdge(twin.Previous) != null)
                        nei.GetNeighbourByEdge(twin.Previous).ReplaceNeighbour(nei, triNeiB);

                    _triangles.Add(triCurA);
                    _triangles.Add(triCurB);
                    _triangles.Add(triNeiA);
                    _triangles.Add(triNeiB);

                    Legalize(triCurA.GetEdge(TriEdge.BC));
                    Legalize(triCurB.GetEdge(TriEdge.AB));
                    Legalize(triNeiA.GetEdge(TriEdge.BC));
                    Legalize(triNeiB.GetEdge(TriEdge.AB));
                }
            }
            //for animation
            Report();
        }

        private void Legalize(Edge edge)
        {
            Triangle cur = edge.Triangle;

            Triangle nei = cur.GetNeighbourByEdge(edge);
            if (nei == null) return;

            if (!IsLegal(cur) && !IsLegal(nei))
            {
                //for animation
                Report(edge);

                Point pointOppositeCur = edge.Next.End;
                Edge twin = nei.GetEdge(edge.End);
                Point pointOppositeNei = twin.Next.End;

                double angleStart = Vector.Angle(edge.End - edge.Start, pointOppositeCur - edge.Start) + Vector.Angle(edge.End - edge.Start, pointOppositeNei - edge.Start);
                double angleEnd = Vector.Angle(edge.Start - edge.End, pointOppositeCur - edge.End) + Vector.Angle(edge.Start - edge.End, pointOppositeNei - edge.End);

                if (angleStart > Math.PI || angleEnd > Math.PI)
                    return;

                var res = FlipEdge(edge);

                var newCur = res.Current;
                var newNei = res.Neighbour;

                Legalize(newCur.GetEdge(TriEdge.AB));
                Legalize(newNei.GetEdge(TriEdge.CA));
            }
        }

        private TrianglePair FlipEdge(Edge edge)
        {
            Triangle cur = edge.Triangle;
            Triangle nei = cur.GetNeighbourByEdge(edge);

            Point pointOppositeCur = edge.Next.End;
            Edge twin = nei.GetEdge(edge.End);
            Point pointOppositeNei = twin.Next.End;

            _triangles.Remove(cur);
            _triangles.Remove(nei);

            var newCur = new Triangle(edge.Start, pointOppositeNei, edge.Next.End);
            var newNei = new Triangle(twin.Start, pointOppositeCur, twin.Next.End);

            newCur.NeiAB = nei.GetNeighbourByEdge(twin.Next);
            newCur.NeiBC = newNei;
            newCur.NeiCA = cur.GetNeighbourByEdge(edge.Previous);

            newNei.NeiAB = cur.GetNeighbourByEdge(edge.Next);
            newNei.NeiBC = newCur;
            newNei.NeiCA = nei.GetNeighbourByEdge(twin.Previous);

            if (newCur.NeiAB != null)
                newCur.NeiAB.SetNeighbourByEdge(newCur.NeiAB.GetEdge(newCur.GetEdge(TriEdge.AB).End), newCur);
            if (newCur.NeiBC != null)
                newCur.NeiBC.SetNeighbourByEdge(newCur.NeiBC.GetEdge(newCur.GetEdge(TriEdge.BC).End), newCur);
            if (newCur.NeiCA != null)
                newCur.NeiCA.SetNeighbourByEdge(newCur.NeiCA.GetEdge(newCur.GetEdge(TriEdge.CA).End), newCur);

            if (newNei.NeiAB != null)
                newNei.NeiAB.SetNeighbourByEdge(newNei.NeiAB.GetEdge(newNei.GetEdge(TriEdge.AB).End), newNei);
            if (newNei.NeiBC != null)
                newNei.NeiBC.SetNeighbourByEdge(newNei.NeiBC.GetEdge(newNei.GetEdge(TriEdge.BC).End), newNei);
            if (newNei.NeiCA != null)
                newNei.NeiCA.SetNeighbourByEdge(newNei.NeiCA.GetEdge(newNei.GetEdge(TriEdge.CA).End), newNei);

            _triangles.Add(newCur);
            _triangles.Add(newNei);
            return new TrianglePair(newCur, newNei);
        }

        private bool IsLegal(Triangle triTest)
        {
            Point? onBb = null;
            List<Edge> checkedEdges = new List<Edge>();
            int bbCount = 0;
            var ownPoints = triTest.ToList();
            foreach (var p in ownPoints)
            {
                if (_bbPoints.Contains(p))
                {
                    var e = triTest.GetEdge(p);
                    if (_bbPoints.Contains(e.Next.Start))
                        checkedEdges.Add(e.Previous);
                    else
                        checkedEdges.Add(e.Next);

                    onBb = p;
                    bbCount++;
                }
            }
            if (bbCount == 2)
            {
                foreach (var e in checkedEdges)
                {
                    foreach (var tri in _triangles)
                    {
                        if (tri != triTest)
                            foreach (var p in tri.ToList())
                            {
                                if (ownPoints.Contains(p))
                                    continue;
                                if (_bbPoints.Contains(p))
                                    continue;
                                if (Vector.Determinant(p - e.Start, e.End - e.Start) < 0)
                                {
                                    return false;
                                }
                            }
                    }
                }
            }

            if (onBb != null)//if one point is on Bounding Triangle then we must simulate that this bounding point is in infinite distance
            {
                int indexOnBb = ownPoints.IndexOf(onBb.Value);
                Point startLine = ownPoints[(indexOnBb + 1) % 3];
                Point endLine = ownPoints[(indexOnBb + 2) % 3];

                foreach (var tri in _triangles)
                {
                    if (tri != triTest)
                        foreach (var p in tri.ToList())
                        {
                            if (ownPoints.Contains(p))
                                continue;
                            if (_bbPoints.Contains(p))
                                continue;
                            if (Vector.Determinant(p - startLine, endLine - startLine) < 0)
                            {
                                return false;
                            }
                        }
                }
                return true;
            }
            else
            {
                Point center = triTest.CircumcircleCenter;
                double radius = (triTest.A - center).Magnitude;
                foreach (var tri in _triangles)
                {
                    if (tri != triTest)
                        foreach (var p in tri.ToList())
                        {
                            if (ownPoints.Contains(p))
                                continue;
                            if (_bbPoints.Contains(p))
                                continue;
                            if ((p - center).Magnitude < radius)
                                return false;
                        }
                }
                return true;
            }
        }

        private TriangleHitTest GetHitTriangle(Point p)
        {
            //TODO
            //can be optimized, now in O(n) time

            foreach (var tri in _triangles)
            {
                var hit = tri.IsInside(p);
                if (hit.IsHit)
                    return hit;
            }
            throw new InvalidOperationException("Point is not inside of any triangle.");
        }

        private void RemoveBoundingTriangle()
        {
            List<int> toDelete = new List<int>();
            for (int i = 0; i < _triangles.Count; i++)
            {
                var tri = _triangles[i];
                if (_bbPoints.Contains(tri.A) ||
                    _bbPoints.Contains(tri.B) ||
                    _bbPoints.Contains(tri.C))
                {
                    if (tri.NeiAB != null)
                        tri.NeiAB.ReplaceNeighbour(tri, null);
                    if (tri.NeiBC != null)
                        tri.NeiBC.ReplaceNeighbour(tri, null);
                    if (tri.NeiCA != null)
                        tri.NeiCA.ReplaceNeighbour(tri, null);
                    toDelete.Add(i);
                }
            }
            toDelete.Reverse();
            foreach (var i in toDelete)
            {
                _triangles.RemoveAt(i);
            }
        }

        private void Report(Edge e = null)
        {
            if (_reporter != null)
            {
                var geo = _triangles.Select((o) => { return o.ToList(); }).ToList();
                if (e != null)
                {
                    geo.Add(new List<Point>() { e.Start, e.End });
                }
                _reporter(geo);
            }
        }

        private class TrianglePair
        {
            public Triangle Current { get; private set; }
            public Triangle Neighbour { get; private set; }

            public TrianglePair(Triangle current, Triangle neighbour)
            {
                Current = current;
                Neighbour = neighbour;
            }
        }


        IEnumerable<Triangle> IAlgorithm<Triangle>.Compute()
        {
            return Compute();
        }
    }
}