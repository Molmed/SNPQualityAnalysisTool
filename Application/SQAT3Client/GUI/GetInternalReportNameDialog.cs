using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Molmed.SQAT.GUI
{
    public partial class GetInternalReportNameDialog : Form
    {
        private String MyProjectName;
        public GetInternalReportNameDialog(String projectName)
        {
            MyProjectName = projectName;
            InitializeComponent();
            Init();
        }

        private void Init()
        { 
            DateTime today;
            today = DateTime.Now;
            InternalReportNameTextBox.Text = MyProjectName +
                "_" + today.ToString("yyMMdd") + "_ResultReport";
            InternalReportNameTextBox.Select();
        }

        public String GetName()
        {
            return InternalReportNameTextBox.Text.Trim();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (InternalReportNameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a name for the internal report!", "Internal report name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
