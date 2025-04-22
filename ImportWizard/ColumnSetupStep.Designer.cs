namespace UI.WinForms
{
    partial class ColumnSetupStep
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
            dgvColumnPreferences = new DataGridView();
            label1 = new Label();
            txtDestinationTableName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvColumnPreferences).BeginInit();
            SuspendLayout();
            // 
            // dgvColumnPreferences
            // 
            dgvColumnPreferences.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvColumnPreferences.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvColumnPreferences.Location = new Point(12, 74);
            dgvColumnPreferences.Name = "dgvColumnPreferences";
            dgvColumnPreferences.Size = new Size(776, 351);
            dgvColumnPreferences.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 51);
            label1.Name = "label1";
            label1.Size = new Size(129, 15);
            label1.TabIndex = 9;
            label1.Text = "Destination table name";
            // 
            // txtDestinationTableName
            // 
            txtDestinationTableName.Location = new Point(147, 48);
            txtDestinationTableName.Name = "txtDestinationTableName";
            txtDestinationTableName.Size = new Size(210, 23);
            txtDestinationTableName.TabIndex = 10;
            // 
            // ColumnSetupStep
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtDestinationTableName);
            Controls.Add(label1);
            Controls.Add(dgvColumnPreferences);
            Name = "ColumnSetupStep";
            Text = "ColumnSetupStep";
            Load += ColumnSetupStep_Load;
            Controls.SetChildIndex(dgvColumnPreferences, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(txtDestinationTableName, 0);
            ((System.ComponentModel.ISupportInitialize)dgvColumnPreferences).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvColumnPreferences;
        private Label label1;
        private TextBox txtDestinationTableName;
    }
}