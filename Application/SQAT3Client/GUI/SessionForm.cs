using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;
using Molmed.SQAT.ChiasmaObjects;


namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for Session.
	/// </summary>
	public class SessionForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Panel TopPanel;
		private System.Windows.Forms.Panel ItemPanel;
		private System.Windows.Forms.Splitter ItemExperimentSplitter;
        private System.Windows.Forms.Panel ExperimentPanel;
        private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel ControlItemPanel;
        private System.Windows.Forms.GroupBox ControlItemGroupBox;
		private System.Windows.Forms.Panel ResultItemPanel;
        private System.Windows.Forms.GroupBox ResultItemGroupBox;
        private System.Windows.Forms.GroupBox ExperimentGroupBox;
        private ToolStrip SessionToolStrip;
        private ToolStripButton SettingsToolStripButton;
        private ToolStripButton SaveSessionToolStripButton;
        private ToolStripButton ApproveSessionToolStripButton;
        private ToolStripButton SaveReportToolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton PlatesToolStripButton;
        private ToolStripButton RerunToolStripButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton AnnotationsToolStripButton;
        private ToolStripDropDownButton ShowResultsDropDownButton;
        private ToolStripMenuItem ResultsExpRowsToolStripMenuItem;
        private ToolStripMenuItem ResultsExpColToolStripMenuItem;
        private ToolStripMenuItem ResultsPairwiseAllToolStripMenuItem;
        private ToolStripMenuItem ResultsPairwiseApprovedToolStripMenuItem;
        private ToolStripDropDownButton SelectionDropDownButton;
        private ToolStripMenuItem SessionPlatesToolStripMenuItem;
        private ToolStripMenuItem SessionGroupsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton RefreshToolStripButton;
        private ToolStripButton StopToolStripButton;
        private ToolStripButton SubmitStatusToolStripButton;
        private ToolStripButton ClearStatusToolStripButton;
        private ToolStripMenuItem SessionDetailedFilterToolStripMenuItem;
        private ToolStripButton InspectChangesToolStripButton;
        private ToolStripMenuItem AllExpBlankTestToolStripMenuItem;
        private ToolStripMenuItem AllExpFailedBlankToolStripMenuItem;
        private ToolStripMenuItem AllExpPassedBlankToolStripMenuItem;
        private ToolStripMenuItem AllExpTestedBlankToolStripMenuItem;
        private ToolStripMenuItem AllExpHomozygTestToolStripMenuItem;
        private ToolStripMenuItem AllExpFailedHomozygToolStripMenuItem;
        private ToolStripMenuItem AllExpPassedHomozygToolStripMenuItem;
        private ToolStripMenuItem AllExpTestedHomozygToolStripMenuItem;
        private ToolStripMenuItem AllExpInhTestToolStripMenuItem;
        private ToolStripMenuItem AllExpFailedInhToolStripMenuItem;
        private ToolStripMenuItem AllExpPassedInhToolStripMenuItem;
        private ToolStripMenuItem AllExpTestedInhToolStripMenuItem;
        private ToolStripMenuItem AllExpDupTestToolStripMenuItem;
        private ToolStripMenuItem AllExpFailedDupToolStripMenuItem;
        private ToolStripMenuItem AllExpPassedDupToolStripMenuItem;
        private ToolStripMenuItem AllExpTestedDupToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem AllExpShowAllToolStripMenuItem;
        private ToolStripMenuItem AllExpShowNoResultToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton ExperimentCutoffToolStripButton;
        private ToolStrip ItemToolStrip;
        private ToolStripButton ItemCutoffToolStripButton;
        private ToolStripSeparator toolStripSeparator6;

		private Configuration MyConfiguration;
		private DataServer MyDataServer;
        private DataSet MyResultSet;
		private SessionSettings MySessionSettings;
		private Form MyParentForm;
		private Preferences MyPreferences;
        private FilterIndicator MyFilters;
        private bool MyDataReadyFlag;
        private AsyncExecutionMode MyAsyncExecutionMode;
        private ExperimentQueue MyExperimentQueue;
        private System.Windows.Forms.Timer DataPollTimer;
        private ToolStrip ExperimentToolStrip;
        private ToolStripDropDownButton AllExperimentsDropDownButton;
        private ToolStripMenuItem AllExpRejectMinorityToolStripMenuItem;
		private bool MyNeedsRefresh;
        private bool MyIsAborted;
        private ReportSettings MyAutomaticReportSettings;
        private bool MyForStreaming;
        private DateTime MyPrepStartTime, MyPrepEndTime, MyComputeStartTime, MyComputeEndTime;
        private DateTime MyOutputStartTime, MyOutputEndTime;
        private AdvancedListView2 ExperimentList;
        private StatusBar MyStatusBar;
        private StatusBarPanel TextStatusBarPanel;
        private StatusBarPanel PrepTimeStatusBarPanel;
        private StatusBarPanel CompTimeStatusBarPanel;
        private StatusBarPanel OutputTimeStatusBarPanel;
        private Panel BottomPanel;
        private AdvancedListView2 ResultItemList;
        private AdvancedListView2 ControlItemList;
        private ContextMenuStrip MyExperimentContextMenuStrip;
        private ToolStripMenuItem SelExpDuplicateTestMenuItem;
        private ToolStripMenuItem SelExpShowFailedDupMenuItem;
        private ToolStripMenuItem SelExpShowPassedDupMenuItem;
        private ToolStripMenuItem SelExpInheritanceTestMenuItem;
        private ToolStripMenuItem SelExpBlankTestMenuItem;
        private ToolStripMenuItem SelExpHomozygoteTestMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem SelExpShowAllMenuItem;
        private ToolStripMenuItem SelExpShowNAMenuItem;
        private ToolStripMenuItem SelExpShowItemDetailsMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem SelExpRejectMinorityResultsMenuItem;
        private ToolStripMenuItem SelExpStatusMenuItem;
        private ToolStripMenuItem SelExpShowTestedDupMenuItem;
        private ToolStripMenuItem SelExpShowFailedInhMenuItem;
        private ToolStripMenuItem SelExpShowPassedInhMenuItem;
        private ToolStripMenuItem SelExpShowTestedInhMenuItem;
        private ToolStripMenuItem SelExpShowFailedBlankMenuItem2;
        private ToolStripMenuItem SelExpShowPassedBlankMenuItem2;
        private ToolStripMenuItem SelExpShowTestedBlankMenuItem;
        private ToolStripMenuItem SelExpShowFailedHomMenuItem;
        private ToolStripMenuItem SelExpShowPassedHomMenuItem;
        private ToolStripMenuItem SelExpShowTestedHomMenuItem;
        private ContextMenuStrip MyItemContextMenuStrip;
        private ToolStripMenuItem SelItemShowAllMenuItem;
        private ToolStripMenuItem SelItemShowNAMenuItem;
        private ToolStripMenuItem SelItemShowExperimentDetailsMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem SelItemStatusMenuItem;
        private ContextMenuStrip MyControlItemContextMenuStrip;
        private ToolStripMenuItem SelCtrlItemShowExperimentDetailsMenuItem;
        private ToolStripMenuItem SelCtrlItemShowAllMenuItem;
        private ToolStripMenuItem SelCtrlItemShowNAMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripLabel MaxRowsToolStripLabel;
        private ToolStripComboBox MaxRowsComboBox;
        private ToolStripLabel SortColumnToolStripLabel;
        private ToolStripComboBox SortColumnComboBox;
        private ToolStripLabel SortOrderToolStripLabel;
        private ToolStripComboBox SortOrderComboBox;
        private ToolStripButton RefreshSortToolStripButton;
        private ToolStripMenuItem SelExpSetStatusLoadMenuItem;
        private ToolStripMenuItem SelExpSetStatusRejectedMenuItem;
        private ToolStripMenuItem AllExpSetStatusMenuItem;
        private ToolStripMenuItem AllExpSetStatusLoadMenuItem;
        private ToolStripMenuItem AllExpSetStatusRejectedMenuItem;
        private ToolStripMenuItem SelItemSetStatusLoadMenuItem;
        private ToolStripMenuItem SelItemSetStatusRejectedMenuItem;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem SelCtrlItemStatusMenuItem;
        private ToolStripMenuItem SelCtrlItemSetStatusLoadMenuItem;
        private ToolStripMenuItem SelCtrlItemSetStatusRejectedMenuItem;
        private ToolStripMenuItem SelExpShowFailedInhTestTriosMenuItem;
        private ToolStripMenuItem AllExpFailedInhTriosToolStripMenuItem;
        private String MyConnectionString;

        private const string TIMER_BLANK = "--:--:--";

        private delegate void SetFormStatusThreadSafeDelegate(FormStatusMode mode, string statusText, string prepTime, string compTime, string outputTime);

        private enum AsyncExecutionMode
        {
            GetExperiments = 0,
            PerformTest = 1,
            GetResults = 2
        }

        private enum FormStatusMode
        {
            AllowInput = 0,
            Stoppable = 1,
            Unstoppable = 2
        }



		public SessionForm(Form parentForm, Configuration config, Preferences prefs, string connectionString)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			MyParentForm = parentForm;
			MyConfiguration = config;
			MyPreferences = prefs;
			this.MdiParent = MyParentForm;
			MyNeedsRefresh = false;
            MyConnectionString = connectionString;
			MyDataServer = new DataServer(
				connectionString,
				(string)MyConfiguration.Get("IsolationLevel"),
				Convert.ToInt32(MyConfiguration.Get("SyncTimeout")), 
				Convert.ToInt32(MyConfiguration.Get("AsyncTimeout")));

			TextStatusBarPanel.Text = "Ready";
		}

		public enum StartMode
		{
			CreateSession = 0,
			LoadSession = 1
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
				if (MyDataServer != null)
				{
					MyDataServer.Close();
				}
			}
			base.Dispose( disposing );
		}

		private void SessionForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string prompt;

			if (this.IsPending)
			{
				prompt = "";
				prompt += "The session '" + this.Text + "' has unsubmitted changes. Close anyway?";
				if (MessageManager.ShowQuestion(prompt, this) == DialogResult.No)
				{
					e.Cancel = true;
				}
			}		
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionForm));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.SessionToolStrip = new System.Windows.Forms.ToolStrip();
            this.SettingsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveSessionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ApproveSessionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveReportToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PlatesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RerunToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.AnnotationsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ShowResultsDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ResultsExpRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultsExpColToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultsPairwiseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultsPairwiseApprovedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectionDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SessionPlatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SessionGroupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SessionDetailedFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.StopToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SubmitStatusToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ClearStatusToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.InspectChangesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ItemPanel = new System.Windows.Forms.Panel();
            this.ResultItemPanel = new System.Windows.Forms.Panel();
            this.ResultItemGroupBox = new System.Windows.Forms.GroupBox();
            this.ResultItemList = new Molmed.SQAT.GUI.AdvancedListView2();
            this.MyItemContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SelItemShowAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelItemShowNAMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelItemShowExperimentDetailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.SelItemStatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelItemSetStatusLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelItemSetStatusRejectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemToolStrip = new System.Windows.Forms.ToolStrip();
            this.ItemCutoffToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ControlItemPanel = new System.Windows.Forms.Panel();
            this.ControlItemGroupBox = new System.Windows.Forms.GroupBox();
            this.ControlItemList = new Molmed.SQAT.GUI.AdvancedListView2();
            this.MyControlItemContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SelCtrlItemShowAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelCtrlItemShowNAMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelCtrlItemShowExperimentDetailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.SelCtrlItemStatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelCtrlItemSetStatusLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelCtrlItemSetStatusRejectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemExperimentSplitter = new System.Windows.Forms.Splitter();
            this.ExperimentPanel = new System.Windows.Forms.Panel();
            this.ExperimentGroupBox = new System.Windows.Forms.GroupBox();
            this.ExperimentList = new Molmed.SQAT.GUI.AdvancedListView2();
            this.MyExperimentContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SelExpDuplicateTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowFailedDupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowPassedDupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowTestedDupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpInheritanceTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowFailedInhMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowPassedInhMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowTestedInhMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowFailedInhTestTriosMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpBlankTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowFailedBlankMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowPassedBlankMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowTestedBlankMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpHomozygoteTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowFailedHomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowPassedHomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowTestedHomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.SelExpShowAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowNAMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpShowItemDetailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.SelExpRejectMinorityResultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpStatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpSetStatusLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelExpSetStatusRejectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExperimentToolStrip = new System.Windows.Forms.ToolStrip();
            this.ExperimentCutoffToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.AllExperimentsDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AllExpDupTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpFailedDupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpPassedDupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpTestedDupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpInhTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpFailedInhToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpPassedInhToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpTestedInhToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpFailedInhTriosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpBlankTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpFailedBlankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpPassedBlankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpTestedBlankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpHomozygTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpFailedHomozygToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpPassedHomozygToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpTestedHomozygToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.AllExpShowAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpShowNoResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.AllExpRejectMinorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpSetStatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpSetStatusLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllExpSetStatusRejectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.MaxRowsToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.MaxRowsComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.SortColumnToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.SortColumnComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.SortOrderToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.SortOrderComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.RefreshSortToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.DataPollTimer = new System.Windows.Forms.Timer(this.components);
            this.MyStatusBar = new System.Windows.Forms.StatusBar();
            this.TextStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.PrepTimeStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.CompTimeStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.OutputTimeStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.TopPanel.SuspendLayout();
            this.SessionToolStrip.SuspendLayout();
            this.ItemPanel.SuspendLayout();
            this.ResultItemPanel.SuspendLayout();
            this.ResultItemGroupBox.SuspendLayout();
            this.MyItemContextMenuStrip.SuspendLayout();
            this.ItemToolStrip.SuspendLayout();
            this.ControlItemPanel.SuspendLayout();
            this.ControlItemGroupBox.SuspendLayout();
            this.MyControlItemContextMenuStrip.SuspendLayout();
            this.ExperimentPanel.SuspendLayout();
            this.ExperimentGroupBox.SuspendLayout();
            this.MyExperimentContextMenuStrip.SuspendLayout();
            this.ExperimentToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextStatusBarPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrepTimeStatusBarPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompTimeStatusBarPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputTimeStatusBarPanel)).BeginInit();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.SessionToolStrip);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(984, 48);
            this.TopPanel.TabIndex = 0;
            // 
            // SessionToolStrip
            // 
            this.SessionToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SessionToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.SessionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsToolStripButton,
            this.SaveSessionToolStripButton,
            this.ApproveSessionToolStripButton,
            this.SaveReportToolStripButton,
            this.toolStripSeparator1,
            this.PlatesToolStripButton,
            this.RerunToolStripButton,
            this.toolStripSeparator2,
            this.AnnotationsToolStripButton,
            this.ShowResultsDropDownButton,
            this.SelectionDropDownButton,
            this.toolStripSeparator3,
            this.RefreshToolStripButton,
            this.StopToolStripButton,
            this.SubmitStatusToolStripButton,
            this.ClearStatusToolStripButton,
            this.InspectChangesToolStripButton});
            this.SessionToolStrip.Location = new System.Drawing.Point(0, 0);
            this.SessionToolStrip.Name = "SessionToolStrip";
            this.SessionToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 40, 0);
            this.SessionToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.SessionToolStrip.Size = new System.Drawing.Size(984, 48);
            this.SessionToolStrip.Stretch = true;
            this.SessionToolStrip.TabIndex = 0;
            this.SessionToolStrip.Text = "Session ToolStrip";
            this.SessionToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SessionToolStrip_ItemClicked);
            // 
            // SettingsToolStripButton
            // 
            this.SettingsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SettingsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SettingsToolStripButton.Image")));
            this.SettingsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SettingsToolStripButton.Name = "SettingsToolStripButton";
            this.SettingsToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.SettingsToolStripButton.Text = "Session settings";
            this.SettingsToolStripButton.ToolTipText = "Adjust session settings";
            // 
            // SaveSessionToolStripButton
            // 
            this.SaveSessionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveSessionToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveSessionToolStripButton.Image")));
            this.SaveSessionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveSessionToolStripButton.Name = "SaveSessionToolStripButton";
            this.SaveSessionToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.SaveSessionToolStripButton.Text = "Save session";
            this.SaveSessionToolStripButton.ToolTipText = "Save session";
            // 
            // ApproveSessionToolStripButton
            // 
            this.ApproveSessionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApproveSessionToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ApproveSessionToolStripButton.Image")));
            this.ApproveSessionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApproveSessionToolStripButton.Name = "ApproveSessionToolStripButton";
            this.ApproveSessionToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.ApproveSessionToolStripButton.Text = "Approve session";
            // 
            // SaveReportToolStripButton
            // 
            this.SaveReportToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveReportToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveReportToolStripButton.Image")));
            this.SaveReportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveReportToolStripButton.Name = "SaveReportToolStripButton";
            this.SaveReportToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.SaveReportToolStripButton.Text = "Upload internal report";
            this.SaveReportToolStripButton.ToolTipText = "Upload internal report to the Chiasma database";
            //this.SaveReportToolStripButton.Click += new System.EventHandler(this.SaveReportToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 48);
            // 
            // PlatesToolStripButton
            // 
            this.PlatesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PlatesToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("PlatesToolStripButton.Image")));
            this.PlatesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PlatesToolStripButton.Name = "PlatesToolStripButton";
            this.PlatesToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.PlatesToolStripButton.Text = "Inspect plates";
            this.PlatesToolStripButton.ToolTipText = "Inspect individual plates";
            // 
            // RerunToolStripButton
            // 
            this.RerunToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RerunToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RerunToolStripButton.Image")));
            this.RerunToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RerunToolStripButton.Name = "RerunToolStripButton";
            this.RerunToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.RerunToolStripButton.Text = "Rerun";
            this.RerunToolStripButton.ToolTipText = "Select items to genotype again";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 48);
            // 
            // AnnotationsToolStripButton
            // 
            this.AnnotationsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AnnotationsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("AnnotationsToolStripButton.Image")));
            this.AnnotationsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AnnotationsToolStripButton.Name = "AnnotationsToolStripButton";
            this.AnnotationsToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.AnnotationsToolStripButton.Text = "Marker annotations";
            this.AnnotationsToolStripButton.ToolTipText = "Show marker annotations";
            // 
            // ShowResultsDropDownButton
            // 
            this.ShowResultsDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowResultsDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResultsExpRowsToolStripMenuItem,
            this.ResultsExpColToolStripMenuItem,
            this.ResultsPairwiseAllToolStripMenuItem,
            this.ResultsPairwiseApprovedToolStripMenuItem});
            this.ShowResultsDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("ShowResultsDropDownButton.Image")));
            this.ShowResultsDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShowResultsDropDownButton.Name = "ShowResultsDropDownButton";
            this.ShowResultsDropDownButton.Size = new System.Drawing.Size(45, 45);
            this.ShowResultsDropDownButton.Text = "Show results";
            // 
            // ResultsExpRowsToolStripMenuItem
            // 
            this.ResultsExpRowsToolStripMenuItem.Name = "ResultsExpRowsToolStripMenuItem";
            this.ResultsExpRowsToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.ResultsExpRowsToolStripMenuItem.Text = "Table, Experiments in Rows";
            this.ResultsExpRowsToolStripMenuItem.Click += new System.EventHandler(this.ResultsExpRowsToolStripMenuItem_Click);
            // 
            // ResultsExpColToolStripMenuItem
            // 
            this.ResultsExpColToolStripMenuItem.Name = "ResultsExpColToolStripMenuItem";
            this.ResultsExpColToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.ResultsExpColToolStripMenuItem.Text = "Table, Experiments in Columns";
            this.ResultsExpColToolStripMenuItem.Click += new System.EventHandler(this.ResultsExpColToolStripMenuItem_Click);
            // 
            // ResultsPairwiseAllToolStripMenuItem
            // 
            this.ResultsPairwiseAllToolStripMenuItem.Name = "ResultsPairwiseAllToolStripMenuItem";
            this.ResultsPairwiseAllToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.ResultsPairwiseAllToolStripMenuItem.Text = "Pairwise, All Combinations";
            this.ResultsPairwiseAllToolStripMenuItem.Click += new System.EventHandler(this.ResultsPairwiseAllToolStripMenuItem_Click);
            // 
            // ResultsPairwiseApprovedToolStripMenuItem
            // 
            this.ResultsPairwiseApprovedToolStripMenuItem.Name = "ResultsPairwiseApprovedToolStripMenuItem";
            this.ResultsPairwiseApprovedToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.ResultsPairwiseApprovedToolStripMenuItem.Text = "Pairwise, Approved Combinations";
            this.ResultsPairwiseApprovedToolStripMenuItem.Click += new System.EventHandler(this.ResultsPairwiseApprovedToolStripMenuItem_Click);
            // 
            // SelectionDropDownButton
            // 
            this.SelectionDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectionDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SessionPlatesToolStripMenuItem,
            this.SessionGroupsToolStripMenuItem,
            this.SessionDetailedFilterToolStripMenuItem});
            this.SelectionDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectionDropDownButton.Image")));
            this.SelectionDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectionDropDownButton.Name = "SelectionDropDownButton";
            this.SelectionDropDownButton.Size = new System.Drawing.Size(45, 45);
            this.SelectionDropDownButton.Text = "Selections";
            this.SelectionDropDownButton.ToolTipText = "Show selections";
            // 
            // SessionPlatesToolStripMenuItem
            // 
            this.SessionPlatesToolStripMenuItem.Name = "SessionPlatesToolStripMenuItem";
            this.SessionPlatesToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.SessionPlatesToolStripMenuItem.Text = "Plates";
            this.SessionPlatesToolStripMenuItem.Click += new System.EventHandler(this.SessionPlatesToolStripMenuItem_Click);
            // 
            // SessionGroupsToolStripMenuItem
            // 
            this.SessionGroupsToolStripMenuItem.Name = "SessionGroupsToolStripMenuItem";
            this.SessionGroupsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.SessionGroupsToolStripMenuItem.Text = "Working Sets";
            this.SessionGroupsToolStripMenuItem.Click += new System.EventHandler(this.SessionGroupsToolStripMenuItem_Click);
            // 
            // SessionDetailedFilterToolStripMenuItem
            // 
            this.SessionDetailedFilterToolStripMenuItem.Name = "SessionDetailedFilterToolStripMenuItem";
            this.SessionDetailedFilterToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.SessionDetailedFilterToolStripMenuItem.Text = "Detailed Filter";
            this.SessionDetailedFilterToolStripMenuItem.Click += new System.EventHandler(this.SessionDetailedFilterToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 48);
            // 
            // RefreshToolStripButton
            // 
            this.RefreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshToolStripButton.Image")));
            this.RefreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshToolStripButton.Name = "RefreshToolStripButton";
            this.RefreshToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.RefreshToolStripButton.Text = "Refresh";
            // 
            // StopToolStripButton
            // 
            this.StopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("StopToolStripButton.Image")));
            this.StopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StopToolStripButton.Name = "StopToolStripButton";
            this.StopToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.StopToolStripButton.Text = "Stop";
            this.StopToolStripButton.ToolTipText = "Abort calculations";
            // 
            // SubmitStatusToolStripButton
            // 
            this.SubmitStatusToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SubmitStatusToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SubmitStatusToolStripButton.Image")));
            this.SubmitStatusToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SubmitStatusToolStripButton.Name = "SubmitStatusToolStripButton";
            this.SubmitStatusToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.SubmitStatusToolStripButton.Text = "Submit";
            this.SubmitStatusToolStripButton.ToolTipText = "Submit pending genotype status changes";
            // 
            // ClearStatusToolStripButton
            // 
            this.ClearStatusToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ClearStatusToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ClearStatusToolStripButton.Image")));
            this.ClearStatusToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearStatusToolStripButton.Name = "ClearStatusToolStripButton";
            this.ClearStatusToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.ClearStatusToolStripButton.Text = "Clear";
            this.ClearStatusToolStripButton.ToolTipText = "Clear pending genotype status changes";
            // 
            // InspectChangesToolStripButton
            // 
            this.InspectChangesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.InspectChangesToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("InspectChangesToolStripButton.Image")));
            this.InspectChangesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InspectChangesToolStripButton.Name = "InspectChangesToolStripButton";
            this.InspectChangesToolStripButton.Size = new System.Drawing.Size(36, 45);
            this.InspectChangesToolStripButton.Text = "Inspect status changes";
            this.InspectChangesToolStripButton.ToolTipText = "Inspect pending genotype status changes";
            // 
            // ItemPanel
            // 
            this.ItemPanel.Controls.Add(this.ResultItemPanel);
            this.ItemPanel.Controls.Add(this.ControlItemPanel);
            this.ItemPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ItemPanel.Location = new System.Drawing.Point(0, 48);
            this.ItemPanel.Name = "ItemPanel";
            this.ItemPanel.Size = new System.Drawing.Size(264, 449);
            this.ItemPanel.TabIndex = 2;
            // 
            // ResultItemPanel
            // 
            this.ResultItemPanel.Controls.Add(this.ResultItemGroupBox);
            this.ResultItemPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultItemPanel.Location = new System.Drawing.Point(0, 168);
            this.ResultItemPanel.Name = "ResultItemPanel";
            this.ResultItemPanel.Size = new System.Drawing.Size(264, 281);
            this.ResultItemPanel.TabIndex = 5;
            // 
            // ResultItemGroupBox
            // 
            this.ResultItemGroupBox.Controls.Add(this.ResultItemList);
            this.ResultItemGroupBox.Controls.Add(this.ItemToolStrip);
            this.ResultItemGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultItemGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ResultItemGroupBox.Name = "ResultItemGroupBox";
            this.ResultItemGroupBox.Size = new System.Drawing.Size(264, 281);
            this.ResultItemGroupBox.TabIndex = 0;
            this.ResultItemGroupBox.TabStop = false;
            this.ResultItemGroupBox.Text = "Result Items";
            // 
            // ResultItemList
            // 
            this.ResultItemList.ContextMenuStrip = this.MyItemContextMenuStrip;
            this.ResultItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultItemList.Location = new System.Drawing.Point(3, 16);
            this.ResultItemList.Name = "ResultItemList";
            this.ResultItemList.Size = new System.Drawing.Size(258, 237);
            this.ResultItemList.TabIndex = 2;
            // 
            // MyItemContextMenuStrip
            // 
            this.MyItemContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelItemShowAllMenuItem,
            this.SelItemShowNAMenuItem,
            this.SelItemShowExperimentDetailsMenuItem,
            this.toolStripSeparator9,
            this.SelItemStatusMenuItem});
            this.MyItemContextMenuStrip.Name = "MyItemContextMenuStrip";
            this.MyItemContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MyItemContextMenuStrip.Size = new System.Drawing.Size(142, 98);
            this.MyItemContextMenuStrip.Opened += new System.EventHandler(this.MyItemContextMenu_Popup);
            // 
            // SelItemShowAllMenuItem
            // 
            this.SelItemShowAllMenuItem.Name = "SelItemShowAllMenuItem";
            this.SelItemShowAllMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelItemShowAllMenuItem.Text = "Show All";
            this.SelItemShowAllMenuItem.Click += new System.EventHandler(this.ItemMenuItem_Click);
            // 
            // SelItemShowNAMenuItem
            // 
            this.SelItemShowNAMenuItem.Name = "SelItemShowNAMenuItem";
            this.SelItemShowNAMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelItemShowNAMenuItem.Text = "Show N/A";
            this.SelItemShowNAMenuItem.Click += new System.EventHandler(this.ItemMenuItem_Click);
            // 
            // SelItemShowExperimentDetailsMenuItem
            // 
            this.SelItemShowExperimentDetailsMenuItem.Name = "SelItemShowExperimentDetailsMenuItem";
            this.SelItemShowExperimentDetailsMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelItemShowExperimentDetailsMenuItem.Text = "Show Details";
            this.SelItemShowExperimentDetailsMenuItem.Click += new System.EventHandler(this.ItemMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(138, 6);
            // 
            // SelItemStatusMenuItem
            // 
            this.SelItemStatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelItemSetStatusLoadMenuItem,
            this.SelItemSetStatusRejectedMenuItem});
            this.SelItemStatusMenuItem.Name = "SelItemStatusMenuItem";
            this.SelItemStatusMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelItemStatusMenuItem.Text = "Set Status";
            // 
            // SelItemSetStatusLoadMenuItem
            // 
            this.SelItemSetStatusLoadMenuItem.Name = "SelItemSetStatusLoadMenuItem";
            this.SelItemSetStatusLoadMenuItem.Size = new System.Drawing.Size(119, 22);
            this.SelItemSetStatusLoadMenuItem.Text = "Load";
            this.SelItemSetStatusLoadMenuItem.Click += new System.EventHandler(this.ItemMenuItem_Click);
            // 
            // SelItemSetStatusRejectedMenuItem
            // 
            this.SelItemSetStatusRejectedMenuItem.Name = "SelItemSetStatusRejectedMenuItem";
            this.SelItemSetStatusRejectedMenuItem.Size = new System.Drawing.Size(119, 22);
            this.SelItemSetStatusRejectedMenuItem.Text = "Rejected";
            this.SelItemSetStatusRejectedMenuItem.Click += new System.EventHandler(this.ItemMenuItem_Click);
            // 
            // ItemToolStrip
            // 
            this.ItemToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ItemToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ItemToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemCutoffToolStripButton});
            this.ItemToolStrip.Location = new System.Drawing.Point(3, 253);
            this.ItemToolStrip.Name = "ItemToolStrip";
            this.ItemToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ItemToolStrip.Size = new System.Drawing.Size(258, 25);
            this.ItemToolStrip.TabIndex = 1;
            this.ItemToolStrip.Text = "Item Tool Strip";
            // 
            // ItemCutoffToolStripButton
            // 
            this.ItemCutoffToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ItemCutoffToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ItemCutoffToolStripButton.Image")));
            this.ItemCutoffToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ItemCutoffToolStripButton.Name = "ItemCutoffToolStripButton";
            this.ItemCutoffToolStripButton.Size = new System.Drawing.Size(54, 22);
            this.ItemCutoffToolStripButton.Text = "Cutoff...";
            this.ItemCutoffToolStripButton.ToolTipText = "Reject genotypes from items with success rates below cutoff";
            this.ItemCutoffToolStripButton.Click += new System.EventHandler(this.ItemCutoffToolStripButton_Click);
            // 
            // ControlItemPanel
            // 
            this.ControlItemPanel.Controls.Add(this.ControlItemGroupBox);
            this.ControlItemPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlItemPanel.Location = new System.Drawing.Point(0, 0);
            this.ControlItemPanel.Name = "ControlItemPanel";
            this.ControlItemPanel.Size = new System.Drawing.Size(264, 168);
            this.ControlItemPanel.TabIndex = 4;
            // 
            // ControlItemGroupBox
            // 
            this.ControlItemGroupBox.Controls.Add(this.ControlItemList);
            this.ControlItemGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlItemGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ControlItemGroupBox.Name = "ControlItemGroupBox";
            this.ControlItemGroupBox.Size = new System.Drawing.Size(264, 168);
            this.ControlItemGroupBox.TabIndex = 0;
            this.ControlItemGroupBox.TabStop = false;
            this.ControlItemGroupBox.Text = "Control Items";
            // 
            // ControlItemList
            // 
            this.ControlItemList.ContextMenuStrip = this.MyControlItemContextMenuStrip;
            this.ControlItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlItemList.Location = new System.Drawing.Point(3, 16);
            this.ControlItemList.Name = "ControlItemList";
            this.ControlItemList.Size = new System.Drawing.Size(258, 149);
            this.ControlItemList.TabIndex = 0;
            // 
            // MyControlItemContextMenuStrip
            // 
            this.MyControlItemContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelCtrlItemShowAllMenuItem,
            this.SelCtrlItemShowNAMenuItem,
            this.SelCtrlItemShowExperimentDetailsMenuItem,
            this.toolStripSeparator11,
            this.SelCtrlItemStatusMenuItem});
            this.MyControlItemContextMenuStrip.Name = "MyControlItemContextMenuStrip";
            this.MyControlItemContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MyControlItemContextMenuStrip.Size = new System.Drawing.Size(142, 98);
            this.MyControlItemContextMenuStrip.Opened += new System.EventHandler(this.MyControlContextMenu_Popup);
            // 
            // SelCtrlItemShowAllMenuItem
            // 
            this.SelCtrlItemShowAllMenuItem.Name = "SelCtrlItemShowAllMenuItem";
            this.SelCtrlItemShowAllMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelCtrlItemShowAllMenuItem.Text = "Show All";
            this.SelCtrlItemShowAllMenuItem.Click += new System.EventHandler(this.ControlItemMenuItem_Click);
            // 
            // SelCtrlItemShowNAMenuItem
            // 
            this.SelCtrlItemShowNAMenuItem.Name = "SelCtrlItemShowNAMenuItem";
            this.SelCtrlItemShowNAMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelCtrlItemShowNAMenuItem.Text = "Show N/A";
            this.SelCtrlItemShowNAMenuItem.Click += new System.EventHandler(this.ControlItemMenuItem_Click);
            // 
            // SelCtrlItemShowExperimentDetailsMenuItem
            // 
            this.SelCtrlItemShowExperimentDetailsMenuItem.Name = "SelCtrlItemShowExperimentDetailsMenuItem";
            this.SelCtrlItemShowExperimentDetailsMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelCtrlItemShowExperimentDetailsMenuItem.Text = "Show Details";
            this.SelCtrlItemShowExperimentDetailsMenuItem.Click += new System.EventHandler(this.ControlItemMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(138, 6);
            // 
            // SelCtrlItemStatusMenuItem
            // 
            this.SelCtrlItemStatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelCtrlItemSetStatusLoadMenuItem,
            this.SelCtrlItemSetStatusRejectedMenuItem});
            this.SelCtrlItemStatusMenuItem.Name = "SelCtrlItemStatusMenuItem";
            this.SelCtrlItemStatusMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SelCtrlItemStatusMenuItem.Text = "Set Status";
            // 
            // SelCtrlItemSetStatusLoadMenuItem
            // 
            this.SelCtrlItemSetStatusLoadMenuItem.Name = "SelCtrlItemSetStatusLoadMenuItem";
            this.SelCtrlItemSetStatusLoadMenuItem.Size = new System.Drawing.Size(119, 22);
            this.SelCtrlItemSetStatusLoadMenuItem.Text = "Load";
            this.SelCtrlItemSetStatusLoadMenuItem.Click += new System.EventHandler(this.ControlItemMenuItem_Click);
            // 
            // SelCtrlItemSetStatusRejectedMenuItem
            // 
            this.SelCtrlItemSetStatusRejectedMenuItem.Name = "SelCtrlItemSetStatusRejectedMenuItem";
            this.SelCtrlItemSetStatusRejectedMenuItem.Size = new System.Drawing.Size(119, 22);
            this.SelCtrlItemSetStatusRejectedMenuItem.Text = "Rejected";
            this.SelCtrlItemSetStatusRejectedMenuItem.Click += new System.EventHandler(this.ControlItemMenuItem_Click);
            // 
            // ItemExperimentSplitter
            // 
            this.ItemExperimentSplitter.Location = new System.Drawing.Point(264, 48);
            this.ItemExperimentSplitter.Name = "ItemExperimentSplitter";
            this.ItemExperimentSplitter.Size = new System.Drawing.Size(8, 449);
            this.ItemExperimentSplitter.TabIndex = 3;
            this.ItemExperimentSplitter.TabStop = false;
            // 
            // ExperimentPanel
            // 
            this.ExperimentPanel.Controls.Add(this.ExperimentGroupBox);
            this.ExperimentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentPanel.Location = new System.Drawing.Point(272, 48);
            this.ExperimentPanel.Name = "ExperimentPanel";
            this.ExperimentPanel.Size = new System.Drawing.Size(712, 449);
            this.ExperimentPanel.TabIndex = 4;
            // 
            // ExperimentGroupBox
            // 
            this.ExperimentGroupBox.Controls.Add(this.ExperimentList);
            this.ExperimentGroupBox.Controls.Add(this.ExperimentToolStrip);
            this.ExperimentGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ExperimentGroupBox.Name = "ExperimentGroupBox";
            this.ExperimentGroupBox.Size = new System.Drawing.Size(712, 449);
            this.ExperimentGroupBox.TabIndex = 2;
            this.ExperimentGroupBox.TabStop = false;
            this.ExperimentGroupBox.Text = "Experiments";
            // 
            // ExperimentList
            // 
            this.ExperimentList.ContextMenuStrip = this.MyExperimentContextMenuStrip;
            this.ExperimentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentList.Location = new System.Drawing.Point(3, 16);
            this.ExperimentList.Name = "ExperimentList";
            this.ExperimentList.Size = new System.Drawing.Size(706, 405);
            this.ExperimentList.TabIndex = 2;
            // 
            // MyExperimentContextMenuStrip
            // 
            this.MyExperimentContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelExpDuplicateTestMenuItem,
            this.SelExpInheritanceTestMenuItem,
            this.SelExpBlankTestMenuItem,
            this.SelExpHomozygoteTestMenuItem,
            this.toolStripSeparator7,
            this.SelExpShowAllMenuItem,
            this.SelExpShowNAMenuItem,
            this.SelExpShowItemDetailsMenuItem,
            this.toolStripSeparator8,
            this.SelExpRejectMinorityResultsMenuItem,
            this.SelExpStatusMenuItem});
            this.MyExperimentContextMenuStrip.Name = "MyExperimentContextMenuStrip";
            this.MyExperimentContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MyExperimentContextMenuStrip.Size = new System.Drawing.Size(195, 214);
            this.MyExperimentContextMenuStrip.Opened += new System.EventHandler(this.MyExperimentContextMenu_Popup);
            // 
            // SelExpDuplicateTestMenuItem
            // 
            this.SelExpDuplicateTestMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelExpShowFailedDupMenuItem,
            this.SelExpShowPassedDupMenuItem,
            this.SelExpShowTestedDupMenuItem});
            this.SelExpDuplicateTestMenuItem.Name = "SelExpDuplicateTestMenuItem";
            this.SelExpDuplicateTestMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpDuplicateTestMenuItem.Text = "Duplicate Test";
            // 
            // SelExpShowFailedDupMenuItem
            // 
            this.SelExpShowFailedDupMenuItem.Name = "SelExpShowFailedDupMenuItem";
            this.SelExpShowFailedDupMenuItem.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowFailedDupMenuItem.Text = "Show Failed";
            this.SelExpShowFailedDupMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowPassedDupMenuItem
            // 
            this.SelExpShowPassedDupMenuItem.Name = "SelExpShowPassedDupMenuItem";
            this.SelExpShowPassedDupMenuItem.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowPassedDupMenuItem.Text = "Show Passed";
            this.SelExpShowPassedDupMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowTestedDupMenuItem
            // 
            this.SelExpShowTestedDupMenuItem.Name = "SelExpShowTestedDupMenuItem";
            this.SelExpShowTestedDupMenuItem.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowTestedDupMenuItem.Text = "Show Tested";
            this.SelExpShowTestedDupMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpInheritanceTestMenuItem
            // 
            this.SelExpInheritanceTestMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelExpShowFailedInhMenuItem,
            this.SelExpShowPassedInhMenuItem,
            this.SelExpShowTestedInhMenuItem,
            this.SelExpShowFailedInhTestTriosMenuItem});
            this.SelExpInheritanceTestMenuItem.Name = "SelExpInheritanceTestMenuItem";
            this.SelExpInheritanceTestMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpInheritanceTestMenuItem.Text = "Inheritance Test";
            // 
            // SelExpShowFailedInhMenuItem
            // 
            this.SelExpShowFailedInhMenuItem.Name = "SelExpShowFailedInhMenuItem";
            this.SelExpShowFailedInhMenuItem.Size = new System.Drawing.Size(166, 22);
            this.SelExpShowFailedInhMenuItem.Text = "Show Failed";
            this.SelExpShowFailedInhMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowPassedInhMenuItem
            // 
            this.SelExpShowPassedInhMenuItem.Name = "SelExpShowPassedInhMenuItem";
            this.SelExpShowPassedInhMenuItem.Size = new System.Drawing.Size(166, 22);
            this.SelExpShowPassedInhMenuItem.Text = "Show Passed";
            this.SelExpShowPassedInhMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowTestedInhMenuItem
            // 
            this.SelExpShowTestedInhMenuItem.Name = "SelExpShowTestedInhMenuItem";
            this.SelExpShowTestedInhMenuItem.Size = new System.Drawing.Size(166, 22);
            this.SelExpShowTestedInhMenuItem.Text = "Show Tested";
            this.SelExpShowTestedInhMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowFailedInhTestTriosMenuItem
            // 
            this.SelExpShowFailedInhTestTriosMenuItem.Name = "SelExpShowFailedInhTestTriosMenuItem";
            this.SelExpShowFailedInhTestTriosMenuItem.Size = new System.Drawing.Size(166, 22);
            this.SelExpShowFailedInhTestTriosMenuItem.Text = "Show Failed Trios";
            this.SelExpShowFailedInhTestTriosMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpBlankTestMenuItem
            // 
            this.SelExpBlankTestMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelExpShowFailedBlankMenuItem2,
            this.SelExpShowPassedBlankMenuItem2,
            this.SelExpShowTestedBlankMenuItem});
            this.SelExpBlankTestMenuItem.Name = "SelExpBlankTestMenuItem";
            this.SelExpBlankTestMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpBlankTestMenuItem.Text = "Blank Test";
            // 
            // SelExpShowFailedBlankMenuItem2
            // 
            this.SelExpShowFailedBlankMenuItem2.Name = "SelExpShowFailedBlankMenuItem2";
            this.SelExpShowFailedBlankMenuItem2.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowFailedBlankMenuItem2.Text = "Show Failed";
            this.SelExpShowFailedBlankMenuItem2.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowPassedBlankMenuItem2
            // 
            this.SelExpShowPassedBlankMenuItem2.Name = "SelExpShowPassedBlankMenuItem2";
            this.SelExpShowPassedBlankMenuItem2.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowPassedBlankMenuItem2.Text = "Show Passed";
            this.SelExpShowPassedBlankMenuItem2.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowTestedBlankMenuItem
            // 
            this.SelExpShowTestedBlankMenuItem.Name = "SelExpShowTestedBlankMenuItem";
            this.SelExpShowTestedBlankMenuItem.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowTestedBlankMenuItem.Text = "Show Tested";
            this.SelExpShowTestedBlankMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpHomozygoteTestMenuItem
            // 
            this.SelExpHomozygoteTestMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelExpShowFailedHomMenuItem,
            this.SelExpShowPassedHomMenuItem,
            this.SelExpShowTestedHomMenuItem});
            this.SelExpHomozygoteTestMenuItem.Name = "SelExpHomozygoteTestMenuItem";
            this.SelExpHomozygoteTestMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpHomozygoteTestMenuItem.Text = "Homozygote Test";
            // 
            // SelExpShowFailedHomMenuItem
            // 
            this.SelExpShowFailedHomMenuItem.Name = "SelExpShowFailedHomMenuItem";
            this.SelExpShowFailedHomMenuItem.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowFailedHomMenuItem.Text = "Show Failed";
            this.SelExpShowFailedHomMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowPassedHomMenuItem
            // 
            this.SelExpShowPassedHomMenuItem.Name = "SelExpShowPassedHomMenuItem";
            this.SelExpShowPassedHomMenuItem.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowPassedHomMenuItem.Text = "Show Passed";
            this.SelExpShowPassedHomMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowTestedHomMenuItem
            // 
            this.SelExpShowTestedHomMenuItem.Name = "SelExpShowTestedHomMenuItem";
            this.SelExpShowTestedHomMenuItem.Size = new System.Drawing.Size(142, 22);
            this.SelExpShowTestedHomMenuItem.Text = "Show Tested";
            this.SelExpShowTestedHomMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(191, 6);
            // 
            // SelExpShowAllMenuItem
            // 
            this.SelExpShowAllMenuItem.Name = "SelExpShowAllMenuItem";
            this.SelExpShowAllMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpShowAllMenuItem.Text = "Show All";
            this.SelExpShowAllMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowNAMenuItem
            // 
            this.SelExpShowNAMenuItem.Name = "SelExpShowNAMenuItem";
            this.SelExpShowNAMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpShowNAMenuItem.Text = "Show N/A";
            this.SelExpShowNAMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpShowItemDetailsMenuItem
            // 
            this.SelExpShowItemDetailsMenuItem.Name = "SelExpShowItemDetailsMenuItem";
            this.SelExpShowItemDetailsMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpShowItemDetailsMenuItem.Text = "Show Details";
            this.SelExpShowItemDetailsMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(191, 6);
            // 
            // SelExpRejectMinorityResultsMenuItem
            // 
            this.SelExpRejectMinorityResultsMenuItem.Name = "SelExpRejectMinorityResultsMenuItem";
            this.SelExpRejectMinorityResultsMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpRejectMinorityResultsMenuItem.Text = "Reject Minority Results";
            this.SelExpRejectMinorityResultsMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpStatusMenuItem
            // 
            this.SelExpStatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelExpSetStatusLoadMenuItem,
            this.SelExpSetStatusRejectedMenuItem});
            this.SelExpStatusMenuItem.Name = "SelExpStatusMenuItem";
            this.SelExpStatusMenuItem.Size = new System.Drawing.Size(194, 22);
            this.SelExpStatusMenuItem.Text = "Set Status";
            // 
            // SelExpSetStatusLoadMenuItem
            // 
            this.SelExpSetStatusLoadMenuItem.Name = "SelExpSetStatusLoadMenuItem";
            this.SelExpSetStatusLoadMenuItem.Size = new System.Drawing.Size(119, 22);
            this.SelExpSetStatusLoadMenuItem.Text = "Load";
            this.SelExpSetStatusLoadMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // SelExpSetStatusRejectedMenuItem
            // 
            this.SelExpSetStatusRejectedMenuItem.Name = "SelExpSetStatusRejectedMenuItem";
            this.SelExpSetStatusRejectedMenuItem.Size = new System.Drawing.Size(119, 22);
            this.SelExpSetStatusRejectedMenuItem.Text = "Rejected";
            this.SelExpSetStatusRejectedMenuItem.Click += new System.EventHandler(this.ExperimentMenuItem_Click);
            // 
            // ExperimentToolStrip
            // 
            this.ExperimentToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExperimentToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ExperimentToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExperimentCutoffToolStripButton,
            this.toolStripSeparator6,
            this.AllExperimentsDropDownButton,
            this.toolStripSeparator10,
            this.MaxRowsToolStripLabel,
            this.MaxRowsComboBox,
            this.SortColumnToolStripLabel,
            this.SortColumnComboBox,
            this.SortOrderToolStripLabel,
            this.SortOrderComboBox,
            this.RefreshSortToolStripButton});
            this.ExperimentToolStrip.Location = new System.Drawing.Point(3, 421);
            this.ExperimentToolStrip.Name = "ExperimentToolStrip";
            this.ExperimentToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ExperimentToolStrip.Size = new System.Drawing.Size(706, 25);
            this.ExperimentToolStrip.TabIndex = 1;
            this.ExperimentToolStrip.Text = "Experiment Tool Strip";
            // 
            // ExperimentCutoffToolStripButton
            // 
            this.ExperimentCutoffToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ExperimentCutoffToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ExperimentCutoffToolStripButton.Image")));
            this.ExperimentCutoffToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExperimentCutoffToolStripButton.Name = "ExperimentCutoffToolStripButton";
            this.ExperimentCutoffToolStripButton.Size = new System.Drawing.Size(54, 22);
            this.ExperimentCutoffToolStripButton.Text = "Cutoff...";
            this.ExperimentCutoffToolStripButton.ToolTipText = "Reject genotypes from experiments with success rates below cutoff";
            this.ExperimentCutoffToolStripButton.Click += new System.EventHandler(this.ExperimentCutoffToolStripButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // AllExperimentsDropDownButton
            // 
            this.AllExperimentsDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AllExperimentsDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllExpDupTestToolStripMenuItem,
            this.AllExpInhTestToolStripMenuItem,
            this.AllExpBlankTestToolStripMenuItem,
            this.AllExpHomozygTestToolStripMenuItem,
            this.toolStripSeparator4,
            this.AllExpShowAllToolStripMenuItem,
            this.AllExpShowNoResultToolStripMenuItem,
            this.toolStripSeparator5,
            this.AllExpRejectMinorityToolStripMenuItem,
            this.AllExpSetStatusMenuItem});
            this.AllExperimentsDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("AllExperimentsDropDownButton.Image")));
            this.AllExperimentsDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AllExperimentsDropDownButton.Name = "AllExperimentsDropDownButton";
            this.AllExperimentsDropDownButton.Size = new System.Drawing.Size(101, 22);
            this.AllExperimentsDropDownButton.Text = "All Experiments";
            this.AllExperimentsDropDownButton.DropDownOpening += new System.EventHandler(this.AllExperimentsDropDownButton_DropDownOpening);
            // 
            // AllExpDupTestToolStripMenuItem
            // 
            this.AllExpDupTestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllExpFailedDupToolStripMenuItem,
            this.AllExpPassedDupToolStripMenuItem,
            this.AllExpTestedDupToolStripMenuItem});
            this.AllExpDupTestToolStripMenuItem.Name = "AllExpDupTestToolStripMenuItem";
            this.AllExpDupTestToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpDupTestToolStripMenuItem.Text = "Duplicate Test";
            // 
            // AllExpFailedDupToolStripMenuItem
            // 
            this.AllExpFailedDupToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AllExpFailedDupToolStripMenuItem.Name = "AllExpFailedDupToolStripMenuItem";
            this.AllExpFailedDupToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpFailedDupToolStripMenuItem.Text = "Show Failed";
            this.AllExpFailedDupToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AllExpFailedDupToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpPassedDupToolStripMenuItem
            // 
            this.AllExpPassedDupToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AllExpPassedDupToolStripMenuItem.Name = "AllExpPassedDupToolStripMenuItem";
            this.AllExpPassedDupToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpPassedDupToolStripMenuItem.Text = "Show Passed";
            this.AllExpPassedDupToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpTestedDupToolStripMenuItem
            // 
            this.AllExpTestedDupToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AllExpTestedDupToolStripMenuItem.Name = "AllExpTestedDupToolStripMenuItem";
            this.AllExpTestedDupToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpTestedDupToolStripMenuItem.Text = "Show Tested";
            this.AllExpTestedDupToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpInhTestToolStripMenuItem
            // 
            this.AllExpInhTestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllExpFailedInhToolStripMenuItem,
            this.AllExpPassedInhToolStripMenuItem,
            this.AllExpTestedInhToolStripMenuItem,
            this.AllExpFailedInhTriosToolStripMenuItem});
            this.AllExpInhTestToolStripMenuItem.Name = "AllExpInhTestToolStripMenuItem";
            this.AllExpInhTestToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpInhTestToolStripMenuItem.Text = "Inheritance Test";
            // 
            // AllExpFailedInhToolStripMenuItem
            // 
            this.AllExpFailedInhToolStripMenuItem.Name = "AllExpFailedInhToolStripMenuItem";
            this.AllExpFailedInhToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.AllExpFailedInhToolStripMenuItem.Text = "Show Failed";
            this.AllExpFailedInhToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpPassedInhToolStripMenuItem
            // 
            this.AllExpPassedInhToolStripMenuItem.Name = "AllExpPassedInhToolStripMenuItem";
            this.AllExpPassedInhToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.AllExpPassedInhToolStripMenuItem.Text = "Show Passed";
            this.AllExpPassedInhToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpTestedInhToolStripMenuItem
            // 
            this.AllExpTestedInhToolStripMenuItem.Name = "AllExpTestedInhToolStripMenuItem";
            this.AllExpTestedInhToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.AllExpTestedInhToolStripMenuItem.Text = "Show Tested";
            this.AllExpTestedInhToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpFailedInhTriosToolStripMenuItem
            // 
            this.AllExpFailedInhTriosToolStripMenuItem.Name = "AllExpFailedInhTriosToolStripMenuItem";
            this.AllExpFailedInhTriosToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.AllExpFailedInhTriosToolStripMenuItem.Text = "Show Failed Trios";
            this.AllExpFailedInhTriosToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpBlankTestToolStripMenuItem
            // 
            this.AllExpBlankTestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllExpFailedBlankToolStripMenuItem,
            this.AllExpPassedBlankToolStripMenuItem,
            this.AllExpTestedBlankToolStripMenuItem});
            this.AllExpBlankTestToolStripMenuItem.Name = "AllExpBlankTestToolStripMenuItem";
            this.AllExpBlankTestToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpBlankTestToolStripMenuItem.Text = "Blank Test";
            // 
            // AllExpFailedBlankToolStripMenuItem
            // 
            this.AllExpFailedBlankToolStripMenuItem.Name = "AllExpFailedBlankToolStripMenuItem";
            this.AllExpFailedBlankToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpFailedBlankToolStripMenuItem.Text = "Show Failed";
            this.AllExpFailedBlankToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpPassedBlankToolStripMenuItem
            // 
            this.AllExpPassedBlankToolStripMenuItem.Name = "AllExpPassedBlankToolStripMenuItem";
            this.AllExpPassedBlankToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpPassedBlankToolStripMenuItem.Text = "Show Passed";
            this.AllExpPassedBlankToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpTestedBlankToolStripMenuItem
            // 
            this.AllExpTestedBlankToolStripMenuItem.Name = "AllExpTestedBlankToolStripMenuItem";
            this.AllExpTestedBlankToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpTestedBlankToolStripMenuItem.Text = "Show Tested";
            this.AllExpTestedBlankToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpHomozygTestToolStripMenuItem
            // 
            this.AllExpHomozygTestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllExpFailedHomozygToolStripMenuItem,
            this.AllExpPassedHomozygToolStripMenuItem,
            this.AllExpTestedHomozygToolStripMenuItem});
            this.AllExpHomozygTestToolStripMenuItem.Name = "AllExpHomozygTestToolStripMenuItem";
            this.AllExpHomozygTestToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpHomozygTestToolStripMenuItem.Text = "Homozygote Test";
            // 
            // AllExpFailedHomozygToolStripMenuItem
            // 
            this.AllExpFailedHomozygToolStripMenuItem.Name = "AllExpFailedHomozygToolStripMenuItem";
            this.AllExpFailedHomozygToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpFailedHomozygToolStripMenuItem.Text = "Show Failed";
            this.AllExpFailedHomozygToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpPassedHomozygToolStripMenuItem
            // 
            this.AllExpPassedHomozygToolStripMenuItem.Name = "AllExpPassedHomozygToolStripMenuItem";
            this.AllExpPassedHomozygToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpPassedHomozygToolStripMenuItem.Text = "Show Passed";
            this.AllExpPassedHomozygToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpTestedHomozygToolStripMenuItem
            // 
            this.AllExpTestedHomozygToolStripMenuItem.Name = "AllExpTestedHomozygToolStripMenuItem";
            this.AllExpTestedHomozygToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.AllExpTestedHomozygToolStripMenuItem.Text = "Show Tested";
            this.AllExpTestedHomozygToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(200, 6);
            // 
            // AllExpShowAllToolStripMenuItem
            // 
            this.AllExpShowAllToolStripMenuItem.Name = "AllExpShowAllToolStripMenuItem";
            this.AllExpShowAllToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpShowAllToolStripMenuItem.Text = "Show All";
            this.AllExpShowAllToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpShowNoResultToolStripMenuItem
            // 
            this.AllExpShowNoResultToolStripMenuItem.Name = "AllExpShowNoResultToolStripMenuItem";
            this.AllExpShowNoResultToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpShowNoResultToolStripMenuItem.Text = "Show N/A";
            this.AllExpShowNoResultToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(200, 6);
            // 
            // AllExpRejectMinorityToolStripMenuItem
            // 
            this.AllExpRejectMinorityToolStripMenuItem.Name = "AllExpRejectMinorityToolStripMenuItem";
            this.AllExpRejectMinorityToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpRejectMinorityToolStripMenuItem.Text = "Reject Minority Results...";
            this.AllExpRejectMinorityToolStripMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpSetStatusMenuItem
            // 
            this.AllExpSetStatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllExpSetStatusLoadMenuItem,
            this.AllExpSetStatusRejectedMenuItem});
            this.AllExpSetStatusMenuItem.Name = "AllExpSetStatusMenuItem";
            this.AllExpSetStatusMenuItem.Size = new System.Drawing.Size(203, 22);
            this.AllExpSetStatusMenuItem.Text = "Set Status";
            // 
            // AllExpSetStatusLoadMenuItem
            // 
            this.AllExpSetStatusLoadMenuItem.Name = "AllExpSetStatusLoadMenuItem";
            this.AllExpSetStatusLoadMenuItem.Size = new System.Drawing.Size(119, 22);
            this.AllExpSetStatusLoadMenuItem.Text = "Load";
            this.AllExpSetStatusLoadMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // AllExpSetStatusRejectedMenuItem
            // 
            this.AllExpSetStatusRejectedMenuItem.Name = "AllExpSetStatusRejectedMenuItem";
            this.AllExpSetStatusRejectedMenuItem.Size = new System.Drawing.Size(119, 22);
            this.AllExpSetStatusRejectedMenuItem.Text = "Rejected";
            this.AllExpSetStatusRejectedMenuItem.Click += new System.EventHandler(this.AllExperimentsMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // MaxRowsToolStripLabel
            // 
            this.MaxRowsToolStripLabel.Name = "MaxRowsToolStripLabel";
            this.MaxRowsToolStripLabel.Size = new System.Drawing.Size(63, 22);
            this.MaxRowsToolStripLabel.Text = "Max Rows:";
            // 
            // MaxRowsComboBox
            // 
            this.MaxRowsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaxRowsComboBox.Items.AddRange(new object[] {
            "100",
            "200",
            "300",
            "400",
            "500",
            "1000",
            "2000",
            "3000",
            "4000",
            "5000",
            "10000",
            "50000"});
            this.MaxRowsComboBox.Name = "MaxRowsComboBox";
            this.MaxRowsComboBox.Size = new System.Drawing.Size(80, 25);
            // 
            // SortColumnToolStripLabel
            // 
            this.SortColumnToolStripLabel.Name = "SortColumnToolStripLabel";
            this.SortColumnToolStripLabel.Size = new System.Drawing.Size(77, 22);
            this.SortColumnToolStripLabel.Text = "Sort Column:";
            // 
            // SortColumnComboBox
            // 
            this.SortColumnComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SortColumnComboBox.Name = "SortColumnComboBox";
            this.SortColumnComboBox.Size = new System.Drawing.Size(100, 25);
            // 
            // SortOrderToolStripLabel
            // 
            this.SortOrderToolStripLabel.Name = "SortOrderToolStripLabel";
            this.SortOrderToolStripLabel.Size = new System.Drawing.Size(64, 22);
            this.SortOrderToolStripLabel.Text = "Sort Order:";
            // 
            // SortOrderComboBox
            // 
            this.SortOrderComboBox.Items.AddRange(new object[] {
            "Asc",
            "Desc"});
            this.SortOrderComboBox.Name = "SortOrderComboBox";
            this.SortOrderComboBox.Size = new System.Drawing.Size(75, 25);
            // 
            // RefreshSortToolStripButton
            // 
            this.RefreshSortToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RefreshSortToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshSortToolStripButton.Image")));
            this.RefreshSortToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshSortToolStripButton.Name = "RefreshSortToolStripButton";
            this.RefreshSortToolStripButton.Size = new System.Drawing.Size(50, 22);
            this.RefreshSortToolStripButton.Text = "Refresh";
            this.RefreshSortToolStripButton.Click += new System.EventHandler(this.RefreshSortToolStripButton_Click);
            // 
            // DataPollTimer
            // 
            this.DataPollTimer.Tick += new System.EventHandler(this.DataPollTimer_Tick);
            // 
            // MyStatusBar
            // 
            this.MyStatusBar.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MyStatusBar.Location = new System.Drawing.Point(0, 8);
            this.MyStatusBar.Name = "MyStatusBar";
            this.MyStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.TextStatusBarPanel,
            this.PrepTimeStatusBarPanel,
            this.CompTimeStatusBarPanel,
            this.OutputTimeStatusBarPanel});
            this.MyStatusBar.ShowPanels = true;
            this.MyStatusBar.Size = new System.Drawing.Size(984, 24);
            this.MyStatusBar.TabIndex = 0;
            // 
            // TextStatusBarPanel
            // 
            this.TextStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.TextStatusBarPanel.Name = "TextStatusBarPanel";
            this.TextStatusBarPanel.Text = "<status>";
            this.TextStatusBarPanel.Width = 557;
            // 
            // PrepTimeStatusBarPanel
            // 
            this.PrepTimeStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.PrepTimeStatusBarPanel.Name = "PrepTimeStatusBarPanel";
            this.PrepTimeStatusBarPanel.Text = "Prep time 00:00:00";
            this.PrepTimeStatusBarPanel.Width = 137;
            // 
            // CompTimeStatusBarPanel
            // 
            this.CompTimeStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.CompTimeStatusBarPanel.Name = "CompTimeStatusBarPanel";
            this.CompTimeStatusBarPanel.Text = "QC time 00:00:00";
            this.CompTimeStatusBarPanel.Width = 123;
            // 
            // OutputTimeStatusBarPanel
            // 
            this.OutputTimeStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.OutputTimeStatusBarPanel.Name = "OutputTimeStatusBarPanel";
            this.OutputTimeStatusBarPanel.Text = "Output time 00:00:00";
            this.OutputTimeStatusBarPanel.Width = 150;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.MyStatusBar);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 497);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(984, 32);
            this.BottomPanel.TabIndex = 1;
            // 
            // SessionForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(984, 529);
            this.Controls.Add(this.ExperimentPanel);
            this.Controls.Add(this.ItemExperimentSplitter);
            this.Controls.Add(this.ItemPanel);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.TopPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SessionForm";
            this.Text = "Session";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SessionForm_Closing);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.SessionToolStrip.ResumeLayout(false);
            this.SessionToolStrip.PerformLayout();
            this.ItemPanel.ResumeLayout(false);
            this.ResultItemPanel.ResumeLayout(false);
            this.ResultItemGroupBox.ResumeLayout(false);
            this.ResultItemGroupBox.PerformLayout();
            this.MyItemContextMenuStrip.ResumeLayout(false);
            this.ItemToolStrip.ResumeLayout(false);
            this.ItemToolStrip.PerformLayout();
            this.ControlItemPanel.ResumeLayout(false);
            this.ControlItemGroupBox.ResumeLayout(false);
            this.MyControlItemContextMenuStrip.ResumeLayout(false);
            this.ExperimentPanel.ResumeLayout(false);
            this.ExperimentGroupBox.ResumeLayout(false);
            this.ExperimentGroupBox.PerformLayout();
            this.MyExperimentContextMenuStrip.ResumeLayout(false);
            this.ExperimentToolStrip.ResumeLayout(false);
            this.ExperimentToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextStatusBarPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrepTimeStatusBarPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompTimeStatusBarPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputTimeStatusBarPanel)).EndInit();
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion



		public void Open(StartMode mode, string freeName)
		{
            string sessionName;
			StartSessionForm startForm;

            try
            {
                MyForStreaming = false;
                MySessionSettings = null;
                sessionName = "";
                this.Cursor = Cursors.WaitCursor;
                //Create the temporary database tables which are used to communicate data.
                MyDataServer.CreateCommonTempTables();
                //Fetch the experiment statistics column names and add to the combo box.
                this.SetExperimentColumns();
                //Set the sort order.
                SortOrderComboBox.SelectedIndex = 0;
                //Set max experiment rows.
                MaxRowsComboBox.SelectedIndex = 6;
                //Set form unavailable for the time being.
                SetFormStatus(FormStatusMode.Unstoppable, "", null, null, null);
                this.Cursor = Cursors.Default;
                startForm = new StartSessionForm(MyParentForm, MyDataServer, MyConfiguration);
                //Bring up the form for selecting data.
                if (startForm.StartSession(out MySessionSettings, out sessionName, out MyFilters, out MyForStreaming, mode))
                {
                    MyDataServer.GenotypeStatusChange += new EventHandler(GenotypeStatusChange_Fire);
                    MySessionSettings.SettingsChanged += new EventHandler(SettingsChange_Fire);
                    MyDataServer.SetDataAsyncEventHandlers(new EventHandler(DataServer_DataReady), new ThreadExceptionEventHandler(DataServer_DataError));
                    this.Text = (sessionName != "") ? sessionName : freeName;  //Set the given name or a free name if unnamed.
                    this.Show();
                    RefreshResults();
                }
                else
                {
                    //The user cancelled.
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Cannot open session.", this);
            }
		}

        public DataTable GetAllGenotypes(StringCollection includedItemTypes, StringCollection includedGenotypeStatuses, StringCollection includedItems, StringCollection includedExperiments)
        {
            return MyDataServer.GetAllResults(MySessionSettings, includedItemTypes, includedGenotypeStatuses, includedItems, includedExperiments);
        }

        public DataTable GetAllItems()
        {
            return MyDataServer.GetAllItems();
        }

        public DataTable GetAllExperiments()
        {
            return MyDataServer.GetResultExperiments();
        }

        private void PrepareExperimentQueue()
        {
            DataSet resultSet;
            DataTable experimentTable;
            int[] experimentID;

            //Get the experiments from the data server.
            resultSet = MyDataServer.PopDeliveredDataSet();
            experimentTable = resultSet.Tables[0];

            experimentID = new int[experimentTable.Rows.Count];
            for (int i = 0; i < experimentID.GetLength(0); i++)
            {
                experimentID[i] = Convert.ToInt32(experimentTable.Rows[i]["identifiable_id"]);
            }

            //Create a queue with the experiments.
            MyExperimentQueue = new ExperimentQueue(experimentID);

            MyPrepEndTime = DateTime.Now;
            MyComputeStartTime = DateTime.Now;

            SetFormStatus(FormStatusMode.Stoppable, "Preparations finished" + MyExperimentQueue.PercentFinished.ToString() + "%)", GetElapsedTime(MyPrepStartTime, MyPrepEndTime), null, null);
            //Start testing.
            TestExperimentBatch(true);
        }


        private void TestExperimentBatch(bool initiate)
        {
            //This method is repeatedly called until all batches have been analyzed.
            //The initiate flag should be set to true the first time the method is called,
            //and false all subsequent times.
            int[] experimentID;
            int batchSize;

            MyDataReadyFlag = false;   //No data yet.
            batchSize = Convert.ToInt32(MyConfiguration.Get("ExperimentBatchSize").ToString());

            SetFormStatus(FormStatusMode.Stoppable, "Performing QC...(" + MyExperimentQueue.PercentFinished.ToString() + "%)", null, null, null);
            experimentID = MyExperimentQueue.GetNextBatch(batchSize);
            if (experimentID != null)
            {
                MyAsyncExecutionMode = AsyncExecutionMode.PerformTest;
                MyDataServer.TestExperimentBatch(MySessionSettings, MyFilters, experimentID, initiate, DataServer.Mode.Async);
                DataPollTimer.Enabled = true;
            }
            else
            {
                MyComputeEndTime = DateTime.Now;

                SetFormStatus(FormStatusMode.Unstoppable, "QC completed", null, GetElapsedTime(MyComputeStartTime, MyComputeEndTime), null);

                //No more experiments to analyze. Make the data server prepare the results or stream results to file.
                if (MyForStreaming)
                {
                    StreamTestResults();
                }
                else
                {
                    FetchTestResults();              
                }
            }

        }

        private void ShowTestResults()
        {
            AdvancedListView2 currentList;

            MyOutputStartTime = DateTime.Now;

            //Get the test results from the data server.
            MyResultSet = MyDataServer.PopDeliveredDataSet();

            if (MyResultSet != null)
            {
                for (int currentTable = 0; currentTable < 3; currentTable++)
                {
                    //Select correct list according to the order in the result set.
                    switch (currentTable)
                    {
                        case DataServer.EXPERIMENT_STAT_TABLE_INDEX:
                            currentList = ExperimentList;
                            break;
                        case DataServer.ITEM_STAT_TABLE_INDEX:
                            currentList = ResultItemList;
                            break;
                        case DataServer.CONTROL_ITEM_STAT_TABLE_INDEX:
                            currentList = ControlItemList;
                            break;
                        default:
                            currentList = ExperimentList;
                            break;
                    }

                    //Display the contents of the data table in the list.
                    currentList.ShowFormattedTable(MyResultSet.Tables[currentTable]);
                }
            }

            //Must make the lists visible before the end time is set.
            ExperimentList.Visible = true;
            ResultItemList.Visible = true;
            ControlItemList.Visible = true;

            MyOutputEndTime = DateTime.Now;

            SetFormStatus(FormStatusMode.AllowInput, "Ready", null, null, GetElapsedTime(MyOutputStartTime, MyOutputEndTime));
        }

        private void StreamTestResults()
        {
            ReportGenerator generator;

            SetFormStatus(FormStatusMode.Unstoppable, "Writing output to disk...", null, null, null);
            MyOutputStartTime = DateTime.Now;
            generator = new ReportGenerator(MyDataServer);
            generator.WriteFromServer(MyAutomaticReportSettings, MySessionSettings);

            MyOutputEndTime = DateTime.Now;

            SetFormStatus(FormStatusMode.AllowInput, "Ready", null, null, GetElapsedTime(MyOutputStartTime, MyOutputEndTime));

            MessageManager.ShowInformation("The report was successfully written to file.");
            //Close form after streaming.
            this.Close();
        }

        private void FetchTestResults()
        {
            int experimentMaxRows;
            string experimentOrderClause;
            FormattedColumn sortColumn;

            MyDataReadyFlag = false;   //No data yet.
            SetFormStatus(FormStatusMode.Stoppable, "Outputting results to screen...", null, null, TIMER_BLANK);

            MyAsyncExecutionMode = AsyncExecutionMode.GetResults;
            if (MyPreferences.Get(Preferences.Bools.ExperimentFilter))
            {
                experimentMaxRows = Convert.ToInt32(MaxRowsComboBox.Text);
                sortColumn = (FormattedColumn)SortColumnComboBox.SelectedItem;
                experimentOrderClause = "[" + sortColumn.FullName + "] " + SortOrderComboBox.Text + " ";
                MyDataServer.GetTestResults(DataServer.Mode.Async, experimentMaxRows, experimentOrderClause);
            }
            else
            {
                MyDataServer.GetTestResults(DataServer.Mode.Async, int.MaxValue, null);
            } 
            DataPollTimer.Enabled = true; 
        }

		public void Save()
		{
			SessionNameForm nameForm;
			string projectName, sessionName, description;
	
			nameForm = new SessionNameForm(MyDataServer);
			if (!nameForm.GetName(out projectName, out sessionName, out description, "Session name", "Saved session"))
			{
				//User cancelled.
				return;
			}

			this.Cursor = Cursors.WaitCursor;
			MyDataServer.SaveNormalSession(MySessionSettings, projectName, sessionName, description);
            MyDataServer.LogResultPlatesInSession(sessionName, "Save session");
			this.Cursor = Cursors.Default;
			this.Text = sessionName;
		}

		public void Approve()
		{
			SessionNameForm nameForm;
			string projectName, sessionName, description;
	
			if (MessageManager.ShowQuestion("Are you sure you want to approve the current results (the session will close after approval)?", 
				this) != DialogResult.Yes)
			{
				return;
			}

			nameForm = new SessionNameForm(MyDataServer);
			if (!nameForm.GetName(out projectName, out sessionName, out description, "Session name", "Approved results"))
			{
				//User cancelled.
				return;
			}

			this.Cursor = Cursors.WaitCursor;
			MyDataServer.SaveApprovedSession(MySessionSettings, projectName, sessionName, description);
			this.Cursor = Cursors.Default;
			this.Close();
		}

		public void EditSettings()
		{
			SessionSettingsForm settingsForm;

			settingsForm = new SessionSettingsForm();
			//Set form status after closing settings form with OK
			//(recalculation depends on changes and preferences).
			if (settingsForm.GetSettings(MySessionSettings) == DialogResult.OK)
			{
				SetFormStatus(FormStatusMode.AllowInput, "Ready", null, null, null);	
			}
		}

		public void InspectPlates()
		{
			PlateForm plateDisplayForm;

			plateDisplayForm = new PlateForm(MyDataServer, MySessionSettings);
			plateDisplayForm.ShowDialog();
		}

		public void PickRerunItems()
		{
			RerunForm rerun;

			rerun = new RerunForm(MySessionSettings, MyDataServer);
			rerun.ShowDialog(this);
		}

        public void SaveInternalReport()
        {
            Project currentProject;
            InternalReportManager.BackgroundUploadInternalReport backgroundUploadInternalReport;
            BackgroundWorkerDialog workerDialog;
            String itemSetting, experimentSetting, internalReportName;
            GetInternalReportNameDialog getInternalReportNameDialog;
            currentProject = GetCurrentProject();

            if (currentProject == null)
            {
                MessageBox.Show("Error, the current project could not be determined, upload canceled!", "Upload error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            itemSetting = MySessionSettings.Get(SessionSettings.Strings.ViewMode);
            experimentSetting = MySessionSettings.Get(SessionSettings.Strings.ExperimentMode);
            if (experimentSetting.ToUpper() != "MARKER")
            {
                MessageBox.Show("Error, the report must contain information for markers, upload canceled!", "Upload error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            getInternalReportNameDialog = new GetInternalReportNameDialog(currentProject.Name);
            if (getInternalReportNameDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            internalReportName = getInternalReportNameDialog.GetName();
            workerDialog = new BackgroundWorkerDialog();
            backgroundUploadInternalReport = new InternalReportManager.BackgroundUploadInternalReport
                (currentProject, internalReportName, MySessionSettings, workerDialog.Worker, MyDataServer,
                ExperimentList, ResultItemList, ControlItemList);
            workerDialog.Start();
        }

        private static Boolean IsNull(Object testObject)
        {
            return (testObject == null);
        }

        private static Boolean IsNotNull(Object testObject)
        {
            return !IsNull(testObject);
        }

        private Project GetCurrentProject()
        {
            ProjectCollection projects;
            String currentProjectName;
            ObjectServer oServer;
            oServer = new ObjectServer(MyDataServer);
            projects = oServer.GetProjects();
            currentProjectName = MyDataServer.GetSessionProject();
            if (currentProjectName == null)
            {
                return null;
            }
            foreach (Project project in projects)
            {
                if (project.Name == currentProjectName)
                {
                    return project;
                }
            }
            return null;
        }

		public void SaveReport()
		{
            ReportSettings settings;
            ReportGenerator generator;

            settings = this.GetReportSettings();
            
            if (settings == null)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            generator = new ReportGenerator(MyDataServer);
            if (MyPreferences.Get(Preferences.Bools.ExperimentFilter))
            {
                generator.WriteFromServer(settings, MySessionSettings);
            }
            else
            {
                generator.WriteFromClient(settings, MySessionSettings, ExperimentList, ResultItemList, ControlItemList);
            }
            this.Cursor = Cursors.Default;

		}

		public void ShowMarkerAnnotations()
		{
			StaticListForm list;
			CheckedListForm selectForm;
			DataTable termsTable, annotationTable;
			Identifiable [] annotationTypes, selectedAnnotationTypes;
			object [] selectedAnnotationTypeObjects = null;

			//Get available annotation types.
			this.Cursor = Cursors.WaitCursor;
			termsTable = MyDataServer.GetAnnotationTypes();
			annotationTypes = new Identifiable[termsTable.Rows.Count];
			for (int i = 0 ; i < termsTable.Rows.Count ; i++)
			{
				annotationTypes[i] = new Identifiable(Convert.ToInt32(termsTable.Rows[i][0]), 
					termsTable.Rows[i][1].ToString());

			}
			this.Cursor = Cursors.Default;

			//Let the user pick the desired annotation types.
			selectForm = new CheckedListForm();
			if (selectForm.DisplayItemsDialog("Select Annotation Types", annotationTypes, out selectedAnnotationTypeObjects) == DialogResult.Cancel)
			{
				return;
			}

			if (selectedAnnotationTypeObjects == null)
			{
				MessageManager.ShowInformation("Nothing selected.", this);
				return;
			}

			if (selectedAnnotationTypeObjects.GetLength(0) < 1)
			{
				MessageManager.ShowInformation("Nothing selected.", this);
				return;				
			}

			this.Cursor = Cursors.WaitCursor;
			selectedAnnotationTypes = new Identifiable[selectedAnnotationTypeObjects.GetLength(0)];
			for (int i = 0 ; i < selectedAnnotationTypeObjects.GetLength(0) ; i++)
			{
				selectedAnnotationTypes[i] = (Identifiable)selectedAnnotationTypeObjects[i];
			}

			//Show annotations.
			annotationTable = MyDataServer.GetMarkerAnnotations(selectedAnnotationTypes, MySessionSettings);
			this.Cursor = Cursors.Default;

			list = new StaticListForm();
			list.ShowInformation("Marker Annotations", annotationTable);
		}

        public void ShowPendingChanges()
        {
            StaticListForm list;
            DataTable pendingGenotypeTable;

            pendingGenotypeTable = MyDataServer.GetPendingGenotypes();

            list = new StaticListForm();
            list.ShowInformation("Pending Status Changes", pendingGenotypeTable);
        }

		public bool IsBusy
		{
			get
			{
				return StopToolStripButton.Enabled;
			}
		}

        public bool IsAborted
        {
            get
            {
                return MyIsAborted;
            }
        }

		public bool IsSaveable
		{
			get
			{
				return SaveSessionToolStripButton.Enabled;
			}
		}

		public bool IsPending
		{
			get
			{
				return SubmitStatusToolStripButton.Enabled;
			}

		}

		public SessionSettings Settings
		{
			get
			{
				return MySessionSettings;
			}
		}

		public void ShowResultMatrix(ResultTable.Mode mode)
		{
            StaticListForm list;
            ResultTable result;

            this.Cursor = Cursors.WaitCursor;
            result = new ResultTable(MyDataServer, mode, "");
            list = new StaticListForm();
            this.Cursor = Cursors.Default;
            list.ShowMatrix("Results", result.GetResultMatrix());
        }
        
		public void ShowResultPairwise(bool includeEmptyCombinations)
		{
			StaticListForm list;
			DataTable alleleTable;

			this.Cursor = Cursors.WaitCursor;
			//Get data.
			if (includeEmptyCombinations)
			{
				alleleTable = MyDataServer.GetApprovedResultFormattedOuter(true, "");
			}
			else
			{
				alleleTable = MyDataServer.GetApprovedResultFormatted();			
			}
			this.Cursor = Cursors.Default;

			list = new StaticListForm();
			list.ShowInformation("Results", alleleTable);
		}

        public void ShowSessionPlates()
        {
            StaticListForm list;
            DataTable plateTable;

            this.Cursor = Cursors.WaitCursor;
            //Get data.
            plateTable = MyDataServer.GetPlatesInSelection();
            this.Cursor = Cursors.Default;

            list = new StaticListForm();
            list.ShowInformation("Plates in Current Session", plateTable);
        }

        public void ShowSessionGroups()
        {
            StaticListForm list;
            DataTable groupTable;

            this.Cursor = Cursors.WaitCursor;
            //Get data.
            groupTable = MyDataServer.GetGroupsInSelection();
            this.Cursor = Cursors.Default;

            list = new StaticListForm();
            list.ShowInformation("Working Sets in Current Session", groupTable);
        }

        public void ShowSessionDetailedFilter()
        {
            StaticListForm list;
            DataTable selectionTable;

            this.Cursor = Cursors.WaitCursor;
            //Get data.
            selectionTable = MyDataServer.GetDetailedFilter();
            this.Cursor = Cursors.Default;

            list = new StaticListForm();
            list.ShowInformation("Detailed Filter in Current Session", selectionTable);
        }

		private void RefreshResults()
		{
            string experimentMode;
            DataServer.IdentifiableType experimentType;

            if (MyForStreaming)
            {
                //Automatic mode, get report settings from the user.
                MyAutomaticReportSettings = this.GetReportSettings();
                if (MyAutomaticReportSettings == null)
                {
                    this.Dispose();
                    return;
                }
            }

            MyIsAborted = false;
            MyPrepStartTime = DateTime.Now;
            SetFormStatus(FormStatusMode.Stoppable, "Preparing...", TIMER_BLANK, TIMER_BLANK, TIMER_BLANK);
            //Make the data server start extracting the IDs of the experiments.
            MyDataReadyFlag = false;
            MyAsyncExecutionMode = AsyncExecutionMode.GetExperiments;
            experimentMode = MySessionSettings.GetString(SessionSettings.Strings.ExperimentMode);
            experimentType = (experimentMode == "ASSAYS" ? DataServer.IdentifiableType.Assay : DataServer.IdentifiableType.Marker);
            MyDataServer.GetSelectedItems(experimentType, MyFilters, DataServer.Mode.Async);
            DataPollTimer.Enabled = true;
			MyNeedsRefresh = false;
		}


		public void AutoRefresh()
		{
			//Recalculate the results if needed and if the preferences are set for automatic
			//recalculation.
			if (MyPreferences.Get(Preferences.Bools.Recalculate) && MyNeedsRefresh)
			{
				RefreshResults();
			}
		}

        private string GetElapsedTime(DateTime startTime, DateTime endTime)
        {
            string time = "";
            TimeSpan diff = endTime.Subtract(startTime);

            time += (diff.Hours < 10 ? "0" + diff.Hours.ToString() : diff.Hours.ToString());
            time += ":";
            time += (diff.Minutes < 10 ? "0" + diff.Minutes.ToString() : diff.Minutes.ToString());
            time += ":";
            time += (diff.Seconds < 10 ? "0" + diff.Seconds.ToString() : diff.Seconds.ToString());

            return time;
        }

		private void SetFormStatus(FormStatusMode mode, string statusText, string newPrepTime, string newCompTime, string newOutputTime)
		{
            //Sets the enabled and visible properties of the controls in the form
            //to a state such that the form is ready for user input if allowInput is true, or
            //locked for user input due to calculations if allowInput is false.
            //Set the newPrepTime, newCompTime and newOutput time to null in order to leave them unchanged.

            SetFormStatusThreadSafeDelegate d;
            bool isPendingGenotypeStatus;
            bool allowInput;

            if (this.InvokeRequired)
            {
                d = new SetFormStatusThreadSafeDelegate(SetFormStatus);
                this.Invoke(d, new object[] { mode, statusText, newPrepTime, newCompTime, newOutputTime });
            }
            else
            {
                isPendingGenotypeStatus = MyDataServer.IsPendingGenotypeStatus();
                allowInput = (mode == FormStatusMode.AllowInput);

                ControlItemList.Visible = allowInput && !MyIsAborted;
                ResultItemList.Visible = allowInput && !MyIsAborted;
                ExperimentList.Visible = allowInput && !MyIsAborted;

                RefreshToolStripButton.Enabled = allowInput;
                SaveSessionToolStripButton.Enabled = allowInput ? !isPendingGenotypeStatus : false;
                SubmitStatusToolStripButton.Enabled = (allowInput && !MyIsAborted) ? isPendingGenotypeStatus : false;
                InspectChangesToolStripButton.Enabled = (allowInput && !MyIsAborted) ? isPendingGenotypeStatus : false;
                ClearStatusToolStripButton.Enabled = SubmitStatusToolStripButton.Enabled;
                StopToolStripButton.Enabled = (mode == FormStatusMode.Stoppable);
                SettingsToolStripButton.Enabled = allowInput;
                AnnotationsToolStripButton.Enabled = allowInput && !MyIsAborted;
                PlatesToolStripButton.Enabled = allowInput && !MyIsAborted;
                SelectionDropDownButton.Enabled = allowInput;
                RerunToolStripButton.Enabled = allowInput && !MyIsAborted && !isPendingGenotypeStatus;
                ShowResultsDropDownButton.Enabled = allowInput && !MyIsAborted;
                SaveReportToolStripButton.Enabled = (allowInput && !MyIsAborted) ? !isPendingGenotypeStatus : false;
                ApproveSessionToolStripButton.Enabled = (allowInput && !MyIsAborted) ? !isPendingGenotypeStatus : false;
                ExperimentToolStrip.Enabled = allowInput && !MyIsAborted;
                ItemToolStrip.Enabled = allowInput && !MyIsAborted;

                MaxRowsToolStripLabel.Visible = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
                MaxRowsComboBox.Visible = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
                SortColumnToolStripLabel.Visible = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
                SortColumnComboBox.Visible = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
                SortOrderToolStripLabel.Visible = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
                SortOrderComboBox.Visible = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
                RefreshSortToolStripButton.Visible = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
                ExperimentList.TheListView.HeaderStyle = MyPreferences.Get(Preferences.Bools.ExperimentFilter) ? ColumnHeaderStyle.Nonclickable : ColumnHeaderStyle.Clickable;

                TextStatusBarPanel.Text = statusText;
                if (newPrepTime != null) { PrepTimeStatusBarPanel.Text = "Prep time: " + newPrepTime; }
                if (newCompTime != null) { CompTimeStatusBarPanel.Text = "QC time: " + newCompTime; }
                if (newOutputTime != null) { OutputTimeStatusBarPanel.Text = "Output time: " + newOutputTime; }
                this.Refresh();
            }
		
		}

        private void SetExperimentColumns()
        {
            DataTable expStatTable;

            expStatTable = MyDataServer.GetExperimentStatColumns();
            SortColumnComboBox.Items.Clear();
            for (int i = 1; i < expStatTable.Columns.Count; i++)
            {
                SortColumnComboBox.Items.Add(new FormattedColumn(expStatTable.Columns[i].ColumnName));
            }
            if (SortColumnComboBox.Items.Count > 0)
            {
                SortColumnComboBox.SelectedIndex = 0;
            }
        }

        private ReportSettings GetReportSettings()
        {
            ReportForm repForm;
            ReportSettings repSettings;
            DataTable expmTable, itemTable;
            string[] expmColumns, itemColumns;
            bool isExperimentFilter;

            this.Cursor = Cursors.WaitCursor;
            expmTable = MyDataServer.GetExperimentStatColumns();
            itemTable = MyDataServer.GetItemStatColumns();

            expmColumns = new string[expmTable.Columns.Count - 1];
            itemColumns = new string[itemTable.Columns.Count - 1];

            for (int i = 1; i < expmTable.Columns.Count; i++)
            {
                expmColumns[i - 1] = expmTable.Columns[i].ColumnName;
            }
            for (int i = 1; i < itemTable.Columns.Count; i++)
            {
                itemColumns[i - 1] = itemTable.Columns[i].ColumnName;
            }
            isExperimentFilter = MyPreferences.Get(Preferences.Bools.ExperimentFilter);
            this.Cursor = Cursors.Default;

            repForm = new ReportForm();
            repSettings = repForm.GetReportSettings(this, expmColumns, itemColumns, MyDataServer, MyForStreaming || isExperimentFilter);

            return repSettings;
        }


        private void RejectMinority(int[] experimentID)
        {
            int majorityPercent;
            string prompt;
            GetNumberForm numberForm;

            prompt = "";
            prompt += "Inconsistent results among selected experiments will be rejected ";
            prompt += "when there is a winning majority. ";
            prompt += "Please select the percentage required for a majority call. ";

            numberForm = new GetNumberForm();

            if (numberForm.GetNumber(this, "Reject Minorities", 51, 100, 51, prompt, out majorityPercent))
            {
                this.Cursor = Cursors.WaitCursor;
                MyDataServer.RejectMinority(experimentID, majorityPercent);
                this.Cursor = Cursors.Default;
            }
        }


		private void ExperimentMenuItem_Click(object sender, System.EventArgs e)
		{
			GenotypeListForm gtList;
			StaticListForm list;
			UpdateableListForm.ListFetcher fetcher;
			GenotypeFetcher.TestRetrievalMethodDelegate testRetrievalDelegate;
			GenotypeFetcher.StringRetrievalMethodDelegate stringRetrievalDelegate;
			int selectedExperimentID;
			int [] experimentID;
			string selectedExperimentIdentifier, title;
			DataTable tempTable;

			try
			{
				experimentID = new Int32[ExperimentList.TheListView.SelectedItems.Count];
				this.Cursor = Cursors.WaitCursor;
				for (int i = 0; i < ExperimentList.TheListView.SelectedItems.Count; i++)
				{
					experimentID[i] = Convert.ToInt32(ExperimentList.TheListView.SelectedItems[i].Tag);	
				}
				this.Cursor = Cursors.Default;

				if (sender != SelExpRejectMinorityResultsMenuItem && sender != SelExpShowItemDetailsMenuItem 
                    && sender != SelExpSetStatusLoadMenuItem && sender != SelExpSetStatusRejectedMenuItem
                    && sender != SelExpShowFailedInhTestTriosMenuItem)
				{
					//For these menu alternatives we will be showing genotypes.
					gtList = new GenotypeListForm(MyDataServer);

					if (sender == SelExpShowFailedDupMenuItem)
					{
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetDuplicateTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Failure);
						gtList.ShowInformation("Duplicate Test (Failure)", fetcher); 
					}
					else if (sender == SelExpShowPassedDupMenuItem)
					{
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetDuplicateTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Success);
						gtList.ShowInformation("Duplicate Test (Success)", fetcher); 				
					}
					else if (sender == SelExpShowTestedDupMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetDuplicateTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.All);
						gtList.ShowInformation("Duplicate Test (Tested)", fetcher); 				
					}
					else if (sender == SelExpShowFailedInhMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetInheritanceTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Failure);
						gtList.ShowInformation("Inheritance Test (Failure)", fetcher); 				
					}
					else if (sender == SelExpShowPassedInhMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetInheritanceTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Success);
						gtList.ShowInformation("Inheritance Test (Success)", fetcher); 				
					}
					else if (sender == SelExpShowTestedInhMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetInheritanceTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.All);
						gtList.ShowInformation("Inheritance Test (Tested)", fetcher); 				
					}
					else if (sender == SelExpShowFailedBlankMenuItem2)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetBlankTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Failure);
						gtList.ShowInformation("Blank Test (Failure)", fetcher); 				
					}
					else if (sender == SelExpShowPassedBlankMenuItem2)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetBlankTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Success);
                        gtList.ShowInformation("Blank Test (Success)", fetcher); 				
					}
					else if (sender == SelExpShowTestedBlankMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetBlankTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.All);
                        gtList.ShowInformation("Blank Test (Tested)", fetcher); 				
					}
					else if (sender == SelExpShowFailedHomMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetHomozygoteTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Failure);
                        gtList.ShowInformation("Homozygote Test (Failure)", fetcher); 				
					}
					else if (sender == SelExpShowPassedHomMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetHomozygoteTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.Success);
                        gtList.ShowInformation("Homozygote Test (Success)", fetcher); 				
					}
					else if (sender == SelExpShowTestedHomMenuItem)
					{ 
						testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetHomozygoteTest);
						fetcher = new GenotypeFetcher(testRetrievalDelegate, experimentID, DataServer.TestSuccess.All);
                        gtList.ShowInformation("Homozygote Test (Tested)", fetcher); 				
					}
					else if (sender == SelExpShowAllMenuItem)
					{ 
						stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllExperimentGenotypes);
						fetcher = new GenotypeFetcher(stringRetrievalDelegate, experimentID, null);
                        gtList.ShowInformation("All Genotypes", fetcher);
					}
					else if (sender == SelExpShowNAMenuItem)
					{ 
						stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllExperimentGenotypes);
						fetcher = new GenotypeFetcher(stringRetrievalDelegate, experimentID, "alleles = 'N/A'");
                        gtList.ShowInformation("All N/A Genotypes", fetcher);				
					}
				}
                else if (sender == SelExpShowFailedInhTestTriosMenuItem)
                {
                    list = new StaticListForm();
                    this.Cursor = Cursors.WaitCursor;
                    tempTable = MyDataServer.GetFailedInheritanceTrios(experimentID);
                    this.Cursor = Cursors.Default;
                    list.ShowInformation("Failed trios for selected experiments", tempTable);
                }
                else if (sender == SelExpShowItemDetailsMenuItem)
                {
                    selectedExperimentID = experimentID[0];
                    selectedExperimentIdentifier = ExperimentList.TheListView.SelectedItems[0].Text;

                    list = new StaticListForm();
                    this.Cursor = Cursors.WaitCursor;
                    tempTable = MyDataServer.GetItemStat(experimentID[0]);
                    this.Cursor = Cursors.Default;
                    title = (MySessionSettings.GetString(SessionSettings.Strings.ExperimentMode) == "MARKER" ? "Details for Marker " :
                        "Details for Assay ") + selectedExperimentIdentifier;
                    list.ShowInformation(title, tempTable);
                }
                else if (sender == SelExpRejectMinorityResultsMenuItem)
                {
                    RejectMinority(experimentID);
                }

                else if (sender == SelExpSetStatusLoadMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to change status to 'load' for genotypes from the selected "
                        + experimentID.GetLength(0).ToString() + " experiment(s)?") == DialogResult.No)
                    {
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    MyDataServer.UpdateExperimentStatus(experimentID, "LOAD");
                    this.Cursor = Cursors.Default;
                }
                else if (sender == SelExpSetStatusRejectedMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to reject all results from the selected "
                        + experimentID.GetLength(0).ToString() + " experiment(s)?") == DialogResult.No)
                    {
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    MyDataServer.UpdateExperimentStatus(experimentID, "REJECTED");
                    this.Cursor = Cursors.Default;
                }
                AutoRefresh();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when performing operation on experiment(s).", this);
			}

		}


        private void AllExperimentsMenuItem_Click(object sender, System.EventArgs e)
        {
            GenotypeListForm gtList;
            UpdateableListForm.ListFetcher fetcher;
            GenotypeFetcher.TestRetrievalMethodDelegate testRetrievalDelegate;
            GenotypeFetcher.StringRetrievalMethodDelegate stringRetrievalDelegate;
            StaticListForm list;
            DataTable tempTable;

            try
            {
                if (sender != AllExpRejectMinorityToolStripMenuItem && sender != AllExpSetStatusLoadMenuItem 
                    && sender != AllExpSetStatusRejectedMenuItem && sender != AllExpFailedInhTriosToolStripMenuItem)
                {
                    //For these menu alternatives we will be showing genotypes.
                    gtList = new GenotypeListForm(MyDataServer);

                    if (sender == AllExpFailedDupToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetDuplicateTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Failure);
                        gtList.ShowInformation("Duplicate Test (Failure)", fetcher);
                    }
                    else if (sender == AllExpPassedDupToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetDuplicateTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Success);
                        gtList.ShowInformation("Duplicate Test (Success)", fetcher);
                    }
                    else if (sender == AllExpTestedDupToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetDuplicateTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.All);
                        gtList.ShowInformation("Duplicate Test (Tested)", fetcher);
                    }
                    else if (sender == AllExpFailedInhToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetInheritanceTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Failure);
                        gtList.ShowInformation("Inheritance Test (Failure)", fetcher);
                    }
                    else if (sender == AllExpPassedInhToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetInheritanceTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Success);
                        gtList.ShowInformation("Inheritance Test (Success)", fetcher);
                    }
                    else if (sender == AllExpTestedInhToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetInheritanceTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.All);
                        gtList.ShowInformation("Inheritance Test (Tested)", fetcher);
                    }
                    else if (sender == AllExpFailedBlankToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetBlankTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Failure);
                        gtList.ShowInformation("Blank Test (Failure)", fetcher);
                    }
                    else if (sender == AllExpPassedBlankToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetBlankTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Success);
                        gtList.ShowInformation("Blank Test (Success)", fetcher);
                    }
                    else if (sender == AllExpTestedBlankToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetBlankTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.All);
                        gtList.ShowInformation("Blank Test (Tested)", fetcher);
                    }
                    else if (sender == AllExpFailedHomozygToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetHomozygoteTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Failure);
                        gtList.ShowInformation("Homozygote Test (Failure)", fetcher);
                    }
                    else if (sender == AllExpPassedHomozygToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetHomozygoteTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.Success);
                        gtList.ShowInformation("Homozygote Test (Success)", fetcher);
                    }
                    else if (sender == AllExpTestedHomozygToolStripMenuItem)
                    {
                        testRetrievalDelegate = new GenotypeFetcher.TestRetrievalMethodDelegate(MyDataServer.GetHomozygoteTest);
                        fetcher = new GenotypeFetcher(testRetrievalDelegate, null, DataServer.TestSuccess.All);
                        gtList.ShowInformation("Homozygote Test (Tested)", fetcher);
                    }
                    else if (sender == AllExpShowAllToolStripMenuItem)
                    {
                        stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllExperimentGenotypes);
                        fetcher = new GenotypeFetcher(stringRetrievalDelegate, null, null);
                        gtList.ShowInformation("All Genotypes", fetcher);
                    }
                    else if (sender == AllExpShowNoResultToolStripMenuItem)
                    {
                        stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllExperimentGenotypes);
                        fetcher = new GenotypeFetcher(stringRetrievalDelegate, null, "alleles = 'N/A'");
                        gtList.ShowInformation("All N/A Genotypes", fetcher);
                    }
                }
                else if (sender == AllExpFailedInhTriosToolStripMenuItem)
                {
                    list = new StaticListForm();
                    this.Cursor = Cursors.WaitCursor;
                    tempTable = MyDataServer.GetFailedInheritanceTrios(null);
                    this.Cursor = Cursors.Default;
                    list.ShowInformation("Failed trios for all experiments", tempTable);
                }
                else if (sender == AllExpRejectMinorityToolStripMenuItem)
                {
                    RejectMinority(null);
                }
                else if (sender == AllExpSetStatusLoadMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to change status to 'load' for all genotypes?") == DialogResult.No)
                    {
                        return;
                    }
                    MyDataServer.UpdateAllStatus("LOAD");
                }
                else if (sender == AllExpSetStatusRejectedMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to reject all genotypes?") == DialogResult.No)
                    {
                        return;
                    }
                    MyDataServer.UpdateAllStatus("REJECTED");
                }
                AutoRefresh();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when performing operation on all experiments.", this);
            }

        }


        private void ItemMenuItem_Click(object sender, System.EventArgs e)
        {
            GenotypeListForm gtList;
            StaticListForm list;
            UpdateableListForm.ListFetcher fetcher;
            GenotypeFetcher.StringRetrievalMethodDelegate stringRetrievalDelegate;
            int selectedItemID;
            int[] itemID;
            string selectedItemIdentifier, title;
            DataTable tempTable;
 
            try
            {
                itemID = new Int32[ResultItemList.TheListView.SelectedItems.Count];
                this.Cursor = Cursors.WaitCursor;
                for (int i = 0; i < ResultItemList.TheListView.SelectedItems.Count; i++)
                {
                    itemID[i] = Convert.ToInt32(ResultItemList.TheListView.SelectedItems[i].Tag);
                }
                this.Cursor = Cursors.Default;

                if (sender != SelItemShowExperimentDetailsMenuItem && sender != SelItemSetStatusLoadMenuItem && sender != SelItemSetStatusRejectedMenuItem)
                {
                    //For these menu alternatives we will be showing genotypes.
                    gtList = new GenotypeListForm(MyDataServer);

				    if (sender == SelItemShowAllMenuItem)
					{ 
						stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllItemGenotypes);
						fetcher = new GenotypeFetcher(stringRetrievalDelegate, itemID, null);
                        gtList.ShowInformation("All Genotypes", fetcher);
					}
					else if (sender == SelItemShowNAMenuItem)
					{ 
						stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllItemGenotypes);
						fetcher = new GenotypeFetcher(stringRetrievalDelegate, itemID, "alleles = 'N/A'");
                        gtList.ShowInformation("All N/A Genotypes", fetcher);				
					}
                }
                else if (sender == SelItemShowExperimentDetailsMenuItem)
                {
				    selectedItemID = Convert.ToInt32(ResultItemList.TheListView.SelectedItems[0].Tag);
				    selectedItemIdentifier = ResultItemList.TheListView.SelectedItems[0].Text;

				    title = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SAMPLE" ? "Details for Sample " :
					    "Details for Individual ") + selectedItemIdentifier;

				    list = new StaticListForm();
				    this.Cursor = Cursors.WaitCursor;
				    tempTable = MyDataServer.GetExperimentStat(selectedItemID);
				    this.Cursor = Cursors.Default;
				    list.ShowInformation(title, tempTable);
                }
                else if (sender == SelItemSetStatusLoadMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to change status to 'load' for all results from the selected "
                        + itemID.GetLength(0).ToString() + " item(s)?") == DialogResult.No)
                    {
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    MyDataServer.UpdateItemStatus(itemID, "LOAD");
                    this.Cursor = Cursors.Default;
                }
                else if (sender == SelItemSetStatusRejectedMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to reject all results from the selected "
                        + itemID.GetLength(0).ToString() + " item(s)?") == DialogResult.No)
                    {
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    MyDataServer.UpdateItemStatus(itemID, "REJECTED");
                    this.Cursor = Cursors.Default;
                }

                
                AutoRefresh();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when performing operation on item.", this);
			}
        }

        private void ControlItemMenuItem_Click(object sender, System.EventArgs e)
        {
            GenotypeListForm gtList;
            StaticListForm list;
            UpdateableListForm.ListFetcher fetcher;
            GenotypeFetcher.StringRetrievalMethodDelegate stringRetrievalDelegate;
            int[] itemID;
            string selectedItemIdentifier, title;
            DataTable tempTable;
            int sampleId;

            try
            {
                itemID = new Int32[ControlItemList.TheListView.SelectedItems.Count];
                this.Cursor = Cursors.WaitCursor;
                for (int i = 0; i < ControlItemList.TheListView.SelectedItems.Count; i++)
                {
                    itemID[i] = Convert.ToInt32(ControlItemList.TheListView.SelectedItems[i].Tag);
                }
                this.Cursor = Cursors.Default;

                if (sender != SelCtrlItemShowExperimentDetailsMenuItem && sender != SelCtrlItemSetStatusLoadMenuItem && sender != SelCtrlItemSetStatusRejectedMenuItem)
                {
                    //For these menu alternatives we will be showing genotypes.
                    gtList = new GenotypeListForm(MyDataServer);

                    if (sender == SelCtrlItemShowAllMenuItem)
                    {
                        stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllItemGenotypes);
                        fetcher = new GenotypeFetcher(stringRetrievalDelegate, itemID, null);
                        gtList.ShowInformation("All Genotypes", fetcher);
                    }
                    else if (sender == SelCtrlItemShowNAMenuItem)
                    {
                        stringRetrievalDelegate = new GenotypeFetcher.StringRetrievalMethodDelegate(MyDataServer.GetAllItemGenotypes);
                        fetcher = new GenotypeFetcher(stringRetrievalDelegate, itemID, "alleles = 'N/A'");
                        gtList.ShowInformation("All N/A Genotypes", fetcher);
                    }
                }
                else if (sender == SelCtrlItemShowExperimentDetailsMenuItem)
                {
                    sampleId = Convert.ToInt32(ControlItemList.TheListView.SelectedItems[0].Tag);
                    selectedItemIdentifier = ControlItemList.TheListView.SelectedItems[0].Text;

                    title = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SAMPLE" ? "Details for Sample " :
                        "Details for Individual ") + selectedItemIdentifier;

                    list = new StaticListForm();
                    this.Cursor = Cursors.WaitCursor;
                    tempTable = MyDataServer.GetControlStat(sampleId);
                    this.Cursor = Cursors.Default;
                    list.ShowInformation(title, tempTable);
                }
                else if (sender == SelCtrlItemSetStatusLoadMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to change status to 'load' for all results from the selected "
                        + itemID.GetLength(0).ToString() + " item(s)?") == DialogResult.No)
                    {
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    MyDataServer.UpdateItemStatus(itemID, "LOAD");
                    this.Cursor = Cursors.Default;
                }
                else if (sender == SelCtrlItemSetStatusRejectedMenuItem)
                {
                    if (MessageManager.ShowQuestion("Are you sure you want to reject all results from the selected "
                        + itemID.GetLength(0).ToString() + " item(s)?") == DialogResult.No)
                    {
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    MyDataServer.UpdateItemStatus(itemID, "REJECTED");
                    this.Cursor = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when performing operation on control item.", this);
            }

        }


		private void GenotypeStatusChange_Fire(object sender, System.EventArgs e)
		{
			MyNeedsRefresh = true;
			SetFormStatus(FormStatusMode.AllowInput, "Ready", null, null, null);
		}

		private void SettingsChange_Fire(object sender, System.EventArgs e)
		{
			MyNeedsRefresh = true;
			SetFormStatus(FormStatusMode.AllowInput, "Ready", null, null, null);
		}

        private void DataServer_DataReady(object sender, System.EventArgs e)
        {
            //Indicate to the thread owning the form that it can start processing data.
            MyDataReadyFlag = true;
        }


		private void DataServer_DataError(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			MessageManager.ShowError(e.Exception, "Error when processing data.", this);

            MyIsAborted = true;
            SetFormStatus(FormStatusMode.AllowInput, "Error", TIMER_BLANK, TIMER_BLANK, TIMER_BLANK);
		}

		private void MyExperimentContextMenu_Popup(object sender, System.EventArgs e)
		{
			//Disable all menu items if no experiments are selected.

			try
			{
				if (ExperimentList.TheListView.SelectedItems.Count > 0)
				{
					for (int i = 0 ; i <  MyExperimentContextMenuStrip.Items.Count ; i++)
					{
						MyExperimentContextMenuStrip.Items[i].Enabled = true;
					}
				}
				else
				{
                    for (int i = 0; i < MyExperimentContextMenuStrip.Items.Count; i++)
					{
                        MyExperimentContextMenuStrip.Items[i].Enabled = false;
					}
					return;
				}

				//Enable or disable menu items only valid for individuals.
                SelExpShowFailedInhMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                SelExpShowPassedInhMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                SelExpShowTestedInhMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                SelExpShowFailedInhTestTriosMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                SelExpInheritanceTestMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
				//Disable the menu item for showing item statistics for an experiment if there are
				//more than one experiment selected.
				SelExpShowItemDetailsMenuItem.Enabled = (ExperimentList.TheListView.SelectedItems.Count <= 1);

			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing context menu.", this);
			}
		}

		private void MyItemContextMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				for (int i = 0 ; i <  MyItemContextMenuStrip.Items.Count ; i++)
				{
					MyItemContextMenuStrip.Items[i].Enabled = (ResultItemList.TheListView.SelectedItems.Count > 0) ;
				}
				//Disable the menu item for showing experiment statistics for a item if there are
				//more than one item selected.
				if (ResultItemList.TheListView.SelectedItems.Count > 1)
				{
					SelItemShowExperimentDetailsMenuItem.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing context menu.", this);
			}
		}

		private void MyControlContextMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				for (int i = 0 ; i <  MyControlItemContextMenuStrip.Items.Count ; i++)
				{
					MyControlItemContextMenuStrip.Items[i].Enabled = (ControlItemList.TheListView.SelectedItems.Count > 0) ;
				}
				//Disable the menu item for showing experiment statistics for a control sample if there are
				//more than one control sample selected.
				if (ControlItemList.TheListView.SelectedItems.Count > 1)
				{
					SelCtrlItemShowExperimentDetailsMenuItem.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing context menu.", this);
			}		
		}

		private void ApproveButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				Approve();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when approving results.", this);
			}
		}

        private void DataPollTimer_Tick(object sender, EventArgs e)
        {
            if (MyAsyncExecutionMode == AsyncExecutionMode.GetExperiments)
            {
                if (MyDataReadyFlag)
                {
                    DataPollTimer.Enabled = false;
                    PrepareExperimentQueue();
                }
            }
            else if (MyAsyncExecutionMode == AsyncExecutionMode.PerformTest)
            {
                if (MyDataReadyFlag)
                {
                    DataPollTimer.Enabled = false;
                    TestExperimentBatch(false);
                }
            }
            else if (MyAsyncExecutionMode == AsyncExecutionMode.GetResults)
            {
                if (MyDataReadyFlag)
                {
                    DataPollTimer.Enabled = false;
                    ShowTestResults();
                }
            }
        }

        private void SessionToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem == RefreshToolStripButton)
                {
                    RefreshResults();
                }
                else if (e.ClickedItem == SubmitStatusToolStripButton)
                {
                    if (MessageManager.ShowQuestion("Submit pending genotype status changes?", this) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        MyDataServer.SubmitGenotypeStatus();
                        this.Cursor = Cursors.Default;
                    }
                }
                else if (e.ClickedItem == StopToolStripButton)
                {
                    DataPollTimer.Enabled = false;
                    MyIsAborted = true;
                    MyDataServer.Abort();
                    SetFormStatus(FormStatusMode.AllowInput, "Aborted", TIMER_BLANK, TIMER_BLANK, TIMER_BLANK);
                }
                else if (e.ClickedItem == SaveSessionToolStripButton)
                {
                    this.Save();
                }
                else if (e.ClickedItem == ClearStatusToolStripButton)
                {
                    if (MessageManager.ShowQuestion("Clear all pending genotype status changes since the last submission?",
                        this) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        MyDataServer.ClearGenotypeStatus();
                        this.Cursor = Cursors.Default;
                    }
                }
                else if (e.ClickedItem == SettingsToolStripButton)
                {
                    EditSettings();
                }
                else if (e.ClickedItem == RerunToolStripButton)
                {
                    PickRerunItems();
                }
                else if (e.ClickedItem == PlatesToolStripButton)
                {
                    InspectPlates();
                }
                else if (e.ClickedItem == SaveReportToolStripButton)
                {
                    SaveInternalReport();
                }
                else if (e.ClickedItem == ApproveSessionToolStripButton)
                {
                    Approve();
                }
                else if (e.ClickedItem == AnnotationsToolStripButton)
                {
                    ShowMarkerAnnotations();
                }
                else if (e.ClickedItem == InspectChangesToolStripButton)
                {
                    ShowPendingChanges();
                }

                AutoRefresh();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when performing requested operation.", this);
            }
        }

        private void ResultsExpRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowResultMatrix(ResultTable.Mode.ExperimentByItem);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing results.", this);
            }
        }

        private void ResultsExpColToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowResultMatrix(ResultTable.Mode.ItemByExperiment);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing results.", this);
            }
        }

        private void ResultsPairwiseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowResultPairwise(true);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing results.", this);
            }
        }

        private void ResultsPairwiseApprovedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowResultPairwise(false);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing results.", this);
            }
        }

        private void SessionPlatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowSessionPlates();   
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing plates in session.", this);
            }
        }

        private void SessionGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowSessionGroups();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing working sets in session.", this);
            }
        }

        private void SessionDetailedFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowSessionDetailedFilter();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing selections.", this);
            }
        }

        private void ExperimentCutoffToolStripButton_Click(object sender, EventArgs e)
        {
            int cutoffPercent;
            GetNumberForm inputForm;

            try
            {
                inputForm = new GetNumberForm();
                if (inputForm.GetNumber(this, "Experiment Cutoff", 0, 100, 80, "Select experiment success rate cutoff: ", out cutoffPercent) == false)
                {
                    //User cancelled.
                    return;
                }
                MyDataServer.ApplyExperimentCutoff(cutoffPercent);
                AutoRefresh();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when applying cutoff on experiments.", this);
            }
        }

        private void ItemCutoffToolStripButton_Click(object sender, EventArgs e)
        {
            int limit;
            ItemSuccessCutoffForm.ItemCutoffMode mode;
            ItemSuccessCutoffForm inputForm;

            try
            {
                inputForm = new ItemSuccessCutoffForm();
                if (inputForm.GetSettings(out limit, out mode) == false)
                {
                    //User cancelled.
                    return;
                }
                MyDataServer.ApplyItemCutoff(limit,
                    (mode == ItemSuccessCutoffForm.ItemCutoffMode.AllExperiments));
                AutoRefresh();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when applying cutoff on items.", this);
            }
        }

        private void RefreshSortToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Turn off the list's own sorting.
                ExperimentList.TheListView.Sorting = System.Windows.Forms.SortOrder.None;
                ExperimentList.TheListView.ListViewItemSorter = null;

                this.FetchTestResults();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when updating result display.", this);
            }
        }

        private void AllExperimentsDropDownButton_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                AllExpInhTestToolStripMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                AllExpFailedInhToolStripMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                AllExpPassedInhToolStripMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                AllExpTestedInhToolStripMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
                AllExpFailedInhTriosToolStripMenuItem.Enabled = (MySessionSettings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT");
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing experiment menu.", this);
            }
        }
	}
}
