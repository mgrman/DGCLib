using DGCLib_Base;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using DGCLib_Misc;
using DGCLib_Presenter;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Presenter
{
    //Presenter
    public class AlgorithmControler<ALGORITM, RESULTINGGEO> : BaseAlgorithmControler<ALGORITM> 
        where ALGORITM : class, IAlgorithm<RESULTINGGEO>, new()
        where RESULTINGGEO : IGeoStructure
    {
        protected override List<IGeoDraw> ComputeInner(Point[] points)
        {
            var resSpecial = Engine.Compute();
            var res = resSpecial.Cast<IGeoStructure>() as IEnumerable<IGeoStructure>;

            if (res == null || !res.Any())
                return new List<IGeoDraw>();
            
            return res
                .ToDrawableGeo()
                .ToList();

            //var bsGeo = Enumerable.Range(0, res.Length - 1).Select(j => new Line(res[j], res[j + 1]) as IGeoStructure);

            //if (ShowDebug)
            //    return IKEngine
            //        .AdditionalGeo
            //        .Concat(bsGeo)
            //        .ToDrawableGeo()
            //        .ToList();
            //else
            //    return bsGeo
            //        .ToDrawableGeo()
            //        .ToList();
        }
    }
}