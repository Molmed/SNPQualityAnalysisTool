using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Threading;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;


namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for StartSessionForm.
	/// </summary>
	internal class StartSessionForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel TopPanel;
		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Panel LeftPanel;
		private System.Windows.Forms.Panel RightPanel;
		private System.Windows.Forms.Panel CenterPanel;
		private System.Windows.Forms.GroupBox GroupsFrame;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.GroupBox DetailsFrame;
		private System.Windows.Forms.ImageList MyImageList;
		private System.Windows.Forms.Button FinishButton;
		private System.Windows.Forms.Button PreviousButton;
		private System.Windows.Forms.Button NextButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Panel AvailableGroupsPanel;
		private System.Windows.Forms.Label ProjectLabel;
		private System.Windows.Forms.ComboBox ProjectList;
		private System.Windows.Forms.Label AvailableGroupsLabel;
		private System.Windows.Forms.ListView AvailableGroupsList;
		private System.Windows.Forms.ColumnHeader GroupNameColHead;
		private System.Windows.Forms.ColumnHeader DescrColHead;
		private System.Windows.Forms.ColumnHeader TypeColHead;
        private System.Windows.Forms.Button SelectGroupButton;
		private System.Windows.Forms.Panel SelectedGroupsPanel;
		private System.Windows.Forms.Label SelectedGroupsLabel;
		private System.Windows.Forms.ListView SelectedGroupsList;
		private System.Windows.Forms.ColumnHeader SelGroupNameColHead;
		private System.Windows.Forms.ColumnHeader SelDescrColHead;
		private System.Windows.Forms.ColumnHeader SelTypeColHead;
		private System.Windows.Forms.ColumnHeader SelProjectColHead;
		private System.Windows.Forms.Button DeselectButton;
        private System.Windows.Forms.Panel ItemPanel;
        private System.Windows.Forms.Panel ExperimentPanel;
        private System.Windows.Forms.Splitter MarkerAssaySplitter;
        private System.Windows.Forms.Button SettingsButton;

		private DataServer MyDataServer;
        private Configuration MyConfiguration;
        private ObjectServer MyObjectServer;
		private int MyCurrentFrame;
		private Identifiable MyCurrentParent;
		private bool MyReturnFlag;
		private Form MyParentForm;
		private SessionForm.StartMode MyStartMode;
        private SessionSettings MySessionSettings;
        private string MySessionName;
        private FilterIndicator MyFilterIndicator;
        private System.Windows.Forms.ContextMenu SessionContextMenu;
		private System.Windows.Forms.MenuItem ShowGroupsMenuItem;
        private System.Windows.Forms.MenuItem DeleteSessionMenuItem;
        private Button FillExperimentsButton;
        private Button FillItemsButton;
        private Button StopButton;
        private event EventHandler MyDataReadyEventHandler;
        private ThreadExceptionEventHandler MyDataErrorEventHandler;
        private Splitter GroupSplitter;
        private RadioButton SampleRadioButton;
        private RadioButton MarkerRadioButton;
        private RadioButton AssayRadioButton;
        private RadioButton IndividualRadioButton;
        private Button UpButton;
        private Button StreamToFileButton;

        private const int TotalFrameNumber = 2;
        private MenuItem UnlockSessionMenuItem;
        private AdvancedCheckedListBox2 SelectedExperimentList;
        private AdvancedCheckedListBox2 SelectedItemList;
        private bool MyInteractiveFlag;
        
        private delegate void AddSingleItemToPickListCallback(Identifiable item);
        private delegate void SetFormModeDelegate(bool readyForInput);
        private delegate void RefreshCounterCallback(AdvancedCheckedListBox2 list);

		public StartSessionForm(Form parentForm, DataServer dServer, Configuration config)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			AvailableGroupsList.DoubleClick += new System.EventHandler(this.PlateGroup_Doubleclick);
			MyParentForm = parentForm;
			MyDataServer = dServer;
            MyConfiguration = config;
            MyObjectServer = new ObjectServer(MyDataServer);
			MySessionSettings = new SessionSettings();
            SetFormMode(true);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartSessionForm));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.StreamToFileButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.FinishButton = new System.Windows.Forms.Button();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.CenterPanel = new System.Windows.Forms.Panel();
            this.DetailsFrame = new System.Windows.Forms.GroupBox();
            this.ExperimentPanel = new System.Windows.Forms.Panel();
            this.SelectedExperimentList = new Molmed.SQAT.GUI.AdvancedCheckedListBox2();
            this.MarkerRadioButton = new System.Windows.Forms.RadioButton();
            this.AssayRadioButton = new System.Windows.Forms.RadioButton();
            this.FillExperimentsButton = new System.Windows.Forms.Button();
            this.MarkerAssaySplitter = new System.Windows.Forms.Splitter();
            this.ItemPanel = new System.Windows.Forms.Panel();
            this.SelectedItemList = new Molmed.SQAT.GUI.AdvancedCheckedListBox2();
            this.IndividualRadioButton = new System.Windows.Forms.RadioButton();
            this.SampleRadioButton = new System.Windows.Forms.RadioButton();
            this.FillItemsButton = new System.Windows.Forms.Button();
            this.GroupsFrame = new System.Windows.Forms.GroupBox();
            this.GroupSplitter = new System.Windows.Forms.Splitter();
            this.SelectedGroupsPanel = new System.Windows.Forms.Panel();
            this.DeselectButton = new System.Windows.Forms.Button();
            this.SelectedGroupsLabel = new System.Windows.Forms.Label();
            this.SelectedGroupsList = new System.Windows.Forms.ListView();
            this.SelGroupNameColHead = new System.Windows.Forms.ColumnHeader();
            this.SelDescrColHead = new System.Windows.Forms.ColumnHeader();
            this.SelTypeColHead = new System.Windows.Forms.ColumnHeader();
            this.SelProjectColHead = new System.Windows.Forms.ColumnHeader();
            this.MyImageList = new System.Windows.Forms.ImageList(this.components);
            this.AvailableGroupsPanel = new System.Windows.Forms.Panel();
            this.UpButton = new System.Windows.Forms.Button();
            this.SelectGroupButton = new System.Windows.Forms.Button();
            this.AvailableGroupsList = new System.Windows.Forms.ListView();
            this.GroupNameColHead = new System.Windows.Forms.ColumnHeader();
            this.DescrColHead = new System.Windows.Forms.ColumnHeader();
            this.TypeColHead = new System.Windows.Forms.ColumnHeader();
            this.AvailableGroupsLabel = new System.Windows.Forms.Label();
            this.ProjectList = new System.Windows.Forms.ComboBox();
            this.ProjectLabel = new System.Windows.Forms.Label();
            this.SessionContextMenu = new System.Windows.Forms.ContextMenu();
            this.ShowGroupsMenuItem = new System.Windows.Forms.MenuItem();
            this.UnlockSessionMenuItem = new System.Windows.Forms.MenuItem();
            this.DeleteSessionMenuItem = new System.Windows.Forms.MenuItem();
            this.BottomPanel.SuspendLayout();
            this.CenterPanel.SuspendLayout();
            this.DetailsFrame.SuspendLayout();
            this.ExperimentPanel.SuspendLayout();
            this.ItemPanel.SuspendLayout();
            this.GroupsFrame.SuspendLayout();
            this.SelectedGroupsPanel.SuspendLayout();
            this.AvailableGroupsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(1074, 16);
            this.TopPanel.TabIndex = 0;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.StreamToFileButton);
            this.BottomPanel.Controls.Add(this.StopButton);
            this.BottomPanel.Controls.Add(this.SettingsButton);
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.NextButton);
            this.BottomPanel.Controls.Add(this.PreviousButton);
            this.BottomPanel.Controls.Add(this.FinishButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 480);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(1074, 64);
            this.BottomPanel.TabIndex = 2;
            // 
            // StreamToFileButton
            // 
            this.StreamToFileButton.Location = new System.Drawing.Point(100, 32);
            this.StreamToFileButton.Name = "StreamToFileButton";
            this.StreamToFileButton.Size = new System.Drawing.Size(72, 24);
            this.StreamToFileButton.TabIndex = 2;
            this.StreamToFileButton.Text = "Report...";
            this.StreamToFileButton.UseVisualStyleBackColor = true;
            this.StreamToFileButton.Click += new System.EventHandler(this.StreamToFileButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(274, 32);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(72, 24);
            this.StopButton.TabIndex = 4;
            this.StopButton.Text = "S&top";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Location = new System.Drawing.Point(186, 32);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(72, 24);
            this.SettingsButton.TabIndex = 3;
            this.SettingsButton.Text = "&Settings...";
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(984, 32);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 7;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NextButton.Location = new System.Drawing.Point(544, 32);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(72, 24);
            this.NextButton.TabIndex = 6;
            this.NextButton.Text = "&Details >";
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PreviousButton.Location = new System.Drawing.Point(464, 32);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(72, 24);
            this.PreviousButton.TabIndex = 5;
            this.PreviousButton.Text = "< &Plates";
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // FinishButton
            // 
            this.FinishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FinishButton.Location = new System.Drawing.Point(16, 32);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(72, 24);
            this.FinishButton.TabIndex = 1;
            this.FinishButton.Text = "&Load";
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // LeftPanel
            // 
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 16);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(16, 464);
            this.LeftPanel.TabIndex = 3;
            // 
            // RightPanel
            // 
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(1058, 16);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(16, 464);
            this.RightPanel.TabIndex = 4;
            // 
            // CenterPanel
            // 
            this.CenterPanel.Controls.Add(this.DetailsFrame);
            this.CenterPanel.Controls.Add(this.GroupsFrame);
            this.CenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CenterPanel.Location = new System.Drawing.Point(16, 16);
            this.CenterPanel.Name = "CenterPanel";
            this.CenterPanel.Size = new System.Drawing.Size(1042, 464);
            this.CenterPanel.TabIndex = 5;
            // 
            // DetailsFrame
            // 
            this.DetailsFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailsFrame.Controls.Add(this.ExperimentPanel);
            this.DetailsFrame.Controls.Add(this.MarkerAssaySplitter);
            this.DetailsFrame.Controls.Add(this.ItemPanel);
            this.DetailsFrame.Location = new System.Drawing.Point(6, 123);
            this.DetailsFrame.Name = "DetailsFrame";
            this.DetailsFrame.Size = new System.Drawing.Size(936, 350);
            this.DetailsFrame.TabIndex = 1;
            this.DetailsFrame.TabStop = false;
            this.DetailsFrame.Text = "Detailed Filter";
            // 
            // ExperimentPanel
            // 
            this.ExperimentPanel.Controls.Add(this.SelectedExperimentList);
            this.ExperimentPanel.Controls.Add(this.MarkerRadioButton);
            this.ExperimentPanel.Controls.Add(this.AssayRadioButton);
            this.ExperimentPanel.Controls.Add(this.FillExperimentsButton);
            this.ExperimentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentPanel.Location = new System.Drawing.Point(451, 16);
            this.ExperimentPanel.Name = "ExperimentPanel";
            this.ExperimentPanel.Size = new System.Drawing.Size(482, 331);
            this.ExperimentPanel.TabIndex = 6;
            // 
            // SelectedExperimentList
            // 
            this.SelectedExperimentList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedExperimentList.Location = new System.Drawing.Point(0, 51);
            this.SelectedExperimentList.Name = "SelectedExperimentList";
            this.SelectedExperimentList.Size = new System.Drawing.Size(482, 275);
            this.SelectedExperimentList.TabIndex = 8;
            // 
            // MarkerRadioButton
            // 
            this.MarkerRadioButton.AutoSize = true;
            this.MarkerRadioButton.Location = new System.Drawing.Point(16, 32);
            this.MarkerRadioButton.Name = "MarkerRadioButton";
            this.MarkerRadioButton.Size = new System.Drawing.Size(58, 17);
            this.MarkerRadioButton.TabIndex = 7;
            this.MarkerRadioButton.Text = "Marker";
            this.MarkerRadioButton.UseVisualStyleBackColor = true;
            this.MarkerRadioButton.CheckedChanged += new System.EventHandler(this.ExperimentRadioButtons_CheckedChanged);
            // 
            // AssayRadioButton
            // 
            this.AssayRadioButton.AutoSize = true;
            this.AssayRadioButton.Checked = true;
            this.AssayRadioButton.Location = new System.Drawing.Point(16, 9);
            this.AssayRadioButton.Name = "AssayRadioButton";
            this.AssayRadioButton.Size = new System.Drawing.Size(58, 17);
            this.AssayRadioButton.TabIndex = 6;
            this.AssayRadioButton.TabStop = true;
            this.AssayRadioButton.Text = "Assays";
            this.AssayRadioButton.UseVisualStyleBackColor = true;
            this.AssayRadioButton.CheckedChanged += new System.EventHandler(this.ExperimentRadioButtons_CheckedChanged);
            // 
            // FillExperimentsButton
            // 
            this.FillExperimentsButton.Location = new System.Drawing.Point(80, 21);
            this.FillExperimentsButton.Name = "FillExperimentsButton";
            this.FillExperimentsButton.Size = new System.Drawing.Size(72, 24);
            this.FillExperimentsButton.TabIndex = 5;
            this.FillExperimentsButton.Text = "Show";
            this.FillExperimentsButton.UseVisualStyleBackColor = true;
            this.FillExperimentsButton.Click += new System.EventHandler(this.FillExperimentsButton_Click);
            // 
            // MarkerAssaySplitter
            // 
            this.MarkerAssaySplitter.Location = new System.Drawing.Point(441, 16);
            this.MarkerAssaySplitter.Name = "MarkerAssaySplitter";
            this.MarkerAssaySplitter.Size = new System.Drawing.Size(10, 331);
            this.MarkerAssaySplitter.TabIndex = 7;
            this.MarkerAssaySplitter.TabStop = false;
            // 
            // ItemPanel
            // 
            this.ItemPanel.Controls.Add(this.SelectedItemList);
            this.ItemPanel.Controls.Add(this.IndividualRadioButton);
            this.ItemPanel.Controls.Add(this.SampleRadioButton);
            this.ItemPanel.Controls.Add(this.FillItemsButton);
            this.ItemPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ItemPanel.Location = new System.Drawing.Point(3, 16);
            this.ItemPanel.Name = "ItemPanel";
            this.ItemPanel.Size = new System.Drawing.Size(438, 331);
            this.ItemPanel.TabIndex = 3;
            // 
            // SelectedItemList
            // 
            this.SelectedItemList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedItemList.Location = new System.Drawing.Point(0, 51);
            this.SelectedItemList.Name = "SelectedItemList";
            this.SelectedItemList.Size = new System.Drawing.Size(438, 275);
            this.SelectedItemList.TabIndex = 6;
            // 
            // IndividualRadioButton
            // 
            this.IndividualRadioButton.AutoSize = true;
            this.IndividualRadioButton.Location = new System.Drawing.Point(12, 32);
            this.IndividualRadioButton.Name = "IndividualRadioButton";
            this.IndividualRadioButton.Size = new System.Drawing.Size(75, 17);
            this.IndividualRadioButton.TabIndex = 5;
            this.IndividualRadioButton.Text = "Individuals";
            this.IndividualRadioButton.UseVisualStyleBackColor = true;
            this.IndividualRadioButton.CheckedChanged += new System.EventHandler(this.ItemRadioButtons_CheckedChanged);
            // 
            // SampleRadioButton
            // 
            this.SampleRadioButton.AutoSize = true;
            this.SampleRadioButton.Checked = true;
            this.SampleRadioButton.Location = new System.Drawing.Point(12, 9);
            this.SampleRadioButton.Name = "SampleRadioButton";
            this.SampleRadioButton.Size = new System.Drawing.Size(65, 17);
            this.SampleRadioButton.TabIndex = 4;
            this.SampleRadioButton.TabStop = true;
            this.SampleRadioButton.Text = "Samples";
            this.SampleRadioButton.UseVisualStyleBackColor = true;
            this.SampleRadioButton.CheckedChanged += new System.EventHandler(this.ItemRadioButtons_CheckedChanged);
            // 
            // FillItemsButton
            // 
            this.FillItemsButton.Location = new System.Drawing.Point(93, 21);
            this.FillItemsButton.Name = "FillItemsButton";
            this.FillItemsButton.Size = new System.Drawing.Size(72, 24);
            this.FillItemsButton.TabIndex = 3;
            this.FillItemsButton.Text = "Show";
            this.FillItemsButton.UseVisualStyleBackColor = true;
            this.FillItemsButton.Click += new System.EventHandler(this.FillItemsButton_Click);
            // 
            // GroupsFrame
            // 
            this.GroupsFrame.Controls.Add(this.GroupSplitter);
            this.GroupsFrame.Controls.Add(this.SelectedGroupsPanel);
            this.GroupsFrame.Controls.Add(this.AvailableGroupsPanel);
            this.GroupsFrame.Location = new System.Drawing.Point(16, 8);
            this.GroupsFrame.Name = "GroupsFrame";
            this.GroupsFrame.Size = new System.Drawing.Size(1016, 344);
            this.GroupsFrame.TabIndex = 0;
            this.GroupsFrame.TabStop = false;
            this.GroupsFrame.Text = "Plates and Working Sets";
            // 
            // GroupSplitter
            // 
            this.GroupSplitter.Location = new System.Drawing.Point(463, 16);
            this.GroupSplitter.Name = "GroupSplitter";
            this.GroupSplitter.Size = new System.Drawing.Size(8, 325);
            this.GroupSplitter.TabIndex = 11;
            this.GroupSplitter.TabStop = false;
            // 
            // SelectedGroupsPanel
            // 
            this.SelectedGroupsPanel.Controls.Add(this.DeselectButton);
            this.SelectedGroupsPanel.Controls.Add(this.SelectedGroupsLabel);
            this.SelectedGroupsPanel.Controls.Add(this.SelectedGroupsList);
            this.SelectedGroupsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectedGroupsPanel.Location = new System.Drawing.Point(463, 16);
            this.SelectedGroupsPanel.Name = "SelectedGroupsPanel";
            this.SelectedGroupsPanel.Size = new System.Drawing.Size(550, 325);
            this.SelectedGroupsPanel.TabIndex = 10;
            // 
            // DeselectButton
            // 
            this.DeselectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeselectButton.Location = new System.Drawing.Point(13, 291);
            this.DeselectButton.Name = "DeselectButton";
            this.DeselectButton.Size = new System.Drawing.Size(72, 24);
            this.DeselectButton.TabIndex = 10;
            this.DeselectButton.Text = "&Deselect";
            this.DeselectButton.Click += new System.EventHandler(this.DeselectButton_Click);
            // 
            // SelectedGroupsLabel
            // 
            this.SelectedGroupsLabel.Location = new System.Drawing.Point(10, 80);
            this.SelectedGroupsLabel.Name = "SelectedGroupsLabel";
            this.SelectedGroupsLabel.Size = new System.Drawing.Size(177, 16);
            this.SelectedGroupsLabel.TabIndex = 8;
            this.SelectedGroupsLabel.Text = "Selected plates and working sets";
            // 
            // SelectedGroupsList
            // 
            this.SelectedGroupsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedGroupsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SelGroupNameColHead,
            this.SelDescrColHead,
            this.SelTypeColHead,
            this.SelProjectColHead});
            this.SelectedGroupsList.FullRowSelect = true;
            this.SelectedGroupsList.GridLines = true;
            this.SelectedGroupsList.Location = new System.Drawing.Point(13, 99);
            this.SelectedGroupsList.Name = "SelectedGroupsList";
            this.SelectedGroupsList.Size = new System.Drawing.Size(527, 186);
            this.SelectedGroupsList.SmallImageList = this.MyImageList;
            this.SelectedGroupsList.TabIndex = 9;
            this.SelectedGroupsList.UseCompatibleStateImageBehavior = false;
            this.SelectedGroupsList.View = System.Windows.Forms.View.Details;
            // 
            // SelGroupNameColHead
            // 
            this.SelGroupNameColHead.Text = "Name";
            this.SelGroupNameColHead.Width = 211;
            // 
            // SelDescrColHead
            // 
            this.SelDescrColHead.Text = "Description";
            this.SelDescrColHead.Width = 140;
            // 
            // SelTypeColHead
            // 
            this.SelTypeColHead.Text = "Type";
            this.SelTypeColHead.Width = 79;
            // 
            // SelProjectColHead
            // 
            this.SelProjectColHead.Text = "Project";
            this.SelProjectColHead.Width = 79;
            // 
            // MyImageList
            // 
            this.MyImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MyImageList.ImageStream")));
            this.MyImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MyImageList.Images.SetKeyName(0, "");
            this.MyImageList.Images.SetKeyName(1, "");
            this.MyImageList.Images.SetKeyName(2, "");
            this.MyImageList.Images.SetKeyName(3, "");
            this.MyImageList.Images.SetKeyName(4, "");
            this.MyImageList.Images.SetKeyName(5, "");
            this.MyImageList.Images.SetKeyName(6, "");
            this.MyImageList.Images.SetKeyName(7, "");
            this.MyImageList.Images.SetKeyName(8, "");
            // 
            // AvailableGroupsPanel
            // 
            this.AvailableGroupsPanel.Controls.Add(this.UpButton);
            this.AvailableGroupsPanel.Controls.Add(this.SelectGroupButton);
            this.AvailableGroupsPanel.Controls.Add(this.AvailableGroupsList);
            this.AvailableGroupsPanel.Controls.Add(this.AvailableGroupsLabel);
            this.AvailableGroupsPanel.Controls.Add(this.ProjectList);
            this.AvailableGroupsPanel.Controls.Add(this.ProjectLabel);
            this.AvailableGroupsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.AvailableGroupsPanel.Location = new System.Drawing.Point(3, 16);
            this.AvailableGroupsPanel.Name = "AvailableGroupsPanel";
            this.AvailableGroupsPanel.Size = new System.Drawing.Size(460, 325);
            this.AvailableGroupsPanel.TabIndex = 8;
            // 
            // UpButton
            // 
            this.UpButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.UpButton.Image = ((System.Drawing.Image)(resources.GetObject("UpButton.Image")));
            this.UpButton.Location = new System.Drawing.Point(8, 56);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(23, 21);
            this.UpButton.TabIndex = 7;
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // SelectGroupButton
            // 
            this.SelectGroupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectGroupButton.Location = new System.Drawing.Point(8, 291);
            this.SelectGroupButton.Name = "SelectGroupButton";
            this.SelectGroupButton.Size = new System.Drawing.Size(72, 24);
            this.SelectGroupButton.TabIndex = 6;
            this.SelectGroupButton.Text = "&Select";
            this.SelectGroupButton.Click += new System.EventHandler(this.SelectGroupButton_Click);
            // 
            // AvailableGroupsList
            // 
            this.AvailableGroupsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailableGroupsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GroupNameColHead,
            this.DescrColHead,
            this.TypeColHead});
            this.AvailableGroupsList.FullRowSelect = true;
            this.AvailableGroupsList.GridLines = true;
            this.AvailableGroupsList.Location = new System.Drawing.Point(8, 99);
            this.AvailableGroupsList.Name = "AvailableGroupsList";
            this.AvailableGroupsList.Size = new System.Drawing.Size(452, 186);
            this.AvailableGroupsList.SmallImageList = this.MyImageList;
            this.AvailableGroupsList.TabIndex = 5;
            this.AvailableGroupsList.UseCompatibleStateImageBehavior = false;
            this.AvailableGroupsList.View = System.Windows.Forms.View.Details;
            // 
            // GroupNameColHead
            // 
            this.GroupNameColHead.Text = "Name";
            this.GroupNameColHead.Width = 210;
            // 
            // DescrColHead
            // 
            this.DescrColHead.Text = "Description";
            this.DescrColHead.Width = 140;
            // 
            // TypeColHead
            // 
            this.TypeColHead.Text = "Type";
            this.TypeColHead.Width = 80;
            // 
            // AvailableGroupsLabel
            // 
            this.AvailableGroupsLabel.Location = new System.Drawing.Point(8, 80);
            this.AvailableGroupsLabel.Name = "AvailableGroupsLabel";
            this.AvailableGroupsLabel.Size = new System.Drawing.Size(187, 16);
            this.AvailableGroupsLabel.TabIndex = 4;
            this.AvailableGroupsLabel.Text = "Available plates and working sets";
            // 
            // ProjectList
            // 
            this.ProjectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProjectList.Location = new System.Drawing.Point(8, 24);
            this.ProjectList.Name = "ProjectList";
            this.ProjectList.Size = new System.Drawing.Size(452, 21);
            this.ProjectList.TabIndex = 0;
            this.ProjectList.SelectedIndexChanged += new System.EventHandler(this.ProjectList_SelectedIndexChanged);
            // 
            // ProjectLabel
            // 
            this.ProjectLabel.Location = new System.Drawing.Point(8, 8);
            this.ProjectLabel.Name = "ProjectLabel";
            this.ProjectLabel.Size = new System.Drawing.Size(112, 16);
            this.ProjectLabel.TabIndex = 2;
            this.ProjectLabel.Text = "Project";
            // 
            // SessionContextMenu
            // 
            this.SessionContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ShowGroupsMenuItem,
            this.UnlockSessionMenuItem,
            this.DeleteSessionMenuItem});
            this.SessionContextMenu.Popup += new System.EventHandler(this.SessionContextMenu_Popup);
            // 
            // ShowGroupsMenuItem
            // 
            this.ShowGroupsMenuItem.Index = 0;
            this.ShowGroupsMenuItem.Text = "Show selections...";
            this.ShowGroupsMenuItem.Click += new System.EventHandler(this.ShowGroupsMenuItem_Click);
            // 
            // UnlockSessionMenuItem
            // 
            this.UnlockSessionMenuItem.Index = 1;
            this.UnlockSessionMenuItem.Text = "Unlock";
            this.UnlockSessionMenuItem.Click += new System.EventHandler(this.UnlockSessionMenuItem_Click);
            // 
            // DeleteSessionMenuItem
            // 
            this.DeleteSessionMenuItem.Index = 2;
            this.DeleteSessionMenuItem.Text = "Delete";
            this.DeleteSessionMenuItem.Click += new System.EventHandler(this.DeleteSessionMenuItem_Click);
            // 
            // StartSessionForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1074, 544);
            this.Controls.Add(this.CenterPanel);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.BottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartSessionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Session";
            this.BottomPanel.ResumeLayout(false);
            this.CenterPanel.ResumeLayout(false);
            this.DetailsFrame.ResumeLayout(false);
            this.ExperimentPanel.ResumeLayout(false);
            this.ExperimentPanel.PerformLayout();
            this.ItemPanel.ResumeLayout(false);
            this.ItemPanel.PerformLayout();
            this.GroupsFrame.ResumeLayout(false);
            this.SelectedGroupsPanel.ResumeLayout(false);
            this.AvailableGroupsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion



		private void ShowFrame(int number)
		{
			//First make all frames invisible.
			GroupsFrame.Visible = false;
			DetailsFrame.Visible = false;
			//Then make the selected frame visible.
			switch (number)
			{
				case 1:
					GroupsFrame.Dock = DockStyle.Fill;
					GroupsFrame.Visible = true;
					PreviousButton.Enabled = false;
					NextButton.Enabled = true;
					break;
				case 2:
                    SelectedItemList.ClearItems();
                    SelectedExperimentList.ClearItems();
					DetailsFrame.Dock = DockStyle.Fill;
					DetailsFrame.Visible = true;
					PreviousButton.Enabled = true;
					NextButton.Enabled = false;
                    //Need to store the selected groups and plates from frame 1 so that
                    //lists of included items and experiments can be made in frame 2.
					MyFilterIndicator = StoreSelection();
					break;
				default:
					break;
			}
		}


		public bool StartSession(out SessionSettings settings, out string sessionName, out FilterIndicator usedFilters, out bool isForStreaming, SessionForm.StartMode mode)
		{
			MyStartMode = mode;
			MyCurrentFrame = 1;

			SelectedGroupsList.Visible = (MyStartMode == SessionForm.StartMode.CreateSession);
			AvailableGroupsList.MultiSelect = (MyStartMode == SessionForm.StartMode.CreateSession);
			AvailableGroupsList.HideSelection = (MyStartMode == SessionForm.StartMode.CreateSession);
			GroupSplitter.Enabled = (MyStartMode == SessionForm.StartMode.CreateSession);
			SelectedGroupsLabel.Visible = (MyStartMode == SessionForm.StartMode.CreateSession);
			SelectGroupButton.Visible = (MyStartMode == SessionForm.StartMode.CreateSession);
			DeselectButton.Visible = (MyStartMode == SessionForm.StartMode.CreateSession);
			SettingsButton.Visible = (MyStartMode == SessionForm.StartMode.CreateSession);
			if (MyStartMode == SessionForm.StartMode.LoadSession)
			{
				SelectedGroupsPanel.Width = 0;
				SelectedGroupsPanel.Dock = DockStyle.Right;
				AvailableGroupsPanel.Dock = DockStyle.Fill;
				AvailableGroupsList.ContextMenu = SessionContextMenu;
				this.Text = "Open Session";
                GroupsFrame.Text = "Select Session";
                AvailableGroupsLabel.Text = "Available sessions";
                SelectedGroupsLabel.Text = "Selected sessions";
                UpButton.Visible = false;
            }
			else
			{
				this.Text = "New Session";
                GroupsFrame.Text = "Select Plates and Working Sets";
                AvailableGroupsLabel.Text = "Available plates and working sets";
                SelectedGroupsLabel.Text = "Selected plates and working sets";
                UpButton.Enabled = false;
            }

			FillProjectList();
			
			ShowFrame(MyCurrentFrame);
			ShowDialog(MyParentForm);
			
			if (MyReturnFlag)
			{
				settings = MySessionSettings;
				sessionName = MySessionName;
                usedFilters = MyFilterIndicator;
                isForStreaming = !MyInteractiveFlag;
			}
			else
			{
				settings = null;
				sessionName = "";
                usedFilters = null;
                isForStreaming = false;
			}

			return MyReturnFlag;
		}


		private void FillProjectList()
		{
            ProjectCollection projects;

			this.Cursor = Cursors.WaitCursor;
            projects = MyObjectServer.GetProjects();
			ProjectList.Items.Clear();
            foreach (Project tempProject in projects)
            {
                ProjectList.Items.Add(tempProject);
            }
			this.Cursor = Cursors.Default;

            //if (ProjectList.Items.Count > 0)
            //{
            //    ProjectList.SelectedIndex = 0;
            //}
		}


		private void FillGroupList()
		{
            IdentifiableGroupCollection groups;
            PlateCollection plates;
            SessionItemCollection sessions;

            if (MyCurrentParent == null)
            {
                return;
            }

			this.Cursor = Cursors.WaitCursor;
            AvailableGroupsList.Items.Clear();

            if (MyStartMode == SessionForm.StartMode.CreateSession)
            {
                //Get the groups and plates within the parent.
                groups = MyObjectServer.GetGroups(MyCurrentParent); 
                foreach (IdentifiableGroup tempItem in groups)
                {
                    AddListViewItem(tempItem, tempItem.Name, tempItem.Description, tempItem.GroupType);
                }

                plates = MyObjectServer.GetPlates(MyCurrentParent);
                foreach (Plate tempItem in plates)
                {
                    AddListViewItem(tempItem, tempItem.Name, tempItem.Description, "Plate");
                    
                }
            }
            else
            {
                //Get the sessions within the parent.
                sessions = MyObjectServer.GetSessions(MyCurrentParent);
                foreach (SessionItem tempItem in sessions)
                {
                    AddListViewItem(tempItem, tempItem.Name, tempItem.Description, tempItem.SessionType);
                }
            }

			this.Cursor = Cursors.Default;
		}

        private void AddListViewItem(Identifiable item, string name, string description, string type)
        {
            ListViewItem tempListItem;

            tempListItem = new ListViewItem(name);
            tempListItem.Tag = item;  //Set the object on the tag.
            tempListItem.SubItems.Add(description);
            tempListItem.SubItems.Add(type);
            tempListItem.SubItems.Add(ProjectList.Text);  //Also add the project name.
            tempListItem.ImageIndex = GetItemImageIndex(item);
            //Add the item if it is not already in the selected list.
            if (!GroupIsSelected(((Identifiable)tempListItem.Tag).ID))
            {
                AvailableGroupsList.Items.Add(tempListItem);
            }
        }


        private int GetItemImageIndex(Identifiable item)
        {
            //Return the number in the image list corresponding to each type of item.
            if (item is AssayGroup) { return 1; }
            else if (item is GenotypeGroup) { return 2; }
            else if (item is MarkerGroup) { return 3; }
            else if (item is Plate) { return 4; }
            else if (item is PlateGroup) { return 5; }
            else if (item is SampleGroup) { return 6; }
            else if (item is OldSavedSession) { return 7; }
            else if (item is SavedSession) { return 7; }
            else if (item is OldApprovedSession) { return 8; }
            else if (item is ApprovedSession) { return 8; }
            else { throw new Exception("Unknown item type encountered."); }

        }

		private FilterIndicator StoreSelection()
		{
            //Stores the selected items and returns an object indicating
            //which types of items that have been used for filtering.
			Identifiable idf;
			ListViewItem selectedListViewItem;
            FilterIndicator filters;

			this.Cursor = Cursors.WaitCursor;
			MyDataServer.ClearSelectedGroups();
			MyDataServer.ClearSelectedItems();

            filters = new FilterIndicator();
			//Insert the contents of all selected plates, groups or sessions into the right temporary tables.
			if (MyStartMode == SessionForm.StartMode.CreateSession)
			{
				foreach (ListViewItem lvi in SelectedGroupsList.Items)
				{
                    if (lvi.Tag is Plate) { filters.Plate = true; }
                    else if (lvi.Tag is PlateGroup) { filters.Plate = true; }
                    else if (lvi.Tag is GenotypeGroup) { filters.GenotypeGroup = true; }
                    else if (lvi.Tag is SampleGroup) { filters.SampleGroup = true; }
                    else if (lvi.Tag is IndividualGroup) { filters.IndividualGroup = true; }
                    else if (lvi.Tag is AssayGroup) { filters.AssayGroup = true; }
                    else if (lvi.Tag is MarkerGroup) { filters.MarkerGroup = true; }

                    MyObjectServer.AddSelectedItem((Identifiable)lvi.Tag);
				}
			}
			else
			{
                //Can only select one session.
                selectedListViewItem = AvailableGroupsList.SelectedItems[0];
                MyDataServer.StoreSelectionFromSession(((Identifiable)selectedListViewItem.Tag).ID);

                //Get the types of items in the session.
                filters = MyDataServer.GetUsedFilters();

			}

			//Insert any specially selected objects into the right temporary tables.
            if (SampleRadioButton.Checked)
            {
                for (int i = 0; i < SelectedItemList.TheCheckedListBox.Items.Count; i++)
                {
                    if (SelectedItemList.TheCheckedListBox.GetItemChecked(i))
                    {
                        filters.Sample = true;
                        idf = (Identifiable)SelectedItemList.TheCheckedListBox.Items[i];
                        MyDataServer.AddSelectedSample(idf.ID);
                    }
                }
            }
            else
            {
                for (int i = 0; i < SelectedItemList.TheCheckedListBox.Items.Count; i++)
                {
                    if (SelectedItemList.TheCheckedListBox.GetItemChecked(i))
                    {
                        filters.Individual = true;
                        idf = (Identifiable)SelectedItemList.TheCheckedListBox.Items[i];
                        MyDataServer.AddSelectedIndividual(idf.ID);
                    }
                }
            }

            if (AssayRadioButton.Checked)
            {
                for (int i = 0; i < SelectedExperimentList.TheCheckedListBox.Items.Count; i++)
                {
                    if (SelectedExperimentList.TheCheckedListBox.GetItemChecked(i))
                    {
                        filters.Assay = true;
                        idf = (Identifiable)SelectedExperimentList.TheCheckedListBox.Items[i];
                        MyDataServer.AddSelectedAssay(idf.ID);
                    }
                }
            }
            else
            {
                for (int i = 0; i < SelectedExperimentList.TheCheckedListBox.Items.Count; i++)
                {
                    if (SelectedExperimentList.TheCheckedListBox.GetItemChecked(i))
                    {
                        filters.Marker = true;
                        idf = (Identifiable)SelectedExperimentList.TheCheckedListBox.Items[i];
                        MyDataServer.AddSelectedMarker(idf.ID);
                    }
                }
            }
			this.Cursor = Cursors.Default;

            return filters;
		}


        private void AddSingleItemToPickList(Identifiable item)
        {
            //Perform thread-safe adding of an item to the item list.
            if (this.SelectedItemList.InvokeRequired)
            {
                AddSingleItemToPickListCallback del = new AddSingleItemToPickListCallback(AddSingleItemToPickList);
                this.Invoke(del, new object[] { item });
            }
            else
            {
                SelectedItemList.TheCheckedListBox.Items.Add(item);
            }
        }

        private void AddSingleExperimentToPickList(Identifiable item)
        {
            //Perform thread-safe adding of an experiment to the experiment list.
            if (this.SelectedExperimentList.InvokeRequired)
            {
                AddSingleItemToPickListCallback del = new AddSingleItemToPickListCallback(AddSingleExperimentToPickList);
                this.Invoke(del, new object[] { item });
            }
            else
            {
                SelectedExperimentList.TheCheckedListBox.Items.Add(item);
            }
        }

        private void RefreshCounter(AdvancedCheckedListBox2 list)
        {
            //Perform thread-safe counter refreshing.
            if (this.InvokeRequired)
            {
                RefreshCounterCallback del = new RefreshCounterCallback(RefreshCounter);
                this.Invoke(del, new object[] { list });
            }
            else
            {
                list.RefreshCounterText();
            }
        }

		private bool GroupIsSelected(int Id)
		{
            //Investigates whether a group or plate is already selected or not.
			foreach (ListViewItem lvi in SelectedGroupsList.Items)
			{
				if (((Identifiable)lvi.Tag).ID == Id)
				{
					return true;
				}
			}
			return false;
		}

        private void SetFormMode(bool readyForInput)
        {
            //Set the enabled property for controls visible during asynchronous operations.
            SelectedItemList.Enabled = readyForInput;
            SelectedExperimentList.Enabled = readyForInput;
            FinishButton.Enabled = readyForInput;
            StreamToFileButton.Enabled = readyForInput;
            SettingsButton.Enabled=readyForInput;
            PreviousButton.Enabled = (readyForInput ? (MyCurrentFrame > 1) : false);
            NextButton.Enabled = (readyForInput ? (MyCurrentFrame < TotalFrameNumber) : false);
            CloseButton.Enabled = readyForInput;
            FillItemsButton.Enabled = readyForInput;
            FillExperimentsButton.Enabled = readyForInput;
            SampleRadioButton.Enabled = readyForInput;
            IndividualRadioButton.Enabled = readyForInput;
            AssayRadioButton.Enabled = readyForInput;
            MarkerRadioButton.Enabled = readyForInput;

            StopButton.Enabled = !readyForInput;
        }

	    private void LogResultPlates(string action)
	    {
            var selectedPlates = new List<string>();
            for (int i = 0; i < SelectedGroupsList.Items.Count; i++)
            {
                var item = SelectedGroupsList.Items[i];
                if (item.Tag is Plate)
                {
                    selectedPlates.Add(item.Text);
                }
            }
	        foreach (var selectedPlate in selectedPlates)
	        {
                MyDataServer.LogResultPlate(selectedPlate, action);
            }
        }

        private void Finish()
        {
            bool loadError;
            bool platesFound;

            if ((MyStartMode == SessionForm.StartMode.LoadSession) && (AvailableGroupsList.SelectedItems.Count < 1))
            {
                MessageManager.ShowInformation("Please select a session.", this);
                return;
            }
            else if (MyStartMode == SessionForm.StartMode.CreateSession)
            {
                platesFound = false;
                for (int i = 0; i < SelectedGroupsList.Items.Count; i++)
                {
                    if (SelectedGroupsList.Items[i].Tag is Plate || SelectedGroupsList.Items[i].Tag is PlateGroup)
                    {
                        platesFound = true;
                        break;
                    }
                }
                if (!platesFound)
                {
                    MessageManager.ShowInformation("Please select at least one plate or plate working set.", this);
                    return;
                }
                LogResultPlates("Open unsaved session");
            }

            //Insert selections into temporary database tables.
            MyFilterIndicator = StoreSelection();

            if (MyStartMode == SessionForm.StartMode.LoadSession)
            {
                //Load the saved settings for an existing session.
                MySessionName = AvailableGroupsList.SelectedItems[0].Text;
                this.Cursor = Cursors.WaitCursor;
                MySessionSettings = MyDataServer.LoadSessionSettings(MySessionName, out loadError);
                this.Cursor = Cursors.Default;
                if (loadError)
                {
                    MessageManager.ShowWarning("All settings were not properly loaded.", this);
                }
                MyDataServer.LogResultPlatesInSession(MySessionName, "Open session");
            }
            else
            {
                MySessionName = "";
            }

            MyReturnFlag = true;
            Hide();

        }

        private void Items_DataReady(object sender, EventArgs e)
        {
            Identifiable tempIdentifiable;
            DataTable selectedItemsTable;
            SetFormModeDelegate d;

            try
            {
                //Make sure the user cannot abort while fetching data from DataServer.
                d = new SetFormModeDelegate(SetFormMode);
                this.Invoke(d, new object[] { true });
                //Fill items.
                selectedItemsTable = (MyDataServer.PopDeliveredDataSet()).Tables[0];
                for (int i = 0; i < selectedItemsTable.Rows.Count; i++)
                {
                    tempIdentifiable = new Identifiable(Convert.ToInt32(selectedItemsTable.Rows[i]["identifiable_id"]),
                        selectedItemsTable.Rows[i]["name"].ToString());
                    AddSingleItemToPickList(tempIdentifiable);
                }
                RefreshCounter(SelectedItemList);
            }
            catch (ThreadAbortException)
            {
                //Do nothing.
            }
        }

        private void Experiments_DataReady(object sender, EventArgs e)
        {
            Identifiable tempIdentifiable;
            DataTable selectedItemsTable;
            SetFormModeDelegate d;

            try
            {
                //Make sure the user cannot abort while fetching data from DataServer.
                d = new SetFormModeDelegate(SetFormMode);
                this.Invoke(d, new object[] { true });
                //Fill assays.
                selectedItemsTable = (MyDataServer.PopDeliveredDataSet()).Tables[0];
                for (int i = 0; i < selectedItemsTable.Rows.Count; i++)
                {
                    tempIdentifiable = new Identifiable(Convert.ToInt32(selectedItemsTable.Rows[i]["identifiable_id"]),
                        selectedItemsTable.Rows[i]["name"].ToString());
                    AddSingleExperimentToPickList(tempIdentifiable);
                }
                RefreshCounter(SelectedExperimentList);
            }
            catch (ThreadAbortException)
            {
                //Do nothing.
            }
        }

        private void DataServer_DataError(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageManager.ShowError(e.Exception, "Data error.", this);
        }

		private void ProjectList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				MyCurrentParent = (Identifiable)ProjectList.SelectedItem;
                if (!MyDataServer.GetCurrentUserPermittedInProject(MyCurrentParent.Name))
                {
                    MessageManager.ShowInformation("You do not have read permissions for project " + ProjectList.Text + ".");
                    AvailableGroupsList.Items.Clear();
                    return;
                }
                UpButton.Enabled = false;
				FillGroupList();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting project.", this);
			}
		}

		private void SelectGroupButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				foreach (ListViewItem lvi in AvailableGroupsList.Items)
				{
					if (lvi.Selected)
					{
						AvailableGroupsList.Items.Remove(lvi);
						SelectedGroupsList.Items.Add(lvi);
					}
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting plates and working sets.", this);
			}
		}

		private void DeselectButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				foreach (ListViewItem lvi in SelectedGroupsList.Items)
				{
					if (lvi.Selected)
					{
						SelectedGroupsList.Items.Remove(lvi);
					}
				}
				//Redraw the selected list.
				FillGroupList();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when deselecting plates and working sets.", this);
			}
		}

		private void PlateGroup_Doubleclick(object sender, System.EventArgs e)
		{
			ListViewItem selectedItem;

            //Open it up if it is a plate group, otherwise do nothing.
			try
			{
				if (AvailableGroupsList.SelectedItems.Count == 1)
				{
					selectedItem = AvailableGroupsList.SelectedItems[0];
					if (selectedItem.Tag is PlateGroup)
					{
						MyCurrentParent = (Identifiable)selectedItem.Tag;
						FillGroupList();
                        UpButton.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when opening plate working set.", this);
			}
		}


		private void FinishButton_Click(object sender, System.EventArgs e)
		{
			try
			{
                MyInteractiveFlag = true;
                Finish();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when starting session.", this);
			}
		}


		private void PreviousButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				MyCurrentFrame = (MyCurrentFrame > 1 ? MyCurrentFrame - 1 : MyCurrentFrame);
				ShowFrame(MyCurrentFrame);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when changing frame.", this);
			}
		}

		private void NextButton_Click(object sender, System.EventArgs e)
		{
            bool platesFound;

			try
			{
				if ((MyStartMode == SessionForm.StartMode.LoadSession) && (AvailableGroupsList.SelectedItems.Count < 1)
					&& (MyCurrentFrame == 1))
				{
					MessageManager.ShowInformation("Please select a session.", this);
					return;
				}
				if ((MyStartMode == SessionForm.StartMode.CreateSession) && (SelectedGroupsList.Items.Count < 1)
					&& (MyCurrentFrame == 1))
				{
                    platesFound = false;
                    for (int i = 0; i < SelectedGroupsList.Items.Count; i++)
                    {
                        if (SelectedGroupsList.Items[i].Tag is Plate || SelectedGroupsList.Items[i].Tag is PlateGroup)
                        {
                            platesFound = true;
                            break;
                        }
                    }
                    if (!platesFound)
                    {
                        MessageManager.ShowInformation("Please select at least one plate or plate working set.", this);
                        return;
                    }				
				}
				MyCurrentFrame = (MyCurrentFrame < TotalFrameNumber ? MyCurrentFrame + 1 : MyCurrentFrame);
				ShowFrame(MyCurrentFrame);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when changing frame.", this);
			}
		}

		private void CloseButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				Close();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when closing form.", this);
			}
		}

		private void SettingsButton_Click(object sender, System.EventArgs e)
		{
			SessionSettingsForm settingsForm;

			try
			{
				settingsForm = new SessionSettingsForm();
				settingsForm.GetSettings(MySessionSettings);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when attempting to change settings.", this);
			}
		}

		private void ShowGroupsMenuItem_Click(object sender, System.EventArgs e)
		{
			StaticListForm groupForm;
			DataTable groupTable;
            Identifiable session;

			try
			{
                //Show used plates and groups for a saved session.
				groupForm = new StaticListForm();
                session = (Identifiable)(AvailableGroupsList.SelectedItems[0].Tag);
				this.Cursor = Cursors.WaitCursor;
                groupTable = MyDataServer.GetUsedWsets(session.ID);
				this.Cursor = Cursors.Default;
				if (groupTable.Rows.Count < 1)
				{
					MessageManager.ShowInformation("No information could be found.", this);
					return;
				}
				groupForm.ShowInformation("Plates and Working Sets Used for " + session.Name, groupTable);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when attempting to show used plates and working sets.", this);
			}
		}

		private void DeleteSessionMenuItem_Click(object sender, System.EventArgs e)
		{
			string sessionName;
			int sessionId;

			try
			{
				sessionName = ((Identifiable)AvailableGroupsList.SelectedItems[0].Tag).Name;
				sessionId = ((Identifiable)AvailableGroupsList.SelectedItems[0].Tag).ID;
				if (MessageManager.ShowQuestion("Delete " + sessionName + "?", this) == DialogResult.No)
				{
					return;
				}
				MyDataServer.DeleteSession(sessionId);
				//Refresh.
				FillGroupList();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when attempting to delete session.", this);		
			}
		}

        private void FillItemsButton_Click(object sender, EventArgs e)
        {
            DataServer.IdentifiableType type;

            try
            {
                //Fetch items included in the selected plates and groups asynchronously.
                SetFormMode(false);
                type = (SampleRadioButton.Checked ? DataServer.IdentifiableType.Sample : DataServer.IdentifiableType.Individual);
                SelectedItemList.TheCheckedListBox.Items.Clear();
                MyDataReadyEventHandler = new EventHandler(Items_DataReady);
                MyDataErrorEventHandler = new ThreadExceptionEventHandler(DataServer_DataError);
                MyDataServer.SetDataAsyncEventHandlers(MyDataReadyEventHandler, MyDataErrorEventHandler);
                MyDataServer.GetSelectedItems(type, MyFilterIndicator, DataServer.Mode.Async);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to retrieve items.", this);
            }
        }

        private void FillExperimentsButton_Click(object sender, EventArgs e)
        {
            DataServer.IdentifiableType type;

            try
            {
                //Fetch assays included in the selected plates and groups asynchronously.
                SetFormMode(false);
                type = (AssayRadioButton.Checked ? DataServer.IdentifiableType.Assay : DataServer.IdentifiableType.Marker);
                SelectedExperimentList.TheCheckedListBox.Items.Clear();
                MyDataReadyEventHandler = new EventHandler(Experiments_DataReady);
                MyDataErrorEventHandler = new ThreadExceptionEventHandler(DataServer_DataError);
                MyDataServer.SetDataAsyncEventHandlers(MyDataReadyEventHandler, MyDataErrorEventHandler);
                MyDataServer.GetSelectedItems(type, MyFilterIndicator, DataServer.Mode.Async);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to retrieve assays.", this);
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Abort asynchronous database operations.
                MyDataServer.Abort();
                SetFormMode(true);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to abort operation.", this);
            }
        }

        private void ItemRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedItemList.ClearItems();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing item mode.", this);
            }
        }

        private void ExperimentRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedExperimentList.ClearItems();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing experiment mode.", this);
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            try
            {
                MyCurrentParent = (Identifiable)ProjectList.SelectedItem;
                FillGroupList();
                UpButton.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when leaving working set.", this);
            }
        }

        private void StreamToFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                MyInteractiveFlag = false;
                Finish();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when starting session.", this);
            }
        }

        private void SessionContextMenu_Popup(object sender, EventArgs e)
        {
            try
            {
                UnlockSessionMenuItem.Visible = (((string)MyConfiguration.Get("AllowUnlockFlag")).ToLower() == "true")
                    && (AvailableGroupsList.SelectedItems[0].Tag is ApprovedSession);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when showing session context menu.", this);
            }
        }

        private void UnlockSessionMenuItem_Click(object sender, EventArgs e)
        {
            string sessionName;
            int sessionId;

            try
            {
                sessionName = ((Identifiable)AvailableGroupsList.SelectedItems[0].Tag).Name;
                sessionId = ((Identifiable)AvailableGroupsList.SelectedItems[0].Tag).ID;
                if (MessageManager.ShowQuestion("Unlock " + sessionName + "?", this) == DialogResult.No)
                {
                    return;
                }
                MyDataServer.UnlockApprovedSession(sessionId);
                //Refresh.
                FillGroupList();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to unlock session.", this);
            }
        }

	}
}
