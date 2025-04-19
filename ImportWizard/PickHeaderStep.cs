using SpreadsheetGear;
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
    public partial class PickHeaderStep : WizardStep
    {
        public PickHeaderStep()
        {
            InitializeComponent();
        }

        private void PickHeaderStep_Load(object sender, EventArgs e)
        {
            // Sample DataTable
            var table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Age");
            table.Rows.Add("Alice", 30);
            table.Rows.Add("Bob", 25);
            table.Rows.Add("Charlie", 35);

            // Create a new workbook and get the first worksheet
            IWorkbook workbook = Factory.GetWorkbook();
            IWorksheet worksheet = workbook.Worksheets[0];

            // Write column headers
            for (int col = 0; col < table.Columns.Count; col++)
            {
                worksheet.Cells[0, col].Value = table.Columns[col].ColumnName;
            }

            // Write data rows
            for (int row = 0; row < table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    worksheet.Cells[row + 1, col].Value = table.Rows[row][col];
                }
            }

            // Optional: Autofit columns
            worksheet.Cells[worksheet.UsedRange.Row, worksheet.UsedRange.Column,
                            worksheet.UsedRange.Row + worksheet.UsedRange.RowCount - 1,
                            worksheet.UsedRange.Column + worksheet.UsedRange.ColumnCount - 1]
                .Columns.AutoFit();

            // Load the workbook into the control
            workbookView1.ActiveWorkbook = workbook;
        }
    }
}
