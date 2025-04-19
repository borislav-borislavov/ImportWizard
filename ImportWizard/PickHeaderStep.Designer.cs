namespace UI.WinForms
{
    partial class PickHeaderStep
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickHeaderStep));
            workbookView1 = new SpreadsheetGear.Windows.Forms.WorkbookView();
            SuspendLayout();
            // 
            // workbookView1
            // 
            workbookView1.FormulaBar = null;
            workbookView1.Location = new Point(12, 143);
            workbookView1.Name = "workbookView1";
            workbookView1.Size = new Size(776, 282);
            workbookView1.TabIndex = 8;
            workbookView1.WorkbookSetState = resources.GetString("workbookView1.WorkbookSetState");
            // 
            // PickHeaderStep
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(workbookView1);
            Name = "PickHeaderStep";
            Text = "PickHeaderStep";
            Load += PickHeaderStep_Load;
            Controls.SetChildIndex(workbookView1, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SpreadsheetGear.Windows.Forms.WorkbookView workbookView1;
    }
}