using UI.WinForms.Extensions;
using OfficeOpenXml;
using ImportWizard.Services;
using UI.WinForms.Framework;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;
using ImportWizard.Dtos;
using OfficeOpenXml.Table;

namespace UI.WinForms
{
    public partial class Main : WizardStep
    {
        private int selectedRowIndex = -1;
        public Main()
        {
            InitializeComponent();

            lbTables.DisplayMember = nameof(ExcelTable.Name);
            lbTables.DeSelectItemOnClick();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtFilePath.LoadAndHook();

            this.NextStep = new PickHeaderStep();
        }

        private void DgvSheets_SelectionChanged(object? sender, EventArgs e)
        {
            SelectedGridRowChanged();
        }

        private DataGridViewRow GetSelectedGridRow()
        {
            if (dgvSheets.SelectedRows.Count == 0)
                return null;

            return dgvSheets.SelectedRows[0];
        }

        private void SelectedGridRowChanged()
        {
            var selectedRow = GetSelectedGridRow();

            lbTables.Items.Clear();

            if (selectedRow == null) return;

            var worksheetDto = selectedRow.DataBoundItem as WorksheetDto;

            if (worksheetDto.NrTables == 0) return;

            foreach (var table in worksheetDto.Tables)
            {
                lbTables.Items.Add(table);
            }

            lbTables.Visible = true;
        }

        private void LoadImportFile()
        {
            var file = txtFilePath.Text;

            if (string.IsNullOrEmpty(file))
            {
                MessageBox.Show("Please supply a valid file.");
                return;
            }

            ExcelService = new ExcelService(file);
            ExcelService.Initialize();
        }

        private void BindSheetsGrid()
        {
            dgvSheets.AllowUserToAddRows = false;
            dgvSheets.AllowUserToDeleteRows = false;
            dgvSheets.AllowUserToOrderColumns = false;
            dgvSheets.AutoGenerateColumns = false;
            dgvSheets.RowHeadersVisible = false;
            dgvSheets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSheets.MultiSelect = false;
            dgvSheets.AllowUserToResizeRows = false;
            dgvSheets.ReadOnly = true;

            dgvSheets.Columns.Clear();

            dgvSheets.AddCheckBoxCol(nameof(WorksheetDto.IsHidden), "Is hidden", 64);

            dgvSheets.AddTextBoxCol(nameof(WorksheetDto.Name), "Sheet name")
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvSheets.AddTextBoxCol(nameof(WorksheetDto.NrTables));

            dgvSheets.DataSource = ExcelService.Worksheets;
            SelectedGridRowChanged();
            dgvSheets.SelectionChanged -= DgvSheets_SelectionChanged;
            dgvSheets.SelectionChanged += DgvSheets_SelectionChanged;
        }

        public override bool CanGoNext()
        {
            if (ExcelService == null)
            {
                MessageBox.Show("Load file first");
                return false;
            }

            if (dgvSheets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a sheet first");
                return false;
            }

            var worksheetDto = dgvSheets.SelectedRows[0].DataBoundItem as WorksheetDto;
            ExcelService.SelectWorksheet(worksheetDto);

            if (lbTables.SelectedItem is ExcelTable table)
            {
                ExcelService.SelectTable(table);

                //go to a different step because the table has already a header
                NextStep = new ColumnSetupStep();
            }
            else
            {
                //there is no table selected, i have to figure out where the header begins
                //next step remains the same
            }

            NextStep.ExcelService = ExcelService;

            return true;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Enabled = false;
            try
            {
                await Task.Run(LoadImportFile);
                BindSheetsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                Enabled = true;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = ".xlsx|*.xlsx|.xls|*.xls|.csv|*.csv";
                fileDialog.RestoreDirectory = true;
                fileDialog.ShowPreview = true;
                fileDialog.ShowHelp = true;
                fileDialog.ShowHiddenFiles = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = fileDialog.FileName;
                }
            }
        }
    }
}
