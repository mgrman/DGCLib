using DGCLib_Base;
using DGCLib_Presenter.VisualizationTypes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DGCLib_Presenter
{
    public delegate void ForceRedrawDel();

    public interface IPresentableAlgorithm : IDisposable
    {
        string Name { get; }

        List<IGeoDraw> Compute(List<PointD> geometry);

        event EventHandler RequestForceRedraw;

        List<ISettings> Settings { get; }
    }

    public interface IPresentableAlgorithmPopulator
    {
        IEnumerable<NamedTypePair> GetSupportedAlgorithms();
    }

    public class NamedTypePair
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        
        public NamedTypePair(Type type,  string name)
        {
            Name = name;
            Type = type;
        }

        public NamedTypePair(Type type)
        {
            Name = type.GetTypeInfo().GetDisplayName();
            Type = type;
        }
    }
}