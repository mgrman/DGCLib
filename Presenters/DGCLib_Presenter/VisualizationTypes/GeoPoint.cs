using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DGCLib_Presenter.VisualizationTypes
{
    [DataContract]
    public class GeoPoint : IGeoDraw
    {
        [DataMember]
        public PointD Position { get; set; }

        [DataMember]
        public ColorF Color { get; set; }

        [DataMember]
        public double Size { get; set; }

        public IEnumerable<PointD> PointsToDraw
        {
            get
            {
                yield return Position;
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
                yield break;
            }
        }

        public GeoPoint(PointD pos, ColorF col, double size)
        {
            Position = pos;
            Color = col;
            Size = size;
        }
    }
}