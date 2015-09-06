using System;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class TableItemTextNumericNull : TableItem
    {
        private TextBox _text;

        public TableItemTextNumericNull()
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

            _text.Validating += (o, e) =>
            {
                ConstrainDecimalPlaces();
            };
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

        public double? Value
        {
            get
            {
                double pars;
                return Double.TryParse(_text.Text, out pars) ? pars : (double?)null;
            }
            set
            {
                if (value != null)
                    _text.Text = value.ToString();
                else
                    _text.Text = "";
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
                Value = (double?)value;
            }
        }

        private void ConstrainDecimalPlaces()
        {
            double pars;
            var value = Double.TryParse(_text.Text, out pars) ? pars : (double?)null;

            if (value.HasValue)
                _text.Text = Math.Round(value.Value, DecimalPlaces).ToString();
            else
                _text.Text = "";
        }

        public bool ReadOnly { get { return _text.ReadOnly; } set { _text.ReadOnly = value; } }
    }
}