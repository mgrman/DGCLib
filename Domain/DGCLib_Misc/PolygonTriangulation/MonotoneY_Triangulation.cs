using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Misc
{
    public class MonotoneY_Triangulation : IAlgorithm<Triangle>
    {
        [BindInGUI]
        public IEnumerable<Point> Points { get; set; }

        public List<Triangle> Compute()
        {
            var points = Points.ToList();

            List<Triangle> triangles = new List<Triangle>();

            var paths = SplitPolyToPaths(points);
            //final.AddRange(Utils.PointsToGeoPolyIncremental(paths.LeftPath, Color.Red, size * 2, true));
            //final.AddRange(Utils.PointsToGeoPolyIncremental(paths.RightPath, Color.Green, size * 2, true));

            points.Sort(new CompareVectorYthenX());

            Stack<Point> stack = new Stack<Point>(points.Count);
            stack.Push(points[0]);
            stack.Push(points[1]);
            for (int i = 2; i < points.Count; i++)
            {
                Point p = points[i];
                var samePath = paths.GetPath(p) & paths.GetPath(stack.Peek());
                if (samePath != 0)
                {
                    var cur = stack.Pop();
                    var next = stack.Pop();
                    while (GoodTriangle(p, cur, next, samePath) && stack.Count != 0)
                    {
                        triangles.Add(new Triangle(p, cur, next));
                        //drawing anim
                        //if (reporter != null) reporter(triangles);
                        // end
                        cur = next;
                        next = stack.Pop();
                    }
                    stack.Push(next);
                    if (GoodTriangle(p, cur, next, samePath))
                    {
                        triangles.Add(new Triangle(p, cur, next));
                        //drawing anim
                        //if (reporter != null) reporter(triangles);
                        // end
                    }
                    else
                    {
                        stack.Push(cur);
                    }
                    stack.Push(p);
                }
                else
                {
                    var vTop = stack.Pop();
                    var cur = vTop;
                    while (stack.Count != 0)
                    {
                        var next = stack.Pop();
                        triangles.Add(new Triangle(p, cur, next));
                        //drawing anim
                        //if (reporter != null) reporter(triangles);
                        // end
                        cur = next;
                    }
                    stack.Push(vTop);
                    stack.Push(p);
                }
            }
            return triangles;
        }

        private static bool GoodTriangle(Point a, Point b, Point c, Path path)
        {
            var det = Vector.Determinant(b - a, c - a);

            return path == Path.Left ? det < 0 : det > 0;
        }

        private static Paths SplitPolyToPaths(List<Point> points)
        {
            int n = points.Count;
            List<Point> leftPath = new List<Point>();
            List<Point> rightPath = new List<Point>();
            var comparer = (IComparer<Point>)new CompareVectorYthenX();

            int max = -1;
            int min = -1;
            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                if (max == -1)
                {
                    max = i;
                    min = i;
                }
                if (comparer.Compare(points[i], points[max]) == 1)
                    max = i;
                if (comparer.Compare(points[i], points[min]) == -1)
                    min = i;
            }

            int cur = max;
            while (cur != min)//(min + 1) % n)
            {
                leftPath.Add(points[cur]);
                cur = (cur + 1) % n;
            }
            leftPath.Add(points[min]);

            cur = min;
            while (cur != max)//(max+1)%n)
            {
                rightPath.Add(points[cur]);
                cur = (cur + 1) % n;
            }
            rightPath.Add(points[max]);
            leftPath.Reverse();
            return new Paths(leftPath, rightPath);
        }

        private enum Path { Left = 1, Right = 2, Both = 3, None = 0 }

        private class Paths
        {
            public List<Point> LeftPath { get; private set; }
            public List<Point> RightPath { get; private set; }

            public Paths(List<Point> leftPath, List<Point> rightPath)
            {
                LeftPath = leftPath;
                RightPath = rightPath;
            }

            public Path GetPath(Point p)
            {
                var res = Path.None;
                if (LeftPath.Contains(p))
                    res = res | Path.Left;
                if (RightPath.Contains(p))
                    res = res | Path.Right;
                return res;
            }
        }

        private class CompareVectorYthenX : IComparer<Point>
        {
            int IComparer<Point>.Compare(Point x, Point y)
            {
                if (x.Y < y.Y)
                    return -1;
                if (x.Y > y.Y)
                    return 1;

                if (x.X < y.X)
                    return -1;
                if (x.X > y.X)
                    return 1;
                return 0;
            }
        }


        IEnumerable<Triangle> IAlgorithm<Triangle>.Compute()
        {
            return Compute();
        }
    }
}