using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Molmed.SQAT.GUI
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            SetText();
            CloseButton.Focus();
        }

        private void SetText()
        {
            string appVersion, appName;

            appVersion = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Revision;

            appName = Assembly.GetExecutingAssembly().GetName().Name;

            AboutTextBox.Text = appName + Environment.NewLine + "Version: " + appVersion;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}