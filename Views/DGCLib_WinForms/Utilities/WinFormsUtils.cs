using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DGCLib_WinForms.Utilities
{
    public static class WinFormsUtils
    {
        public static void SetDoubleBuffered(Control c)
        {
            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        public static Point TransformPoint(this Matrix mat, Point p)
        {
            var arr = new Point[] { p };
            mat.TransformPoints(arr);
            return arr[0];
        }

        public static PointF TransformPoint(this Matrix mat, PointF p)
        {
            var arr = new PointF[] { p };
            mat.TransformPoints(arr);
            return arr[0];
        }
    }
}