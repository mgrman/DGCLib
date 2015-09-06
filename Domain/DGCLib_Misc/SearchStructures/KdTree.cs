using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace DGCLib_Misc
{
    public class KdTree
    {
        private enum KdDirection { AlongX = 0, AlongY = 1 }

        public class KdNode
        {
            public int Depth { get; set; }
            public Vector Position { get; set; }
            public KdNode Parent { get; set; }
            public KdNode Greater { get; set; }
            public KdNode Lesser { get; set; }

            public KdNode(Vector pos, int depth)
            {
                Position = pos;
                Depth = depth;
            }

            public bool IsLeaf { get { return Lesser == null && Greater == null; } }
        }

        private class KDComparer : IComparer<Vector>
        {
            private KdDirection _dir;
            public KdDirection Direction { get { return _dir; } set { _dir = value; } }
            public int Depth { get { return (int)_dir; } set { _dir = (KdDirection)(value % 2); } }

            public KDComparer(KdDirection dir)
            {
                _dir = dir;
            }

            public KDComparer(int depth)
            {
                _dir = (KdDirection)(depth % 2);
            }

            public int Compare(Vector a, Vector b)
            {
                switch (_dir)
                {
                    case KdDirection.AlongX:
                        if (a.X < b.X)
                            return -1;
                        if (a.X > b.X)
                            return 1;
                        return 0;

                    case KdDirection.AlongY:
                        if (a.Y < b.Y)
                            return -1;
                        if (a.Y > b.Y)
                            return 1;
                        return 0;
                }
                return 0;
            }
        }

        private KdNode _root;

        private bool _useMiddle;

        private KDComparer _compare = new KDComparer(0);

        public KdTree(List<Vector> points, bool useMiddle = false)
        {
            _useMiddle = useMiddle;
            _root = BuildKdTree_recursive(points, 0);
        }

        private KdNode BuildKdTree_recursive(List<Vector> points, int depth)
        {
            if (points.Count == 1)
            {
                return new KdNode(points[0], depth);
            }
            else
            {
                var dir = (KdDirection)(depth % 2);
                points.Sort(new KDComparer(dir));
                int medianIndex = (int)points.Count / 2;
                List<Vector> firstHalf = points.GetRange(0, medianIndex);
                List<Vector> secondHalf = points.GetRange(medianIndex, points.Count - medianIndex);
                var nodeLeft = BuildKdTree_recursive(firstHalf, depth + 1);
                var nodeRight = BuildKdTree_recursive(secondHalf, depth + 1);

                //Uncomment for nicer visualization of KDTree
                KdNode node;
                if (_useMiddle)
                    node = new KdNode((points[medianIndex] + points[medianIndex - 1]) / 2.0, depth);
                else
                    node = new KdNode(points[medianIndex], depth);

                node.Lesser = nodeLeft;
                nodeLeft.Parent = node;

                node.Greater = nodeRight;
                nodeRight.Parent = node;

                return node;
            }
        }

        #region Public methods

        public void AddPoint(Vector point)
        {
            var oldLeaf = GetNode(point);
            var leafPos = oldLeaf.Position;
            oldLeaf.Position = (oldLeaf.Position + point) / 2;
            KdNode lesser;
            KdNode greater;
            if (_compare.Compare(point, leafPos) == -1)
            {
                lesser = new KdNode(point, oldLeaf.Depth + 1);
                greater = new KdNode(leafPos, oldLeaf.Depth + 1);
            }
            else
            {
                lesser = new KdNode(leafPos, oldLeaf.Depth + 1);
                greater = new KdNode(point, oldLeaf.Depth + 1);
            }
            oldLeaf.Lesser = lesser;
            oldLeaf.Greater = greater;
        }

        private KdNode GetNode(Vector pos)
        {
            var node = _root;
            while (!node.IsLeaf)
            {
                _compare.Depth = node.Depth;
                int c = _compare.Compare(pos, node.Position);
                if (c == -1)
                {
                    node = node.Lesser;
                }
                else
                {
                    node = node.Greater;
                }
            }
            return node;
        }

        public List<Vector> GetRange(Vector topLeft, Vector bottomRight)
        {
            throw new NotImplementedException();
        }

        public KdNode Root { get { return _root; } }

        #endregion Public methods
    }
}