using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.GUI
{
    public partial class InvestigationReportForm : Form
    {
        private string MyReport;
        private string MyStatus;
        private Thread MyBackgroundWorkingThread;
        private string MyConnectionString;
        private InvestigationReportGenerator MyInvestigationReportGenerator;
        

        public InvestigationReportForm(string connectionString)
        {
            InitializeComponent();
            MyConnectionString = connectionString;
        }

        private void MyInvestigationReportGenerator_StatusChange(InvestigationStatusEventArgs e)
        {
            MyStatus = e.Status;
        }

        public void GenerateComparisonReport(InvestigationSourceInfo sourceInfo1, InvestigationSourceInfo sourceInfo2, ComparisonSettings settings)
        {
            MyInvestigationReportGenerator = new ComparisonReportGenerator(sourceInfo1, sourceInfo2, settings, MyConnectionString);
            GenerateReport();
        }

        public void GenerateIdentificationReport(InvestigationSourceInfo sourceInfo, IdentificationSettings settings)
        {
            MyInvestigationReportGenerator = new IdentificationReportGenerator(sourceInfo, settings, MyConnectionString);
            GenerateReport();
        }

        private void GenerateReport()
        {
            MyReport = null;
            MyStatus = "";
            SaveButton.Enabled = false;
            MyInvestigationReportGenerator.StatusChange += new Investigation.InvestigationStatusChangeHandler(MyInvestigationReportGenerator_StatusChange);

            MyBackgroundWorkingThread = new Thread(new ThreadStart(ThreadEntryGenerateReport));
            MyBackgroundWorkingThread.Start();
            //Start checking progress at regular intervals.
            WorkTimer.Enabled = true;
            this.ShowDialog();
        }

        private void DisplayReport()
        {
            MainTextBox.Text = MyReport;
            SaveButton.Enabled = true;
            StatusLabel.Text = "Ready";
        }

        private void ThreadEntryGenerateReport()
        {
            try
            {
                MyReport = MyInvestigationReportGenerator.GenerateReport();
            }
            catch (ThreadAbortException)
            {
                //Do nothing.
            }
            catch (Exception ex)
            {
                MyStatus = "Aborted due to errors";
                MessageManager.ShowError(ex, "Error when trying to perform comparison.", this);
            }
        }

        private void WorkTimer_Tick(object sender, EventArgs e)
        {
            StatusLabel.Text = MyStatus;
            if (MyReport != null)
            {
                WorkTimer.Enabled = false;
                MyBackgroundWorkingThread = null;
                DisplayReport();
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when closing form.", this);
            }
        }

        private void InvestigationReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Clean up the background thread if it is running.
            if (MyBackgroundWorkingThread != null)
            {
                StatusLabel.Text = "Aborting...";
                MyBackgroundWorkingThread.Abort();
                MyBackgroundWorkingThread = null;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string filename;
            FileServer fs;

            try
            {
                if (MySaveFileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                filename = MySaveFileDialog.FileName;
                fs = new FileServer();
                this.Cursor = Cursors.WaitCursor;
                fs.SaveFile(MainTextBox.Text, filename);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when saving file.", this);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}