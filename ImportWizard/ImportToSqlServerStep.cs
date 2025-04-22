using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.WinForms.Framework;

namespace UI.WinForms
{
    public partial class ImportToSqlServerStep : WizardStep
    {
        public ImportToSqlServerStep()
        {
            InitializeComponent();
        }

        private void ImportToSqlServerStep_Load(object sender, EventArgs e)
        {
            txtConnectionString.LoadAndHook();
        }

        public override bool CanGoNext()
        {
            return base.CanGoNext();
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtConnectionString.Text))
                {
                    MessageBox.Show("Please specify Connection String");
                    return;
                }

                ExcelService.ConnectionString = txtConnectionString.Text;

                await ExcelService.Import();

                MessageBox.Show("Import completed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
