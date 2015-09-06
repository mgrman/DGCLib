using DGCLib_Base;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using DGCLib_Presenter.VisualizationTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DGCLib_Presenter
{
    [DisplayName("Testing")]
    public abstract class BaseAlgorithmControler<T> : IPresentableAlgorithm where T : class, new()
    {
        protected T Engine { get; private set; }

        public bool Enabled { get; set; }

        public bool ShowDebug { get; set; }

        public BaseAlgorithmControler()
        {
            Enabled = true;
            Settings = new List<ISettings>();

            var enabledSett = new BoolSetting("Enabled", true);

            enabledSett.BoundObject = this;
            enabledSett.BoundProperty = ReflexionUtils.PropertyOf<BaseAlgorithmControler<T>, bool>(o => o.Enabled);
            enabledSett.Value = Enabled;
            Settings.Add(enabledSett);

            BoolSetting debugSett = new BoolSetting("ShowDebug");
            debugSett.BoundObject = this;
            debugSett.BoundProperty = ReflexionUtils.PropertyOf<BaseAlgorithmControler<T>, bool>(o => o.ShowDebug);
            debugSett.Value = ShowDebug;
            Settings.Add(debugSett);

            Engine = Activator.CreateInstance(typeof(T)) as T;

            foreach (var prop in Engine.GetType().GetRuntimeProperties().Where(o => o.IsBindable()))
            {
                ISettings iSett = null;
                var name = prop.GetDisplayName();

                if (prop.PropertyType == typeof(double))
                {
                    iSett = new DoubleSetting(name);
                }
                if (prop.PropertyType == typeof(double[]))
                {
                    iSett = new DoubleCollectionSetting(name);
                }
                if (prop.PropertyType == typeof(bool))
                {
                    iSett = new BoolSetting(name);
                }

                if (prop.PropertyType == typeof(Point))
                {
                    iSett = new PointSetting(name);
                }

                if (prop.PropertyType == typeof(IEnumerable<Point>))
                {
                    iSett = new PointListSetting(name);
                }

                if (prop.PropertyType == typeof(string))
                {
                    iSett = new StringSetting(name);
                }
                if (prop.PropertyType.GetTypeInfo().IsEnum)
                {
                    var templateEnum = (Enum)prop.GetValue(Engine, null);
                    iSett = new EnumSetting(name, templateEnum);
                }

                if (iSett != null)
                {
                    iSett.BoundObject = Engine;
                    iSett.BoundProperty = prop;
                    iSett.ValueGeneral = prop.GetValue(Engine, null);

                    Settings.Add(iSett);
                }
            }
        }

        static BaseAlgorithmControler()
        {
            //NUTNE NASTAVIT REPORTING podla view
            //Reporting.MessageAdded += (o, e) => ???.LogMessage(e.Message);
            //Reporting.TimeProvider = () => ???..Seconds;
        }

        protected virtual List<IGeoDraw> ComputeInner(Point[] points)
        {
            return new List<IGeoDraw>();
        }

        #region IAlgorithm

        public string Name
        {
            get
            {
                return typeof(T).GetTypeInfo().GetDisplayName(true);
            }
        }

        public List<IGeoDraw> Compute(List<PointD> geometry)
        {
            if (geometry == null || geometry.Count == 0)
                return new List<IGeoDraw>();

            var refPoint = geometry.First();

            var points = geometry
                    .Select(o => { return o.ToPoint(); })
                    .ToArray();
            BindPoints(points);

            foreach (var sett in Settings)
            {
                sett.BoundProperty.SetValue(sett.BoundObject, sett.ValueGeneral, null);
            }

            if (!Enabled)
                return new List<IGeoDraw>();

            return ComputeInner(points);
        }

        protected virtual void BindPoints(Point[] points)
        {
            int i = 0;
            foreach (var set in Settings.OfType<PointSetting>())
            {
                if (i >= points.Length)
                    break;
                set.Value = points[i];
                i++;
            }

            var listSett = Settings.OfType<PointListSetting>().FirstOrDefault();
            if (listSett != null)
            {
                var vectors = points.Skip(i).ToArray();

                listSett.Value = vectors;
            }
        }

        public event EventHandler RequestForceRedraw;

        protected void FireRequestForceRedraw()
        {
            var handler = RequestForceRedraw;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public List<ISettings> Settings { get; private set; }

        #endregion IAlgorithm

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }

        #endregion IDisposable
    }
}