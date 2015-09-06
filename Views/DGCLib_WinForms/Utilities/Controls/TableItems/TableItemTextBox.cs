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
    public partial class ResultItemTextBox : ResultItem
    {
        TextBox _text;

        public ResultItemTextBox()
        {
            _text = new TextBox();
            _text.Dock = DockStyle.Fill;
            _table.Controls.Add(_text, 1, 0);
            
            _text.Validated += (o, e) =>
            {
                if (!_text.ReadOnly)
                {
                    FireValueChanged();
                }
            };
        }

        public string Value { get { return _text.Text; } set { _text.Text = value; } }
        public bool ReadOnly { get { return _text.ReadOnly; } set { _text.ReadOnly = value; } }

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
