using DGCLib_Presenter.VisualizationTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DGCLib_WinForms.DataTypes
{
    [DataContract]
    internal class GuiUserSettings
    {
        internal GuiUserSettings()
        {
            BackColor = Color.White;
            PointColor = Color.Black;
            PointSize = 20;
            WindowsSize = new System.Drawing.Size(800, 600);
            WindowsLocation = new Point(100, 100);
            WindowState = FormWindowState.Normal;

            AddedAlgorithms = new List<string>();
            AddedAlgorithmsData = new List<string>();
        }

        [Browsable(true)]
        [DataMember]
        public Color BackColor { get; set; }

        [Browsable(true)]
        [DataMember]
        public double PointSize { get; set; }

        [Browsable(true)]
        [DataMember]
        public Color PointColor { get; set; }

        [Browsable(false)]
        [DataMember]
        public System.Drawing.Size WindowsSize { get; set; }

        [Browsable(false)]
        [DataMember]
        public Point WindowsLocation { get; set; }

        [Browsable(false)]
        [DataMember]
        public FormWindowState WindowState { get; set; }

        [Browsable(false)]
        [DataMember]
        public List<string> AddedAlgorithms { get; set; }

        [Browsable(false)]
        [DataMember]
        public List<string> AddedAlgorithmsData { get; set; }

        [Browsable(false)]
        [DataMember]
        public List<GeoPoint> AddedPoints { get; set; }
    }
}