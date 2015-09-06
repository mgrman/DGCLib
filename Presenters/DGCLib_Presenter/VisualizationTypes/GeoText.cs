using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DGCLib_Presenter.VisualizationTypes
{
    [DataContract]
    public class GeoText : IGeoDraw
    {
        [DataMember]
        public PointD Position { get; set; }

        [DataMember]
        public ColorF Color { get; set; }

        [DataMember]
        public double Size { get; set; }

        [DataMember]
        public string Text { get; set; }

        public IEnumerable<PointD> PointsToDraw
        {
            get
            {
                yield break;
            }
        }

        public IEnumerable<SplineD> LinesToDraw
        {
            get
            {
                yield break;
            }
        }

        public IEnumerable<TextPoint> TextToDraw
        {
            get
            {
                yield return new TextPoint(Position, Text);
            }
        }

        public GeoText(PointD pos, ColorF col, double size, string text)
        {
            Position = pos;
            Color = col;
            Size = size;
            Text = text;
        }
    }
}