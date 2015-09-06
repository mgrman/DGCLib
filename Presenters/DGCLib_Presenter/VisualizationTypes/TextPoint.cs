using System.Runtime.Serialization;

namespace DGCLib_Presenter.VisualizationTypes
{
    public class TextPoint
    {
        [DataMember]
        public PointD Position { get; set; }

        [DataMember]
        public string Text { get; set; }

        public TextPoint(PointD position, string text)
        {
            Position = position;
            Text = text;
        }
    }
}