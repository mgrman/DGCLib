using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DGCLib_Presenter.VisualizationTypes
{
    [DataContract]
    public class GeoSpline : IGeoDraw
    {
        [DataMember]
        public SplineD Spline { get; set; }

        [DataMember]
        public ColorF Color { get; set; }

        [DataMember]
        public double Size { get; set; }

        [DataMember]
        public bool HighlightPoints { get; set; }

        public TextPoint Label { get; set; }

        public IEnumerable<PointD> PointsToDraw
        {
            get
            {
                if (HighlightPoints)
                {
                    yield return Spline.PosA;
                    yield return Spline.PosB;
                    yield return Spline.PosC;
                    yield return Spline.PosD;
                }
            }
        }

        public IEnumerable<SplineD> LinesToDraw
        {
            get
            {
                yield return Spline;
            }
        }

        public IEnumerable<TextPoint> TextToDraw
        {
            get
            {
                if (Label != null)
                    yield return Label;
            }
        }

        public GeoSpline(PointD a, PointD d, ColorF col, double size, bool highlighPoints, TextPoint label = null)
        {
            Spline = new SplineD(a, d);
            Color = col;
            Size = size;
            HighlightPoints = highlighPoints;
            Label = label;
        }

        public GeoSpline(PointD a, PointD b, PointD c, PointD d, ColorF col, double size, bool highlighPoints, TextPoint label = null)
        {
            Spline = new SplineD(a, b, c, d);
            Color = col;
            Size = size;
            HighlightPoints = highlighPoints;
            Label = label;
        }
    }
}