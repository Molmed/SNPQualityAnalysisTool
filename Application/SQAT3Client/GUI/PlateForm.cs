using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;


namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for PlateForm.
	/// </summary>
	public class PlateForm : System.Windows.Forms.Form
    {
        private IContainer components;
		private System.Windows.Forms.Panel TopPanel;
		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.TabControl ModeTabControl;
		private System.Windows.Forms.TabPage SelectionTabPage;
		private System.Windows.Forms.Panel PlatePanel;
		private System.Windows.Forms.Panel PlateListPanel;
		private System.Windows.Forms.GroupBox ItemGroupBox;
		private System.Windows.Forms.Splitter ExperimentItemSplitter;
		private System.Windows.Forms.GroupBox ExperimentGroupBox;
		private System.Windows.Forms.Panel PlateBottomPanel;
		private System.Windows.Forms.Panel PlateWellPanel;
		private System.Windows.Forms.GroupBox WellGroupBox;
		private Molmed.SQAT.GUI.PlateGrid Plate;
		private System.Windows.Forms.Label SelectedPlateLabel;
		private System.Windows.Forms.ComboBox PlateComboBox;
		private System.Windows.Forms.TabPage ExperimentRateTabPage;
		private System.Windows.Forms.GroupBox ExperimentRateListGroupBox;
		private System.Windows.Forms.Panel ExperimentBottomPanel;
        private System.Windows.Forms.Button ExperimentCutoffButton;
		private System.Windows.Forms.TabPage ItemRateTabPage;
		private System.Windows.Forms.GroupBox ItemRateListGroupBox;
        private System.Windows.Forms.Panel ItemBottomPanel;
		private System.Windows.Forms.Button ItemCutoffButton;
        private System.Windows.Forms.Button RejectSelectionButton;

        private DataServer MyDataServer;
        private DataTable MyExperimentTable, MyItemTable, MyWellTable;
        private TabPage WellRateTabPage;
        private Panel WellBottomPanel;
        private GroupBox WellRateListGroupBox;
        private Button WellCutoffButton;
        private AdvancedListView2 ExperimentRateListView;
        private AdvancedListView2 ItemRateListView;
        private AdvancedListView2 WellRateListView;
        private AdvancedCheckedListBox2 ItemCheckedListBox;
        private AdvancedCheckedListBox2 ExperimentCheckedListBox;
        private Button SetSelectionLoadButton;
        private SessionSettings MySessionSettings;

		public PlateForm(DataServer server, SessionSettings settings)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			MyDataServer = server;
            MySessionSettings = settings;
			FillPlateList();
            ExperimentCheckedListBox.ClearItems();
            ItemCheckedListBox.ClearItems();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlateForm));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.PlateComboBox = new System.Windows.Forms.ComboBox();
            this.SelectedPlateLabel = new System.Windows.Forms.Label();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ModeTabControl = new System.Windows.Forms.TabControl();
            this.SelectionTabPage = new System.Windows.Forms.TabPage();
            this.PlatePanel = new System.Windows.Forms.Panel();
            this.PlateListPanel = new System.Windows.Forms.Panel();
            this.ItemGroupBox = new System.Windows.Forms.GroupBox();
            this.ExperimentItemSplitter = new System.Windows.Forms.Splitter();
            this.ExperimentGroupBox = new System.Windows.Forms.GroupBox();
            this.PlateBottomPanel = new System.Windows.Forms.Panel();
            this.SetSelectionLoadButton = new System.Windows.Forms.Button();
            this.RejectSelectionButton = new System.Windows.Forms.Button();
            this.PlateWellPanel = new System.Windows.Forms.Panel();
            this.WellGroupBox = new System.Windows.Forms.GroupBox();
            this.ExperimentRateTabPage = new System.Windows.Forms.TabPage();
            this.ExperimentRateListGroupBox = new System.Windows.Forms.GroupBox();
            this.ExperimentBottomPanel = new System.Windows.Forms.Panel();
            this.ExperimentCutoffButton = new System.Windows.Forms.Button();
            this.ItemRateTabPage = new System.Windows.Forms.TabPage();
            this.ItemRateListGroupBox = new System.Windows.Forms.GroupBox();
            this.ItemBottomPanel = new System.Windows.Forms.Panel();
            this.ItemCutoffButton = new System.Windows.Forms.Button();
            this.WellRateTabPage = new System.Windows.Forms.TabPage();
            this.WellRateListGroupBox = new System.Windows.Forms.GroupBox();
            this.WellBottomPanel = new System.Windows.Forms.Panel();
            this.WellCutoffButton = new System.Windows.Forms.Button();
            this.ItemCheckedListBox = new Molmed.SQAT.GUI.AdvancedCheckedListBox2();
            this.ExperimentCheckedListBox = new Molmed.SQAT.GUI.AdvancedCheckedListBox2();
            this.Plate = new Molmed.SQAT.GUI.PlateGrid();
            this.ExperimentRateListView = new Molmed.SQAT.GUI.AdvancedListView2();
            this.ItemRateListView = new Molmed.SQAT.GUI.AdvancedListView2();
            this.WellRateListView = new Molmed.SQAT.GUI.AdvancedListView2();
            this.TopPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.ModeTabControl.SuspendLayout();
            this.SelectionTabPage.SuspendLayout();
            this.PlatePanel.SuspendLayout();
            this.PlateListPanel.SuspendLayout();
            this.ItemGroupBox.SuspendLayout();
            this.ExperimentGroupBox.SuspendLayout();
            this.PlateBottomPanel.SuspendLayout();
            this.PlateWellPanel.SuspendLayout();
            this.WellGroupBox.SuspendLayout();
            this.ExperimentRateTabPage.SuspendLayout();
            this.ExperimentRateListGroupBox.SuspendLayout();
            this.ExperimentBottomPanel.SuspendLayout();
            this.ItemRateTabPage.SuspendLayout();
            this.ItemRateListGroupBox.SuspendLayout();
            this.ItemBottomPanel.SuspendLayout();
            this.WellRateTabPage.SuspendLayout();
            this.WellRateListGroupBox.SuspendLayout();
            this.WellBottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.PlateComboBox);
            this.TopPanel.Controls.Add(this.SelectedPlateLabel);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(832, 32);
            this.TopPanel.TabIndex = 2;
            // 
            // PlateComboBox
            // 
            this.PlateComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PlateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlateComboBox.Location = new System.Drawing.Point(96, 8);
            this.PlateComboBox.Name = "PlateComboBox";
            this.PlateComboBox.Size = new System.Drawing.Size(464, 21);
            this.PlateComboBox.TabIndex = 0;
            this.PlateComboBox.SelectedIndexChanged += new System.EventHandler(this.PlateComboBox_SelectedIndexChanged);
            // 
            // SelectedPlateLabel
            // 
            this.SelectedPlateLabel.Location = new System.Drawing.Point(8, 8);
            this.SelectedPlateLabel.Name = "SelectedPlateLabel";
            this.SelectedPlateLabel.Size = new System.Drawing.Size(80, 16);
            this.SelectedPlateLabel.TabIndex = 3;
            this.SelectedPlateLabel.Text = "Selected plate:";
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 670);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(832, 48);
            this.BottomPanel.TabIndex = 4;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(752, 16);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "&Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ModeTabControl
            // 
            this.ModeTabControl.Controls.Add(this.SelectionTabPage);
            this.ModeTabControl.Controls.Add(this.ExperimentRateTabPage);
            this.ModeTabControl.Controls.Add(this.ItemRateTabPage);
            this.ModeTabControl.Controls.Add(this.WellRateTabPage);
            this.ModeTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModeTabControl.Location = new System.Drawing.Point(0, 32);
            this.ModeTabControl.Name = "ModeTabControl";
            this.ModeTabControl.SelectedIndex = 0;
            this.ModeTabControl.Size = new System.Drawing.Size(832, 638);
            this.ModeTabControl.TabIndex = 1;
            // 
            // SelectionTabPage
            // 
            this.SelectionTabPage.Controls.Add(this.PlatePanel);
            this.SelectionTabPage.Location = new System.Drawing.Point(4, 22);
            this.SelectionTabPage.Name = "SelectionTabPage";
            this.SelectionTabPage.Size = new System.Drawing.Size(824, 612);
            this.SelectionTabPage.TabIndex = 0;
            this.SelectionTabPage.Text = "Selection";
            this.SelectionTabPage.UseVisualStyleBackColor = true;
            // 
            // PlatePanel
            // 
            this.PlatePanel.Controls.Add(this.PlateListPanel);
            this.PlatePanel.Controls.Add(this.PlateBottomPanel);
            this.PlatePanel.Controls.Add(this.PlateWellPanel);
            this.PlatePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlatePanel.Location = new System.Drawing.Point(0, 0);
            this.PlatePanel.Name = "PlatePanel";
            this.PlatePanel.Size = new System.Drawing.Size(824, 612);
            this.PlatePanel.TabIndex = 4;
            // 
            // PlateListPanel
            // 
            this.PlateListPanel.Controls.Add(this.ItemGroupBox);
            this.PlateListPanel.Controls.Add(this.ExperimentItemSplitter);
            this.PlateListPanel.Controls.Add(this.ExperimentGroupBox);
            this.PlateListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlateListPanel.Location = new System.Drawing.Point(0, 336);
            this.PlateListPanel.Name = "PlateListPanel";
            this.PlateListPanel.Size = new System.Drawing.Size(824, 228);
            this.PlateListPanel.TabIndex = 6;
            // 
            // ItemGroupBox
            // 
            this.ItemGroupBox.Controls.Add(this.ItemCheckedListBox);
            this.ItemGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemGroupBox.Location = new System.Drawing.Point(392, 0);
            this.ItemGroupBox.Name = "ItemGroupBox";
            this.ItemGroupBox.Size = new System.Drawing.Size(432, 228);
            this.ItemGroupBox.TabIndex = 2;
            this.ItemGroupBox.TabStop = false;
            this.ItemGroupBox.Text = "Items";
            // 
            // ExperimentItemSplitter
            // 
            this.ExperimentItemSplitter.Location = new System.Drawing.Point(384, 0);
            this.ExperimentItemSplitter.Name = "ExperimentItemSplitter";
            this.ExperimentItemSplitter.Size = new System.Drawing.Size(8, 228);
            this.ExperimentItemSplitter.TabIndex = 1;
            this.ExperimentItemSplitter.TabStop = false;
            // 
            // ExperimentGroupBox
            // 
            this.ExperimentGroupBox.Controls.Add(this.ExperimentCheckedListBox);
            this.ExperimentGroupBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.ExperimentGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ExperimentGroupBox.Name = "ExperimentGroupBox";
            this.ExperimentGroupBox.Size = new System.Drawing.Size(384, 228);
            this.ExperimentGroupBox.TabIndex = 0;
            this.ExperimentGroupBox.TabStop = false;
            this.ExperimentGroupBox.Text = "Experiments";
            // 
            // PlateBottomPanel
            // 
            this.PlateBottomPanel.Controls.Add(this.SetSelectionLoadButton);
            this.PlateBottomPanel.Controls.Add(this.RejectSelectionButton);
            this.PlateBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PlateBottomPanel.Location = new System.Drawing.Point(0, 564);
            this.PlateBottomPanel.Name = "PlateBottomPanel";
            this.PlateBottomPanel.Size = new System.Drawing.Size(824, 48);
            this.PlateBottomPanel.TabIndex = 5;
            // 
            // SetSelectionLoadButton
            // 
            this.SetSelectionLoadButton.Location = new System.Drawing.Point(616, 16);
            this.SetSelectionLoadButton.Name = "SetSelectionLoadButton";
            this.SetSelectionLoadButton.Size = new System.Drawing.Size(104, 24);
            this.SetSelectionLoadButton.TabIndex = 1;
            this.SetSelectionLoadButton.Text = "Set Status \'&Load\'";
            this.SetSelectionLoadButton.UseVisualStyleBackColor = true;
            this.SetSelectionLoadButton.Click += new System.EventHandler(this.SetSelectionLoadButton_Click);
            // 
            // RejectSelectionButton
            // 
            this.RejectSelectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RejectSelectionButton.Location = new System.Drawing.Point(736, 16);
            this.RejectSelectionButton.Name = "RejectSelectionButton";
            this.RejectSelectionButton.Size = new System.Drawing.Size(72, 24);
            this.RejectSelectionButton.TabIndex = 0;
            this.RejectSelectionButton.Text = "&Reject";
            this.RejectSelectionButton.Click += new System.EventHandler(this.RejectSelectionButton_Click);
            // 
            // PlateWellPanel
            // 
            this.PlateWellPanel.Controls.Add(this.WellGroupBox);
            this.PlateWellPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.PlateWellPanel.Location = new System.Drawing.Point(0, 0);
            this.PlateWellPanel.Name = "PlateWellPanel";
            this.PlateWellPanel.Size = new System.Drawing.Size(824, 336);
            this.PlateWellPanel.TabIndex = 4;
            // 
            // WellGroupBox
            // 
            this.WellGroupBox.Controls.Add(this.Plate);
            this.WellGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WellGroupBox.Location = new System.Drawing.Point(0, 0);
            this.WellGroupBox.Name = "WellGroupBox";
            this.WellGroupBox.Size = new System.Drawing.Size(824, 336);
            this.WellGroupBox.TabIndex = 2;
            this.WellGroupBox.TabStop = false;
            this.WellGroupBox.Text = "Positions";
            // 
            // ExperimentRateTabPage
            // 
            this.ExperimentRateTabPage.Controls.Add(this.ExperimentRateListGroupBox);
            this.ExperimentRateTabPage.Controls.Add(this.ExperimentBottomPanel);
            this.ExperimentRateTabPage.Location = new System.Drawing.Point(4, 22);
            this.ExperimentRateTabPage.Name = "ExperimentRateTabPage";
            this.ExperimentRateTabPage.Size = new System.Drawing.Size(824, 612);
            this.ExperimentRateTabPage.TabIndex = 1;
            this.ExperimentRateTabPage.Text = "Experiment Success Rates";
            this.ExperimentRateTabPage.UseVisualStyleBackColor = true;
            // 
            // ExperimentRateListGroupBox
            // 
            this.ExperimentRateListGroupBox.Controls.Add(this.ExperimentRateListView);
            this.ExperimentRateListGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentRateListGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ExperimentRateListGroupBox.Name = "ExperimentRateListGroupBox";
            this.ExperimentRateListGroupBox.Size = new System.Drawing.Size(824, 564);
            this.ExperimentRateListGroupBox.TabIndex = 0;
            this.ExperimentRateListGroupBox.TabStop = false;
            this.ExperimentRateListGroupBox.Text = "Experiment Success Rates in Plate";
            // 
            // ExperimentBottomPanel
            // 
            this.ExperimentBottomPanel.Controls.Add(this.ExperimentCutoffButton);
            this.ExperimentBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExperimentBottomPanel.Location = new System.Drawing.Point(0, 564);
            this.ExperimentBottomPanel.Name = "ExperimentBottomPanel";
            this.ExperimentBottomPanel.Size = new System.Drawing.Size(824, 48);
            this.ExperimentBottomPanel.TabIndex = 1;
            // 
            // ExperimentCutoffButton
            // 
            this.ExperimentCutoffButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExperimentCutoffButton.Location = new System.Drawing.Point(736, 16);
            this.ExperimentCutoffButton.Name = "ExperimentCutoffButton";
            this.ExperimentCutoffButton.Size = new System.Drawing.Size(72, 24);
            this.ExperimentCutoffButton.TabIndex = 0;
            this.ExperimentCutoffButton.Text = "&Cutoff...";
            this.ExperimentCutoffButton.Click += new System.EventHandler(this.ExperimentCutoffButton_Click);
            // 
            // ItemRateTabPage
            // 
            this.ItemRateTabPage.Controls.Add(this.ItemRateListGroupBox);
            this.ItemRateTabPage.Controls.Add(this.ItemBottomPanel);
            this.ItemRateTabPage.Location = new System.Drawing.Point(4, 22);
            this.ItemRateTabPage.Name = "ItemRateTabPage";
            this.ItemRateTabPage.Size = new System.Drawing.Size(824, 612);
            this.ItemRateTabPage.TabIndex = 2;
            this.ItemRateTabPage.Text = "Item Success Rates";
            this.ItemRateTabPage.UseVisualStyleBackColor = true;
            // 
            // ItemRateListGroupBox
            // 
            this.ItemRateListGroupBox.Controls.Add(this.ItemRateListView);
            this.ItemRateListGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemRateListGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ItemRateListGroupBox.Name = "ItemRateListGroupBox";
            this.ItemRateListGroupBox.Size = new System.Drawing.Size(824, 564);
            this.ItemRateListGroupBox.TabIndex = 0;
            this.ItemRateListGroupBox.TabStop = false;
            this.ItemRateListGroupBox.Text = "Item Success Rates in Plate";
            // 
            // ItemBottomPanel
            // 
            this.ItemBottomPanel.Controls.Add(this.ItemCutoffButton);
            this.ItemBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ItemBottomPanel.Location = new System.Drawing.Point(0, 564);
            this.ItemBottomPanel.Name = "ItemBottomPanel";
            this.ItemBottomPanel.Size = new System.Drawing.Size(824, 48);
            this.ItemBottomPanel.TabIndex = 1;
            // 
            // ItemCutoffButton
            // 
            this.ItemCutoffButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemCutoffButton.Location = new System.Drawing.Point(736, 16);
            this.ItemCutoffButton.Name = "ItemCutoffButton";
            this.ItemCutoffButton.Size = new System.Drawing.Size(72, 24);
            this.ItemCutoffButton.TabIndex = 2;
            this.ItemCutoffButton.Text = "&Cutoff...";
            this.ItemCutoffButton.Click += new System.EventHandler(this.ItemCutoffButton_Click);
            // 
            // WellRateTabPage
            // 
            this.WellRateTabPage.Controls.Add(this.WellRateListGroupBox);
            this.WellRateTabPage.Controls.Add(this.WellBottomPanel);
            this.WellRateTabPage.Location = new System.Drawing.Point(4, 22);
            this.WellRateTabPage.Name = "WellRateTabPage";
            this.WellRateTabPage.Size = new System.Drawing.Size(824, 612);
            this.WellRateTabPage.TabIndex = 3;
            this.WellRateTabPage.Text = "Well Success Rates";
            this.WellRateTabPage.UseVisualStyleBackColor = true;
            // 
            // WellRateListGroupBox
            // 
            this.WellRateListGroupBox.Controls.Add(this.WellRateListView);
            this.WellRateListGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WellRateListGroupBox.Location = new System.Drawing.Point(0, 0);
            this.WellRateListGroupBox.Name = "WellRateListGroupBox";
            this.WellRateListGroupBox.Size = new System.Drawing.Size(824, 564);
            this.WellRateListGroupBox.TabIndex = 0;
            this.WellRateListGroupBox.TabStop = false;
            this.WellRateListGroupBox.Text = "Well Success Rates in Plate";
            // 
            // WellBottomPanel
            // 
            this.WellBottomPanel.Controls.Add(this.WellCutoffButton);
            this.WellBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.WellBottomPanel.Location = new System.Drawing.Point(0, 564);
            this.WellBottomPanel.Name = "WellBottomPanel";
            this.WellBottomPanel.Size = new System.Drawing.Size(824, 48);
            this.WellBottomPanel.TabIndex = 1;
            // 
            // WellCutoffButton
            // 
            this.WellCutoffButton.Location = new System.Drawing.Point(736, 16);
            this.WellCutoffButton.Name = "WellCutoffButton";
            this.WellCutoffButton.Size = new System.Drawing.Size(72, 24);
            this.WellCutoffButton.TabIndex = 2;
            this.WellCutoffButton.Text = "&Cutoff...";
            this.WellCutoffButton.UseVisualStyleBackColor = true;
            this.WellCutoffButton.Click += new System.EventHandler(this.WellCutoffButton_Click);
            // 
            // ItemCheckedListBox
            // 
            this.ItemCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemCheckedListBox.Location = new System.Drawing.Point(3, 16);
            this.ItemCheckedListBox.Name = "ItemCheckedListBox";
            this.ItemCheckedListBox.Size = new System.Drawing.Size(426, 209);
            this.ItemCheckedListBox.TabIndex = 0;
            // 
            // ExperimentCheckedListBox
            // 
            this.ExperimentCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentCheckedListBox.Location = new System.Drawing.Point(3, 16);
            this.ExperimentCheckedListBox.Name = "ExperimentCheckedListBox";
            this.ExperimentCheckedListBox.Size = new System.Drawing.Size(378, 209);
            this.ExperimentCheckedListBox.TabIndex = 0;
            // 
            // Plate
            // 
            this.Plate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Plate.Location = new System.Drawing.Point(16, 24);
            this.Plate.Name = "Plate";
            this.Plate.Size = new System.Drawing.Size(800, 304);
            this.Plate.TabIndex = 2;
            // 
            // ExperimentRateListView
            // 
            this.ExperimentRateListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentRateListView.Location = new System.Drawing.Point(3, 16);
            this.ExperimentRateListView.Name = "ExperimentRateListView";
            this.ExperimentRateListView.Size = new System.Drawing.Size(818, 545);
            this.ExperimentRateListView.TabIndex = 0;
            // 
            // ItemRateListView
            // 
            this.ItemRateListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemRateListView.Location = new System.Drawing.Point(3, 16);
            this.ItemRateListView.Name = "ItemRateListView";
            this.ItemRateListView.Size = new System.Drawing.Size(818, 545);
            this.ItemRateListView.TabIndex = 0;
            // 
            // WellRateListView
            // 
            this.WellRateListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WellRateListView.Location = new System.Drawing.Point(3, 16);
            this.WellRateListView.Name = "WellRateListView";
            this.WellRateListView.Size = new System.Drawing.Size(818, 545);
            this.WellRateListView.TabIndex = 0;
            // 
            // PlateForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(832, 718);
            this.Controls.Add(this.ModeTabControl);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.TopPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Plates";
            this.TopPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.ModeTabControl.ResumeLayout(false);
            this.SelectionTabPage.ResumeLayout(false);
            this.PlatePanel.ResumeLayout(false);
            this.PlateListPanel.ResumeLayout(false);
            this.ItemGroupBox.ResumeLayout(false);
            this.ExperimentGroupBox.ResumeLayout(false);
            this.PlateBottomPanel.ResumeLayout(false);
            this.PlateWellPanel.ResumeLayout(false);
            this.WellGroupBox.ResumeLayout(false);
            this.ExperimentRateTabPage.ResumeLayout(false);
            this.ExperimentRateListGroupBox.ResumeLayout(false);
            this.ExperimentBottomPanel.ResumeLayout(false);
            this.ItemRateTabPage.ResumeLayout(false);
            this.ItemRateListGroupBox.ResumeLayout(false);
            this.ItemBottomPanel.ResumeLayout(false);
            this.WellRateTabPage.ResumeLayout(false);
            this.WellRateListGroupBox.ResumeLayout(false);
            this.WellBottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void FillPlateList()
		{
			DataTable plateTable;
			Identifiable tempItem;
			string tempIdentifier;
			int tempID;

			this.Cursor = Cursors.WaitCursor;
			plateTable = MyDataServer.GetPlatesInSelection();
            if (plateTable.Rows.Count < 1)
            {
                throw new Exception("No plates could be loaded for the current selection or session.");
            }
			for (int i = 0; i < plateTable.Rows.Count ; i++)
			{
				tempID = Convert.ToInt32(plateTable.Rows[i][0]);
				tempIdentifier = plateTable.Rows[i][1].ToString();
				tempItem = new Identifiable(tempID, tempIdentifier);
				PlateComboBox.Items.Add(tempItem);
			}
			this.Cursor = Cursors.Default;
		}

		private void FillPlate(int plateId)
		{
			DataSet plateDataSet;
			DataTable positionTable, allItemTable;
			Identifiable tempListItem;
			int x, y;
			string [] colHeader, rowHeader;
			object [,] matrix;

            const int cellsX = 24, cellsY = 16;

			this.Cursor = Cursors.WaitCursor;
			plateDataSet = MyDataServer.GetPlateContents(plateId);

			positionTable = plateDataSet.Tables[0];
			//Save item and experiment tables in "object global" variables to
			//be able to read information later.
			MyItemTable = plateDataSet.Tables[1];
			MyExperimentTable = plateDataSet.Tables[2];
            MyWellTable = plateDataSet.Tables[3];

            //Get a list of all items, including controls.
            allItemTable = MyDataServer.GetAllItems();

			colHeader = new string[cellsX];
			rowHeader = new string[cellsY];
			matrix = new object[cellsX, cellsY];

			//Check that we don't have more position info than number of cells.
			if (positionTable.Rows.Count > cellsX * cellsY)
			{
				throw new Exception("Invalid number of plate positions.");
			}

            //Create column headers.
            for (int i = 1; i <= 24; i++)
            {
                colHeader[i - 1] = i.ToString(); 
            }

            //Create row headers.
            for (int i = 1; i <= 16; i++)
            {
                rowHeader[i - 1] = Convert.ToChar(i + 64).ToString();
            }

			//Read the positions and set the corresponding cells to indicate that they are used.
			for (int i = 0 ; i < positionTable.Rows.Count ; i++)
			{
				x = Convert.ToInt32(positionTable.Rows[i]["pos_x"]);
				y = Convert.ToInt32(positionTable.Rows[i]["pos_y"]);
				if (x > cellsX || y > cellsY)
				{
					throw new Exception("Invalid plate position.");
				}
				//Set the matrix position to anything other than null, in this case the integer 1.
				matrix[x - 1, y - 1] = 1;
			}

			//Show used wells.
			Plate.ShowGrid(matrix, colHeader, rowHeader, new Size(19, 17), new Size(17, 15));
            WellRateListView.ShowFormattedTable(MyWellTable);

			//Show used items.
			ItemCheckedListBox.TheCheckedListBox.BeginUpdate();
			ItemCheckedListBox.TheCheckedListBox.Items.Clear();
			for (int i = 0 ; i < allItemTable.Rows.Count ; i++)
			{
				tempListItem = new Identifiable(Convert.ToInt32(allItemTable.Rows[i]["item_id"]),
					allItemTable.Rows[i]["Item"].ToString());
				ItemCheckedListBox.TheCheckedListBox.Items.Add(tempListItem, CheckState.Checked);
			}
			ItemCheckedListBox.TheCheckedListBox.EndUpdate();
			ItemRateListView.ShowFormattedTable(MyItemTable);
            ItemCheckedListBox.RefreshCounterText();

			//Show used experiments.
			ExperimentCheckedListBox.TheCheckedListBox.BeginUpdate();
			ExperimentCheckedListBox.TheCheckedListBox.Items.Clear();
			for (int i = 0 ; i < MyExperimentTable.Rows.Count ; i++)
			{
				tempListItem = new Identifiable(Convert.ToInt32(MyExperimentTable.Rows[i]["experiment_id"]),
					MyExperimentTable.Rows[i]["Experiment"].ToString());
				ExperimentCheckedListBox.TheCheckedListBox.Items.Add(tempListItem, CheckState.Checked);
			}
			ExperimentCheckedListBox.TheCheckedListBox.EndUpdate();
			ExperimentRateListView.ShowFormattedTable(MyExperimentTable);
            ExperimentCheckedListBox.RefreshCounterText();
			this.Cursor = Cursors.Default;

		}

        private void UpdateSelectionStatus(string newStatus)
        {
            Identifiable tempItem, selectedPlate;
            Point[] selectedWells;


            this.Cursor = Cursors.WaitCursor;
            //Get the selected plate.
            selectedPlate = (Identifiable)PlateComboBox.SelectedItem;
            //Clear any previous selections.
            MyDataServer.ClearPlateSelection();

            //Add selected wells.
            selectedWells = Plate.GetSelectedIndices();
            if (selectedWells.GetLength(0) < 1)
            {
                this.Cursor = Cursors.Default;
                MessageManager.ShowInformation("Please select at least one well.", this);
                return;
            }
            for (int i = 0; i < selectedWells.GetLength(0); i++)
            {
                MyDataServer.AddPlateWell(selectedWells[i].X, selectedWells[i].Y);
            }

            //Add checked experiments.
            if (ExperimentCheckedListBox.TheCheckedListBox.CheckedItems.Count < 1)
            {
                this.Cursor = Cursors.Default;
                MessageManager.ShowInformation("Please select at least one experiment.", this);
                return;
            }
            for (int i = 0; i < ExperimentCheckedListBox.TheCheckedListBox.CheckedItems.Count; i++)
            {
                tempItem = (Identifiable)ExperimentCheckedListBox.TheCheckedListBox.CheckedItems[i];
                MyDataServer.AddPlateExperiment(tempItem.ID);
            }

            //Add checked items.
            if (ItemCheckedListBox.TheCheckedListBox.CheckedItems.Count < 1)
            {
                this.Cursor = Cursors.Default;
                MessageManager.ShowInformation("Please select at least one item.", this);
                return;
            }
            for (int i = 0; i < ItemCheckedListBox.TheCheckedListBox.CheckedItems.Count; i++)
            {
                tempItem = (Identifiable)ItemCheckedListBox.TheCheckedListBox.CheckedItems[i];
                MyDataServer.AddPlateItem(tempItem.ID);
            }
            this.Cursor = Cursors.Default;

            if (MessageManager.ShowQuestion("Set status to '" + newStatus + "' for the selected items from the " +
                "selected experiments in the marked wells?") == DialogResult.No)
            {
                return;
            }

            MyDataServer.UpdatePlateSelectionStatus(selectedPlate.ID, MySessionSettings, newStatus);

        }

		private void CloseButton_Click(object sender, System.EventArgs e)
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

		private void PlateComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Identifiable selectedPlate;

			try
			{
				selectedPlate = (Identifiable)PlateComboBox.SelectedItem;
				FillPlate(selectedPlate.ID);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when loading plate.", this);
			}
		}


		private void RejectSelectionButton_Click(object sender, System.EventArgs e)
		{

            try
            {
                UpdateSelectionStatus("rejected");
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to reject selection.", this);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
		}

        private void SetSelectionLoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateSelectionStatus("load");
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to reject selection.", this);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


		private void ExperimentCutoffButton_Click(object sender, System.EventArgs e)
		{
			int tempExperimentId, cutoffPercent, experimentCount = 0;
			double tempRate;
            Identifiable selectedPlate;
            GetNumberForm inputForm;

			try
			{
                inputForm = new GetNumberForm();
                if (inputForm.GetNumber(this, "Experiment Cutoff", 0, 100, 80, "Select experiment success rate cutoff: ", out cutoffPercent) == false)
                {
                    //User cancelled.
                    return;
                }
				this.Cursor = Cursors.WaitCursor;
			    //Get the selected plate.
			    selectedPlate = (Identifiable)PlateComboBox.SelectedItem;
                //Clear any previous selections.
                MyDataServer.ClearPlateSelection();

				for (int i = 0 ; i < MyExperimentTable.Rows.Count ; i++)
				{
					//Read information from the DataTable variable to be able
					//to get the success rate properly.
					tempExperimentId = Convert.ToInt32(MyExperimentTable.Rows[i]["experiment_id"] );
					tempRate = Convert.ToDouble(MyExperimentTable.Rows[i]["Genotyping success excluding controls"]);
					//Add to rejection list if below cutoff.
					if (100.0 * tempRate < (double) cutoffPercent)
					{
						MyDataServer.AddPlateExperiment(tempExperimentId);
                        experimentCount++;
					}
				}
				this.Cursor = Cursors.Default;

				if (experimentCount < 1)
				{
					MessageManager.ShowInformation("No results to reject.", this);
					return;
				}
                if (MessageManager.ShowQuestion("Reject genotypes from the " + experimentCount + " experiments below " +
                    "the cutoff in the selected plate?") == DialogResult.No)
                {
                    return;
                }
                MyDataServer.RejectPlateExperiments(selectedPlate.ID, MySessionSettings);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when rejecting.", this);
			}
		}

		private void ItemCutoffButton_Click(object sender, System.EventArgs e)
		{
			int tempItemId, cutoffPercent, itemCount = 0;
			double tempRate;
            Identifiable selectedPlate;
            GetNumberForm inputForm;

			try
			{
                inputForm = new GetNumberForm();
                if (inputForm.GetNumber(this, "Item Cutoff", 0, 100, 80, "Select item success rate cutoff: ", out cutoffPercent) == false)
                {
                    //User cancelled.
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
			    //Get the selected plate.
			    selectedPlate = (Identifiable)PlateComboBox.SelectedItem;
				//Clear any previous selections.
                MyDataServer.ClearPlateSelection();

				for (int i = 0 ; i < MyItemTable.Rows.Count ; i++)
				{
					//Read information from the DataTable variable to be able
					//to get the success rate properly.
					tempItemId = Convert.ToInt32(MyItemTable.Rows[i]["item_id"] );
					tempRate = Convert.ToDouble(MyItemTable.Rows[i]["Genotyping success excluding controls"]);
					//Add to rejection list if below cutoff.
					if ((100.0 * tempRate) < (double) cutoffPercent)
					{
						MyDataServer.AddPlateItem(tempItemId);
                        itemCount++;
					}
				}
				this.Cursor = Cursors.Default;

				if (itemCount < 1)
				{
					MessageManager.ShowInformation("No results to reject.", this);
					return;
				}
                if (MessageManager.ShowQuestion("Reject genotypes from the " + itemCount + " items below " +
                    "the cutoff in the selected plate?") == DialogResult.No)
                {
                    return;
                }

                MyDataServer.RejectPlateItems(selectedPlate.ID, MySessionSettings);
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when rejecting.", this);
			}
		}

        private void WellCutoffButton_Click(object sender, EventArgs e)
        {
            Point tempWellId;
            int cutoffPercent, wellCount = 0;
            double tempRate;
            Identifiable selectedPlate;
            string [] pos;
            GetNumberForm inputForm;

            try
            {
                inputForm = new GetNumberForm();
                if (inputForm.GetNumber(this, "Well Cutoff", 0, 100, 80, "Select well success rate cutoff: ", out cutoffPercent) == false)
                {
                    //User cancelled.
                    return;
                } 
                this.Cursor = Cursors.WaitCursor;
                //Get the selected plate.
                selectedPlate = (Identifiable)PlateComboBox.SelectedItem;
                //Clear any previous selections.
                MyDataServer.ClearPlateSelection();

                for (int i = 0; i < MyWellTable.Rows.Count; i++)
                {
                    //Read information from the DataTable variable to be able
                    //to get the success rate properly.
                    pos = MyWellTable.Rows[i]["well_id"].ToString().Split(new char[1] {','});
                    tempWellId = new Point(Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]));
                    tempRate = Convert.ToDouble(MyWellTable.Rows[i]["Genotyping success excluding controls"]);
                    //Add to rejection list if below cutoff.
                    if ((100.0 * tempRate) < (double)cutoffPercent)
                    {
                        MyDataServer.AddPlateWell(tempWellId.X, tempWellId.Y);
                        wellCount++;
                    }
                }
                this.Cursor = Cursors.Default;

                if (wellCount < 1)
                {
                    MessageManager.ShowInformation("No results to reject.", this);
                    return;
                }
                if (MessageManager.ShowQuestion("Reject genotypes from the " + wellCount + " wells below " +
                    "the cutoff in the selected plate?") == DialogResult.No)
                {
                    return;
                }

                MyDataServer.RejectPlateWells(selectedPlate.ID);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when rejecting.", this);
            }
        }



	}
}
