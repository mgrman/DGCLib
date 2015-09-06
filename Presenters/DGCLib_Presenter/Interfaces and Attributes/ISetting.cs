using System;
using System.Reflection;

namespace DGCLib_Presenter
{
    public interface ISettings
    {
        string Name { get; }

        object ValueGeneral { get; set; }
        Type ValueType { get; }

        PropertyInfo BoundProperty { get; set; }
        object BoundObject { get; set; }

        string Serialize();

        void Parse(string str);
    }
}