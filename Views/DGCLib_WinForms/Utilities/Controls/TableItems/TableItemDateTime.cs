using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PotentialMethod_Utilities.Services;

namespace PotentialMethod_Utilities.Controls
{
    public partial class ResultItemDateTime : ResultItem
    {
        DateTimePicker _control;

        public ResultItemDateTime()
        {
            _control = new DateTimePicker();
            _control.Dock = DockStyle.Fill;
            //_control.CustomFormat = GlobalProperties.Instance.DateTimeFormat;
            _control.Format = DateTimePickerFormat.Long;
            _table.Controls.Add(_control, 1, 0);

            _control.Validated += (o, e) =>
            {
                FireValueChanged();

            };


        }

        public string CustomFormat
        {
            get
            {
                return _control.CustomFormat;
            }
            set
            {
                _control.CustomFormat = value;
            }
        }

        public DateTimePickerFormat Format
        {
            get
            {
                return _control.Format;
            }
            set
            {
                _control.Format = value;
                switch (value)
                {
                    case DateTimePickerFormat.Time:
                        _control.ShowUpDown = true;
                        break;
                    default:
                        _control.ShowUpDown = false;
                        break;
                }
            }
        }

        public DateTime Value
        {
            get
            {
                return _control.Value;
            }
            set
            {
                _control.Value = value;
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
