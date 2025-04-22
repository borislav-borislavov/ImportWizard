using OfficeOpenXml;
using OfficeOpenXml.Style;
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
            this.NextStep = new ColumnSetupStep();

            // Create a new workbook and get the first worksheet
            IWorkbook workbook = Factory.GetWorkbook();
            IWorksheet newWorksheet = workbook.Worksheets[0];

            var fileWorksheet = ExcelService.SelectedWorksheet;

            foreach (ExcelRangeColumn xCol in fileWorksheet.Columns)
            {
                if (xCol.Hidden)
                {
                    xCol.Hidden = false;
                }
            }

            //var dimension = fileWorksheet.Dimension;
            //int startColumn = dimension.Start.Column;
            //int endColumn = dimension.End.Column;
            //var startRow = 1;

            //for (int rowNum = startRow; rowNum <= fileWorksheet.Dimension.End.Row; rowNum++)
            //{
            //    for (int columnIndex = startColumn; columnIndex <= endColumn; columnIndex++)
            //    {
            //        //TODO test how correct this is, also .Text might be more suitable
            //        var sourceCell = fileWorksheet.Cells[rowNum, columnIndex];
            //        var destinationCell = newWorksheet.Cells[rowNum - 1, columnIndex - 1];

            //        destinationCell.Formula = sourceCell.Formula;
            //        destinationCell.Style.NumberFormat = sourceCell.Style.Numberformat.Format;

            //        destinationCell.Value = sourceCell.Value;
            //    }
            //}

            //// Load the workbook into the control
            //workbookView1.ActiveWorkbook = workbook;
        }

        public override bool CanGoNext()
        {
            ExcelService.HeaderRowNumber = int.Parse(textBox1.Text);
            NextStep.ExcelService = ExcelService;

            return base.CanGoNext();
        }
    }
}
