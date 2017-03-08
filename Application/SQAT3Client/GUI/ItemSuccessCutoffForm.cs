using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Molmed.SQAT.GUI
{
    public partial class ItemSuccessCutoffForm : Form
    {
        private bool MyOKFlag = false;

        public enum ItemCutoffMode
        {
            AllExperiments = 0,
            NonfailedExperiments = 1
        }


        public ItemSuccessCutoffForm()
        {
            InitializeComponent();
        }

        public bool GetSettings(out int limit, out ItemCutoffMode mode)
        {
            this.ShowDialog();
            limit = (int)LimitNumericUpDown.Value;
            if (SuccessExpRadioButton.Checked)
            {
                mode = ItemCutoffMode.AllExperiments;
            }
            else
            {
                mode = ItemCutoffMode.NonfailedExperiments;
            }

            return MyOKFlag;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            MyOKFlag = true;
            this.Close();
        }
    }
}