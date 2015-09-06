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
    public partial class ResultItemTextNumericNull : ResultItem
    {
        TextBox _text;

        public ResultItemTextNumericNull()
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
                return Double.TryParse( _text.Text,out pars)?pars:(double?)null;
            }
            set
            {
                if (value != null)
                    _text.Text = value.ToString();
                else
                    _text.Text = "";
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
