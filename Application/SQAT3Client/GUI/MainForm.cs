using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.Properties;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu MasterMenu;
		private System.Windows.Forms.MenuItem FileMenu;
		private System.Windows.Forms.MenuItem FileNewSessionMenuItem;
		private System.Windows.Forms.MenuItem FileSeparatorMenu1;
        private System.Windows.Forms.MenuItem FileExitMenuItem;
        private IContainer components;
		private System.Windows.Forms.MenuItem OpenSessionMenuItem;
		private System.Windows.Forms.MenuItem SaveSessionMenuItem;
		private System.Windows.Forms.MenuItem FileSeparatorMenu2;
		private System.Windows.Forms.MenuItem OptionsMenu;
		private System.Windows.Forms.MenuItem PreferencesMenuItem;

		private System.Windows.Forms.MenuItem FileSeparatorMenu3;
		private System.Windows.Forms.MenuItem ApproveSessionMenuItem;
		private System.Windows.Forms.MenuItem SessionSettingsMenuItem;
		private System.Windows.Forms.MenuItem ToolsMenu;
		private System.Windows.Forms.MenuItem InspectPlatesMenuItem;
		private System.Windows.Forms.MenuItem RerunMenuItem;
		private System.Windows.Forms.MenuItem SaveReportMenuItem;
		private System.Windows.Forms.MenuItem ViewMenu;
		private System.Windows.Forms.MenuItem ShowResultsMenuItem;
		private System.Windows.Forms.MenuItem TableExperimentRowMenuItem;
		private System.Windows.Forms.MenuItem TableExperimentColumnMenuItem;
		private System.Windows.Forms.MenuItem PairwiseApprovedMenuItem;
		private System.Windows.Forms.MenuItem PairwiseAllMenuItem;
		private System.Windows.Forms.MenuItem MarkerAnnotationsMenuItem;
		private System.Windows.Forms.MenuItem CloseSessionMenuItem;
		private System.Windows.Forms.StatusBar MyStatusBar;
		private System.Windows.Forms.StatusBarPanel DatabaseStatusBarPanel;
		private System.Windows.Forms.StatusBarPanel UserStatusBarPanel;
		private Preferences MyPreferences;
		private Configuration MyConfiguration;
        private MenuItem ContentsMenuItem;
        private MenuItem ContentsPlatesMenuItem;
        private MenuItem ContentsGroupsMenuItem;
        private MenuItem ContentsDetailedFilterMenuItem;
        private MenuItem PendingChangesMenuItem;
        private MenuItem CompareMenuItem;
        private MenuItem HelpMenu;
        private MenuItem AboutMenuItem;
        private MenuItem IdentifyMenuItem;
        private MenuItem SaveInternalReportMenuItem;
		private string MyConnectionString;

		public MainForm(Configuration config, Preferences prefs, string connectionString)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			MyConfiguration = config;
			MyPreferences = prefs;
			MyConnectionString = connectionString;
			MyStatusBar.Panels[0].Text = DataServer.GetDatabaseName(connectionString);
			MyStatusBar.Panels[1].Text = DataServer.GetUserName(connectionString);
            SetBackground(connectionString);
		}

	    private void SetBackground(string connectionString)
	    {
	        if (connectionString.ToLower().Contains("devel"))
	        {
	            BackgroundImage = Resources.DevelBackground;
	        }
            else if (connectionString.ToLower().Contains("practice"))
	        {
	            BackgroundImage = Resources.ValidationBackground;
	        }
	    }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MasterMenu = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenu = new System.Windows.Forms.MenuItem();
            this.FileNewSessionMenuItem = new System.Windows.Forms.MenuItem();
            this.OpenSessionMenuItem = new System.Windows.Forms.MenuItem();
            this.CloseSessionMenuItem = new System.Windows.Forms.MenuItem();
            this.FileSeparatorMenu2 = new System.Windows.Forms.MenuItem();
            this.SaveSessionMenuItem = new System.Windows.Forms.MenuItem();
            this.SaveReportMenuItem = new System.Windows.Forms.MenuItem();
            this.FileSeparatorMenu3 = new System.Windows.Forms.MenuItem();
            this.ApproveSessionMenuItem = new System.Windows.Forms.MenuItem();
            this.FileSeparatorMenu1 = new System.Windows.Forms.MenuItem();
            this.FileExitMenuItem = new System.Windows.Forms.MenuItem();
            this.ViewMenu = new System.Windows.Forms.MenuItem();
            this.ShowResultsMenuItem = new System.Windows.Forms.MenuItem();
            this.TableExperimentRowMenuItem = new System.Windows.Forms.MenuItem();
            this.TableExperimentColumnMenuItem = new System.Windows.Forms.MenuItem();
            this.PairwiseApprovedMenuItem = new System.Windows.Forms.MenuItem();
            this.PairwiseAllMenuItem = new System.Windows.Forms.MenuItem();
            this.MarkerAnnotationsMenuItem = new System.Windows.Forms.MenuItem();
            this.ContentsMenuItem = new System.Windows.Forms.MenuItem();
            this.ContentsPlatesMenuItem = new System.Windows.Forms.MenuItem();
            this.ContentsGroupsMenuItem = new System.Windows.Forms.MenuItem();
            this.ContentsDetailedFilterMenuItem = new System.Windows.Forms.MenuItem();
            this.PendingChangesMenuItem = new System.Windows.Forms.MenuItem();
            this.ToolsMenu = new System.Windows.Forms.MenuItem();
            this.InspectPlatesMenuItem = new System.Windows.Forms.MenuItem();
            this.RerunMenuItem = new System.Windows.Forms.MenuItem();
            this.CompareMenuItem = new System.Windows.Forms.MenuItem();
            this.IdentifyMenuItem = new System.Windows.Forms.MenuItem();
            this.OptionsMenu = new System.Windows.Forms.MenuItem();
            this.SessionSettingsMenuItem = new System.Windows.Forms.MenuItem();
            this.PreferencesMenuItem = new System.Windows.Forms.MenuItem();
            this.HelpMenu = new System.Windows.Forms.MenuItem();
            this.AboutMenuItem = new System.Windows.Forms.MenuItem();
            this.MyStatusBar = new System.Windows.Forms.StatusBar();
            this.DatabaseStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.UserStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.SaveInternalReportMenuItem = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.DatabaseStatusBarPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserStatusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // MasterMenu
            // 
            this.MasterMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenu,
            this.ViewMenu,
            this.ToolsMenu,
            this.OptionsMenu,
            this.HelpMenu});
            // 
            // FileMenu
            // 
            this.FileMenu.Index = 0;
            this.FileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileNewSessionMenuItem,
            this.OpenSessionMenuItem,
            this.CloseSessionMenuItem,
            this.FileSeparatorMenu2,
            this.SaveSessionMenuItem,
            this.SaveInternalReportMenuItem,
            this.SaveReportMenuItem,
            this.FileSeparatorMenu3,
            this.ApproveSessionMenuItem,
            this.FileSeparatorMenu1,
            this.FileExitMenuItem});
            this.FileMenu.Text = "&File";
            this.FileMenu.Popup += new System.EventHandler(this.FileMenu_Popup);
            // 
            // FileNewSessionMenuItem
            // 
            this.FileNewSessionMenuItem.Index = 0;
            this.FileNewSessionMenuItem.Text = "&New Session...";
            this.FileNewSessionMenuItem.Click += new System.EventHandler(this.FileNewSessionMenuItem_Click);
            // 
            // OpenSessionMenuItem
            // 
            this.OpenSessionMenuItem.Index = 1;
            this.OpenSessionMenuItem.Text = "&Open Session...";
            this.OpenSessionMenuItem.Click += new System.EventHandler(this.OpenSessionMenuItem_Click);
            // 
            // CloseSessionMenuItem
            // 
            this.CloseSessionMenuItem.Index = 2;
            this.CloseSessionMenuItem.Text = "&Close Session";
            this.CloseSessionMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // FileSeparatorMenu2
            // 
            this.FileSeparatorMenu2.Index = 3;
            this.FileSeparatorMenu2.Text = "-";
            // 
            // SaveSessionMenuItem
            // 
            this.SaveSessionMenuItem.Index = 4;
            this.SaveSessionMenuItem.Text = "Save Session...";
            this.SaveSessionMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // SaveReportMenuItem
            // 
            this.SaveReportMenuItem.Index = 6;
            this.SaveReportMenuItem.Text = "Save Report...";
            this.SaveReportMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // FileSeparatorMenu3
            // 
            this.FileSeparatorMenu3.Index = 7;
            this.FileSeparatorMenu3.Text = "-";
            // 
            // ApproveSessionMenuItem
            // 
            this.ApproveSessionMenuItem.Index = 8;
            this.ApproveSessionMenuItem.Text = "Approve Session...";
            this.ApproveSessionMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // FileSeparatorMenu1
            // 
            this.FileSeparatorMenu1.Index = 9;
            this.FileSeparatorMenu1.Text = "-";
            // 
            // FileExitMenuItem
            // 
            this.FileExitMenuItem.Index = 10;
            this.FileExitMenuItem.Text = "&Exit";
            this.FileExitMenuItem.Click += new System.EventHandler(this.FileExitMenuItem_Click);
            // 
            // ViewMenu
            // 
            this.ViewMenu.Index = 1;
            this.ViewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ShowResultsMenuItem,
            this.MarkerAnnotationsMenuItem,
            this.ContentsMenuItem,
            this.PendingChangesMenuItem});
            this.ViewMenu.Text = "&View";
            this.ViewMenu.Popup += new System.EventHandler(this.ViewMenu_Popup);
            // 
            // ShowResultsMenuItem
            // 
            this.ShowResultsMenuItem.Index = 0;
            this.ShowResultsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.TableExperimentRowMenuItem,
            this.TableExperimentColumnMenuItem,
            this.PairwiseApprovedMenuItem,
            this.PairwiseAllMenuItem});
            this.ShowResultsMenuItem.Text = "Show &Results";
            // 
            // TableExperimentRowMenuItem
            // 
            this.TableExperimentRowMenuItem.Index = 0;
            this.TableExperimentRowMenuItem.Text = "Table, Experiments in Rows";
            this.TableExperimentRowMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // TableExperimentColumnMenuItem
            // 
            this.TableExperimentColumnMenuItem.Index = 1;
            this.TableExperimentColumnMenuItem.Text = "Table, Experiments in Columns";
            this.TableExperimentColumnMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // PairwiseApprovedMenuItem
            // 
            this.PairwiseApprovedMenuItem.Index = 2;
            this.PairwiseApprovedMenuItem.Text = "Pairwise, Approved Combinations";
            this.PairwiseApprovedMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // PairwiseAllMenuItem
            // 
            this.PairwiseAllMenuItem.Index = 3;
            this.PairwiseAllMenuItem.Text = "Pairwise, All Combinations";
            this.PairwiseAllMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // MarkerAnnotationsMenuItem
            // 
            this.MarkerAnnotationsMenuItem.Index = 1;
            this.MarkerAnnotationsMenuItem.Text = "Show Marker &Annotations...";
            this.MarkerAnnotationsMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // ContentsMenuItem
            // 
            this.ContentsMenuItem.Index = 2;
            this.ContentsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ContentsPlatesMenuItem,
            this.ContentsGroupsMenuItem,
            this.ContentsDetailedFilterMenuItem});
            this.ContentsMenuItem.Text = "Show Session &Contents";
            // 
            // ContentsPlatesMenuItem
            // 
            this.ContentsPlatesMenuItem.Index = 0;
            this.ContentsPlatesMenuItem.Text = "&Plates";
            this.ContentsPlatesMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // ContentsGroupsMenuItem
            // 
            this.ContentsGroupsMenuItem.Index = 1;
            this.ContentsGroupsMenuItem.Text = "&Working Sets";
            this.ContentsGroupsMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // ContentsDetailedFilterMenuItem
            // 
            this.ContentsDetailedFilterMenuItem.Index = 2;
            this.ContentsDetailedFilterMenuItem.Text = "&Detailed Filter";
            this.ContentsDetailedFilterMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // PendingChangesMenuItem
            // 
            this.PendingChangesMenuItem.Index = 3;
            this.PendingChangesMenuItem.Text = "Show &Pending Status Changes";
            this.PendingChangesMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // ToolsMenu
            // 
            this.ToolsMenu.Index = 2;
            this.ToolsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.InspectPlatesMenuItem,
            this.RerunMenuItem,
            this.CompareMenuItem,
            this.IdentifyMenuItem});
            this.ToolsMenu.Text = "&Tools";
            this.ToolsMenu.Popup += new System.EventHandler(this.ToolsMenu_Popup);
            // 
            // InspectPlatesMenuItem
            // 
            this.InspectPlatesMenuItem.Index = 0;
            this.InspectPlatesMenuItem.Text = "Inspect &Plates...";
            this.InspectPlatesMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // RerunMenuItem
            // 
            this.RerunMenuItem.Index = 1;
            this.RerunMenuItem.Text = "Pick &Rerun Items...";
            this.RerunMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // CompareMenuItem
            // 
            this.CompareMenuItem.Index = 2;
            this.CompareMenuItem.Text = "&Compare Results...";
            this.CompareMenuItem.Click += new System.EventHandler(this.CompareMenuItem_Click);
            // 
            // IdentifyMenuItem
            // 
            this.IdentifyMenuItem.Index = 3;
            this.IdentifyMenuItem.Text = "&Identify Items...";
            this.IdentifyMenuItem.Click += new System.EventHandler(this.IdentifyMenuItem_Click);
            // 
            // OptionsMenu
            // 
            this.OptionsMenu.Index = 3;
            this.OptionsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SessionSettingsMenuItem,
            this.PreferencesMenuItem});
            this.OptionsMenu.Text = "&Options";
            this.OptionsMenu.Popup += new System.EventHandler(this.OptionsMenu_Popup);
            // 
            // SessionSettingsMenuItem
            // 
            this.SessionSettingsMenuItem.Index = 0;
            this.SessionSettingsMenuItem.Text = "Session Settings...";
            this.SessionSettingsMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);
            // 
            // PreferencesMenuItem
            // 
            this.PreferencesMenuItem.Index = 1;
            this.PreferencesMenuItem.Text = "&Preferences...";
            this.PreferencesMenuItem.Click += new System.EventHandler(this.PreferencesMenuItem_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.Index = 4;
            this.HelpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.AboutMenuItem});
            this.HelpMenu.Text = "&Help";
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Index = 0;
            this.AboutMenuItem.Text = "&About...";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // MyStatusBar
            // 
            this.MyStatusBar.Location = new System.Drawing.Point(0, 526);
            this.MyStatusBar.Name = "MyStatusBar";
            this.MyStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.DatabaseStatusBarPanel,
            this.UserStatusBarPanel});
            this.MyStatusBar.ShowPanels = true;
            this.MyStatusBar.Size = new System.Drawing.Size(960, 24);
            this.MyStatusBar.TabIndex = 1;
            this.MyStatusBar.Text = "statusBar1";
            // 
            // DatabaseStatusBarPanel
            // 
            this.DatabaseStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.DatabaseStatusBarPanel.Name = "DatabaseStatusBarPanel";
            this.DatabaseStatusBarPanel.Width = 10;
            // 
            // UserStatusBarPanel
            // 
            this.UserStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.UserStatusBarPanel.Name = "UserStatusBarPanel";
            this.UserStatusBarPanel.Width = 10;
            // 
            // SaveInternalReportMenuItem
            // 
            this.SaveInternalReportMenuItem.Index = 5;
            this.SaveInternalReportMenuItem.Text = "Save Internal Report";
            this.SaveInternalReportMenuItem.Click += new System.EventHandler(this.ActiveSessionMenuEventHandler);

            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(960, 550);
            this.Controls.Add(this.MyStatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.MasterMenu;
            this.Name = "MainForm";
            this.Text = "SNP Quality Analysis Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.DatabaseStatusBarPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserStatusBarPanel)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


        private string FindNextFreeName()
        {
            int freeNumber = 1;
            string freeName = "";

            while (freeName == "")
            {
                freeName = "New Session " + freeNumber.ToString();
                //Check if the name is already taken.
                if (this.MdiChildren != null)
                {
                    for (int i = 0; i < this.MdiChildren.GetLength(0); i++)
                    {
                        if (this.MdiChildren[i].Text.ToUpper() == freeName.ToUpper())
                        {
                            //The name was taken, reset the name and increase counter.
                            freeName = "";
                            freeNumber++;
                            break;
                        }
                    }
                }
            }
            return freeName;
        }

		private void FileExitMenuItem_Click(object sender, System.EventArgs e)
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

		private void FileNewSessionMenuItem_Click(object sender, System.EventArgs e)
		{
			SessionForm newSession;

			try
			{
				//The session will live in the MDIChildren of this form.
				newSession = new SessionForm(this, MyConfiguration, MyPreferences, MyConnectionString);
				newSession.Open(SessionForm.StartMode.CreateSession, FindNextFreeName());
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when starting new session.", this);
			}
		}

		private void OpenSessionMenuItem_Click(object sender, System.EventArgs e)
		{
			SessionForm newSession;

			try
			{
				//The session will live in the MDIChildren of this form.
				newSession = new SessionForm(this, MyConfiguration, MyPreferences, MyConnectionString);
				newSession.Open(SessionForm.StartMode.LoadSession, "");
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when opening session.", this);
			}
		}

		private void ActiveSessionMenuEventHandler(object sender, System.EventArgs e)
		{
			SessionForm activeSession;
		
			try
			{
				activeSession = (SessionForm)this.ActiveMdiChild;
				if (activeSession == null)
				{
					MessageManager.ShowInformation("No active session.", this);
					return;
				}
				if (sender == SaveSessionMenuItem) { activeSession.Save(); }
				else if (sender == CloseSessionMenuItem) { activeSession.Close(); }
				else if (sender == ApproveSessionMenuItem) { activeSession.Approve(); }
				else if (sender == SessionSettingsMenuItem) { activeSession.EditSettings(); }
				else if (sender == InspectPlatesMenuItem) { activeSession.InspectPlates(); }
				else if (sender == RerunMenuItem) { activeSession.PickRerunItems(); }
				else if (sender == SaveReportMenuItem) { activeSession.SaveReport(); }
				else if (sender == TableExperimentRowMenuItem) { activeSession.ShowResultMatrix(ResultTable.Mode.ExperimentByItem); }
				else if (sender == TableExperimentColumnMenuItem) { activeSession.ShowResultMatrix(ResultTable.Mode.ItemByExperiment); }
				else if (sender == PairwiseAllMenuItem) { activeSession.ShowResultPairwise(true); }
				else if (sender == PairwiseApprovedMenuItem) { activeSession.ShowResultPairwise(false); }
				else if (sender == MarkerAnnotationsMenuItem) { activeSession.ShowMarkerAnnotations(); }
                else if (sender == ContentsPlatesMenuItem) { activeSession.ShowSessionPlates(); }
                else if (sender == ContentsGroupsMenuItem) { activeSession.ShowSessionGroups(); }
                else if (sender == ContentsDetailedFilterMenuItem) { activeSession.ShowSessionDetailedFilter(); }
                else if (sender == PendingChangesMenuItem) { activeSession.ShowPendingChanges(); }
                else if (sender == SaveInternalReportMenuItem) { activeSession.SaveInternalReport(); }

				activeSession.AutoRefresh();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when performing requested operation.", this);
			}			
		}

        private void CompareMenuItem_Click(object sender, EventArgs e)
        {
            ComparisonSettingsForm settingsForm;
            SessionForm[] sessions;
            StringCollection individualTypes, genotypeStatuses;
            StringCollection referenceSetNames;

            try
            {
                settingsForm = new ComparisonSettingsForm(MyConnectionString);

                //Find available sessions.
                if (this.MdiChildren != null)
                {
                    sessions = new SessionForm[this.MdiChildren.GetLength(0)];
                    for (int i = 0; i < this.MdiChildren.GetLength(0); i++)
                    {
                        sessions[i] = (SessionForm)this.MdiChildren[i];
                    }

                    individualTypes = DataServer.GetIndividualTypes(MyConnectionString, Convert.ToInt32(MyConfiguration.Get("synctimeout")));
                    genotypeStatuses = DataServer.GetGenotypeStatuses(MyConnectionString, Convert.ToInt32(MyConfiguration.Get("synctimeout")));
                    referenceSetNames = DataServer.GetReferenceSets(MyConnectionString, Convert.ToInt32(MyConfiguration.Get("synctimeout")));

                    settingsForm.InitSessions(sessions, individualTypes, genotypeStatuses);
                    settingsForm.InitReferenceSets(referenceSetNames);
                }

                settingsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to initialize form for comparisons.", this);
            }
        }

        private void IdentifyMenuItem_Click(object sender, EventArgs e)
        {
            IdentificationSettingsForm settingsForm;
            SessionForm[] sessions;
            StringCollection individualTypes, genotypeStatuses;
            StringCollection referenceSetNames;

            try
            {
                settingsForm = new IdentificationSettingsForm(MyConnectionString);

                //Find available sessions.
                if (this.MdiChildren != null)
                {
                    sessions = new SessionForm[this.MdiChildren.GetLength(0)];
                    for (int i = 0; i < this.MdiChildren.GetLength(0); i++)
                    {
                        sessions[i] = (SessionForm)this.MdiChildren[i];
                    }

                    individualTypes = DataServer.GetIndividualTypes(MyConnectionString, Convert.ToInt32(MyConfiguration.Get("synctimeout")));
                    genotypeStatuses = DataServer.GetGenotypeStatuses(MyConnectionString, Convert.ToInt32(MyConfiguration.Get("synctimeout")));
                    referenceSetNames = DataServer.GetReferenceSets(MyConnectionString, Convert.ToInt32(MyConfiguration.Get("synctimeout")));

                    settingsForm.InitSessions(sessions, individualTypes, genotypeStatuses);
                    settingsForm.InitReferenceSets(referenceSetNames);
                }

                settingsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to initialize form for identification.", this);
            }
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//Attempt to close all sessions first.
			if(this.MdiChildren != null)
			{
				foreach (Form tempForm in this.MdiChildren)
				{
					tempForm.Close();
				}
			}
			//If there are still sessions open (the user cancelled the closing
			//because of unsaved changes), cancel exiting.
			if (this.MdiChildren != null)
			{
				if (this.MdiChildren.GetLength(0) > 0)
				{
					e.Cancel = true;
					return;
				}
			}
			//OK, all sessions closed. Go ahead and exit.
			this.Dispose(true);
		}

		private void FileMenu_Popup(object sender, System.EventArgs e)
		{
			SessionForm activeSession;

			try
			{
				activeSession = (SessionForm) this.ActiveMdiChild;
				if (activeSession != null)
				{
					SaveSessionMenuItem.Enabled = activeSession.IsSaveable;
					SaveReportMenuItem.Enabled = activeSession.IsSaveable && !activeSession.IsAborted;
                    SaveInternalReportMenuItem.Enabled = activeSession.IsSaveable && !activeSession.IsAborted;
					ApproveSessionMenuItem.Enabled = activeSession.IsSaveable && !activeSession.IsAborted;
					CloseSessionMenuItem.Enabled = true;
				}
				else
				{
					SaveSessionMenuItem.Enabled = false;
					SaveReportMenuItem.Enabled = false;
                    SaveInternalReportMenuItem.Enabled = false;
					ApproveSessionMenuItem.Enabled = false;
					CloseSessionMenuItem.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing menu.", this);
			}
		}

		private void OptionsMenu_Popup(object sender, System.EventArgs e)
		{
			SessionForm activeSession;

			try
			{
				activeSession = (SessionForm) this.ActiveMdiChild;
				if (activeSession != null)
				{
					SessionSettingsMenuItem.Enabled = !activeSession.IsBusy;
				}
				else
				{
					SessionSettingsMenuItem.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing menu.", this);
			}
		}

		private void ToolsMenu_Popup(object sender, System.EventArgs e)
		{
			SessionForm activeSession;
            SessionForm[] sessions;
            bool anySessionBusy;

			try
			{
                //Set enabled status for menus base on the active session.
				activeSession = (SessionForm) this.ActiveMdiChild;
				if (activeSession != null)
				{
					InspectPlatesMenuItem.Enabled = !activeSession.IsBusy && !activeSession.IsAborted;
					RerunMenuItem.Enabled = !activeSession.IsBusy && !activeSession.IsPending && !activeSession.IsAborted;
				}
				else
				{
					InspectPlatesMenuItem.Enabled = false;
					RerunMenuItem.Enabled = false;
				}
                //Set the Compare menu item status based on if any of the sessions is busy.
                anySessionBusy = false;
                if (this.MdiChildren != null)
                {
                    sessions = new SessionForm[this.MdiChildren.GetLength(0)];
                    for (int i = 0; i < this.MdiChildren.GetLength(0); i++)
                    {
                        sessions[i] = (SessionForm)this.MdiChildren[i];
                        if (sessions[i].IsBusy)
                        {
                            anySessionBusy = true;
                            break;
                        }
                    }
                    CompareMenuItem.Enabled = !anySessionBusy;
                }
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing menu.", this);
			}
		}

		private void ViewMenu_Popup(object sender, System.EventArgs e)
		{
			SessionForm activeSession;

			try
			{
				activeSession = (SessionForm) this.ActiveMdiChild;
				if (activeSession != null)
				{
					ShowResultsMenuItem.Enabled = !activeSession.IsBusy && !activeSession.IsAborted;
					MarkerAnnotationsMenuItem.Enabled = !activeSession.IsBusy && !activeSession.IsAborted;
                    ContentsMenuItem.Enabled = !activeSession.IsBusy;
                    PendingChangesMenuItem.Enabled = activeSession.IsPending && !activeSession.IsAborted;
				}
				else
				{
					ShowResultsMenuItem.Enabled = false;
					MarkerAnnotationsMenuItem.Enabled = false;
                    ContentsMenuItem.Enabled = false;
                    PendingChangesMenuItem.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing menu.", this);
			}
		
		}

		private void PreferencesMenuItem_Click(object sender, System.EventArgs e)
		{
			PreferencesForm prefForm;

			try
			{
				prefForm = new PreferencesForm();
				prefForm.GetPreferences(MyPreferences);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when attempting to change preferences.", this);
			}		
		}

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog aboutBox;

            aboutBox = new AboutDialog();
            aboutBox.ShowDialog();
        }

		private class SessionCollection : CollectionBase
		{
			public void Add(SessionForm newItem)
			{
				List.Add(newItem);
			}
		
			public SessionForm Item(int Index)
			{
				return (SessionForm) List[Index];
			}

			public void SetItem(int Index, SessionForm Item)
			{
				List[Index] = Item;
			}
		}



	}
}
