using System;
using System.Linq;
using System.Reflection;

namespace DGCLib_Base
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class BindInGUIAttribute : Attribute
    {
        public BindInGUIAttribute()
        {
        }
    }

    public static class BindInGUIAttributeExtensions
    {
        public static bool IsBindable(this PropertyInfo prop)
        {
            var atr = prop.GetCustomAttributes(typeof(BindInGUIAttribute), true).FirstOrDefault() as BindInGUIAttribute;
            return atr != null;
        }
    }
}