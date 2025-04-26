namespace UI.WinForms
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvSheets = new DataGridView();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            btnBrowse = new Button();
            txtFilePath = new TextBox();
            groupBox1 = new GroupBox();
            btnLoad = new Button();
            lbTables = new ListBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvSheets).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // dgvSheets
            // 
            dgvSheets.BackgroundColor = Color.White;
            dgvSheets.BorderStyle = BorderStyle.None;
            dgvSheets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSheets.Dock = DockStyle.Fill;
            dgvSheets.Location = new Point(1, 23);
            dgvSheets.Margin = new Padding(0);
            dgvSheets.Name = "dgvSheets";
            dgvSheets.Size = new Size(365, 219);
            dgvSheets.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dgvSheets, 0, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Location = new Point(12, 105);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(367, 243);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(155, 1);
            label1.Name = "label1";
            label1.Size = new Size(56, 21);
            label1.TabIndex = 1;
            label1.Text = "Sheets";
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnBrowse.Location = new Point(698, 17);
            btnBrowse.Margin = new Padding(0);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 25);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // txtFilePath
            // 
            txtFilePath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtFilePath.Location = new Point(6, 18);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(689, 23);
            txtFilePath.TabIndex = 5;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(txtFilePath);
            groupBox1.Controls.Add(btnBrowse);
            groupBox1.Location = new Point(12, 41);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 47);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Browse file";
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(710, 94);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 8;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // lbTables
            // 
            lbTables.BackColor = Color.White;
            lbTables.BorderStyle = BorderStyle.None;
            lbTables.Dock = DockStyle.Fill;
            lbTables.FormattingEnabled = true;
            lbTables.IntegralHeight = false;
            lbTables.ItemHeight = 15;
            lbTables.Location = new Point(1, 23);
            lbTables.Margin = new Padding(0);
            lbTables.Name = "lbTables";
            lbTables.Size = new Size(227, 219);
            lbTables.TabIndex = 9;
            lbTables.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(lbTables, 0, 1);
            tableLayoutPanel2.Controls.Add(label2, 0, 0);
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            tableLayoutPanel2.Location = new Point(385, 105);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(229, 243);
            tableLayoutPanel2.TabIndex = 10;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(88, 1);
            label2.Name = "label2";
            label2.Size = new Size(52, 21);
            label2.TabIndex = 10;
            label2.Text = "Tables";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(btnLoad);
            Controls.Add(groupBox1);
            Controls.Add(tableLayoutPanel1);
            Name = "Main";
            Text = "ImportWizard";
            Load += Main_Load;
            Controls.SetChildIndex(tableLayoutPanel1, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(btnLoad, 0);
            Controls.SetChildIndex(tableLayoutPanel2, 0);
            ((System.ComponentModel.ISupportInitialize)dgvSheets).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvSheets;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Button btnBrowse;
        private TextBox txtFilePath;
        private GroupBox groupBox1;
        private Button btnLoad;
        private ListBox lbTables;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label2;
    }
}
