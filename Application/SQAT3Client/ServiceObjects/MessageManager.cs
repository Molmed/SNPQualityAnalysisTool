using System;
using System.Windows.Forms;


namespace Molmed.SQAT.ServiceObjects
{
	public class MessageManager
	{

        private delegate void ShowErrorThreadSafeDelegate(Exception ex, string message, Form sourceForm);

		public static void ShowError(Exception e, string additionalMessage, Form sourceForm)
		{
            ShowErrorThreadSafeDelegate d;

            d = new ShowErrorThreadSafeDelegate(ShowErrorThreadSafe);
            sourceForm.Invoke(d, new object[] { e, additionalMessage, sourceForm });
		}

        private static void ShowErrorThreadSafe(Exception e, string additionalMessage, Form sourceForm)
        {
            string exceptionMessage;
            Exception innerException;

            //Add all inner exception descriptions to the string.
            innerException = e;
            exceptionMessage = "";
            while (innerException != null)
            {
                exceptionMessage += innerException.Message + Environment.NewLine;
                innerException = innerException.InnerException;
            }
            //Reset the mouse pointer on the source form.
            sourceForm.Cursor = Cursors.Default;
            //Show the message.
            MessageBox.Show(sourceForm, additionalMessage + Environment.NewLine + Environment.NewLine +
                "Details:" + Environment.NewLine + exceptionMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

		public static void ShowWarning(string message)
		{
			MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static void ShowWarning(string message, IWin32Window sourceForm)
		{
			MessageBox.Show(sourceForm, message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}


		public static void ShowInformation(string message)
		{
			MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static void ShowInformation(string message, IWin32Window sourceForm)
		{
			MessageBox.Show(sourceForm, message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}


		public static DialogResult ShowQuestion(string message)
		{
			return MessageBox.Show(message, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}

		public static DialogResult ShowQuestion(string message, IWin32Window sourceForm)
		{
			return MessageBox.Show(sourceForm, message, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}
	}

}