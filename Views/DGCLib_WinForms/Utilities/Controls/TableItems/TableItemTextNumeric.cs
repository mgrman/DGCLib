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
    public partial class ResultItemTextNumeric : ResultItem
    {
        TextBox _text;

        public ResultItemTextNumeric()
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
                double pars;
                e.Cancel = !Double.TryParse(_text.Text, out pars);
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

        public double Value
        {
            get
            {
                double pars;
                return Double.TryParse( _text.Text,out pars)?pars:double.NaN;
            }
            set
            {
                _text.Text = value.ToString();
            }
        }


        private void ConstrainDecimalPlaces()
        {

            double pars;
            var value= Double.TryParse(_text.Text, out pars) ? pars : double.NaN;
            _text.Text = Math.Round(value, DecimalPlaces).ToString();

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
