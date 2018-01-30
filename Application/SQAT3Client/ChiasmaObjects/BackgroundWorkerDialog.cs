using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Molmed.SQAT.ChiasmaObjects
{
    public partial class BackgroundWorkerDialog : Form
    {
        //This form exposes a background worker which can be used to perform operations
        //in the background. Set the DoWork event handler of this form's background worker
        //to the method which should be executed, and then call the Start method. This form
        //reports the progress of the background worker to the user.

        private bool MyCancelled;
        private Exception MyException;

        public BackgroundWorkerDialog()
        {
            InitializeComponent();

            MyCancelled = false;
            MyException = null;
        }

        public bool Cancelled
        {
            get { return MyCancelled; }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                CloseButton.Enabled = false;
                StatusLabel.Text = "Attempting to cancel, please wait...";
                MyBackgroundWorker.CancelAsync();
                MyCancelled = true;
            }
            catch (Exception ex)
            {
                HandleError("Error when cancelling operation.", ex);
            }
        }

        public static void HandleError(String message, Exception exception)
        {
            ShowErrorDialog errorDialog;

            errorDialog = new ShowErrorDialog(message, exception);
            errorDialog.ShowDialog();
        }

        private void MyBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StatusLabel.Text = e.UserState.ToString();
        }

        private void MyBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MyException = e.Error;
            this.Hide();
        }

        public void Start()
        {
            StatusLabel.Text = "Starting...";
            MyBackgroundWorker.RunWorkerAsync();
            this.ShowDialog();
        }

        public Exception ThrownException
        {
            get { return MyException; }
        }

        public BackgroundWorker Worker
        {
            get { return MyBackgroundWorker; }
            set { MyBackgroundWorker = value; }
        }


    }
}