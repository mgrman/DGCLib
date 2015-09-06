namespace DGCLib_WinForms
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._workerPainter = new System.ComponentModel.BackgroundWorker();
            this._mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._flowSettings = new System.Windows.Forms.FlowLayoutPanel();
            this._addAlgorithm = new System.Windows.Forms.Button();
            this._comboAlgTypes = new System.Windows.Forms.ComboBox();
            this._animateCheckbox = new System.Windows.Forms.CheckBox();
            this._forceRecalc = new System.Windows.Forms.Button();
            this._groupAlg = new System.Windows.Forms.GroupBox();
            this._consoleTextbox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._fpsLabel = new System.Windows.Forms.ToolStripLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._buttonClear = new System.Windows.Forms.ToolStripMenuItem();
            this._buttonGenerate = new System.Windows.Forms.ToolStripMenuItem();
            this._buttonSettings = new System.Windows.Forms.ToolStripMenuItem();
            this._saveScreen = new System.Windows.Forms.Button();
            this._saveFileDialogPng = new System.Windows.Forms.SaveFileDialog();
            this._flowAlg = new DGCLib_WinForms.Controls.ExtendedOneColumnFlow();
            this._canvas = new DGCLib_WinForms.Controls.PictureBoxExtended();
            this._mainTable.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this._flowSettings.SuspendLayout();
            this._groupAlg.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // _workerPainter
            // 
            this._workerPainter.WorkerReportsProgress = true;
            this._workerPainter.WorkerSupportsCancellation = true;
            // 
            // _mainTable
            // 
            this._mainTable.ColumnCount = 3;
            this._mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 238F));
            this._mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 261F));
            this._mainTable.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this._mainTable.Controls.Add(this._consoleTextbox, 2, 0);
            this._mainTable.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this._mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainTable.Location = new System.Drawing.Point(0, 24);
            this._mainTable.Name = "_mainTable";
            this._mainTable.RowCount = 1;
            this._mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 615F));
            this._mainTable.Size = new System.Drawing.Size(1124, 615);
            this._mainTable.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this._flowSettings, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._groupAlg, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(232, 609);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // _flowSettings
            // 
            this._flowSettings.AutoSize = true;
            this._flowSettings.Controls.Add(this._addAlgorithm);
            this._flowSettings.Controls.Add(this._comboAlgTypes);
            this._flowSettings.Controls.Add(this._animateCheckbox);
            this._flowSettings.Controls.Add(this._forceRecalc);
            this._flowSettings.Controls.Add(this._saveScreen);
            this._flowSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flowSettings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._flowSettings.Location = new System.Drawing.Point(3, 3);
            this._flowSettings.Name = "_flowSettings";
            this._flowSettings.Size = new System.Drawing.Size(226, 153);
            this._flowSettings.TabIndex = 2;
            // 
            // _addAlgorithm
            // 
            this._addAlgorithm.Location = new System.Drawing.Point(0, 0);
            this._addAlgorithm.Margin = new System.Windows.Forms.Padding(0);
            this._addAlgorithm.Name = "_addAlgorithm";
            this._addAlgorithm.Size = new System.Drawing.Size(60, 30);
            this._addAlgorithm.TabIndex = 7;
            this._addAlgorithm.Text = "Add";
            this._addAlgorithm.UseVisualStyleBackColor = true;
            // 
            // _comboAlgTypes
            // 
            this._comboAlgTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboAlgTypes.FormattingEnabled = true;
            this._comboAlgTypes.Location = new System.Drawing.Point(3, 33);
            this._comboAlgTypes.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this._comboAlgTypes.Name = "_comboAlgTypes";
            this._comboAlgTypes.Size = new System.Drawing.Size(160, 21);
            this._comboAlgTypes.TabIndex = 8;
            // 
            // _animateCheckbox
            // 
            this._animateCheckbox.Location = new System.Drawing.Point(3, 67);
            this._animateCheckbox.Name = "_animateCheckbox";
            this._animateCheckbox.Size = new System.Drawing.Size(105, 23);
            this._animateCheckbox.TabIndex = 6;
            this._animateCheckbox.Text = "Animate";
            this._animateCheckbox.UseVisualStyleBackColor = true;
            this._animateCheckbox.Visible = false;
            // 
            // _forceRecalc
            // 
            this._forceRecalc.Location = new System.Drawing.Point(0, 93);
            this._forceRecalc.Margin = new System.Windows.Forms.Padding(0);
            this._forceRecalc.Name = "_forceRecalc";
            this._forceRecalc.Size = new System.Drawing.Size(163, 30);
            this._forceRecalc.TabIndex = 7;
            this._forceRecalc.Text = "Force recalculation";
            this._forceRecalc.UseVisualStyleBackColor = true;
            // 
            // _groupAlg
            // 
            this._groupAlg.Controls.Add(this._flowAlg);
            this._groupAlg.Dock = System.Windows.Forms.DockStyle.Fill;
            this._groupAlg.Location = new System.Drawing.Point(3, 162);
            this._groupAlg.Name = "_groupAlg";
            this._groupAlg.Size = new System.Drawing.Size(226, 444);
            this._groupAlg.TabIndex = 3;
            this._groupAlg.TabStop = false;
            this._groupAlg.Text = "Algorithms";
            // 
            // _consoleTextbox
            // 
            this._consoleTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._consoleTextbox.Location = new System.Drawing.Point(866, 3);
            this._consoleTextbox.Multiline = true;
            this._consoleTextbox.Name = "_consoleTextbox";
            this._consoleTextbox.ReadOnly = true;
            this._consoleTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._consoleTextbox.Size = new System.Drawing.Size(255, 609);
            this._consoleTextbox.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this._canvas, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this._toolStrip, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(241, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(619, 609);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // _toolStrip
            // 
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fpsLabel});
            this._toolStrip.Location = new System.Drawing.Point(0, 584);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(619, 25);
            this._toolStrip.TabIndex = 3;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _fpsLabel
            // 
            this._fpsLabel.Name = "_fpsLabel";
            this._fpsLabel.Size = new System.Drawing.Size(78, 22);
            this._fpsLabel.Tag = "Fps : {0}";
            this._fpsLabel.Text = "Fps : <value>";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this._buttonSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1124, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // _exit
            // 
            this._exit.Name = "_exit";
            this._exit.Size = new System.Drawing.Size(92, 22);
            this._exit.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonClear,
            this._buttonGenerate});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // _buttonClear
            // 
            this._buttonClear.Name = "_buttonClear";
            this._buttonClear.Size = new System.Drawing.Size(157, 22);
            this._buttonClear.Text = "Clear";
            // 
            // _buttonGenerate
            // 
            this._buttonGenerate.Name = "_buttonGenerate";
            this._buttonGenerate.Size = new System.Drawing.Size(157, 22);
            this._buttonGenerate.Text = "Generate Points";
            // 
            // _buttonSettings
            // 
            this._buttonSettings.Name = "_buttonSettings";
            this._buttonSettings.Size = new System.Drawing.Size(61, 20);
            this._buttonSettings.Text = "Settings";
            // 
            // _saveScreen
            // 
            this._saveScreen.Location = new System.Drawing.Point(0, 123);
            this._saveScreen.Margin = new System.Windows.Forms.Padding(0);
            this._saveScreen.Name = "_saveScreen";
            this._saveScreen.Size = new System.Drawing.Size(163, 30);
            this._saveScreen.TabIndex = 9;
            this._saveScreen.Text = "Save screenshoot";
            this._saveScreen.UseVisualStyleBackColor = true;
            // 
            // _saveFileDialogPng
            // 
            this._saveFileDialogPng.Filter = "Png image|*.png";
            // 
            // _flowAlg
            // 
            this._flowAlg.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flowAlg.Location = new System.Drawing.Point(3, 16);
            this._flowAlg.Name = "_flowAlg";
            this._flowAlg.Size = new System.Drawing.Size(220, 425);
            this._flowAlg.TabIndex = 0;
            // 
            // _canvas
            // 
            this._canvas.BackgroundColor = System.Drawing.Color.White;
            this._canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this._canvas.Location = new System.Drawing.Point(3, 3);
            this._canvas.Name = "_canvas";
            this._canvas.Size = new System.Drawing.Size(613, 578);
            this._canvas.TabIndex = 2;
            this._canvas.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 639);
            this.Controls.Add(this._mainTable);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "DGCLib";
            this._mainTable.ResumeLayout(false);
            this._mainTable.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this._flowSettings.ResumeLayout(false);
            this._groupAlg.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker _workerPainter;
        private System.Windows.Forms.TableLayoutPanel _mainTable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel _flowSettings;
        private System.Windows.Forms.Button _addAlgorithm;
        private System.Windows.Forms.ComboBox _comboAlgTypes;
        private System.Windows.Forms.CheckBox _animateCheckbox;
        private System.Windows.Forms.GroupBox _groupAlg;
        private DGCLib_WinForms.Controls.ExtendedOneColumnFlow _flowAlg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _exit;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _buttonClear;
        private System.Windows.Forms.ToolStripMenuItem _buttonGenerate;
        private System.Windows.Forms.ToolStripMenuItem _buttonSettings;
        private System.Windows.Forms.TextBox _consoleTextbox;
        private System.Windows.Forms.Button _forceRecalc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DGCLib_WinForms.Controls.PictureBoxExtended _canvas;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripLabel _fpsLabel;
        private System.Windows.Forms.Button _saveScreen;
        private System.Windows.Forms.SaveFileDialog _saveFileDialogPng;
    }
}

