using DGCLib_Presenter;
using DGCLib_WinForms.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class PictureBoxExtended : PictureBox
    {
        private List<IGeoDraw> _geometry;
        public Color BackgroundColor { get; set; }

        public PictureBoxExtended()
        {
            InitializeComponent();
        }

        public void Draw(List<IGeoDraw> geo)
        {
            _geometry = geo;
            Refresh();
        }

        public void Clear()
        {
            _geometry = null;
        }

        private Matrix _canvasMatrix;
        private RectangleF _drawRectangle;

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            g.Clear(BackgroundColor);

            DrawCanvas(g, Size, 1.0f);

            base.OnPaint(pe);
        }

        private void DrawCanvas(Graphics g, SizeF windowSize, float scale)
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            windowSize = ClipSize(windowSize);

            var mat = new Matrix();
            //mat.Scale(0.5f, 0.5f);
            mat.Translate(windowSize.Width, windowSize.Height);
            g.Transform = mat;
            _canvasMatrix = mat.Clone();
            _canvasMatrix.Invert();

            _drawRectangle = new RectangleF(-(float)windowSize.Width, -(float)windowSize.Height, (float)windowSize.Width * 2, (float)windowSize.Height * 2);
            g.DrawRectangle(Pens.Black, _drawRectangle.X, _drawRectangle.Y, _drawRectangle.Width, _drawRectangle.Height);

            if (_geometry == null) return;

            foreach (var geo in _geometry)
            {
                try
                {
                    foreach (var point in geo.PointsToDraw)
                    {
                        var pos = point.ToPointF();

                        Brush brush = new SolidBrush(geo.Color.ToColor());
                        PointF posWindow = pos.Scale(windowSize);

                        var size = (float)geo.Size * scale;

                        g.FillEllipse(brush, (posWindow.X - size / 2), (posWindow.Y - size / 2), size, size);
                    }

                    foreach (var spline in geo.LinesToDraw)
                    {
                        var size = (float)geo.Size * scale;
                        Pen pen = new Pen(geo.Color.ToColor(), (float)size / 2);
                        Pen penDashed = pen.Clone() as Pen;
                        penDashed.DashPattern = new float[] { 0.5f, 0.5f };
                        penDashed.DashStyle = DashStyle.Dash;

                        Brush brush = new SolidBrush(geo.Color.ToColor());

                        g.DrawBezier(pen,
                            spline.PosA.ToPointF().Scale(windowSize),
                            spline.PosB.ToPointF().Scale(windowSize),
                            spline.PosC.ToPointF().Scale(windowSize),
                            spline.PosD.ToPointF().Scale(windowSize));
                    }

                    foreach (var text in geo.TextToDraw)
                    {
                        var size = (float)geo.Size * scale;

                        Brush brush = Brushes.Black;
                        Font font = new Font(FontFamily.GenericMonospace, (float)size);
                        var posWindow = (text.Position.ToPointF().Scale(windowSize));
                        g.DrawString(text.Text, font, brush, posWindow);
                    }

                    //var renderer = RendererStorage.GetRenderer(geo);
                    //if (renderer == null)
                    //    continue;
                    //renderer.Draw(geo,g, windowSize, scale);
                }
                catch (Exception ex)
                {
                    InterAppComunication.ReportError("Problem drawing " + geo.ToString(), ex);
                }
            }
        }

        private SizeF ClipSize(SizeF origSize)
        {
            var size = Math.Min(origSize.Width, origSize.Height) / 2;
            return new SizeF(size, size);
        }

        public SizeF GetWindowSize()
        {
            return ClipSize(this.Size);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        public PointF TransformToCanvasSpace(PointF pos)
        {
            pos = _canvasMatrix == null ? new PointF() : _canvasMatrix.TransformPoint(pos);

            var bb = _drawRectangle;
            float x = pos.X / (bb.Width / 2.0f);
            float y = pos.Y / (bb.Height / 2.0f);
            return new PointF(x, y);
        }

        public Bitmap RenderToBitmap(System.Drawing.Size size, float scale)
        {
            var temp = _geometry;
            _geometry = _geometry.Select(o => SerializationUtils.Clone(o)).Cast<IGeoDraw>().ToList();

            var bmp = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                DrawCanvas(g, size, scale);
            }

            _geometry = temp;

            return bmp;
        }
    }
}