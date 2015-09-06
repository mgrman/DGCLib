using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class TableItemComboBox : TableItem
    {
        private ComboBox _comboBox;

        public TableItemComboBox()
        {
            _comboBox = new ComboBox();
            _comboBox.Dock = DockStyle.Fill;
            _comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _table.Controls.Add(_comboBox, 1, 0);

            _comboBox.SelectedIndexChanged += (o, e) =>
            {
                FireValueChanged();
            };
        }

        public object Value { get { return _comboBox.SelectedItem; } set { _comboBox.SelectedItem = value; } }

        public override object ValueGeneral
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

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
    }
}