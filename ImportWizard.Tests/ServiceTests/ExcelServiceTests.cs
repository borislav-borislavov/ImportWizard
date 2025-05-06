using ImportWizard.Services;
using ImportWizard.Tests.Builder;
using ImportWizard.Tests.Builder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Tests.ServiceTests
{
    public class ExcelServiceTests
    {
        [Fact]
        public void ExcelService_Import_()
        {
            //Arrange
            var package = new ExcelBuilder()
                .Worksheet(sheetData =>
                {
                    sheetData
                        .AddRow([new(1), new(2)])
                        .AddRow([new(3), new(4)])
                        .AddRow([new(5), new(6)]);
                }).Build();

            package.SaveAs("test.xlsx");

            //Act
            var service = new ExcelService();

            //Assert
        }
    }
}
