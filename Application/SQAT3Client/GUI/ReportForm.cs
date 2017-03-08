using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using System.Data;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for ReportForm.
	/// </summary>
	public class ReportForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.CheckBox ExperimentStatisticsCheckBox;
		private System.Windows.Forms.CheckBox ItemStatisticsCheckBox;
		private System.Windows.Forms.CheckBox ResultsCheckBox;
		private System.Windows.Forms.RadioButton TableExperimentRowRadioButton;
		private System.Windows.Forms.RadioButton TableExperimentColumnRadioButton;
		private System.Windows.Forms.RadioButton PairwiseAllRadioButton;
		private System.Windows.Forms.RadioButton PairwiseApprovedRadioButton;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.CheckBox SourceInfoCheckBox;
		private System.Windows.Forms.Button ExperimentColumnsButton;
		private System.Windows.Forms.Button ItemColumnsButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CheckedListForm MyMarkerColumnsForm, MyItemColumnsForm, MyAnnotationTypesForm;
		private System.Windows.Forms.SaveFileDialog MySaveFileDialog;
		private System.Windows.Forms.Panel TopPanel;
		private System.Windows.Forms.Label ReportTypeLabel;
		private System.Windows.Forms.RadioButton TextFileRadioButton;
		private System.Windows.Forms.RadioButton XMLFileRadioButton;
		private System.Windows.Forms.CheckBox MarkerAnnotationsCheckBox;
		private System.Windows.Forms.Button MarkerAnnotationsButton;
        private CheckBox SelectionPlatesCheckBox;
        private Label NoResultLabel;
        private TextBox NoResultTextBox;
        private bool MyOKFlag;
        private CheckBox SelectionDetailedFilterCheckBox;
        private CheckBox SelectionGroupsCheckBox;
        private string MyFileName;
        private string MyFilePath;
        private TextBox PathTextBox;
        private CheckBox SplitFileCheckBox;
        private Label FilegroupLabel;
        private Label PathLabel;
        private Button BrowseDirectoriesButton;
        private TextBox FileGroupTextBox;
        private FolderBrowserDialog MyFolderBrowserDialog;
        private CheckBox ControlItemStatisticsCheckBox;
        private bool MyForStreaming;

        public ReportForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ControlItemStatisticsCheckBox = new System.Windows.Forms.CheckBox();
            this.SelectionDetailedFilterCheckBox = new System.Windows.Forms.CheckBox();
            this.SelectionGroupsCheckBox = new System.Windows.Forms.CheckBox();
            this.NoResultTextBox = new System.Windows.Forms.TextBox();
            this.NoResultLabel = new System.Windows.Forms.Label();
            this.SelectionPlatesCheckBox = new System.Windows.Forms.CheckBox();
            this.MarkerAnnotationsButton = new System.Windows.Forms.Button();
            this.MarkerAnnotationsCheckBox = new System.Windows.Forms.CheckBox();
            this.ItemColumnsButton = new System.Windows.Forms.Button();
            this.ExperimentColumnsButton = new System.Windows.Forms.Button();
            this.SourceInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.PairwiseApprovedRadioButton = new System.Windows.Forms.RadioButton();
            this.PairwiseAllRadioButton = new System.Windows.Forms.RadioButton();
            this.TableExperimentColumnRadioButton = new System.Windows.Forms.RadioButton();
            this.TableExperimentRowRadioButton = new System.Windows.Forms.RadioButton();
            this.ResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.ItemStatisticsCheckBox = new System.Windows.Forms.CheckBox();
            this.ExperimentStatisticsCheckBox = new System.Windows.Forms.CheckBox();
            this.MySaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.BrowseDirectoriesButton = new System.Windows.Forms.Button();
            this.FileGroupTextBox = new System.Windows.Forms.TextBox();
            this.FilegroupLabel = new System.Windows.Forms.Label();
            this.PathLabel = new System.Windows.Forms.Label();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.SplitFileCheckBox = new System.Windows.Forms.CheckBox();
            this.XMLFileRadioButton = new System.Windows.Forms.RadioButton();
            this.TextFileRadioButton = new System.Windows.Forms.RadioButton();
            this.ReportTypeLabel = new System.Windows.Forms.Label();
            this.MyFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.BottomPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.SaveButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 423);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(566, 45);
            this.BottomPanel.TabIndex = 0;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(486, 13);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 24;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveButton.Location = new System.Drawing.Point(16, 13);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(72, 24);
            this.SaveButton.TabIndex = 23;
            this.SaveButton.Text = "&Save...";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ControlItemStatisticsCheckBox);
            this.MainPanel.Controls.Add(this.SelectionDetailedFilterCheckBox);
            this.MainPanel.Controls.Add(this.SelectionGroupsCheckBox);
            this.MainPanel.Controls.Add(this.NoResultTextBox);
            this.MainPanel.Controls.Add(this.NoResultLabel);
            this.MainPanel.Controls.Add(this.SelectionPlatesCheckBox);
            this.MainPanel.Controls.Add(this.MarkerAnnotationsButton);
            this.MainPanel.Controls.Add(this.MarkerAnnotationsCheckBox);
            this.MainPanel.Controls.Add(this.ItemColumnsButton);
            this.MainPanel.Controls.Add(this.ExperimentColumnsButton);
            this.MainPanel.Controls.Add(this.SourceInfoCheckBox);
            this.MainPanel.Controls.Add(this.PairwiseApprovedRadioButton);
            this.MainPanel.Controls.Add(this.PairwiseAllRadioButton);
            this.MainPanel.Controls.Add(this.TableExperimentColumnRadioButton);
            this.MainPanel.Controls.Add(this.TableExperimentRowRadioButton);
            this.MainPanel.Controls.Add(this.ResultsCheckBox);
            this.MainPanel.Controls.Add(this.ItemStatisticsCheckBox);
            this.MainPanel.Controls.Add(this.ExperimentStatisticsCheckBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 141);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(566, 282);
            this.MainPanel.TabIndex = 1;
            // 
            // ControlItemStatisticsCheckBox
            // 
            this.ControlItemStatisticsCheckBox.AutoSize = true;
            this.ControlItemStatisticsCheckBox.Checked = true;
            this.ControlItemStatisticsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ControlItemStatisticsCheckBox.Location = new System.Drawing.Point(32, 153);
            this.ControlItemStatisticsCheckBox.Name = "ControlItemStatisticsCheckBox";
            this.ControlItemStatisticsCheckBox.Size = new System.Drawing.Size(161, 17);
            this.ControlItemStatisticsCheckBox.TabIndex = 10;
            this.ControlItemStatisticsCheckBox.Text = "Include control item statistics";
            this.ControlItemStatisticsCheckBox.UseVisualStyleBackColor = true;
            this.ControlItemStatisticsCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // SelectionDetailedFilterCheckBox
            // 
            this.SelectionDetailedFilterCheckBox.AutoSize = true;
            this.SelectionDetailedFilterCheckBox.Checked = true;
            this.SelectionDetailedFilterCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectionDetailedFilterCheckBox.Location = new System.Drawing.Point(32, 120);
            this.SelectionDetailedFilterCheckBox.Name = "SelectionDetailedFilterCheckBox";
            this.SelectionDetailedFilterCheckBox.Size = new System.Drawing.Size(123, 17);
            this.SelectionDetailedFilterCheckBox.TabIndex = 9;
            this.SelectionDetailedFilterCheckBox.Text = "Include detailed filter";
            this.SelectionDetailedFilterCheckBox.UseVisualStyleBackColor = true;
            this.SelectionDetailedFilterCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // SelectionGroupsCheckBox
            // 
            this.SelectionGroupsCheckBox.AutoSize = true;
            this.SelectionGroupsCheckBox.Checked = true;
            this.SelectionGroupsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectionGroupsCheckBox.Location = new System.Drawing.Point(32, 86);
            this.SelectionGroupsCheckBox.Name = "SelectionGroupsCheckBox";
            this.SelectionGroupsCheckBox.Size = new System.Drawing.Size(166, 17);
            this.SelectionGroupsCheckBox.TabIndex = 8;
            this.SelectionGroupsCheckBox.Text = "Include selected working sets";
            this.SelectionGroupsCheckBox.UseVisualStyleBackColor = true;
            this.SelectionGroupsCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // NoResultTextBox
            // 
            this.NoResultTextBox.Location = new System.Drawing.Point(389, 237);
            this.NoResultTextBox.MaxLength = 3;
            this.NoResultTextBox.Name = "NoResultTextBox";
            this.NoResultTextBox.Size = new System.Drawing.Size(40, 20);
            this.NoResultTextBox.TabIndex = 22;
            this.NoResultTextBox.Text = "*/*";
            // 
            // NoResultLabel
            // 
            this.NoResultLabel.AutoSize = true;
            this.NoResultLabel.Location = new System.Drawing.Point(250, 240);
            this.NoResultLabel.Name = "NoResultLabel";
            this.NoResultLabel.Size = new System.Drawing.Size(138, 13);
            this.NoResultLabel.TabIndex = 13;
            this.NoResultLabel.Text = "Indicate missing values with";
            // 
            // SelectionPlatesCheckBox
            // 
            this.SelectionPlatesCheckBox.AutoSize = true;
            this.SelectionPlatesCheckBox.Checked = true;
            this.SelectionPlatesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectionPlatesCheckBox.Location = new System.Drawing.Point(32, 52);
            this.SelectionPlatesCheckBox.Name = "SelectionPlatesCheckBox";
            this.SelectionPlatesCheckBox.Size = new System.Drawing.Size(135, 17);
            this.SelectionPlatesCheckBox.TabIndex = 7;
            this.SelectionPlatesCheckBox.Text = "Include selected plates";
            this.SelectionPlatesCheckBox.UseVisualStyleBackColor = true;
            this.SelectionPlatesCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // MarkerAnnotationsButton
            // 
            this.MarkerAnnotationsButton.Location = new System.Drawing.Point(396, 47);
            this.MarkerAnnotationsButton.Name = "MarkerAnnotationsButton";
            this.MarkerAnnotationsButton.Size = new System.Drawing.Size(72, 24);
            this.MarkerAnnotationsButton.TabIndex = 14;
            this.MarkerAnnotationsButton.Text = "Columns...";
            this.MarkerAnnotationsButton.Click += new System.EventHandler(this.MarkerAnnotationsButton_Click);
            // 
            // MarkerAnnotationsCheckBox
            // 
            this.MarkerAnnotationsCheckBox.Checked = true;
            this.MarkerAnnotationsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MarkerAnnotationsCheckBox.Location = new System.Drawing.Point(214, 52);
            this.MarkerAnnotationsCheckBox.Name = "MarkerAnnotationsCheckBox";
            this.MarkerAnnotationsCheckBox.Size = new System.Drawing.Size(176, 16);
            this.MarkerAnnotationsCheckBox.TabIndex = 13;
            this.MarkerAnnotationsCheckBox.Text = "Include marker annotations";
            this.MarkerAnnotationsCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // ItemColumnsButton
            // 
            this.ItemColumnsButton.Location = new System.Drawing.Point(396, 80);
            this.ItemColumnsButton.Name = "ItemColumnsButton";
            this.ItemColumnsButton.Size = new System.Drawing.Size(72, 24);
            this.ItemColumnsButton.TabIndex = 16;
            this.ItemColumnsButton.Text = "Columns...";
            this.ItemColumnsButton.Click += new System.EventHandler(this.ItemColumnsButton_Click);
            // 
            // ExperimentColumnsButton
            // 
            this.ExperimentColumnsButton.Location = new System.Drawing.Point(396, 14);
            this.ExperimentColumnsButton.Name = "ExperimentColumnsButton";
            this.ExperimentColumnsButton.Size = new System.Drawing.Size(72, 24);
            this.ExperimentColumnsButton.TabIndex = 12;
            this.ExperimentColumnsButton.Text = "Columns...";
            this.ExperimentColumnsButton.Click += new System.EventHandler(this.ExperimentColumnsButton_Click);
            // 
            // SourceInfoCheckBox
            // 
            this.SourceInfoCheckBox.Checked = true;
            this.SourceInfoCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SourceInfoCheckBox.Location = new System.Drawing.Point(32, 19);
            this.SourceInfoCheckBox.Name = "SourceInfoCheckBox";
            this.SourceInfoCheckBox.Size = new System.Drawing.Size(176, 16);
            this.SourceInfoCheckBox.TabIndex = 6;
            this.SourceInfoCheckBox.Text = "Include source information";
            this.SourceInfoCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // PairwiseApprovedRadioButton
            // 
            this.PairwiseApprovedRadioButton.Location = new System.Drawing.Point(253, 212);
            this.PairwiseApprovedRadioButton.Name = "PairwiseApprovedRadioButton";
            this.PairwiseApprovedRadioButton.Size = new System.Drawing.Size(192, 16);
            this.PairwiseApprovedRadioButton.TabIndex = 21;
            this.PairwiseApprovedRadioButton.Text = "Pairwise, approved combinations";
            this.PairwiseApprovedRadioButton.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // PairwiseAllRadioButton
            // 
            this.PairwiseAllRadioButton.Location = new System.Drawing.Point(253, 188);
            this.PairwiseAllRadioButton.Name = "PairwiseAllRadioButton";
            this.PairwiseAllRadioButton.Size = new System.Drawing.Size(192, 16);
            this.PairwiseAllRadioButton.TabIndex = 20;
            this.PairwiseAllRadioButton.Text = "Pairwise, all combinations";
            this.PairwiseAllRadioButton.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // TableExperimentColumnRadioButton
            // 
            this.TableExperimentColumnRadioButton.Location = new System.Drawing.Point(253, 164);
            this.TableExperimentColumnRadioButton.Name = "TableExperimentColumnRadioButton";
            this.TableExperimentColumnRadioButton.Size = new System.Drawing.Size(192, 16);
            this.TableExperimentColumnRadioButton.TabIndex = 19;
            this.TableExperimentColumnRadioButton.Text = "Table, experiments in columns";
            this.TableExperimentColumnRadioButton.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // TableExperimentRowRadioButton
            // 
            this.TableExperimentRowRadioButton.Checked = true;
            this.TableExperimentRowRadioButton.Location = new System.Drawing.Point(253, 140);
            this.TableExperimentRowRadioButton.Name = "TableExperimentRowRadioButton";
            this.TableExperimentRowRadioButton.Size = new System.Drawing.Size(192, 16);
            this.TableExperimentRowRadioButton.TabIndex = 18;
            this.TableExperimentRowRadioButton.TabStop = true;
            this.TableExperimentRowRadioButton.Text = "Table, experiments in rows";
            this.TableExperimentRowRadioButton.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // ResultsCheckBox
            // 
            this.ResultsCheckBox.Checked = true;
            this.ResultsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ResultsCheckBox.Location = new System.Drawing.Point(214, 118);
            this.ResultsCheckBox.Name = "ResultsCheckBox";
            this.ResultsCheckBox.Size = new System.Drawing.Size(176, 16);
            this.ResultsCheckBox.TabIndex = 17;
            this.ResultsCheckBox.Text = "Include results";
            this.ResultsCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // ItemStatisticsCheckBox
            // 
            this.ItemStatisticsCheckBox.Checked = true;
            this.ItemStatisticsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ItemStatisticsCheckBox.Location = new System.Drawing.Point(214, 85);
            this.ItemStatisticsCheckBox.Name = "ItemStatisticsCheckBox";
            this.ItemStatisticsCheckBox.Size = new System.Drawing.Size(176, 16);
            this.ItemStatisticsCheckBox.TabIndex = 15;
            this.ItemStatisticsCheckBox.Text = "Include item statistics";
            this.ItemStatisticsCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // ExperimentStatisticsCheckBox
            // 
            this.ExperimentStatisticsCheckBox.Checked = true;
            this.ExperimentStatisticsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExperimentStatisticsCheckBox.Location = new System.Drawing.Point(214, 19);
            this.ExperimentStatisticsCheckBox.Name = "ExperimentStatisticsCheckBox";
            this.ExperimentStatisticsCheckBox.Size = new System.Drawing.Size(176, 16);
            this.ExperimentStatisticsCheckBox.TabIndex = 11;
            this.ExperimentStatisticsCheckBox.Text = "Include experiment statistics";
            this.ExperimentStatisticsCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // MySaveFileDialog
            // 
            this.MySaveFileDialog.FileName = "FileName";
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.BrowseDirectoriesButton);
            this.TopPanel.Controls.Add(this.FileGroupTextBox);
            this.TopPanel.Controls.Add(this.FilegroupLabel);
            this.TopPanel.Controls.Add(this.PathLabel);
            this.TopPanel.Controls.Add(this.PathTextBox);
            this.TopPanel.Controls.Add(this.SplitFileCheckBox);
            this.TopPanel.Controls.Add(this.XMLFileRadioButton);
            this.TopPanel.Controls.Add(this.TextFileRadioButton);
            this.TopPanel.Controls.Add(this.ReportTypeLabel);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(566, 141);
            this.TopPanel.TabIndex = 2;
            // 
            // BrowseDirectoriesButton
            // 
            this.BrowseDirectoriesButton.Location = new System.Drawing.Point(389, 75);
            this.BrowseDirectoriesButton.Name = "BrowseDirectoriesButton";
            this.BrowseDirectoriesButton.Size = new System.Drawing.Size(25, 25);
            this.BrowseDirectoriesButton.TabIndex = 4;
            this.BrowseDirectoriesButton.Text = "...";
            this.BrowseDirectoriesButton.UseVisualStyleBackColor = true;
            this.BrowseDirectoriesButton.Click += new System.EventHandler(this.BrowseDirectoriesButton_Click);
            // 
            // FileGroupTextBox
            // 
            this.FileGroupTextBox.Location = new System.Drawing.Point(127, 100);
            this.FileGroupTextBox.Name = "FileGroupTextBox";
            this.FileGroupTextBox.Size = new System.Drawing.Size(256, 20);
            this.FileGroupTextBox.TabIndex = 5;
            this.FileGroupTextBox.Text = "Report1";
            // 
            // FilegroupLabel
            // 
            this.FilegroupLabel.AutoSize = true;
            this.FilegroupLabel.Location = new System.Drawing.Point(29, 103);
            this.FilegroupLabel.Name = "FilegroupLabel";
            this.FilegroupLabel.Size = new System.Drawing.Size(82, 13);
            this.FilegroupLabel.TabIndex = 6;
            this.FilegroupLabel.Text = "Filegroup name:";
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Location = new System.Drawing.Point(29, 81);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(87, 13);
            this.PathLabel.TabIndex = 5;
            this.PathLabel.Text = "Destination path:";
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(127, 78);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(256, 20);
            this.PathTextBox.TabIndex = 3;
            // 
            // SplitFileCheckBox
            // 
            this.SplitFileCheckBox.AutoSize = true;
            this.SplitFileCheckBox.Location = new System.Drawing.Point(11, 55);
            this.SplitFileCheckBox.Name = "SplitFileCheckBox";
            this.SplitFileCheckBox.Size = new System.Drawing.Size(159, 17);
            this.SplitFileCheckBox.TabIndex = 2;
            this.SplitFileCheckBox.Text = "Each section in separate file";
            this.SplitFileCheckBox.UseVisualStyleBackColor = true;
            this.SplitFileCheckBox.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // XMLFileRadioButton
            // 
            this.XMLFileRadioButton.Location = new System.Drawing.Point(224, 24);
            this.XMLFileRadioButton.Name = "XMLFileRadioButton";
            this.XMLFileRadioButton.Size = new System.Drawing.Size(72, 16);
            this.XMLFileRadioButton.TabIndex = 1;
            this.XMLFileRadioButton.Text = "XML file";
            this.XMLFileRadioButton.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // TextFileRadioButton
            // 
            this.TextFileRadioButton.Checked = true;
            this.TextFileRadioButton.Location = new System.Drawing.Point(128, 24);
            this.TextFileRadioButton.Name = "TextFileRadioButton";
            this.TextFileRadioButton.Size = new System.Drawing.Size(80, 16);
            this.TextFileRadioButton.TabIndex = 0;
            this.TextFileRadioButton.TabStop = true;
            this.TextFileRadioButton.Text = "Text file";
            this.TextFileRadioButton.CheckedChanged += new System.EventHandler(this.StatusChanged);
            // 
            // ReportTypeLabel
            // 
            this.ReportTypeLabel.Location = new System.Drawing.Point(8, 24);
            this.ReportTypeLabel.Name = "ReportTypeLabel";
            this.ReportTypeLabel.Size = new System.Drawing.Size(112, 16);
            this.ReportTypeLabel.TabIndex = 0;
            this.ReportTypeLabel.Text = "Select report type:";
            // 
            // ReportForm
            // 
            this.AcceptButton = this.SaveButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(566, 468);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.BottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report";
            this.BottomPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion


        public ReportSettings GetReportSettings(IWin32Window owner, string[] experimentStatColumns, string[] itemStatColumns, DataServer dServer, bool forStreaming)
        {
            ReportSettings settings;
            bool [] markerColumnFlags, itemColumnFlags;
            object[] selectedAnnotationTypeObjects;
            Identifiable[] selectedAnnotationTypes;

            MyOKFlag = false;
            MyForStreaming = forStreaming;

            MyMarkerColumnsForm = new CheckedListForm();
            MyItemColumnsForm = new CheckedListForm();
            MyAnnotationTypesForm = new CheckedListForm();

            //Prepare column selection forms.
            MyMarkerColumnsForm.LoadItems(experimentStatColumns);
            MyItemColumnsForm.LoadItems(itemStatColumns);
            MyAnnotationTypesForm.LoadItems(this.GetAnnotationTypes(dServer));

            //Check all columns.
            MyMarkerColumnsForm.SetAllCheckState(true);
            MyItemColumnsForm.SetAllCheckState(true);
            MyAnnotationTypesForm.SetAllCheckState(true);

            //Uncheck item success rate over nonfailed experiments, because should not be in the report by default.
            MyItemColumnsForm.SetItemChecked(2, false);

            PathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            this.SetEnabledStatus();
            this.ShowDialog(owner);
            settings = new ReportSettings();

            //Set included sections.
            settings.Set(ReportSettings.Bools.IncludeAnnotations, MarkerAnnotationsCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeControlItemStat, ControlItemStatisticsCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeExperimentStat, ExperimentStatisticsCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeItemStat, ItemStatisticsCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeSourceInfo, SourceInfoCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeResults, ResultsCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeSelectedPlates, SelectionPlatesCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeSelectedGroups, SelectionGroupsCheckBox.Checked);
            settings.Set(ReportSettings.Bools.IncludeSelectedDetailedFilter, SelectionDetailedFilterCheckBox.Checked);            

            //Set empty result string.
            settings.Set(ReportSettings.Strings.EmptyResultString, NoResultTextBox.Text);
            //Set report type.
            settings.Set(ReportSettings.Strings.ReportType, (TextFileRadioButton.Checked ? ReportSettings.ReportTypeText : ReportSettings.ReportTypeXML));
            if (TableExperimentRowRadioButton.Checked)
            {
                settings.Set(ReportSettings.Strings.ResultTableType, ReportSettings.ResultTableTypeExpmRows);
            }
            else if (TableExperimentColumnRadioButton.Checked)
            {
                settings.Set(ReportSettings.Strings.ResultTableType, ReportSettings.ResultTableTypeExpmCol);
            }
            else if (PairwiseAllRadioButton.Checked)
            {
                settings.Set(ReportSettings.Strings.ResultTableType, ReportSettings.ResultTableTypePairAll);
            }
            else if (PairwiseApprovedRadioButton.Checked)
            {
                settings.Set(ReportSettings.Strings.ResultTableType, ReportSettings.ResultTableTypePairApp);
            }

            //Set selected columns for experiments.
            markerColumnFlags = new Boolean[MyMarkerColumnsForm.GetItems().GetLength(0)];
            for (int i = 0; i < markerColumnFlags.GetLength(0); i++)
            {
                markerColumnFlags[i] = MyMarkerColumnsForm.GetItemChecked(i);
            }
            settings.Set(ReportSettings.BoolArrays.ExperimentStatFlags, markerColumnFlags);

            //Set selected columns for items.
            itemColumnFlags = new Boolean[MyItemColumnsForm.GetItems().GetLength(0)];
            for (int i = 0; i < itemColumnFlags.GetLength(0); i++)
            {
                itemColumnFlags[i] = MyItemColumnsForm.GetItemChecked(i);
            }
            settings.Set(ReportSettings.BoolArrays.ItemStatFlags, itemColumnFlags);

            //Set selected columns for annotations.
            selectedAnnotationTypeObjects = MyAnnotationTypesForm.GetCheckedItems();
            selectedAnnotationTypes = new Identifiable[selectedAnnotationTypeObjects.GetLength(0)];
            for (int i = 0; i < selectedAnnotationTypeObjects.GetLength(0); i++)
            {
                selectedAnnotationTypes[i] = (Identifiable)selectedAnnotationTypeObjects[i];
            }
            settings.Set(ReportSettings.Identifiables.AnnotationTypes, selectedAnnotationTypes);

            //Set split file flag.
            settings.Set(ReportSettings.Bools.SplitFile, SplitFileCheckBox.Checked);

            //Set file path.
            settings.Set(ReportSettings.Strings.FilePath, MyFilePath);            

            //Set destination file name.
            settings.Set(ReportSettings.Strings.FileName, MyFileName);

            return (MyOKFlag ? settings : null);
        }

		private void SetEnabledStatus()
		{
            //Select pairwise results when streaming results.
            if (MyForStreaming && ResultsCheckBox.Checked)
            {
                if (TableExperimentColumnRadioButton.Checked || TableExperimentRowRadioButton.Checked)
                {
                    PairwiseAllRadioButton.Checked = true;
                }
            }
            //Disallow XML file for streaming.
            XMLFileRadioButton.Enabled = !MyForStreaming;

            //Disallow split files for XML.
            if (XMLFileRadioButton.Checked) { SplitFileCheckBox.Checked = false; }
            SplitFileCheckBox.Enabled = !XMLFileRadioButton.Checked;

            PathTextBox.Enabled = SplitFileCheckBox.Checked;
            PathLabel.Enabled = SplitFileCheckBox.Checked;
            FilegroupLabel.Enabled = SplitFileCheckBox.Checked;
            BrowseDirectoriesButton.Enabled = SplitFileCheckBox.Checked;
            FileGroupTextBox.Enabled = SplitFileCheckBox.Checked;
			ExperimentColumnsButton.Enabled = ExperimentStatisticsCheckBox.Checked;
            MarkerAnnotationsButton.Enabled = MarkerAnnotationsCheckBox.Checked;
            ItemColumnsButton.Enabled = ItemStatisticsCheckBox.Checked;
			TableExperimentRowRadioButton.Enabled = ResultsCheckBox.Checked && TextFileRadioButton.Checked && !MyForStreaming;
			TableExperimentColumnRadioButton.Enabled = ResultsCheckBox.Checked && TextFileRadioButton.Checked && !MyForStreaming;
			PairwiseAllRadioButton.Enabled = ResultsCheckBox.Checked && TextFileRadioButton.Checked;
			PairwiseApprovedRadioButton.Enabled = ResultsCheckBox.Checked && TextFileRadioButton.Checked;
            NoResultTextBox.Enabled = ResultsCheckBox.Checked && !PairwiseApprovedRadioButton.Checked;
            NoResultLabel.Enabled = NoResultTextBox.Enabled && !PairwiseApprovedRadioButton.Checked;


		}


		private Identifiable [] GetAnnotationTypes(DataServer dServer)
		{
			DataTable termsTable;
			Identifiable [] annotationTypes;

			this.Cursor = Cursors.WaitCursor;	
			termsTable = dServer.GetAnnotationTypes();
			annotationTypes = new Identifiable[termsTable.Rows.Count];
			for (int i = 0 ; i < termsTable.Rows.Count ; i++)
			{
				annotationTypes[i] = new Identifiable(Convert.ToInt32(termsTable.Rows[i][0]), 
					termsTable.Rows[i][1].ToString());
			}
			this.Cursor = Cursors.Default;

			return annotationTypes;
		}

        private bool FilenameIsAllowed(string filename)
        {
            char[] invalidChars;

            invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char tempChar in invalidChars)
            {
                if (filename.IndexOf(tempChar) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void StatusChanged(object sender, System.EventArgs e)
        {
            try
            {
                SetEnabledStatus();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing control status.", this);
            }
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

		private void ExperimentColumnsButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				MyMarkerColumnsForm.DisplayItems("Select Columns");
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting columns.", this);
			}
		}

		private void MarkerAnnotationsButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				MyAnnotationTypesForm.DisplayItems("Select Annotation Types");
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting annotation types.", this);
			}
		}

		private void ItemColumnsButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				MyItemColumnsForm.DisplayItems("Select Columns");
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting columns.", this);
			}
		}


		private void SaveButton_Click(object sender, System.EventArgs e)
		{

			try
			{
                if (SplitFileCheckBox.Checked)
                {
                    if (!FilenameIsAllowed(FileGroupTextBox.Text))
                    {
                        MessageManager.ShowWarning("The file group name contains invalid characters.");
                        return;
                    }
                    if (!System.IO.Directory.Exists(PathTextBox.Text))
                    {
                        MessageManager.ShowWarning("The specified directory does not exist.");
                        return;
                    }

                    MyFilePath = PathTextBox.Text;
                    MyFileName = FileGroupTextBox.Text;
                }
                else
                {
                    //Set file extension filter.
                    if (TextFileRadioButton.Checked)
                    {
                        MySaveFileDialog.Filter = "Text file (*.txt)|*.txt";
                    }
                    else if (XMLFileRadioButton.Checked)
                    {
                        MySaveFileDialog.Filter = "XML file (*.xml)|*.xml";
                    }

                    //Get file name.
                    if (MySaveFileDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }

                    MyFilePath = System.IO.Path.GetDirectoryName(MySaveFileDialog.FileName);
                    MyFileName = System.IO.Path.GetFileName(MySaveFileDialog.FileName);
                }
                MyOKFlag = true;
                this.Close();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when attempting to save file.", this);
			}
		}

        private void BrowseDirectoriesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(MyFolderBrowserDialog.ShowDialog(this) == DialogResult.Cancel))
                {
                    PathTextBox.Text = MyFolderBrowserDialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when attempting to browse directories.", this);
            }
        }

		

	}
}
