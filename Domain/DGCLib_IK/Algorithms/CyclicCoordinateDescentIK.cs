using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Linq;

namespace DGCLib_IK
{
    public class CyclicCoordinateDescentIK : BaseIK
    {
        private const double toRad = Math.PI / 180.0;

        private double[] _oldLengths = null;
        private Point[] _oldPositions = null;

        [BindInGUI]
        public int IterationCount { get; set; }

        [BindInGUI]
        public bool ApplyConvexityCorrections { get; set; }

        public CyclicCoordinateDescentIK()
            : base()
        {
            Lengths = new double[] { 0.1, 0.11, 0.05, 0.15, 0.08 };

            _oldLengths = Lengths;
            double lengthSumTemp = 0;
            _oldPositions = new Point[] { new Point() }
                .Concat(Lengths
                    .Select(o =>
                    {
                        lengthSumTemp += o;
                        return new Point(lengthSumTemp, 0);
                    }))
                .ToArray();
        }

        protected override Point[] GetPositionsImpl()
        {
            DebugGeo.Clear();

            if (Lengths == null || Lengths.Length < 2)
                return new Point[0];

            var iterCount = IterationCount < 1 ? 1 : IterationCount;

            var start = new Point(0, 0);
            Point[] joints = _oldPositions;

            var axisVector = new Vector(1, 0);

            int compStepCounter = 0;

            for (int i = 0; i < iterCount; i++)
            {
                for (int j = joints.Length - 2; j >= 0; j--)
                {
                    var endJoint = joints[joints.Length - 1];
                    var cur = joints[j];
                    var bone = endJoint - cur;
                    DebugGeo.Add(new BezierSpline(cur, cur, endJoint, endJoint));

                    var lastSegAngle = Vector.AngleOriented(bone, axisVector);
                    var targetAngle = Vector.AngleOriented(Target - cur, axisVector);

                    var angleDif = lastSegAngle - targetAngle;

                    for (int k = joints.Length - 1; k > j; k--)
                    {
                        var boneRotated = joints[k] - cur;
                        joints[k] = cur + boneRotated.GetRotated(angleDif);
                    }
                    compStepCounter++;
                }

                if (ApplyConvexityCorrections)
                {
                    IKUtils.ApplyConvexityCorrections(joints, Lengths, Target);
                }
            }

            _oldPositions = joints;
            return joints;
        }
    }
}