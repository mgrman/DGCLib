using DGCLib_Base.DomainTypes.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Presenter
{
    public class PointListSetting : BaseSettings<IEnumerable<Point>>
    {
        public PointListSetting(string name, IEnumerable<Point> value = null)
            : base(name)
        {
            Value = value ?? new Point[0];
        }

        public override IEnumerable<Point> Value { get; set; }

        protected override IEnumerable<Point> ParseValue(string str)
        {
            return str.Split('|').Select(o => Point.TryParse(o)).Where(o => o != null).Select(o => o.Value).ToArray();
        }

        public override string Serialize()
        {
            return String.Join("|", Value);
        }
    }
}