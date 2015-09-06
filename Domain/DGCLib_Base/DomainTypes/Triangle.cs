using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DGCLib_Base.DomainTypes
{
    public class TriangleHitTest
    {
        public Triangle HitTriangle { get; set; }
        public bool IsHit { get { return HitTriangle != null; } }
        public Edge Edge { get; set; }
        public bool IsEdgeHit { get { return Edge != null; } }

        public TriangleHitTest(Triangle tri, Edge e)
        {
            HitTriangle = tri;
            Edge = e;
        }
    }

    public class Edge
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public Edge Next { get; set; }
        public Edge Previous { get; set; }

        public Triangle Triangle { get; set; }
        public TriEdge Tag { get; set; }

        public Edge(Point start, Point end, Triangle parent, TriEdge tag)
        {
            Start = start;
            End = end;
            Triangle = parent;
            Tag = tag;
        }
    }

    public enum TriEdge { AB = 0, BC = 1, CA = 2 }

    public class Triangle : IEnumerable<Point>, IGeoStructure
    {
        public Point A { get; private set; }
        public Point B { get; private set; }
        public Point C { get; private set; }

        public Triangle NeiAB { get; set; }
        public Triangle NeiBC { get; set; }
        public Triangle NeiCA { get; set; }

        public Edge[] Edges { get; private set; }

        public Triangle(Point a, Point b, Point c)
        {
            A = a;
            B = b;
            C = c;
            NeiAB = null;
            NeiBC = null;
            NeiCA = null;

            var eAB = new Edge(A, B, this, TriEdge.AB);
            var eBC = new Edge(B, C, this, TriEdge.BC);
            var eCA = new Edge(C, A, this, TriEdge.CA);

            eAB.Next = eBC;
            eAB.Previous = eCA;

            eBC.Next = eCA;
            eBC.Previous = eAB;

            eCA.Next = eAB;
            eCA.Previous = eBC;
            Edges = new Edge[] { eAB, eBC, eCA };
        }

        public IEnumerable<Line> EdgeLines
        {
            get
            {
                yield return new Line(A, B);
                yield return new Line(B, C);
                yield return new Line(C, A);
            }
        }

        public TriangleHitTest IsInside(Point p)
        {
            double detAB = Vector.Determinant(p - A, B - A);
            double detBC = Vector.Determinant(p - B, C - B);
            double detCA = Vector.Determinant(p - C, A - C);
            if (detAB < 0 && detBC < 0 && detCA < 0)
                return new TriangleHitTest(this, null);
            if (detAB == 0)
                return new TriangleHitTest(this, GetEdge(TriEdge.AB));
            if (detBC == 0)
                return new TriangleHitTest(this, GetEdge(TriEdge.BC));
            if (detCA == 0)
                return new TriangleHitTest(this, GetEdge(TriEdge.CA));

            return new TriangleHitTest(null, null);
        }

        public List<Point> ToList()
        {
            return new List<Point>() { A, B, C };
        }

        public void ReplaceNeighbour(Triangle o, Triangle n)
        {
            if (NeiAB == o)
            {
                NeiAB = n;
                return;
            }
            if (NeiBC == o)
            {
                NeiBC = n;
                return;
            }
            if (NeiCA == o)
            {
                NeiCA = n;
                return;
            }

#if DEBUG
            throw new InvalidOperationException(" old neighbour isnt among neighbours");
#endif
        }

        public Edge GetEdge(TriEdge e)
        {
            return Edges[(int)e];
        }

        public Triangle GetNeighbourByEdge(Edge e)
        {
            switch (e.Tag)
            {
                case TriEdge.AB:
                    return NeiAB;

                case TriEdge.BC:
                    return NeiBC;

                case TriEdge.CA:
                    return NeiCA;
            }
            throw new InvalidOperationException("This triangle doesnt have edge <e>");
        }

        public Edge GetEdge(Point start)
        {
            foreach (var e in Edges)
            {
                if (e.Start == start)
                    return e;
            }
            throw new InvalidOperationException("No edge with given point");
        }

        public void SetNeighbourByEdge(Edge e, Triangle tri)
        {
            switch (e.Tag)
            {
                case TriEdge.AB:
                    NeiAB = tri;
                    return;

                case TriEdge.BC:
                    NeiBC = tri;
                    return;

                case TriEdge.CA:
                    NeiCA = tri;
                    return;
            }
#if DEBUG
            throw new InvalidOperationException("This triangle doesnt have edge <e>");
#endif
        }

        public IEnumerator<Point> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Point CircumcircleCenter
        {
            get
            {
                var tri = this;
                // Calculate two normalized vectors from triangles two sides near point b:
                Vector v = (tri.A - tri.B).GetNormalized();
                Vector w = (tri.C - tri.B).GetNormalized();

                // If triangle is deformed return default values and print error (exception is not thrown, calculation can continue):

                if (v == w || v == -w)
                    return new Point(double.NaN, double.NaN);

                // Calculates coordinates of center of circumcircle using solved equations defining point that is equidistant from all three points of the triangle:
                double pX = (tri.A.X * tri.A.X * (-tri.B.Y) + tri.A.X * tri.A.X * tri.C.Y + tri.B.X * tri.B.X * tri.A.Y - tri.B.X * tri.B.X * tri.C.Y - tri.C.X * tri.C.X * tri.A.Y + tri.C.X * tri.C.X * tri.B.Y - tri.A.Y * tri.A.Y * tri.B.Y + tri.A.Y * tri.A.Y * tri.C.Y + tri.A.Y * tri.B.Y * tri.B.Y - tri.A.Y * tri.C.Y * tri.C.Y - tri.B.Y * tri.B.Y * tri.C.Y + tri.B.Y * tri.C.Y * tri.C.Y) / (2 * (-tri.A.X * tri.B.Y + tri.A.X * tri.C.Y + tri.B.X * tri.A.Y - tri.B.X * tri.C.Y - tri.C.X * tri.A.Y + tri.C.X * tri.B.Y));
                double pY = -(tri.A.X * tri.A.X * (-tri.B.X) + tri.A.X * tri.A.X * tri.C.X + tri.A.X * tri.B.X * tri.B.X - tri.A.X * tri.C.X * tri.C.X + tri.A.X * tri.B.Y * tri.B.Y - tri.A.X * tri.C.Y * tri.C.Y - tri.B.X * tri.B.X * tri.C.X + tri.B.X * tri.C.X * tri.C.X - tri.B.X * tri.A.Y * tri.A.Y + tri.B.X * tri.C.Y * tri.C.Y + tri.C.X * tri.A.Y * tri.A.Y - tri.C.X * tri.B.Y * tri.B.Y) / (2 * (-tri.A.X * tri.B.Y + tri.A.X * tri.C.Y + tri.B.X * tri.A.Y - tri.B.X * tri.C.Y - tri.C.X * tri.A.Y + tri.C.X * tri.B.Y));
                Point center = new Point(pX, pY);

                return center;
            }
        }
    }
}