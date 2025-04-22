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
            ((System.ComponentModel.ISupportInitialize)dgvSheets).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvSheets
            // 
            dgvSheets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSheets.Dock = DockStyle.Fill;
            dgvSheets.Location = new Point(0, 21);
            dgvSheets.Margin = new Padding(0);
            dgvSheets.Name = "dgvSheets";
            dgvSheets.Size = new Size(190, 193);
            dgvSheets.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dgvSheets, 0, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Location = new Point(12, 136);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(190, 214);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(67, 0);
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
            // 
            // txtFilePath
            // 
            txtFilePath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtFilePath.Location = new Point(6, 18);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(689, 23);
            txtFilePath.TabIndex = 5;
            txtFilePath.Text = "C:\\Users\\bobi\\Desktop\\100mb.xlsx";
            txtFilePath.KeyDown += txtFilePath_KeyDown;
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
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Controls.Add(tableLayoutPanel1);
            Name = "Main";
            Text = "ImportWizard";
            Load += Main_Load;
            Controls.SetChildIndex(tableLayoutPanel1, 0);
            Controls.SetChildIndex(groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)dgvSheets).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
    }
}
