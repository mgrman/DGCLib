using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DGCLib_Base.DomainTypes
{
    [DataContract]
    public class BezierSpline : IGeoStructure
    {
        [DataMember]
        public Point PosA { get; set; }

        [DataMember]
        public Point PosB { get; set; }

        [DataMember]
        public Point PosC { get; set; }

        [DataMember]
        public Point PosD { get; set; }

        public BezierSpline(Point a, Point b, Point c, Point d)
        {
            PosA = a;
            PosB = b;
            PosC = c;
            PosD = d;
        }

        public BezierSpline(Point[] points)
        {
            if (points == null || points.Length < 4)
                throw new ArgumentException("Points is null, or length is too small");

            PosA = points[0];
            PosB = points[1];
            PosC = points[2];
            PosD = points[3];
        }

        public Point ParametricCenter
        {
            get
            {
                return GetPosition(0.5);
            }
        }

        public BezierSpline Clone()
        {
            return new BezierSpline(PosA, PosB, PosC, PosD);
        }

        public IEnumerable<Point> Points
        {
            get
            {
                yield return PosA;
                yield return PosB;
                yield return PosC;
                yield return PosD;
            }
        }

        public Point[] GetResampled(Size scaleFactor, double maxStepSize)
        {
            return new BezierSpline(this.Points.Select(o => o.Scale(scaleFactor)).ToArray()).Resample(maxStepSize);
        }

        public Point[] GetResampledByDistance(double[] distances, bool extrapolate = false)
        {
            return ResampleByDistanceSpline(PosA, distances, extrapolate);
        }

        public double GetLength(int sampleCount)
        {
            double dist = 0;
            Point lastPos = PosA;
            int count = sampleCount;
            double step = 1.0 / (count - 1);

            for (int i = 0; i < count; i++)
            {
                var newPos = GetPosition(i * step);
                dist += (newPos - lastPos).Magnitude;
                lastPos = newPos;
            }

            return dist;
        }

        private struct Range
        {
            private double _min;

            public double Min
            {
                get { return _min; }
                set
                {
                    if (value > _max)
                    {
                        _min = _max;
                        _max = value;
                    }
                    else
                    {
                        _min = value;
                    }
                }
            }

            private double _max;

            public double Max
            {
                get { return _max; }
                set
                {
                    if (value < _min)
                    {
                        _max = _min;
                        _min = value;
                    }
                    else
                    {
                        _max = value;
                    }
                }
            }

            public double Size
            {
                get
                {
                    return Math.Abs(Max - Min);
                }
            }

            public Range(double min, double max)
                : this()
            {
                Min = min;
                Max = max;
            }
        }

        private Point[] ResampleByDistanceEuclidian(double[] distances, double delta = 0.0001)
        {
            var range = new Range(0, 1);

            int stepCount = 1000;

            List<Point> foundPoints = new List<Point>();

            var pointsOnSpline = this.Resample(1.0 / (stepCount - 1)).Select((o, i) => new { point = o, index = i }).ToArray();

            var lastPoint = PosA;
            foundPoints.Add(lastPoint);

            int lastUsedIndex = 0;
            foreach (var dist in distances)
            {
                var dist2 = dist * dist;
                if (lastUsedIndex + 1 >= pointsOnSpline.Length)
                {
                    //lastPoint = lastPoint + lastDir * dist;
                    //foundPoints.Add(lastPoint);
                    continue;
                }
                var temp = pointsOnSpline.Skip(lastUsedIndex + 1).Select(o => (o.point - lastPoint).Magnitude2 - dist2).ToList();
                var tempIndex = temp.TakeWhile(o => o < 0).Count();

                if (tempIndex + lastUsedIndex + 1 == pointsOnSpline.Length)
                    break;

                var pointPair = pointsOnSpline[tempIndex + lastUsedIndex + 1];

                var pointOnSpline = pointPair.point;
                var index = pointPair.index;

                Vector lastDir = (pointOnSpline - lastPoint).GetNormalized();
                lastPoint = lastPoint + lastDir * dist;
                foundPoints.Add(lastPoint);
                lastUsedIndex = index;
            }

            return foundPoints.ToArray();
        }

        private Point[] ResampleByDistanceSpline(Point startPoint, double[] distances, bool extrapolate, double delta = 0.0001)
        {
            var temp = this.Resample(100)
                .Select((o, i) => new { point = o, index = i });

            double distAcc = 0;
            var predecesor = new { point = startPoint, index = 0 };

            var pointsOnSpline = temp
                .Select((point) =>
               {
                   distAcc += (point.point - predecesor.point).Magnitude;
                   predecesor = point;
                   return new { point.point, index = point.index, distance = distAcc };
               })
                .ToArray();

            List<Point> gatheredPoints = new List<Point>();
            gatheredPoints.Add(startPoint);

            double requiredDist = 0;
            Func<double, Point> distVectorFunc = (param) =>
            {
                double index = param * (pointsOnSpline.Length - 1);

                int indexStart = (int)index;
                int indexEnd = Math.Min((indexStart + 1), pointsOnSpline.Length - 1);
                double alfa = index - indexStart;
                var point = Point.Mix(pointsOnSpline[indexStart].point, pointsOnSpline[indexEnd].point, alfa);
                return point;
            };

            Func<double, double> distFunc = (param) =>
            {
                double index = param * (pointsOnSpline.Length - 1);

                int indexStart = (int)index;
                int indexEnd = Math.Min((indexStart + 1), pointsOnSpline.Length - 1);
                double alfa = index - indexStart;
                var point = Point.Mix(pointsOnSpline[indexStart].point, pointsOnSpline[indexEnd].point, alfa);

                var curDist = pointsOnSpline[indexStart].distance + (point - pointsOnSpline[indexStart].point).Magnitude;

                return curDist - requiredDist;
            };

            double lastParam = 0;

            Point lastPoint = startPoint;
            for (int i = 0; i < distances.Length; i++)
            {
                var dist = distances[i];
                requiredDist += dist;

                double param;
                bool success = MathUtils.SolveByBisection(distFunc, lastParam, 1, out param, delta);

                if (!success)
                {
                    if (!extrapolate)
                        return gatheredPoints.ToArray();

                    var extraDir = (pointsOnSpline.Last().point - lastPoint).GetNormalized();
                    for (int j = i; j < distances.Length; j++)
                    {
                        dist = distances[j];
                        var newPointExtra = lastPoint + extraDir * dist;
                        gatheredPoints.Add(newPointExtra);

                        lastPoint = newPointExtra;
                    }
                    return gatheredPoints.ToArray();
                }

                var dir = (distVectorFunc(param) - lastPoint);
                dir.Normalize();

                var newPoint = lastPoint + dir * dist;
                gatheredPoints.Add(newPoint);

                lastPoint = newPoint;
                lastParam = param;
            }
            return gatheredPoints.ToArray();
        }

        private Point[] Resample(double stepSize)
        {
            double defaultStep = 0.1;

            List<Point> resampledPoints = new List<Point>();

            double step = defaultStep;
            double dist = 0;
            Point prevPoint = PosA;
            Point nextPoint = prevPoint;

            resampledPoints.Add(prevPoint);
            while (dist < 1)
            {
                nextPoint = GetPosition(dist + step);
                while ((nextPoint - prevPoint).Magnitude > stepSize)
                {
                    step /= 2;
                    nextPoint = GetPosition(dist + step);
                }
                if (dist + step >= 1)
                {
                    if (PosD != prevPoint)
                        resampledPoints.Add(PosD);
                    break;
                }
                else
                {
                    resampledPoints.Add(nextPoint);
                }
                prevPoint = nextPoint;
                dist += step;
            }

            return resampledPoints.ToArray();
        }

        private Point[] Resample(int count)
        {
            Point[] resampledPoints = new Point[count];

            double step = 1.0 / (count - 1);

            for (int i = 0; i < count; i++)
            {
                resampledPoints[i] = GetPosition(i * step);
            }
            return resampledPoints;
        }

        //http://en.wikipedia.org/wiki/B%C3%A9zier_curve
        //Cubic Bezier curves - explicit form
        private Point GetPosition(double t)
        {
            var x = GetPosition(t, PosA.X, PosB.X, PosC.X, PosD.X);
            var y = GetPosition(t, PosA.Y, PosB.Y, PosC.Y, PosD.Y);
            return new Point(x, y);
        }

        private double GetPosition(double t, double p0, double p0Tan, double p1, double p1Tan)
        {
            return
                Math.Pow(1 - t, 3) * p0 +
                3 * Math.Pow(1 - t, 2) * Math.Pow(t, 1) * p0Tan +
                3 * Math.Pow(1 - t, 1) * Math.Pow(t, 2) * p1 +
                Math.Pow(t, 3) * p1Tan;
        }
    }
}