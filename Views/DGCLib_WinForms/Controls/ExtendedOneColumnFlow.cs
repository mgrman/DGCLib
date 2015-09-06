using System;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public class ExtendedOneColumnFlow : FlowLayoutPanel
    {
        public ExtendedOneColumnFlow()
        {
            this.Resize += FlowResized;

            ControlAdded += SetSizeOfAddedControl;

            FlowResized(this, null);
        }

        private void SetSizeOfAddedControl(object sender, ControlEventArgs e)
        {
            SetFlowSize(e.Control, this);
        }

        private void FlowResized(object sender, EventArgs e)
        {
            this.SuspendLayout();
            var flow = sender as FlowLayoutPanel;
            //Debug.WriteLine("Flow : {0}", flow.Width);
            foreach (Control c in flow.Controls)
            {
                SetFlowSize(c, flow);

                //flow.SetFlowBreak(c, true);
            }

            //HACK to initialize reflow of controls because scrollbars can be set according old widths
            //flow.FlowDirection = FlowDirection.LeftToRight;
            flow.FlowDirection = FlowDirection.TopDown;
            flow.FlowDirection = FlowDirection.LeftToRight;
            this.ResumeLayout();
        }

        private void SetFlowSize(Control c, FlowLayoutPanel flow)
        {
            c.Width = flow.Width - 1 - c.Margin.Left - c.Margin.Right - (flow.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0);
            c.PerformLayout();
        }
    }
}