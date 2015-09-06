using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Linq;

namespace DGCLib_IK
{
    public class SplineConvexIK : BaseIK
    {
        [BindInGUI]
        public bool TargetCorrections { get; set; }

        [BindInGUI]
        public bool ConvexityCorrections { get; set; }

        [BindInGUI]
        public double Angle { get; set; }

        public SplineConvexIK()
            : base()
        {
            Lengths = new double[] { 0.1, 0.11, 0.05, 0.15, 0.08 };

            TargetCorrections = true;

            ConvexityCorrections = true;

            Angle = double.NaN;
        }

        protected override Point[] GetPositionsImpl()
        {
#if DEBUG
            DebugGeo.Clear();
#endif

            var count = Lengths.Length + 1;

            var start = new Point(0, 0);

            if ((Target - start).Magnitude > Lengths.Sum())
            {
                var dir = (Target - start).GetNormalized();
                Point lastPos = start;
                return new Point[] { start }.Concat(
                    Enumerable
                        .Range(0, Lengths.Length)
                        .Select(i =>
                        {
                            var newPos = lastPos + dir * Lengths[i];
                            lastPos = newPos;
                            return newPos;
                        })
                    )
                    .ToArray();
            }

            Vector tan;
            if (double.IsNaN(Angle))
                tan = -(Target - start).Perpendicular;
            else
                tan = new Vector(1, 0).GetRotated(Angle / 180.0 * Math.PI);

            var res = GetPositions(start, Target, Lengths, tan, tan);

            if (res == null)
                return null;

            if (TargetCorrections)
            {
                IKUtils.ApplyTargetCorrections(res, Lengths, Target);
            }
            if (ConvexityCorrections)
            {
                IKUtils.ApplyConvexityCorrections(res, Lengths, Target);
            }

            return res;
        }

        private Point[] GetPositions(Point start, Point end, double[] lengths, Vector startTangent, Vector endTangent)
        {
            var lengthSum = lengths.Sum();
            var dist = (end - start).Magnitude;

            BezierSpline spline = null;

            double relEps = 0.001;

            var startDir = startTangent.GetNormalized();
            var endDir = endTangent.GetNormalized();

            Point[] resPoints = null;

            int lengthSampleCount = (int)(lengthSum / lengths.Min());

#if DEBUG

            #region Time

            double startTime = Reporting.Seconds;
            double prevTime = startTime;
            double curTime;
            Action<string, bool> printTime = (message, fromStart) =>
             {
                 curTime = Reporting.Seconds;
                 var lastTime = fromStart ? startTime : prevTime;
                 Reporting.LogMessage("{0}  = {1} [ms] at {2} [fps]", message, Math.Round((curTime - lastTime) * 1000, 2), Math.Round(1.0 / (curTime - lastTime), 2));
                 if (!fromStart)
                     prevTime = curTime;
             };

            #endregion Time

#endif

            Func<double, double> func = (p) =>
            {
                spline = new BezierSpline(start, start + startDir * p, end + endDir * p, end);
#if DEBUG
                DebugGeo.Add(spline);
#endif
                return spline.GetLength(lengthSampleCount * 2) - lengthSum;
            };

            double param = 0;
            double eps = lengthSum * relEps; ;

            double minParam = (lengthSum - dist) / 2;
            double maxParam = Math.Sqrt(lengthSum * lengthSum - dist * dist);

            var success = MathUtils.SolveByBisection(func, minParam, maxParam, out param, eps);

            if (!success)
            {
                success = MathUtils.SolveByBisection(func, minParam / 2, maxParam * 2, out param, eps);

                if (!success)
                    return null;
            }

#if DEBUG
            printTime("SplinePreparation", false);
#endif

            spline = new BezierSpline(start, start + startDir * param, end + endDir * param, end);
            resPoints = spline.GetResampledByDistance(lengths, true);

#if DEBUG
            printTime("Resample", false);
#endif

#if DEBUG
            printTime("Finished", true);
#endif

            return resPoints;
        }
    }
}