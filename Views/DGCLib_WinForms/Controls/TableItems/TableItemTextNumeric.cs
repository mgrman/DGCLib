using System;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class TableItemTextNumeric : TableItem
    {
        private TextBox _text;

        public TableItemTextNumeric()
        {
            _text = new TextBox();
            _text.Dock = DockStyle.Fill;
            _table.Controls.Add(_text, 1, 0);

            _text.TextChanged += (o, e) =>
            {
                if (!_text.ReadOnly)
                {
                    FireValueChanged();
                }
            };

            _text.KeyPress += (o, e) =>
            {
                if (!_text.ReadOnly)
                {
                    FireValueChanged();
                }
            };
            _text.Validating += (o, e) =>
            {
                double pars;
                e.Cancel = !Double.TryParse(_text.Text, out pars);
                ConstrainDecimalPlaces();
            };
            DecimalPlaces = 3;
        }

        private int _decimalPlaces;

        public int DecimalPlaces
        {
            get
            {
                return _decimalPlaces;
            }
            set
            {
                _decimalPlaces = value;
                ConstrainDecimalPlaces();
            }
        }

        public double Value
        {
            get
            {
                double pars;
                return Double.TryParse(_text.Text, out pars) ? pars : double.NaN;
            }
            set
            {
                _text.Text = value.ToString();
            }
        }

        public override object ValueGeneral
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (double)value;
            }
        }

        private void ConstrainDecimalPlaces()
        {
            double pars;
            var value = Double.TryParse(_text.Text, out pars) ? pars : double.NaN;
            _text.Text = Math.Round(value, DecimalPlaces).ToString();
        }

        public bool ReadOnly { get { return _text.ReadOnly; } set { _text.ReadOnly = value; } }
    }
}