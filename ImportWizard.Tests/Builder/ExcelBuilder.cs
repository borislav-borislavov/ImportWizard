using ImportWizard.Tests.Builder.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Tests.Builder
{
    public class ExcelBuilder
    {
        private List<SheetData> _sheets;

        public ExcelBuilder()
        {
            new EPPlusLicense().SetNonCommercialPersonal("ImportWizard");
            _sheets = new List<SheetData>();
        }

        public ExcelBuilder Worksheet(Action<SheetData> action)
        {
            SheetData sheetData = new();
            _sheets.Add(sheetData);
            action(sheetData);
            return this;
        }

        public ExcelPackage Build()
        {
            ExcelPackage package = new ExcelPackage();

            int pos = 0;

            foreach (SheetData sheetData in _sheets)
            {
                pos++;

                if (sheetData.Name == null)
                    sheetData.Name = $"Sheet{pos}";

                var ws = package.Workbook.Worksheets.Add(sheetData.Name);

                for (int r = 0; r < sheetData.Rows.Count; r++)
                {
                    var rowData = sheetData.Rows[r];

                    for (int c = 0; c < rowData.Cells.Count; c++)
                    {
                        var cellData = rowData.Cells[c];

                        ws.Cells[r + 1, c + 1].Value = cellData.Value;
                    }
                }
            }

            return package;
        }
    }
}
