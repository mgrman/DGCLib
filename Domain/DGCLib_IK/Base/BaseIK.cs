using DGCLib_Base;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using DGCLib_Misc;
using System.Collections.Generic;
using System.Linq;
using System;
using DGCLib_Base.DomainTypes;

namespace DGCLib_IK
{
    public abstract class BaseIK : IAlgorithm<Line>
    {
        [BindInGUI]
        public double[] Lengths { get; set; }

        [BindInGUI]
        public Point Target { get; set; }

        public BaseIK()
        {
            Lengths = new double[0];
            Target = new Point();
        }

        public virtual Point[] Compute()
        {
            if (Lengths.Any(o => double.IsNaN(o) || double.IsInfinity(o) || o == 0.0))
                return new Point[0];

            if (Lengths.Length == 2)
            {
                return IKUtils.GetTriangleIKPositions(Target, Lengths[0], Lengths[1], false);
            }

            return GetPositionsImpl();
        }

        protected abstract Point[] GetPositionsImpl();

        IEnumerable<Line> IAlgorithm<Line>.Compute()
        {
            var points = Compute();
            return points.Zip(points.Skip(1),(a,b)=>new Line(a, b));
        }

        private List<IGeoStructure> _debugGeo = new List<IGeoStructure>();

        protected List<IGeoStructure> DebugGeo { get { return _debugGeo; } }

        public IGeoStructure[] AdditionalGeo { get { return _debugGeo.ToArray(); } }
    }
}