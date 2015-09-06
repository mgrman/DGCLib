using DGCLib_Presenter.VisualizationTypes;
using System.Collections.Generic;

namespace DGCLib_Presenter
{
    public interface IGeoDraw
    {
        ColorF Color { get; }
        double Size { get; }

        IEnumerable<PointD> PointsToDraw { get; }
        IEnumerable<SplineD> LinesToDraw { get; }
        IEnumerable<TextPoint> TextToDraw { get; }
    }
}