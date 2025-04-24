using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Dtos
{
    /// <summary>
    /// A lighweight representation of a worksheet.
    /// </summary>
    public class WorksheetDto
    {
        public string Name { get; set; }
        public bool IsHidden { get; set; }
        public int NrTables { get; set; }
        public int Index { get; set; }
        public ExcelTableCollection Tables { get; internal set; }
    }
}
