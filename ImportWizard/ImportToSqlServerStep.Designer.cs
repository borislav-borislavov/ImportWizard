namespace UI.WinForms
{
    partial class ImportToSqlServerStep
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
            label1 = new Label();
            txtConnectionString = new TextBox();
            btnImport = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 44);
            label1.Name = "label1";
            label1.Size = new Size(102, 15);
            label1.TabIndex = 8;
            label1.Text = "Connection string";
            // 
            // txtConnectionString
            // 
            txtConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtConnectionString.Location = new Point(120, 41);
            txtConnectionString.Name = "txtConnectionString";
            txtConnectionString.Size = new Size(668, 23);
            txtConnectionString.TabIndex = 9;
            // 
            // btnImport
            // 
            btnImport.Location = new Point(12, 76);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(75, 23);
            btnImport.TabIndex = 10;
            btnImport.Text = "Import";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // ImportToSqlServerStep
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnImport);
            Controls.Add(txtConnectionString);
            Controls.Add(label1);
            Name = "ImportToSqlServerStep";
            Text = "ImportToSqlServerStep";
            Load += ImportToSqlServerStep_Load;
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(txtConnectionString, 0);
            Controls.SetChildIndex(btnImport, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtConnectionString;
        private Button btnImport;
    }
}