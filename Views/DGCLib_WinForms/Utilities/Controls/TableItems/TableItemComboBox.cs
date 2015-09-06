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
    public partial class ResultItemComboBox : ResultItem
    {
        ComboBox _comboBox;

        public ResultItemComboBox()
        {

            _comboBox = new ComboBox();
            _comboBox.Dock = DockStyle.Fill;
            _comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _table.Controls.Add(_comboBox, 1, 0);

            _comboBox.SelectedIndexChanged += (o, e) =>
            {
                FireComboBoxItemChanged();
                
            };
        }

        public object Value { get { return _comboBox.SelectedItem; } set { _comboBox.SelectedItem = value; } }

        [Browsable(false)]
        public object[] Items
        {
            get { return _comboBox.Items.Cast<object>().ToArray(); }
            set
            {
                _comboBox.Items.Clear();
                if (value != null) _comboBox.Items.AddRange(value);
            }
        }

        public string[] ItemsString
        {
            get { return _comboBox.Items.Cast<string>().ToArray(); }
            set
            {
                _comboBox.Items.Clear();
                if (value != null) _comboBox.Items.AddRange(value);
            }
        }

        public event EventHandler ComboBoxItemChanged;

        protected virtual void FireComboBoxItemChanged()
        {
            EventHandler handler = ComboBoxItemChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
