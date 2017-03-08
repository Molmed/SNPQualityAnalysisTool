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
	/// Summary description for RerunForm.
	/// </summary>
	public class RerunForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.GroupBox ListGroupBox;
		private System.Windows.Forms.Panel SelectionPanel;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.NumericUpDown MissingGenotypePercentage;
		private System.Windows.Forms.Label MissingGenotypes2Label;
		private System.Windows.Forms.CheckBox MissingGenotypesCheckBox;
		private System.Windows.Forms.CheckBox AlleleFrqCheckBox;
		private System.Windows.Forms.Label AlleleFrq2Label;
		private System.Windows.Forms.CheckBox DuplicateErrorCheckBox;
		private System.Windows.Forms.Button CheckButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private DataServer MyDataServer;
		private SessionSettings MySessionSettings;
        private AdvancedCheckedListBox2 ExperimentList;
        private Label StatusLabel;
        private System.Windows.Forms.NumericUpDown AlleleFrqPercentage;

		public RerunForm(SessionSettings settings, DataServer server)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			DataTable experimentTable;
			Identifiable [] experiments;
			
			MySessionSettings = settings;
			MyDataServer = server;

            StatusLabel.Text = "Ready";

			//Initialize experiment list with all experiments.
			experimentTable = MyDataServer.GetResultExperiments();
			experiments = new Identifiable[experimentTable.Rows.Count];
			for (int i = 0 ; i < experimentTable.Rows.Count ; i++)
			{
				experiments[i] = new Identifiable(Convert.ToInt32(experimentTable.Rows[i]["experiment_id"]),
					experimentTable.Rows[i]["identifier"].ToString());
			}
			ExperimentList.TheCheckedListBox.Items.AddRange(experiments);
			ExperimentList.SetAllCheckStatus(true);

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RerunForm));
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ListGroupBox = new System.Windows.Forms.GroupBox();
            this.SelectionPanel = new System.Windows.Forms.Panel();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.CheckButton = new System.Windows.Forms.Button();
            this.DuplicateErrorCheckBox = new System.Windows.Forms.CheckBox();
            this.AlleleFrq2Label = new System.Windows.Forms.Label();
            this.AlleleFrqPercentage = new System.Windows.Forms.NumericUpDown();
            this.AlleleFrqCheckBox = new System.Windows.Forms.CheckBox();
            this.MissingGenotypesCheckBox = new System.Windows.Forms.CheckBox();
            this.MissingGenotypes2Label = new System.Windows.Forms.Label();
            this.MissingGenotypePercentage = new System.Windows.Forms.NumericUpDown();
            this.ExperimentList = new Molmed.SQAT.GUI.AdvancedCheckedListBox2();
            this.BottomPanel.SuspendLayout();
            this.ListGroupBox.SuspendLayout();
            this.SelectionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlleleFrqPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MissingGenotypePercentage)).BeginInit();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 398);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(736, 40);
            this.BottomPanel.TabIndex = 0;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(656, 8);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "&Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ListGroupBox
            // 
            this.ListGroupBox.Controls.Add(this.ExperimentList);
            this.ListGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ListGroupBox.Name = "ListGroupBox";
            this.ListGroupBox.Size = new System.Drawing.Size(232, 398);
            this.ListGroupBox.TabIndex = 1;
            this.ListGroupBox.TabStop = false;
            this.ListGroupBox.Text = "Experiments";
            // 
            // SelectionPanel
            // 
            this.SelectionPanel.Controls.Add(this.CheckButton);
            this.SelectionPanel.Controls.Add(this.DuplicateErrorCheckBox);
            this.SelectionPanel.Controls.Add(this.AlleleFrq2Label);
            this.SelectionPanel.Controls.Add(this.AlleleFrqPercentage);
            this.SelectionPanel.Controls.Add(this.AlleleFrqCheckBox);
            this.SelectionPanel.Controls.Add(this.MissingGenotypesCheckBox);
            this.SelectionPanel.Controls.Add(this.MissingGenotypes2Label);
            this.SelectionPanel.Controls.Add(this.MissingGenotypePercentage);
            this.SelectionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.SelectionPanel.Location = new System.Drawing.Point(232, 0);
            this.SelectionPanel.Name = "SelectionPanel";
            this.SelectionPanel.Size = new System.Drawing.Size(504, 398);
            this.SelectionPanel.TabIndex = 3;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(3, 417);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(47, 13);
            this.StatusLabel.TabIndex = 7;
            this.StatusLabel.Text = "<status>";
            // 
            // CheckButton
            // 
            this.CheckButton.Location = new System.Drawing.Point(8, 120);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(72, 24);
            this.CheckButton.TabIndex = 5;
            this.CheckButton.Text = "Chec&k...";
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // DuplicateErrorCheckBox
            // 
            this.DuplicateErrorCheckBox.Location = new System.Drawing.Point(8, 80);
            this.DuplicateErrorCheckBox.Name = "DuplicateErrorCheckBox";
            this.DuplicateErrorCheckBox.Size = new System.Drawing.Size(280, 24);
            this.DuplicateErrorCheckBox.TabIndex = 4;
            this.DuplicateErrorCheckBox.Text = "Items with duplicate errors in selected experiments";
            // 
            // AlleleFrq2Label
            // 
            this.AlleleFrq2Label.Location = new System.Drawing.Point(328, 47);
            this.AlleleFrq2Label.Name = "AlleleFrq2Label";
            this.AlleleFrq2Label.Size = new System.Drawing.Size(164, 20);
            this.AlleleFrq2Label.TabIndex = 6;
            this.AlleleFrq2Label.Text = "% in the selected experiments";
            // 
            // AlleleFrqPercentage
            // 
            this.AlleleFrqPercentage.Location = new System.Drawing.Point(280, 45);
            this.AlleleFrqPercentage.Name = "AlleleFrqPercentage";
            this.AlleleFrqPercentage.Size = new System.Drawing.Size(48, 20);
            this.AlleleFrqPercentage.TabIndex = 3;
            this.AlleleFrqPercentage.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // AlleleFrqCheckBox
            // 
            this.AlleleFrqCheckBox.Location = new System.Drawing.Point(8, 47);
            this.AlleleFrqCheckBox.Name = "AlleleFrqCheckBox";
            this.AlleleFrqCheckBox.Size = new System.Drawing.Size(272, 18);
            this.AlleleFrqCheckBox.TabIndex = 2;
            this.AlleleFrqCheckBox.Text = "Items having alleles with frequencies less than";
            // 
            // MissingGenotypesCheckBox
            // 
            this.MissingGenotypesCheckBox.Location = new System.Drawing.Point(8, 8);
            this.MissingGenotypesCheckBox.Name = "MissingGenotypesCheckBox";
            this.MissingGenotypesCheckBox.Size = new System.Drawing.Size(232, 24);
            this.MissingGenotypesCheckBox.TabIndex = 0;
            this.MissingGenotypesCheckBox.Text = "Items having genotypes in less than";
            this.MissingGenotypesCheckBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // MissingGenotypes2Label
            // 
            this.MissingGenotypes2Label.Location = new System.Drawing.Point(328, 14);
            this.MissingGenotypes2Label.Name = "MissingGenotypes2Label";
            this.MissingGenotypes2Label.Size = new System.Drawing.Size(164, 16);
            this.MissingGenotypes2Label.TabIndex = 2;
            this.MissingGenotypes2Label.Text = "% of the selected experiments";
            // 
            // MissingGenotypePercentage
            // 
            this.MissingGenotypePercentage.Location = new System.Drawing.Point(280, 12);
            this.MissingGenotypePercentage.Name = "MissingGenotypePercentage";
            this.MissingGenotypePercentage.Size = new System.Drawing.Size(48, 20);
            this.MissingGenotypePercentage.TabIndex = 1;
            this.MissingGenotypePercentage.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // ExperimentList
            // 
            this.ExperimentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentList.Location = new System.Drawing.Point(3, 16);
            this.ExperimentList.Name = "ExperimentList";
            this.ExperimentList.Size = new System.Drawing.Size(226, 379);
            this.ExperimentList.TabIndex = 0;
            // 
            // RerunForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(736, 438);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.ListGroupBox);
            this.Controls.Add(this.SelectionPanel);
            this.Controls.Add(this.BottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RerunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rerun Criteria";
            this.BottomPanel.ResumeLayout(false);
            this.ListGroupBox.ResumeLayout(false);
            this.SelectionPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AlleleFrqPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MissingGenotypePercentage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void DisplayStatus(string status)
        {
            StatusLabel.Text = status;
            this.Refresh();
            System.Threading.Thread.Sleep(5);
        }


		private void CheckButton_Click(object sender, System.EventArgs e)
		{
			Identifiable [] checkedExperiments, allItems;
			RerunSettings pickSettings;
			DataTable rerunItemTable, allItemTable;
			RerunListForm listForm;
			Identifiable [] selectedItems;

			try
			{
				//Store selected experiments.
				this.Cursor = Cursors.WaitCursor;

                DisplayStatus("Storing selected experiments...");

				checkedExperiments = new Identifiable[ExperimentList.TheCheckedListBox.CheckedItems.Count];
				for (int i = 0 ;  i < ExperimentList.TheCheckedListBox.CheckedItems.Count ; i++)
				{
					checkedExperiments[i] = (Identifiable)ExperimentList.TheCheckedListBox.CheckedItems[i];
				}
				if (checkedExperiments.GetLength(0) < 1)
				{
					this.Cursor = Cursors.Default;
					MessageManager.ShowInformation("No experiments selected.", this);
					return;
				}

                DisplayStatus("Clearing previous rerun experiments...");               

                MyDataServer.ClearRerunExperiments();

                DisplayStatus("Adding new rerun experiments...");                
                
                for (int i = 0 ; i < checkedExperiments.GetLength(0) ; i++)
				{
					MyDataServer.AddRerunExperiment(checkedExperiments[i].ID);
				}

				//Read rerun settings.
				pickSettings = new RerunSettings();
				pickSettings.MissingGenotype = MissingGenotypesCheckBox.Checked;
				pickSettings.MissingGenotypePercent = 100 - (int)MissingGenotypePercentage.Value;
				pickSettings.LowAlleleFrq = AlleleFrqCheckBox.Checked;
				pickSettings.LowAlleleFrqPercent = (int)AlleleFrqPercentage.Value;
				pickSettings.DuplicateErrors = DuplicateErrorCheckBox.Checked;

                DisplayStatus("Processing rerun items..."); 

				//Get items to check.
				rerunItemTable = MyDataServer.GetRerunItems(MySessionSettings, pickSettings);

                DisplayStatus("Fetching all items...");

				//Fill item table with all items.
				allItemTable = MyDataServer.GetResultItems();
				allItems = new Identifiable[allItemTable.Rows.Count];
				for (int i = 0 ; i < allItemTable.Rows.Count ; i++)
				{
					allItems[i] = new Identifiable(Convert.ToInt32(allItemTable.Rows[i]["item_id"]),
						allItemTable.Rows[i]["identifier"].ToString());
				}

                DisplayStatus("Storing rerun items...");

				//Save the items which should be checked.
				selectedItems = new Identifiable[rerunItemTable.Rows.Count];
				for (int i = 0 ; i < rerunItemTable.Rows.Count ; i++)
				{
					selectedItems[i] = new Identifiable(Convert.ToInt32(rerunItemTable.Rows[i][0]), "");  
				}

                DisplayStatus("Preparing display...");

				listForm = new RerunListForm(pickSettings, checkedExperiments, allItems, selectedItems);

                DisplayStatus("Finished, please wait...");
                
                this.Cursor = Cursors.Default;
				listForm.ShowDialog(this);

                DisplayStatus("Finished");

			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting items.", this);
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
	}
}
