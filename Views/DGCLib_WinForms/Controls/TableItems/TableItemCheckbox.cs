using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class TableItemCheckbox : TableItem
    {
        private CheckBox _checkbox;

        public TableItemCheckbox()
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

        public override object ValueGeneral
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (bool)value;
            }
        }

        public bool ReadOnly { get { return _checkbox.Enabled; } set { _checkbox.Enabled = value; } }
    }
}