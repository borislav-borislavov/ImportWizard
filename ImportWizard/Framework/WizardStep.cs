using ImportWizard.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.WinForms.Framework
{
    public partial class WizardStep : Form
    {
        private WizardStep _previousStep;
        public WizardStep PreviousStep
        {
            get => _previousStep;
            set
            {
                _previousStep = value;
                RefreshUI();
            }
        }

        private WizardStep _nextStep;
        public WizardStep NextStep
        {
            get => _nextStep;
            set
            {
                _nextStep = value;
                RefreshUI();
            }
        }

        private string _description = "Description";
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ExcelService ExcelService { get; set; }

        public WizardStep()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void WizardStepb_Load(object sender, EventArgs e)
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            if (string.IsNullOrEmpty(Description))
            {
                lblDescription.Visible = false;
            }
            else
            {
                lblDescription.Visible = true;
                lblDescription.Text = Description;
            }

            btnBack.Visible = PreviousStep != null;

            btnNext.Visible = NextStep != null;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (PreviousStep == null) return;

            if (!CanGoPrevious()) return;

            this.Visible = false;
            PreviousStep.Show();
            PreviousStep.Visible = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (NextStep == null) return;

            if(!CanGoNext()) return;

            this.Visible = false;

            NextStep.PreviousStep = this;
            NextStep.Show();

            NextStep.Visible = true;
        }

        public virtual bool CanGoNext()
        {
            return true;
        }

        public virtual bool CanGoPrevious()
        {
            return true;
        }
    }
}
