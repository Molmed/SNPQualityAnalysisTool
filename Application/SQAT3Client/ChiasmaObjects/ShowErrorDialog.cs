using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Molmed.SQAT.ChiasmaObjects
{
    public partial class ShowErrorDialog : Form
    {
        public ShowErrorDialog(String message, Exception exception)
        {
            String[] errorMessage;

            InitializeComponent();
            errorMessage = new String[4];
            errorMessage[0] = message;
            if (exception != null)
            {
                errorMessage[1] = exception.Message;
                errorMessage[2] = Environment.NewLine;
                errorMessage[3] = exception.StackTrace;
            }
            ErrorTextBox.Lines = errorMessage;
        }
    }
}