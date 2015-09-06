using System;
using System.Linq;
using System.Reflection;

namespace DGCLib_Presenter
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public abstract class BaseSettings<T> : ISettings
    {
        public BaseSettings(string name)
        {
            Name = name;
        }

        public virtual string Name { get; private set; }
        public virtual T Value { get; set; }

        public virtual object ValueGeneral { get { return Value; } set { Value = (T)value; } }
        public virtual Type ValueType { get { return typeof(T); } }

        public PropertyInfo BoundProperty { get; set; }
        public object BoundObject { get; set; }

        public virtual string Serialize()
        {
            if (ValueGeneral == null)
                return "";

            return ValueGeneral.ToString();
        }

        public void Parse(string str)
        {
            ValueGeneral = ParseValue(str);
        }

        protected abstract T ParseValue(string str);
    }

    public class StringSetting : BaseSettings<string>
    {
        public StringSetting(string name, string value = "")
            : base(name)
        {
            Value = value;
        }

        public override string Value { get; set; }

        protected override string ParseValue(string str)
        {
            return str;
        }
    }

    public class DoubleSetting : BaseSettings<double>
    {
        public DoubleSetting(string name, double value = 0)
            : base(name)
        {
            Value = value;
        }

        public override double Value { get; set; }

        protected override double ParseValue(string str)
        {
            double pars;
            return double.TryParse(str, out pars) ? pars : double.NaN;
        }
    }

    public class BoolSetting : BaseSettings<bool>
    {
        public BoolSetting(string name, bool value = true)
            : base(name)
        {
            Value = value;
        }

        public override bool Value { get; set; }

        protected override bool ParseValue(string str)
        {
            Boolean pars;
            return Boolean.TryParse(str, out pars) ? pars : false;
        }
    }

    public class DoubleCollectionSetting : BaseSettings<double[]>
    {
        public DoubleCollectionSetting(string name, double[] value = null)
            : base(name)
        {
            Value = value ?? new double[0];
        }

        public override double[] Value { get; set; }

        public override string Serialize()
        {
            return String.Join(" ", Value.Select(o => o.ToString()).ToArray());
        }

        protected override double[] ParseValue(string str)
        {
            double pars;
            return str.Split(' ').Select(o => double.TryParse(o, out pars) ? pars : double.NaN).ToArray();
        }
    }

    public class EnumSetting : BaseSettings<Enum>
    {
        private Type _valueType;

        public EnumSetting(string name, Enum value)
            : base(name)
        {
            Value = value;
            _valueType = Value.GetType();
        }

        public override Enum Value { get; set; }

        public override Type ValueType { get { return _valueType; } }

        protected override Enum ParseValue(string str)
        {
            try
            {
                var res = Enum.Parse(_valueType, str ?? "");
                return res as Enum;
            }
            catch
            {
                return Enum.GetValues(_valueType).Cast<Enum>().FirstOrDefault();
            }
        }
    }
}