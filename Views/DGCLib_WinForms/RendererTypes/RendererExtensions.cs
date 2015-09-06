using DGCLib_Presenter.VisualizationTypes;
using System.Drawing;

namespace DGCLib_WinForms
{
    public static class RendererExtensions
    {
        public static PointF ToPointF(this PointD vec)
        {
            return new PointF((float)vec.X, (float)vec.Y);
        }

        public static PointD ToPointD(this PointF vec)
        {
            return new PointD(vec.X, vec.Y);
        }

        public static Color ToColor(this ColorF col)
        {
            return Color.FromArgb((int)(col.A * 255), (int)(col.R * 255), (int)(col.G * 255), (int)(col.B * 255));
        }

        public static ColorF ToColorF(this Color col)
        {
            return new ColorF(col.A, col.R, col.G, col.B);
        }

        public static PointF Scale(this PointF point, SizeF scale)
        {
            return new PointF(point.X * scale.Width, point.Y * scale.Height);
        }
    }
}