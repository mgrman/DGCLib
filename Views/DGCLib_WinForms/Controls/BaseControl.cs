using DGCLib_Presenter;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls

{
    public partial class BaseControl : UserControl
    {
        public IPresentableAlgorithm Algorithm { get; private set; }

        private bool _resizingControl;

        public BaseControl(IPresentableAlgorithm alg)
        {
            InitializeComponent();
            Algorithm = alg;
            _group.Text = alg.Name;
            SetTable();
            _group.CloseButtonClicked += (o, e) => FireCloseButtonClicked();

            Resize += ResizeTable;
            ResizeTable();

            Disposed += (o, e) => Algorithm.Dispose();
        }

        #region AdditionalControl handling

        private void SetTable()
        {
            _table.Controls.Clear();

            if (Algorithm.Settings == null)
                return;

            _table.RowCount = Algorithm.Settings.Count; ;

            foreach (ISettings sett in Algorithm.Settings)
            {
                var con = GetControl(sett);

                _table.Controls.Add(con);
            }

            foreach (RowStyle rowStyle in _table.RowStyles)
            {
                rowStyle.SizeType = SizeType.AutoSize;
            }
        }

        private Control GetControl(ISettings sett)
        {
            TableItem item = null;

            if (sett.ValueType == typeof(string))
            {
                item = new TableItemTextBox();
            }
            if (sett.ValueType == typeof(double))
            {
                item = new TableItemTextNumeric();
            }
            if (sett.ValueType == typeof(double?))
            {
                item = new TableItemTextNumericNull();
            }
            if (sett.ValueType == typeof(bool))
            {
                item = new TableItemCheckbox();
            }
            if (sett.ValueType == typeof(double[]))
            {
                item = new TableItemTextNumericCollection();
            }

            if (sett.ValueType.IsEnum)
            {
                var comboItem = new TableItemComboBox();
                comboItem.Items = Enum.GetValues(sett.ValueType).Cast<object>().ToArray();
                item = comboItem;
            }

            if (item != null)
            {
                item.ValueGeneral = sett.ValueGeneral;
                item.Tag = sett;
                item.ValueChanged += ItemValueChanged;
                item.Text = sett.Name;

                item.Resize += ResizeTable;
            }

            return item;
        }

        private void ItemValueChanged(object sender, EventArgs e)
        {
            var item = sender as TableItem;
            var alg = item.Tag as ISettings;
            alg.ValueGeneral = item.ValueGeneral;
            FireValueChanged();
        }

        private void ResizeTable(object sender, EventArgs e)
        {
            ResizeTable();
        }

        private void ResizeTable()
        {
            if (_resizingControl)
                return;

            _resizingControl = true;

            var conHeight = 20;

            foreach (Control con in _table.Controls)
            {
                con.Width = this.Width - 20;
                conHeight += con.Height + con.Margin.Top + con.Margin.Bottom;
            }
            this.Height = conHeight;

            _resizingControl = false;
        }

        #endregion AdditionalControl handling

        public event EventHandler ValueChanged;

        private void FireValueChanged()
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler CloseButtonClicked;

        private void FireCloseButtonClicked()
        {
            var handler = CloseButtonClicked;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}