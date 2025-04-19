using UI.WinForms.Extensions;
using OfficeOpenXml;
using ImportWizard.Services;
using UI.WinForms.Framework;
using System.Reflection.Metadata.Ecma335;

namespace UI.WinForms
{
    public partial class Main : WizardStep
    {
        private ExcelService _excelService;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.NextStep = new PickHeaderStep();
        }

        private void LoadImportFile()
        {
            var file = txtFilePath.Text;

            if (string.IsNullOrEmpty(file))
            {
                MessageBox.Show("Please supply a valid file.");
                return;
            }

            _excelService = new ExcelService(file);

            _excelService.Initialize();

            BindSheetsGrid();
        }

        private void txtFilePath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadImportFile();
            }
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

            dgvSheets.AddCheckBoxCol(nameof(ExcelWorksheet.Hidden), "Hidden", 56);
            dgvSheets.AddTextBoxCol(nameof(ExcelWorksheet.Name), "Sheet name")
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvSheets.DataSource = _excelService.Worksheets;
        }

        public override bool CanGoNext()
        {
            if (_excelService == null)
            {
                MessageBox.Show("Load file first");
                return false;
            }


            NextStep.ExcelService = _excelService;

            return true;
        }
    }
}
