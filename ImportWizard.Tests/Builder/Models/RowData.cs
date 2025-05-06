using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Tests.Builder.Models
{
    public class RowData
    {
        public static implicit operator RowData(List<CellData> cells)
        {
            return new RowData
            {
                Cells = cells
            };
        }

        public List<CellData> Cells { get; set; }
    }
}
