using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public abstract partial class TableItem : UserControl
    {
        public TableItem()
        {
            InitializeComponent();
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
            }
        }

        public bool IsDynamic { get; set; }

        public abstract object ValueGeneral { get; set; }

        public void SetError(string error)
        {
            var con = _table.GetControlFromPosition(1, 0);
            if (con != null)
            {
                _errorProvider.SetError(con, error);
            }
        }

        public event EventHandler ValueChanged;

        protected virtual void FireValueChanged()
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}