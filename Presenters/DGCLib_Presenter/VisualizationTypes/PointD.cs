using System.Runtime.Serialization;

namespace DGCLib_Presenter.VisualizationTypes
{
    public struct PointD
    {
        [DataMember]
        public double X;

        [DataMember]
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}