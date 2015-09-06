using DGCLib_Base.DomainTypes.LinearAlgebra;

namespace DGCLib_Presenter
{
    public class PointSetting : BaseSettings<Point>
    {
        public PointSetting(string name, Point value = new Point())
            : base(name)
        {
            Value = value;
        }

        public override Point Value { get; set; }

        protected override Point ParseValue(string str)
        {
            return Point.TryParse(str) ?? new Point();
        }
    }
}