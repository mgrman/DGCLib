using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PotentialMethod_Utilities.Controls
{
    public partial class ResultItem : UserControl
    {
        public ResultItem()
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

        public void SetError(string error)
        {
            var con=_table.GetControlFromPosition(1,0);
            if (con != null)
            {
                _errorProvider.SetError(con, error);
            }

        }

    }
}
