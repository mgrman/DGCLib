
namespace DGCLib_WinForms.Controls
{
    partial class BaseControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._group = new CloseableGroupBox();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._group.SuspendLayout();
            this.SuspendLayout();
            // 
            // _group
            // 
            this._group.AutoSize = true;
            this._group.Controls.Add(this._table);
            this._group.Dock = System.Windows.Forms.DockStyle.Fill;
            this._group.Location = new System.Drawing.Point(0, 0);
            this._group.Name = "_group";
            this._group.Size = new System.Drawing.Size(263, 53);
            this._group.TabIndex = 0;
            this._group.TabStop = false;
            this._group.Text = "<name>";
            // 
            // _table
            // 
            this._table.AutoSize = true;
            this._table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(3, 16);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._table.Size = new System.Drawing.Size(257, 34);
            this._table.TabIndex = 2;
            // 
            // BaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this._group);
            this.Name = "BaseControl";
            this.Size = new System.Drawing.Size(263, 53);
            this._group.ResumeLayout(false);
            this._group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CloseableGroupBox _group;
        private System.Windows.Forms.TableLayoutPanel _table;


    }
}
