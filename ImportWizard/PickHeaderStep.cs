using ImportWizard;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Packaging;
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

            //dataGridView1.Visible = false;

            //dataGridView1.DataSource = new ExcelDataReader(ExcelService.SelectedWorksheet);

            //dataGridView1.Visible = true;
        }

        public override bool CanGoNext()
        {
            ExcelService.HeaderRowNumber = int.Parse(textBox1.Text);
            NextStep.ExcelService = ExcelService;

            return base.CanGoNext();
        }
    }
}
