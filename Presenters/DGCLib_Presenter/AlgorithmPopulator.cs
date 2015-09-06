using DGCLib_Base;
using DGCLib_Misc;
using DGCLib_Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DGCLib_Presenter
{
    [SampleAssemblyType(typeof(DGCLib_IK.BaseIK))]
    [SampleAssemblyType(typeof(DGCLib_Misc.GiftWrap_ConvexHull))]
    public class AlgorithmPopulator : IPresentableAlgorithmPopulator
    {
        public IEnumerable<NamedTypePair> GetSupportedAlgorithms()
        {
            var interfaceType = typeof(IAlgorithm<>);
            var interfaceTypeInfo = interfaceType.GetTypeInfo();

            var controllerType = typeof(AlgorithmControler<,>);

            var types = typeof(AlgorithmPopulator).GetTypeInfo()
                .GetCustomAttributes<SampleAssemblyTypeAttribute>()
                .Select(attr=>attr.Type.GetTypeInfo().Assembly)
                .Distinct()
                .SelectMany(ass => ass.DefinedTypes)
                .Where(type => !type.IsAbstract)
                .SelectMany(type => type.ImplementedInterfaces
                    .Select(i =>
                    {
                        var iInfo = i.GetTypeInfo();

                        if (!iInfo.IsGenericType)
                            return null;

                        if (iInfo.Name != interfaceTypeInfo.Name)
                            return null;

                        bool isAssignable = interfaceTypeInfo.MakeGenericType(iInfo.GenericTypeArguments[0]).GetTypeInfo().IsAssignableFrom(iInfo);
                        if (!isAssignable)
                            return null;

                        var specificType = controllerType.MakeGenericType(type.AsType(), iInfo.GenericTypeArguments[0]);
                        return new NamedTypePair(specificType, type.GetDisplayName(true));
                    })
                    .Where(o => o != null));
            return types;
        }
    }

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class SampleAssemblyTypeAttribute : Attribute
    {
        readonly Type type;
        
        public SampleAssemblyTypeAttribute(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }
}