using ImportWizard.Models;
using ImportWizard.Services;
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
    public partial class ColumnSetupStep : WizardStep
    {
        public ColumnSetupStep()
        {
            InitializeComponent();

            dgvColumnPreferences.AllowUserToAddRows = false;
            dgvColumnPreferences.AllowUserToDeleteRows = false;
            dgvColumnPreferences.AllowUserToOrderColumns = false;
            //dgvColumnPreferences.AutoGenerateColumns = false;
            dgvColumnPreferences.RowHeadersVisible = false;
            dgvColumnPreferences.MultiSelect = false;

            dgvColumnPreferences.CellValueChanged += DgvColumnPreferences_CellValueChanged;
        }

        private void DgvColumnPreferences_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            ExcelService.ResetImportData();
        }

        private void ColumnSetupStep_Load(object sender, EventArgs e)
        {
            this.NextStep = new ImportToSqlServerStep();

            if (ExcelService.ColumnPreferences.Count == 0)
                ExcelService.LoadColumnPreferences();

            dgvColumnPreferences.DataSource = ExcelService.ColumnPreferences;

            if (ExcelService.SelectedTable != null)
            {
                txtDestinationTableName.Text = ExcelService.SelectedTable.Name;
            }
            else
            {
                txtDestinationTableName.Text = ExcelService.SelectedWorksheet.Name;
            }
        }

        public override void OnReturn()
        {
            if (ExcelService.ColumnPreferences.Count == 0)
                ExcelService.LoadColumnPreferences();
        }

        public override bool CanGoNext()
        {
            if (string.IsNullOrEmpty(txtDestinationTableName.Text))
            {
                MessageBox.Show("Please specify TableName");
                return false;
            }

            ExcelService.TableName = txtDestinationTableName.Text;

            NextStep.ExcelService = ExcelService;

            return base.CanGoNext();
        }
    }
}
