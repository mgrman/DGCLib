using DGCLib_Presenter;
using DGCLib_WinForms.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DGCLib_WinForms
{
    public class GeoPresenterBinder
    {
        private List<Assembly> _loadedAssemblies;

        private List<NamedTypePair> _iAlgorithmTypes;

        public GeoPresenterBinder(bool loadAllLocalAssemblies, bool initiliazeReporting = true)
        {
            LoadAssemblies(loadAllLocalAssemblies);
            LoadTypes();
            if (initiliazeReporting)
                InitializeReporting();
        }

        public IEnumerable<NamedTypePair> IAlgorithmTypes { get { return _iAlgorithmTypes; } }

        private void LoadAssemblies(bool loadAllLocal)
        {
            _loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            if (loadAllLocal)
            {
                var loadedPaths = _loadedAssemblies.Select(a => a.Location).ToArray();

                var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
                foreach (var path in toLoad)
                {
                    try
                    {
                        _loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));
                    }
                    catch { }
                }
            }
        }

        private void LoadTypes()
        {
            _iAlgorithmTypes = new List<NamedTypePair>();
            foreach (var assembly in _loadedAssemblies)
            {
                var allTypes = assembly.GetTypes();
                var populatorTypes = allTypes.Where(o => typeof(IPresentableAlgorithmPopulator).IsAssignableFrom(o) && o.GetConstructor(Type.EmptyTypes) != null && !o.IsAbstract && !o.IsGenericType).ToArray();

                if (populatorTypes.Length != 0)
                {
                    _iAlgorithmTypes.AddRange(populatorTypes.SelectMany(o =>
                    {
                        try
                        {
                            return (Activator.CreateInstance(o) as IPresentableAlgorithmPopulator).GetSupportedAlgorithms();
                        }
                        catch
                        {
                            return new NamedTypePair[0];
                        }
                    }));
                }
                else
                {
                    var iAlgTypes = allTypes.Where(o => typeof(IPresentableAlgorithm).IsAssignableFrom(o) && !o.IsAbstract && !o.IsGenericType).Select(o => new NamedTypePair(o));
                    _iAlgorithmTypes.AddRange(iAlgTypes);
                }
            }
        }

        private void InitializeReporting()
        {
            var setter = new ReportingSetter();

            setter.MessageAdded += MessageAddedHandler;
            setter.TimeProvider = TimeProvider;
        }

        private void MessageAddedHandler(object sender, MessageEventArgs e)
        {
            InterAppComunication.ReportLog(e.Message);
        }

        private double TimeProvider()
        {
            return PerfCounter.Instance.Seconds;
        }
    }
}