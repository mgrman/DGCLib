
namespace PotentialMethod_Utilities.Controls
{
    partial class ResultItem
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
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._label = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider();
            this._table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.ColumnCount = 3;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this._table.Controls.Add(this._label, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 1;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Size = new System.Drawing.Size(219, 28);
            this._table.TabIndex = 0;
            // 
            // _label
            // 
            this._label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._label.AutoEllipsis = true;
            this._label.AutoSize = true;
            this._label.Location = new System.Drawing.Point(3, 7);
            this._label.Margin = new System.Windows.Forms.Padding(3);
            this._label.Name = "_label";
            this._label.Size = new System.Drawing.Size(36, 13);
            this._label.TabIndex = 6;
            this._label.Text = "<text>";
            this._label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // ResultItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._table);
            this.Name = "ResultItem";
            this.Size = new System.Drawing.Size(219, 28);
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Label _label;
        protected System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.ErrorProvider _errorProvider;

    }
}
