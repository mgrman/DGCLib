using DGCLib_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DGCLib_Presenter
{
    public static class AssemblyUtils
    {
        public static IEnumerable<Type> GetAllAssignableTypes<T>()
        {
            return GetAllAssignableTypes(typeof(T));
        }

        public static IEnumerable<Type> GetAllAssignableTypes(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return typeInfo.Assembly.AsIEnumerable()
                .SelectMany(s =>
                {
                    try
                    {
                        return s.DefinedTypes;
                    }
                    catch (Exception ex)
                    {
                        Reporting.LogMessage("Problem getting types from {0}. Ex: {1}", s.FullName, ex.Message);
                        return new TypeInfo[0];
                    }
                })
                .Where(p => typeInfo.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract && !p.IsGenericType)
                .Select(o => o.AsType());
        }
    }

    public delegate Type TypeDelegate();
}