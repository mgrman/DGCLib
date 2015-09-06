using DGCLib_Presenter;
using DGCLib_Presenter.VisualizationTypes;
using DGCLib_WinForms.Controls;
using DGCLib_WinForms.DataTypes;
using DGCLib_WinForms.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DGCLib_WinForms
{
    public partial class MainForm : Form
    {
        private bool _ctrl = false;
        private bool _shift = false;
        private GeoStorage _storage = new GeoStorage();

        private int _movedPoint = -1;
        private Point _oldMousePos;
        private readonly List<IPresentableAlgorithm> _algorithms = new List<IPresentableAlgorithm>();

        private GuiUserSettings _userSettings;
        private string _userSettingsPath;

        private bool _consoleDirty = false;
        private StringBuilder _console = new StringBuilder(5000, 5000);
        private System.Windows.Forms.Timer _consoleTimer = new System.Windows.Forms.Timer();

        private GeoPresenterBinder _binder = new GeoPresenterBinder(true, true);

        public MainForm()
        {
            InitializeComponent();

#if !DEBUG
            _mainTable.Controls.Remove(_consoleTextbox);
            _mainTable.ColumnStyles.Remove(_mainTable.ColumnStyles.Cast<ColumnStyle>().Last());
#endif

            //DrawingConverter.ConverterOverides[typeof(DGCLib_Base.DomainTypes.BezierSpline)] = (geo) =>
            //{
            //    var spline = geo as DGCLib_Base.DomainTypes.BezierSpline;

            //    return new IGeoDraw[] {
            //        new GeoBezierSpline(spline,  DGCLib_Base.DomainTypes.ColorF.Red.SetAlpha(0.5f), 2, true)
            //    };
            //};

            PopulateGui();
            SetEvents();
            _userSettingsPath = ApplicationUtils.LocalAppDataPath + Path.DirectorySeparatorChar + "guiSettings.xml";

            LoadUserSettings();

            WinFormsUtils.SetDoubleBuffered(_consoleTextbox);

            InterAppComunication.Instance.Log += (o, e) =>
            {
                _consoleDirty = true;
                _console.Append(string.Format("\r\nLog {0}:\r\n\t{1}", DateTime.Now, e.Message));
                if (_console.Length > _console.MaxCapacity / 2)
                    _console.Remove(0, _console.Length - _console.MaxCapacity / 2);
            };
            _consoleTimer.Interval = 100;
            _consoleTimer.Tick += (o, e) =>
            {
                UpdateConsole();
            };
            _consoleTimer.Start();

            _forceRecalc.Click += (o, e) =>
            {
                RedrawGeo();
            };

            _saveScreen.Click += (o, e) =>
            {
                if (_saveFileDialogPng.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var path = _saveFileDialogPng.FileName;

                    var bmp = _canvas.RenderToBitmap(new System.Drawing.Size(4096, 4096), 4.0f);
                    bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }
            };

            RedrawGeo();
        }

        private void UpdateConsole()
        {
            if (!_consoleDirty)
                return;

            _consoleTextbox.Text = _console.ToString();
            _consoleTextbox.Select(_consoleTextbox.Text.Length, 0);
            _consoleTextbox.ScrollToCaret();
            _consoleTextbox.Refresh();

            _consoleDirty = false;
        }

        #region GUI managment

        private void PopulateGui()
        {
            //loads assemblies that are in the same folder as current assembly
            //these are assemblies that were referenced and copied during build
            //this works for this setup, more general setup would needed some config files to determine which assemblies to load

            foreach (NamedTypePair alg in _binder.IAlgorithmTypes)
            {
                _comboAlgTypes.Items.Add(new ComboPair<Type>(alg.Name, alg.Type));
            }

            if (_comboAlgTypes.Items.Count != 0)
                _comboAlgTypes.SelectedIndex = 0;
        }

        private void SetEvents()
        {
            KeyDown += UpdateKeys;
            KeyUp += UpdateKeys;
            _buttonClear.Click += (o, e) =>
            {
                _storage.ClearGeo();
                RedrawGeo();
            };
            _buttonGenerate.Click += (o, e) =>
            {
                _storage.Generate();
                RedrawGeo();
            };
            _buttonSettings.Click += (o, e) =>
            {
                var settForm = new SettingsForm();
                settForm.SetObject(_userSettings);
                settForm.ShowDialog();
                UserSettingsChanged();
                RedrawGeo();
            };
            _exit.Click += (o, e) =>
            {
                Close();
            };

            _canvas.MouseDown += GeoMouseDown;
            _canvas.MouseMove += GeoMouseMove;
            _canvas.MouseUp += GeoMouseUp;

            FormClosing += SaveUserSettings;

            _addAlgorithm.Click += (o, e) =>
            {
                var cPair = _comboAlgTypes.SelectedItem as ComboPair<Type>;
                var type = cPair == null ? null : cPair.Value;
                AddAlgoritmControl(CreateAlgInstance(type));
                RedrawGeo();
            };
        }

        private void LoadUserSettings()
        {
            _userSettings = null;
            try
            {
                _userSettings = SerializationUtils.Deserialize<GuiUserSettings>(File.ReadAllText(_userSettingsPath));
            }
            catch (Exception ex)
            {
                InterAppComunication.ReportError("Loading userSettings", ex);
            }

            if (_userSettings == null)
                _userSettings = new GuiUserSettings();

            UserSettingsChanged();
            this.WindowState = _userSettings.WindowState;
            this.Size = _userSettings.WindowsSize;
            this.Location = _userSettings.WindowsLocation;
            if (_userSettings.AddedPoints != null)
                _storage.InputPoints.AddRange(_userSettings.AddedPoints);

            for (int i = 0; i < _userSettings.AddedAlgorithms.Count && i < _userSettings.AddedAlgorithmsData.Count; i++)
            {
                try {
                    var algName = _userSettings.AddedAlgorithms[i];
                    var algData = _userSettings.AddedAlgorithmsData[i];

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var type = Type.GetType(algName);

                    var alg = CreateAlgInstance(type);

                    if (alg == null)
                        continue;

                    var dict = algData.Split(';')
                        .Select(o => { return o.Split(':'); })
                        .GroupBy(o => o[0])
                        .Select(o => o.First())
                        .ToDictionary(k => { return k[0]; }, v => { return v[1]; });

                    foreach (var set in alg.Settings)
                    {
                        if (dict.ContainsKey(set.Name))
                        {
                            var value = dict[set.Name];

                            set.Parse(value);
                        }
                    }
                    AddAlgoritmControl(alg);
                }
                catch(Exception ex)
                {
                    InterAppComunication.ReportError("Adding Alg in LoadUserSettings",ex);
                }
            }
        }

        private void SaveUserSettings(object sender, EventArgs e)
        {
            _userSettings.WindowState = this.WindowState;
            _userSettings.WindowsSize = this.Size;
            _userSettings.WindowsLocation = this.Location;
            _userSettings.PointColor = _storage.PointColor;
            _userSettings.PointSize = _storage.PointSize;
            _userSettings.BackColor = _canvas.BackgroundColor;

            _userSettings.AddedPoints = _storage.InputPoints;

            _userSettings.AddedAlgorithms = _algorithms.Select(o => { return o.GetType().AssemblyQualifiedName; }).ToList();
            _userSettings.AddedAlgorithmsData = _algorithms.Select(o =>
                 {
                     return o.Settings.Select(s =>
                     {
                         return string.Format("{0}:{1}", s.Name, s.Serialize());
                     }).Aggregate((a, b) =>
                     {
                         return string.Format("{0};{1}", a, b);
                     });
                 }).ToList();

            try
            {
                File.WriteAllText(_userSettingsPath, SerializationUtils.Serialize(_userSettings));
            }
            catch (Exception ex)
            {
                InterAppComunication.ReportError("Saving userSettings", ex);
                _userSettings = new GuiUserSettings();
            }
        }

        private void UserSettingsChanged()
        {
            _storage.PointColor = _userSettings.PointColor;
            _storage.PointSize = _userSettings.PointSize;
            _canvas.BackgroundColor = _userSettings.BackColor;
        }

        private IPresentableAlgorithm CreateAlgInstance(Type type)
        {
            if (type == null || !typeof(IPresentableAlgorithm).IsAssignableFrom(type))
                return null;

            return Activator.CreateInstance(type) as IPresentableAlgorithm;
        }

        private BaseControl AddAlgoritmControl(IPresentableAlgorithm alg)
        {
            if (alg == null)
                return null;

            _algorithms.Add(alg);
            alg.RequestForceRedraw += RedrawRequested;

            var con = new BaseControl(alg);

            _flowAlg.Controls.Add(con);

            con.CloseButtonClicked += RemoveAlgorithmEvent;
            con.ValueChanged += RedrawRequested;

            return con;
        }

        private void RedrawRequested(object sender, EventArgs e)
        {
            RedrawGeo();
        }

        private void RemoveAlgorithmEvent(object sender, EventArgs e)
        {
            var con = sender as BaseControl;
            con.CloseButtonClicked -= RemoveAlgorithmEvent;
            con.Algorithm.RequestForceRedraw -= RedrawRequested;
            _flowAlg.Controls.Remove(con);
            _algorithms.Remove(con.Algorithm);
            con.Dispose();

            RedrawGeo();
        }

        #endregion GUI managment

        #region UpdateState

        private void RedrawGeo()
        {
            List<double> times = new List<double>();

            _storage.AlgorithmGeometry.Clear();
            var input = _storage.InputPoints
                .OfType<GeoPoint>()
                .Select(o => o.Position)
                .ToList();

            foreach (var alg in _algorithms)
            {
                var startTime = PerfCounter.Instance.Seconds;

                var geo = alg.Compute(input);

                var curTime = PerfCounter.Instance.Seconds;
                times.Add(1.0 / (curTime - startTime));

                _storage.AlgorithmGeometry.AddRange(geo);
            }

            _fpsLabel.Text = String.Format(_fpsLabel.Tag as string, string.Join(" ", times.Select(o => Math.Round(o, 2).ToString()).ToArray()));
            var geom = _storage.GetDrawedGeometry();
            _toolStrip.Refresh();
            _canvas.Draw(geom);
        }

        #endregion UpdateState

        #region Picture Box User Input

        private void UpdateKeys(object sender, KeyEventArgs e)
        {
            _ctrl = e.Control;
            _shift = e.Shift;
        }

        private void GeoMouseDown(object sender, MouseEventArgs e)
        {
            _movedPoint = -1;
            _oldMousePos = new Point(e.X, e.Y);

            var windowSize = _canvas.GetWindowSize();

            PointF normalPos = _canvas.TransformToCanvasSpace(e.Location);

            if (_shift)
            {
                _storage.AddPoint(normalPos);
                _movedPoint = _storage.InputPoints.Count - 1;
            }
            if (_ctrl)
            {
                _storage.RemovePoint(normalPos, windowSize);
            }
            _movedPoint = _storage.GetHitPoint(normalPos, windowSize);

            RedrawGeo();
        }

        private void GeoMouseMove(object sender, MouseEventArgs e)
        {
            bool redraw = false;

            PointF normalPos = _canvas.TransformToCanvasSpace(e.Location);
            normalPos = ClipPos(normalPos);

            if (_movedPoint >= 0)
            {
                redraw = true;
                _storage.InputPoints[_movedPoint].Position = normalPos.ToPointD();
            }

            if (redraw)
                RedrawGeo();
        }

        private void GeoMouseUp(object sender, MouseEventArgs e)
        {
            _movedPoint = -1;
            _oldMousePos = new Point();
        }

        private PointF ClipPos(PointF pos)
        {
            pos.X = Math.Max(Math.Min(pos.X, 1), -1);
            pos.Y = Math.Max(Math.Min(pos.Y, 1), -1);
            return pos;
        }

        #endregion Picture Box User Input
    }
}