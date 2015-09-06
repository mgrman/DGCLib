using System;
using System.Linq;

namespace DGCLib_Base
{
    public static class MathUtils
    {
        public static bool SolveByBisection(Func<double, double> func, double minParam, double maxParam, out double res, double eps)
        {
            double param = minParam;

            double minValue = func(minParam);
            double maxValue = func(maxParam);

            if (minValue == 0)
            {
                res = minParam;
                return true;
            }
            if (maxValue == 0)
            {
                res = minParam;
                return true;
            }

            if (minValue * maxValue > 0)
            {
                //throw new InvalidOperationException("Cannot be solved by bisection using this minParam and maxParam");
                res = double.NaN;
                return false;
            }

            double lastVal = double.MaxValue;
            while (Math.Abs(lastVal) > eps)
            {
                param = (minParam + maxParam) / 2;
                var val = func(param);
                lastVal = val;

                if (val * minValue < 0)
                {
                    maxParam = param;
                    maxValue = val;
                    continue;
                }

                if (val * maxValue < 0)
                {
                    minParam = param;
                    minValue = val;
                    continue;
                }

                break;
            }
            res = param;
            return true;
        }

        public static bool SolveByRegularSlicing(Func<double, double> func, double minParam, double maxParam, out double res, double eps)
        {
            int slices = 10;

            double lastMinY = double.MinValue;
            double minY = double.MaxValue;
            double xForMinY = double.NaN;
            while (Math.Abs(minY) > eps && Math.Abs(lastMinY - minY) > eps)
            {
                lastMinY = minY;

                double sliceStep = (maxParam - minParam) / (slices - 1);

                var values = Enumerable.Range(0, slices)
                    .Select(o => o * sliceStep + minParam)
                    .Select(o => new { x = o, y = func(o) })
                    .ToArray();

                minY = values.Min(o => Math.Abs(o.y));
                xForMinY = values.Where(o => Math.Abs(o.y) == minY).First().x;

                minParam = xForMinY - sliceStep;
                maxParam = xForMinY + sliceStep;
            }

            res = xForMinY;
            return true;
        }
    }
}