using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class TableItemTextBox : TableItem
    {
        private TextBox _text;

        public TableItemTextBox()
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
        }

        public string Value { get { return _text.Text; } set { _text.Text = value; } }

        public override object ValueGeneral
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (string)value;
            }
        }

        public bool ReadOnly { get { return _text.ReadOnly; } set { _text.ReadOnly = value; } }
    }
}