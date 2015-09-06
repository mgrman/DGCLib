using DGCLib_Presenter;
using DGCLib_Presenter.VisualizationTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DGCLib_WinForms.DataTypes
{
    public class GeoStorage
    {
        public List<GeoPoint> InputPoints { get; private set; }

        public List<IGeoDraw> AlgorithmGeometry { get; private set; }

        public List<IGeoDraw> GetDrawedGeometry()
        {
            List<IGeoDraw> drawed = new List<IGeoDraw>();
            drawed.AddRange(InputPoints.Cast<IGeoDraw>());
            drawed.AddRange(AlgorithmGeometry);
            return drawed;
        }

        private Color _pointColor;

        public Color PointColor
        {
            get { return _pointColor; }
            set
            {
                _pointColor = value;
                foreach (var point in InputPoints)
                {
                    point.Color = _pointColor.ToColorF();
                }
            }
        }

        private double _pointSize;

        public double PointSize
        {
            get { return _pointSize; }
            set
            {
                _pointSize = value;
                foreach (var point in InputPoints)
                {
                    point.Size = _pointSize;
                }
            }
        }

        public GeoStorage()
        {
            InputPoints = new List<GeoPoint>();
            AlgorithmGeometry = new List<IGeoDraw>();
            PointColor = Color.Black;
            PointSize = 4;
        }

        public void ClearGeo()
        {
            InputPoints.Clear();
        }

        public void Generate()
        {
            Random ran = new Random();
            for (int i = 0; i < 10; i++)
            {
                InputPoints.Add(new GeoPoint(new PointD(ran.NextDouble(), ran.NextDouble()), PointColor.ToColorF(), PointSize));
            }
        }

        public void AddPoint(PointF point)
        {
            InputPoints.Add(new GeoPoint(point.ToPointD(), PointColor.ToColorF(), PointSize));
        }

        public int GetHitPoint(PointF hit, SizeF windowSize)
        {
            for (int i = 0; i < InputPoints.Count; i++)
            {
                var geoPoint = InputPoints[i];
                var pos = geoPoint.Position.ToPointF();

                double dist = Math.Sqrt(Math.Pow(pos.X * windowSize.Width - hit.X * windowSize.Width, 2) + Math.Pow(pos.Y * windowSize.Height - hit.Y * windowSize.Height, 2));
                if (dist < geoPoint.Size)
                    return i;
            }
            return -1;
        }

        public void RemovePoint(PointF point, SizeF windowSize)
        {
            int i = GetHitPoint(point, windowSize);
            if (i != -1)
                InputPoints.RemoveAt(i);
        }
    }
}