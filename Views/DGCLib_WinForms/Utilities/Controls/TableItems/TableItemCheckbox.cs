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
    public partial class ResultItemCheckbox : ResultItem
    {
        CheckBox _checkbox;

        public ResultItemCheckbox()
        {
            _checkbox = new CheckBox();
            _checkbox.Dock = DockStyle.Fill;
            _table.Controls.Add(_checkbox, 1, 0);

            _checkbox.CheckedChanged += (o, e) =>
            {
                if (_checkbox.Enabled)
                {
                    FireValueChanged();
                }
            };
        }

        public bool Value { get { return _checkbox.Checked; } set { _checkbox.Checked = value; } }
        public bool ReadOnly { get { return _checkbox.Enabled; } set { _checkbox.Enabled = value; } }

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
