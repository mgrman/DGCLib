using System;
using System.Linq;
using System.Reflection;

namespace DGCLib_Base
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public DisplayNameAttribute(string name)
        {
            DisplayName = name;
        }
    }

    public static class DisplayNameAttributeExtensions
    {
        public static string GetDisplayName(this TypeInfo type, bool clipTypeName = true)
        {
            if (type == null)
                return "<NULL>";

            var atr = type.CustomAttributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (atr != null && !string.IsNullOrEmpty(atr.DisplayName))
                return atr.DisplayName;

            if (clipTypeName)
                return type.Name;

            return type.FullName;
        }

        public static string GetDisplayName(this PropertyInfo prop)
        {
            var atr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;
            if (atr == null)
                return prop.Name;
            else
                return atr.DisplayName;
        }
    }
}