using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using System.Collections.Generic;

namespace DGCLib_Misc
{
    public interface IAlgorithm<T> where T : IGeoStructure
    {
        IEnumerable<T> Compute();
    }
}