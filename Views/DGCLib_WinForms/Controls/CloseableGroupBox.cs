using System;
using System.Drawing;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class CloseableGroupBox : GroupBox
    {
        private Rectangle _closeButtonRectangle;

        public CloseableGroupBox()
        {
        }

        protected override void OnClick(EventArgs e)
        {
            if (_closeButtonRectangle.Contains(this.PointToClient(MousePosition)))
                FireCloseButtonClicked();

            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var pen = new Pen(SystemColors.ControlDark, 1);
            var brush = new SolidBrush(this.BackColor);
            var size = new Size(15, 13);
            var padding = new Size(2, 2);

            var rect = new Rectangle(e.ClipRectangle.Right - size.Width - padding.Width, e.ClipRectangle.Top + padding.Height, size.Width, size.Height);

            e.Graphics.FillRectangle(brush, rect);
            e.Graphics.DrawRectangle(pen, rect);

            e.Graphics.DrawLine(pen, new Point(rect.Left, rect.Top), new Point(rect.Right, rect.Bottom));
            e.Graphics.DrawLine(pen, new Point(rect.Right, rect.Top), new Point(rect.Left, rect.Bottom));

            _closeButtonRectangle = rect;
        }

        public event EventHandler CloseButtonClicked;

        private void FireCloseButtonClicked()
        {
            var handler = CloseButtonClicked;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}