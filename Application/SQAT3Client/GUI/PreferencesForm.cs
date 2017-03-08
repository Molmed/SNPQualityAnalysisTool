using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;


namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for PreferencesForm.
	/// </summary>
	public class PreferencesForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.CheckBox RecalculateCheckBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private CheckBox ExperimentFilterCheckBox;

		private bool MyOKFlag;


		public PreferencesForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			MyOKFlag = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesForm));
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.RecalculateCheckBox = new System.Windows.Forms.CheckBox();
            this.ExperimentFilterCheckBox = new System.Windows.Forms.CheckBox();
            this.BottomPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.OKButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 136);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(333, 40);
            this.BottomPanel.TabIndex = 0;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(253, 8);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(8, 8);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ExperimentFilterCheckBox);
            this.MainPanel.Controls.Add(this.RecalculateCheckBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(333, 136);
            this.MainPanel.TabIndex = 1;
            // 
            // RecalculateCheckBox
            // 
            this.RecalculateCheckBox.Location = new System.Drawing.Point(52, 37);
            this.RecalculateCheckBox.Name = "RecalculateCheckBox";
            this.RecalculateCheckBox.Size = new System.Drawing.Size(184, 24);
            this.RecalculateCheckBox.TabIndex = 0;
            this.RecalculateCheckBox.Text = "Automatically refresh session";
            // 
            // ExperimentFilterCheckBox
            // 
            this.ExperimentFilterCheckBox.AutoSize = true;
            this.ExperimentFilterCheckBox.Location = new System.Drawing.Point(52, 67);
            this.ExperimentFilterCheckBox.Name = "ExperimentFilterCheckBox";
            this.ExperimentFilterCheckBox.Size = new System.Drawing.Size(135, 17);
            this.ExperimentFilterCheckBox.TabIndex = 1;
            this.ExperimentFilterCheckBox.Text = "Enable experiment filter";
            this.ExperimentFilterCheckBox.UseVisualStyleBackColor = true;
            // 
            // PreferencesForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(333, 176);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.BottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.BottomPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion


		public DialogResult GetPreferences(Preferences currentPreferences)
		{
			//Changes currentPreferences and returns DialogResult.OK if
			//the user clicks OK, otherwise DialogResult.Cancel.
			this.SetControlsFromSettings(currentPreferences);
			this.ShowDialog();
			if (MyOKFlag)
			{
				this.GetSettingsFromControls(currentPreferences);
				try
				{
					currentPreferences.Serialize();
				}
				catch (Exception)
				{
					MessageManager.ShowInformation("Unable to permanently save preferences.", this);
				}
				return DialogResult.OK;
			}
			else
			{
				return DialogResult.Cancel;
			}
		}


		private void GetSettingsFromControls(Preferences settings)
		{
			settings.Set(Preferences.Bools.Recalculate, RecalculateCheckBox.Checked);
            settings.Set(Preferences.Bools.ExperimentFilter, ExperimentFilterCheckBox.Checked);
		}

		private void SetControlsFromSettings(Preferences settings)
		{
			RecalculateCheckBox.Checked = settings.Get(Preferences.Bools.Recalculate);
            ExperimentFilterCheckBox.Checked = settings.Get(Preferences.Bools.ExperimentFilter);
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			try
			{
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


	}
}
