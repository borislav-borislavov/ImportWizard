using ImportWizard.Tests.Builder.Models;
using ImportWizard.Tests.Builder;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Tests.ServiceTests
{
    public class ExcelBuilderTests
    {
        [Fact]
        public void Build_CreatesValidExcelWithExpectedData()
        {
            // Arrange
            var package = new ExcelBuilder()
                .Worksheet(sheetData =>
                {
                    sheetData
                        .AddRow([new("Name"), new("Age")])
                        .AddRow([new("Alice"), new(30)]);
                }).Build();

            // Act
            using var ms = new MemoryStream();
            package.SaveAs(ms);

            // Assert
            ms.Position = 0;
            using var loadedPackage = new ExcelPackage(ms);
            var worksheet = loadedPackage.Workbook.Worksheets["Sheet1"];

            Assert.Equal("Name", worksheet.Cells[1, 1].Value);
            Assert.Equal("Age", worksheet.Cells[1, 2].Value);
            Assert.Equal("Alice", worksheet.Cells[2, 1].Value);
            Assert.Equal(30.0, worksheet.Cells[2, 2].Value);
        }
    }
}
