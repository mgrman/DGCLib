using DGCLib_WinForms.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class TableItemTextNumericCollection : TableItem
    {
        private List<TextBox> _texts = new List<TextBox>();
        private List<Button> _removeButtons = new List<Button>();

        private Button _addButton;

        private Bitmap _removeImage;

        public TableItemTextNumericCollection()
        {
            _addButton = new Button();
            _addButton.FlatStyle = FlatStyle.Flat;
            _addButton.Text = "Add another";
            _table.Controls.Add(_addButton, 1, 0);
            _addButton.Click += (o, e) =>
            {
                this.SuspendDrawing();
                CreateTextBox((0.1).ToString());
                this.ResumeDrawing();
                FireValueChanged();
            };
            _table.RowStyles[0].SizeType = SizeType.Absolute;
            _table.RowStyles[0].Height = _addButton.Height + _addButton.Margin.Top + _addButton.Margin.Bottom;

            //this.AutoSize = true;
            //this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            //_table.AutoSize = true;
            //_table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            _removeImage = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(_removeImage))
            {
                g.Clear(Color.Transparent);

                g.FillRectangle(new SolidBrush(Color.DarkRed), 0, 6, 16, 4);
            }
        }

        private void CreateTextBox(string value)
        {
            var tableRowIndex = _texts.Count + 1;

            var text = new TextBox();
            text.Dock = DockStyle.Fill;
            text.Text = value;
            _table.Controls.Add(text, 0, tableRowIndex);
            _table.SetColumnSpan(text, 2);
            _texts.Add(text);

            text.Validating += (o, e) =>
              {
                  double pars;
                  if (!double.TryParse((o as TextBox).Text, out pars))
                      (o as TextBox).Text = "";
              };

            text.Validated += ReactToTextChanged;
            text.KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    ReactToTextChanged(o, e);
            };

            var but = new Button();
            but.Image = _removeImage;
            but.TextImageRelation = TextImageRelation.ImageBeforeText;
            but.ForeColor = Color.Transparent;
            but.FlatStyle = FlatStyle.Flat;
            but.Dock = DockStyle.Fill;
            but.Width = but.Height;
            _table.Controls.Add(but, 2, tableRowIndex);
            _removeButtons.Add(but);
            but.Click += (o, e) =>
            {
                RemoveItemWithSuspending(_table.GetCellPosition(o as Control).Row - 1);
                this.ResumeDrawing();
            };

            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, text.Height + text.Margin.Top + text.Margin.Bottom));
            _table.ColumnStyles[2].SizeType = SizeType.AutoSize;

            AutosizeHeight();
        }

        private void ReactToTextChanged(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            if (!txt.ReadOnly)
            {
                FireValueChanged();
            }
        }

        public void RemoveItemWithSuspending(int index)
        {
            Suspend();

            RemoveItem(index);

            Resume();
        }

        private void Suspend()
        {
            this.SuspendDrawing();
            _table.SuspendLayout();
        }

        private void Resume()
        {
            AutosizeHeight();
            _table.ResumeLayout();
            this.ResumeDrawing();
            Refresh();
            if (Parent != null)
                Parent.Refresh();
        }

        public void RemoveItem(int index)
        {
            _table.Controls.Remove(_texts[index]);
            _table.Controls.Remove(_removeButtons[index]);

            for (int i = index; i < _texts.Count - 1; i++)
            {
                _table.SetCellPosition(_texts[i + 1], new TableLayoutPanelCellPosition(0, i + 1));
                _table.SetCellPosition(_removeButtons[i + 1], new TableLayoutPanelCellPosition(2, i + 1));
            }
            _table.RowStyles.RemoveAt(index + 1);

            _texts.RemoveAt(index);
            _removeButtons.RemoveAt(index);

            FireValueChanged();
        }

        private void AutosizeHeight()
        {
            this.Height = (int)_table.RowStyles.Cast<RowStyle>().Sum(o => o.Height);
        }

        public double[] Value
        {
            get
            {
                double pars;
                return _texts.Select(o => double.TryParse(o.Text, out pars) ? pars : double.NaN).Where(o => !double.IsNaN(o)).ToArray();
            }
            set
            {
                Suspend();
                for (int i = _texts.Count - 1; i >= 0; i--)
                    RemoveItem(i);
                /*
                    foreach (var text in _texts)
                        _table.Controls.Remove(text);
                foreach (var but in _removeButtons)
                    _table.Controls.Remove(but);
                _texts.Clear();
                _removeButtons.Clear();*/

                if (value != null)
                {
                    foreach (var item in value)
                    {
                        CreateTextBox(item.ToString());
                    }
                }
                Resume();
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
                Value = value as double[];
            }
        }
    }
}