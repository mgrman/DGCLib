using System.Runtime.Serialization;

namespace DGCLib_Presenter.VisualizationTypes
{
    public class SplineD
    {
        [DataMember]
        public PointD PosA { get; set; }

        [DataMember]
        public PointD PosB { get; set; }

        [DataMember]
        public PointD PosC { get; set; }

        [DataMember]
        public PointD PosD { get; set; }

        public SplineD(PointD posA, PointD posB, PointD posC, PointD posD)
        {
            PosA = posA;
            PosB = posB;
            PosC = posC;
            PosD = posD;
        }

        public SplineD(PointD start, PointD end)
        {
            PosA = start;
            PosB = start;
            PosC = end;
            PosD = end;
        }
    }
}