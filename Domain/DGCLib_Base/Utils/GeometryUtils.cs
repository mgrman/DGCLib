using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Base
{
    public delegate void TriReporter(List<List<Point>> tri);

    public static class GeometryUtils
    {
        public static Point RightMost(IEnumerable<Point> points)
        {
            var rightMost = points.First();
            foreach (var p in points)
            {
                if (p.X > rightMost.X)
                    rightMost = p;
            }
            return rightMost;
        }

        public static bool CanSee(Point start, Point end, Point point)
        {
            Vector observer = (start - point);
            Vector edge = (end - start);

            return Vector.Determinant(observer, edge) > 0;
        }

        public static Point? IntersectLine(Point p1, Point q1, Point p2, Point q2, bool lineSegment1 = true, bool lineSegment2 = true)
        {
            /* Return intersection of two line, either infinite or line segments.
            Lines are defined as end points in line segments or as any two points in infite lines.

            */
            // Calculate vectors of both lines from given points:
            Vector v1 = q1 - p1;
            Vector v2 = q2 - p2;

            Point? noIntersect = null;
            // If lines are perpendicular then they dont intersect:
            if (Vector.Determinant(v1, v2) == 0)
                return noIntersect;

            // Using solved equations for intersection of parametric lines
            // we get parameter for first line where they intersect:
            double k = (p2.X * v2.Y + p1.Y * v2.X - p2.Y * v2.X - p1.X * v2.Y) / (v1.X * v2.Y - v1.Y * v2.X);

            // If line 2 have zero Y component then we must calculate parameter for line 2 differently:
            double m;
            if (v2.Y != 0)
                m = (p1.Y + k * v1.Y - p2.Y) / v2.Y;
            else
                m = (p1.X + k * v1.X - p2.X) / v2.X;

            // If lines are both line segments we must check whether these lines intersect in the segments:
            if (lineSegment1 && lineSegment2)
            {
                if (k >= 0 && k <= 1 && m >= 0 && m <= 1)
                    return p1 + k * v1;
                else
                    return noIntersect;
            }

            // If only first line is line segment we must check if lines intersect in this segment:
            if (lineSegment1)
            {
                //k=round(k,5)
                if (k >= 0 && k <= 1)
                    return p1 + k * v1;
                else
                    return noIntersect;
            }

            // If only second line is line segment we must check if lines intersect in this segment:
            if (lineSegment2)
            {
                //m=round(m,5)
                if (m >= 0 && m <= 1)
                    return p2 + m * v2;
                else
                    return noIntersect;
            }

            return p1 + k * v1;
        }

        public static List<Point> IntersectHalfPlaneConvex(Point p1, Point q1, List<Point> points)
        {
            int n = points.Count;
            List<Point> newPoly = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                var q = points[(i + 1) % n];
                var inter = IntersectLine(p, q, p1, q1, true, false);

                if (inter != null && !newPoly.Contains(inter.Value))
                    newPoly.Add(inter.Value);
                if (Vector.Determinant(q1 - p1, p - p1) <= 0)
                    newPoly.Add(p);
            }
            return newPoly.SortAngleCCW().ToList();
        }

        public static double CosineLaw(double adjA, double adjB, double opp)
        {
            if (Math.Abs(opp - (adjA + adjB)) < 0.0001)
                return Math.PI;

            if (Math.Abs(adjA - (opp + adjB)) < 0.0001)
                return 0;
            if (Math.Abs(adjB - (adjA + opp)) < 0.0001)
                return 0;

            var angle = Math.Acos((Math.Pow(adjA, 2) + Math.Pow(adjB, 2) - Math.Pow(opp, 2)) / (2 * adjA * adjB));
            return angle;
        }
    }
}