using Molmed.SQAT;
using Molmed.SQAT.GUI;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;
using Molmed.SQAT.ChiasmaObjects;
using System;
using System.Windows.Forms;


public class Starter
{

	//Set STAThreadAttribut because will be using OLE calls to the clipboard.
	[STAThreadAttribute] static void Main()
	{
		MainForm myMainForm;
		ConnectionManager myConnectionManager;
		Configuration myConfiguration = null;
		Preferences myPrefs = null;
		string versionStr, appName, connectionString, chiasmaConnectionString;

		//		SAVED FOR CREATING THE CONFIGURATION FILE.
		//		myConfiguration = new Configuration(); 		
		//		myConfiguration.Serialize();

		try
		{
            //Enable visual styles for some controls to work properly.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			//Load configuration.
			myConfiguration = Configuration.Deserialize();

			//Get connection string.
			myConnectionManager = new ConnectionManager();
            // Changed from GetConnectionString(), nr 2 contains also ChiasmaConnectionString
            connectionString = myConnectionManager.GetConnectionString2(out chiasmaConnectionString);
			if (connectionString == "")
			{
				return;
			}

            if (chiasmaConnectionString == "")
            {
                MessageBox.Show("No connection to Chiasma", "No connection to the Chiasma database available, corrupt SQATconnect.xml file",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            ChiasmaData.SetConnectionString(chiasmaConnectionString);
			//Load user preferences.
			try
			{
				myPrefs = Preferences.Deserialize();
			}
			catch (Exception)
			{
				MessageManager.ShowInformation("User preferences could not be loaded.");
			}
			if (myPrefs == null)
			{
				myPrefs = new Preferences();
			}

			//Authenticate the application if the configuration tells us so.
			if ( ((string)myConfiguration.Get("VersionControlFlag")).ToLower() == "true" )
			{
				//Get version number of the current assembly.
				versionStr = "";
				versionStr += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major + ".";
				versionStr += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor + ".";
				versionStr += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build + ".";
				versionStr += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;

				//Get the name of the current assembly.
				appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
				if (!DataServer.IsCorrectVersion(connectionString, appName, versionStr))
				{
					MessageManager.ShowInformation("This application is not allowed to connect to the database.");
					return;
				}
			}
			else if ( ((string)myConfiguration.Get("VersionControlFlag")).ToLower() != "false" )
			{
				MessageManager.ShowWarning("Invalid authenticate application configuration.");
				return;
			}

			myMainForm = new MainForm(myConfiguration, myPrefs, connectionString);

			//Show main form.
			myMainForm.ShowDialog();
		}
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred from which the program was unable to recover. Details: " + ex.Message);
        }

	}

}