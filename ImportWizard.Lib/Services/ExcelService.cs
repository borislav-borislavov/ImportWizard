using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Services
{
    public class ExcelService
    {
        /// <summary>
        /// The excel file path
        /// </summary>
        public string FilePath { get; }

        private ExcelPackage package = null;
        private ExcelWorkbook workbook = null;

        public List<ExcelWorksheet> Worksheets { get; } = new List<ExcelWorksheet>();

        public ExcelService(string filePath)
        {
            FilePath = filePath;

            new EPPlusLicense().SetNonCommercialPersonal("ImportWizard");
        }

        public void Initialize()
        {
            package = new ExcelPackage(FilePath);
            workbook = package.Workbook;

            foreach (var worksheet in workbook.Worksheets)
            {
                Worksheets.Add(worksheet);
            }

            var debug = 123;
        }
    }
}
