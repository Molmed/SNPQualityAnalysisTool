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
	/// Summary description for SessionNameForm.
	/// </summary>
	public class SessionNameForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.ComboBox ProjectList;
		private System.Windows.Forms.Label ProjectLabel;
		private System.Windows.Forms.Label AvailableSessionsLabel;
		private System.Windows.Forms.Label SelectedNameLabel;
        private System.Windows.Forms.TextBox SelectedNameTextBox;
        private IContainer components;

		private DataServer MyDataServer;
		private System.Windows.Forms.Label DescriptionLabel;
		private System.Windows.Forms.TextBox DescriptionTextBox;
        private AdvancedListView2 SessionListView;
		private bool MyOKFlag;
		
		public SessionNameForm(DataServer dServer)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			MyDataServer = dServer;
			MyOKFlag = false;
			SessionListView.TheListView.Click += new EventHandler(SessionListView_SelectedIndexChanged);
			SessionListView.TheListView.MultiSelect = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionNameForm));
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.SessionListView = new Molmed.SQAT.GUI.AdvancedListView2();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.SelectedNameTextBox = new System.Windows.Forms.TextBox();
            this.SelectedNameLabel = new System.Windows.Forms.Label();
            this.AvailableSessionsLabel = new System.Windows.Forms.Label();
            this.ProjectLabel = new System.Windows.Forms.Label();
            this.ProjectList = new System.Windows.Forms.ComboBox();
            this.BottomPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.OKButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 374);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(432, 48);
            this.BottomPanel.TabIndex = 0;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(344, 16);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(8, 16);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.SessionListView);
            this.MainPanel.Controls.Add(this.DescriptionTextBox);
            this.MainPanel.Controls.Add(this.DescriptionLabel);
            this.MainPanel.Controls.Add(this.SelectedNameTextBox);
            this.MainPanel.Controls.Add(this.SelectedNameLabel);
            this.MainPanel.Controls.Add(this.AvailableSessionsLabel);
            this.MainPanel.Controls.Add(this.ProjectLabel);
            this.MainPanel.Controls.Add(this.ProjectList);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(432, 374);
            this.MainPanel.TabIndex = 1;
            // 
            // SessionListView
            // 
            this.SessionListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SessionListView.Location = new System.Drawing.Point(12, 83);
            this.SessionListView.Name = "SessionListView";
            this.SessionListView.Size = new System.Drawing.Size(412, 194);
            this.SessionListView.TabIndex = 1;
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextBox.Location = new System.Drawing.Point(8, 336);
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(416, 20);
            this.DescriptionTextBox.TabIndex = 3;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DescriptionLabel.Location = new System.Drawing.Point(8, 320);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(104, 16);
            this.DescriptionLabel.TabIndex = 7;
            this.DescriptionLabel.Text = "Session description";
            // 
            // SelectedNameTextBox
            // 
            this.SelectedNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedNameTextBox.Location = new System.Drawing.Point(8, 296);
            this.SelectedNameTextBox.Name = "SelectedNameTextBox";
            this.SelectedNameTextBox.Size = new System.Drawing.Size(416, 20);
            this.SelectedNameTextBox.TabIndex = 2;
            // 
            // SelectedNameLabel
            // 
            this.SelectedNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectedNameLabel.Location = new System.Drawing.Point(8, 280);
            this.SelectedNameLabel.Name = "SelectedNameLabel";
            this.SelectedNameLabel.Size = new System.Drawing.Size(104, 16);
            this.SelectedNameLabel.TabIndex = 4;
            this.SelectedNameLabel.Text = "Session name";
            // 
            // AvailableSessionsLabel
            // 
            this.AvailableSessionsLabel.Location = new System.Drawing.Point(8, 64);
            this.AvailableSessionsLabel.Name = "AvailableSessionsLabel";
            this.AvailableSessionsLabel.Size = new System.Drawing.Size(104, 16);
            this.AvailableSessionsLabel.TabIndex = 2;
            this.AvailableSessionsLabel.Text = "Available sessions";
            // 
            // ProjectLabel
            // 
            this.ProjectLabel.Location = new System.Drawing.Point(8, 24);
            this.ProjectLabel.Name = "ProjectLabel";
            this.ProjectLabel.Size = new System.Drawing.Size(48, 16);
            this.ProjectLabel.TabIndex = 1;
            this.ProjectLabel.Text = "Project";
            // 
            // ProjectList
            // 
            this.ProjectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProjectList.Location = new System.Drawing.Point(56, 24);
            this.ProjectList.Name = "ProjectList";
            this.ProjectList.Size = new System.Drawing.Size(368, 21);
            this.ProjectList.TabIndex = 0;
            this.ProjectList.SelectedIndexChanged += new System.EventHandler(this.ProjectList_SelectedIndexChanged);
            // 
            // SessionNameForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(432, 422);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.BottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SessionNameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save as...";
            this.BottomPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public bool GetName(out string projectName, out string sessionName, out string description, string defaultName, string defaultDescription)
		{
			FillProjectList();
			SelectedNameTextBox.Text = defaultName;
			DescriptionTextBox.Text = defaultDescription;
			this.ShowDialog();
			projectName = ProjectList.Text;
			sessionName = SelectedNameTextBox.Text;
			description = DescriptionTextBox.Text;
			return MyOKFlag;
		}

		private void FillSessions(string projectName)
		{
			DataTable sessionsTable;

			this.Cursor = Cursors.WaitCursor;
			sessionsTable = MyDataServer.GetSessions(projectName);
			this.Cursor = Cursors.Default;
			SessionListView.ShowFormattedTable(sessionsTable);
		}

		private void FillProjectList()
		{
			DataTable projectTable;
            string currentProject;

			this.Cursor = Cursors.WaitCursor;
			projectTable = MyDataServer.GetProjects();
			ProjectList.Items.Clear();
			for (int i = 0 ; i < projectTable.Rows.Count ; i++)
			{
				ProjectList.Items.Add(projectTable.Rows[i]["identifier"]);
			}
			this.Cursor = Cursors.Default;

            //Try to set the correct project.
            currentProject = MyDataServer.GetSessionProject();
            if (currentProject != null)
            {
                if (ProjectList.Items.Contains(currentProject))
                {
                    ProjectList.SelectedIndex = ProjectList.Items.IndexOf(currentProject);
                }
            }
		}

		private void ProjectList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                FillSessions(ProjectList.Text);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing project.", this);
            }
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (ProjectList.Text.Trim() == "")
                {
                    MessageManager.ShowInformation("Please select a project.");
                    return;
                }
                if (!MyDataServer.GetCurrentUserPermittedInProject(ProjectList.Text))
                {
                    MessageManager.ShowInformation("You do not have permissions to edit project " + ProjectList.Text + ".");
                    return;
                }
				this.Cursor = Cursors.WaitCursor;
				if (MyDataServer.GetWsetExists(SelectedNameTextBox.Text))
				{
					this.Cursor = Cursors.Default;
					MessageManager.ShowInformation("The name " + SelectedNameTextBox.Text + " already exists.", this);
					return;
				}
				this.Cursor = Cursors.Default;
				MyOKFlag = true;
				this.Close();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when closing form.", this);
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

		private void SessionListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (SessionListView.TheListView.SelectedItems.Count > 0)
				{
					SelectedNameTextBox.Text = SessionListView.TheListView.SelectedItems[0].Text;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting item.", this);
			}
		}

	}
}
