using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Tests.Builder.Models
{
    public class SheetData
    {
        public string Name { get; set; }

        public List<RowData> Rows { get; set; } = new List<RowData>();

        public SheetData AddRow(List<CellData> cells)
        {
            Rows.Add(cells);

            return this;
        }
    }
}
