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
	/// Summary description for SessionSettingsForm.
	/// </summary>
	public class SessionSettingsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox UnitGroupBox;
		private System.Windows.Forms.GroupBox HardyWeinbergGroupBox;
		private System.Windows.Forms.GroupBox InheritanceTestGroupBox;
		private System.Windows.Forms.RadioButton SampleRadioButton;
		private System.Windows.Forms.RadioButton IndividualRadioButton;
		private System.Windows.Forms.CheckBox DetectXCheckBox;
		private System.Windows.Forms.CheckBox SkipChildrenCheckBox;
		private System.Windows.Forms.Label HWLimitLabel;
		private System.Windows.Forms.NumericUpDown HWLimitNumeric;
		private System.Windows.Forms.CheckBox DemandDuplicatesCheckBox;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CloseButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox DoInheritanceTestCheckBox;
		private System.Windows.Forms.CheckBox DoHWTestCheckBox;
        private GroupBox ExperimentGroupBox;
        private RadioButton MarkerRadioButton;
        private RadioButton AssayRadioButton;

		private bool MyOKFlag;

		public SessionSettingsForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionSettingsForm));
            this.UnitGroupBox = new System.Windows.Forms.GroupBox();
            this.DemandDuplicatesCheckBox = new System.Windows.Forms.CheckBox();
            this.IndividualRadioButton = new System.Windows.Forms.RadioButton();
            this.SampleRadioButton = new System.Windows.Forms.RadioButton();
            this.HardyWeinbergGroupBox = new System.Windows.Forms.GroupBox();
            this.DoHWTestCheckBox = new System.Windows.Forms.CheckBox();
            this.HWLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.HWLimitLabel = new System.Windows.Forms.Label();
            this.SkipChildrenCheckBox = new System.Windows.Forms.CheckBox();
            this.InheritanceTestGroupBox = new System.Windows.Forms.GroupBox();
            this.DoInheritanceTestCheckBox = new System.Windows.Forms.CheckBox();
            this.DetectXCheckBox = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ExperimentGroupBox = new System.Windows.Forms.GroupBox();
            this.MarkerRadioButton = new System.Windows.Forms.RadioButton();
            this.AssayRadioButton = new System.Windows.Forms.RadioButton();
            this.UnitGroupBox.SuspendLayout();
            this.HardyWeinbergGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HWLimitNumeric)).BeginInit();
            this.InheritanceTestGroupBox.SuspendLayout();
            this.ExperimentGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // UnitGroupBox
            // 
            this.UnitGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.UnitGroupBox.Controls.Add(this.DemandDuplicatesCheckBox);
            this.UnitGroupBox.Controls.Add(this.IndividualRadioButton);
            this.UnitGroupBox.Controls.Add(this.SampleRadioButton);
            this.UnitGroupBox.Location = new System.Drawing.Point(13, 13);
            this.UnitGroupBox.Name = "UnitGroupBox";
            this.UnitGroupBox.Size = new System.Drawing.Size(338, 85);
            this.UnitGroupBox.TabIndex = 0;
            this.UnitGroupBox.TabStop = false;
            this.UnitGroupBox.Text = "Item Report Unit";
            // 
            // DemandDuplicatesCheckBox
            // 
            this.DemandDuplicatesCheckBox.Location = new System.Drawing.Point(176, 32);
            this.DemandDuplicatesCheckBox.Name = "DemandDuplicatesCheckBox";
            this.DemandDuplicatesCheckBox.Size = new System.Drawing.Size(136, 16);
            this.DemandDuplicatesCheckBox.TabIndex = 2;
            this.DemandDuplicatesCheckBox.Text = "Demand duplicates";
            // 
            // IndividualRadioButton
            // 
            this.IndividualRadioButton.Location = new System.Drawing.Point(16, 48);
            this.IndividualRadioButton.Name = "IndividualRadioButton";
            this.IndividualRadioButton.Size = new System.Drawing.Size(80, 16);
            this.IndividualRadioButton.TabIndex = 1;
            this.IndividualRadioButton.Text = "Individual";
            this.IndividualRadioButton.CheckedChanged += new System.EventHandler(this.CheckChangedEventHandler);
            // 
            // SampleRadioButton
            // 
            this.SampleRadioButton.Checked = true;
            this.SampleRadioButton.Location = new System.Drawing.Point(16, 24);
            this.SampleRadioButton.Name = "SampleRadioButton";
            this.SampleRadioButton.Size = new System.Drawing.Size(96, 16);
            this.SampleRadioButton.TabIndex = 0;
            this.SampleRadioButton.TabStop = true;
            this.SampleRadioButton.Text = "Sample";
            this.SampleRadioButton.CheckedChanged += new System.EventHandler(this.CheckChangedEventHandler);
            // 
            // HardyWeinbergGroupBox
            // 
            this.HardyWeinbergGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.HardyWeinbergGroupBox.Controls.Add(this.DoHWTestCheckBox);
            this.HardyWeinbergGroupBox.Controls.Add(this.HWLimitNumeric);
            this.HardyWeinbergGroupBox.Controls.Add(this.HWLimitLabel);
            this.HardyWeinbergGroupBox.Controls.Add(this.SkipChildrenCheckBox);
            this.HardyWeinbergGroupBox.Location = new System.Drawing.Point(13, 334);
            this.HardyWeinbergGroupBox.Name = "HardyWeinbergGroupBox";
            this.HardyWeinbergGroupBox.Size = new System.Drawing.Size(338, 85);
            this.HardyWeinbergGroupBox.TabIndex = 1;
            this.HardyWeinbergGroupBox.TabStop = false;
            this.HardyWeinbergGroupBox.Text = "Frequencies";
            // 
            // DoHWTestCheckBox
            // 
            this.DoHWTestCheckBox.Location = new System.Drawing.Point(16, 28);
            this.DoHWTestCheckBox.Name = "DoHWTestCheckBox";
            this.DoHWTestCheckBox.Size = new System.Drawing.Size(112, 16);
            this.DoHWTestCheckBox.TabIndex = 7;
            this.DoHWTestCheckBox.Text = "Perform H-W test";
            this.DoHWTestCheckBox.CheckedChanged += new System.EventHandler(this.CheckChangedEventHandler);
            // 
            // HWLimitNumeric
            // 
            this.HWLimitNumeric.Location = new System.Drawing.Point(200, 28);
            this.HWLimitNumeric.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.HWLimitNumeric.Name = "HWLimitNumeric";
            this.HWLimitNumeric.Size = new System.Drawing.Size(72, 20);
            this.HWLimitNumeric.TabIndex = 9;
            // 
            // HWLimitLabel
            // 
            this.HWLimitLabel.Location = new System.Drawing.Point(136, 28);
            this.HWLimitLabel.Name = "HWLimitLabel";
            this.HWLimitLabel.Size = new System.Drawing.Size(56, 16);
            this.HWLimitLabel.TabIndex = 2;
            this.HWLimitLabel.Text = "Chi2 limit";
            // 
            // SkipChildrenCheckBox
            // 
            this.SkipChildrenCheckBox.Location = new System.Drawing.Point(16, 52);
            this.SkipChildrenCheckBox.Name = "SkipChildrenCheckBox";
            this.SkipChildrenCheckBox.Size = new System.Drawing.Size(176, 16);
            this.SkipChildrenCheckBox.TabIndex = 8;
            this.SkipChildrenCheckBox.Text = "Skip children";
            // 
            // InheritanceTestGroupBox
            // 
            this.InheritanceTestGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.InheritanceTestGroupBox.Controls.Add(this.DoInheritanceTestCheckBox);
            this.InheritanceTestGroupBox.Controls.Add(this.DetectXCheckBox);
            this.InheritanceTestGroupBox.Location = new System.Drawing.Point(13, 223);
            this.InheritanceTestGroupBox.Name = "InheritanceTestGroupBox";
            this.InheritanceTestGroupBox.Size = new System.Drawing.Size(338, 85);
            this.InheritanceTestGroupBox.TabIndex = 2;
            this.InheritanceTestGroupBox.TabStop = false;
            this.InheritanceTestGroupBox.Text = "Inheritance and Chromosomes";
            // 
            // DoInheritanceTestCheckBox
            // 
            this.DoInheritanceTestCheckBox.Location = new System.Drawing.Point(16, 24);
            this.DoInheritanceTestCheckBox.Name = "DoInheritanceTestCheckBox";
            this.DoInheritanceTestCheckBox.Size = new System.Drawing.Size(192, 16);
            this.DoInheritanceTestCheckBox.TabIndex = 5;
            this.DoInheritanceTestCheckBox.Text = "Perform inheritance test";
            this.DoInheritanceTestCheckBox.CheckedChanged += new System.EventHandler(this.CheckChangedEventHandler);
            // 
            // DetectXCheckBox
            // 
            this.DetectXCheckBox.Location = new System.Drawing.Point(16, 48);
            this.DetectXCheckBox.Name = "DetectXCheckBox";
            this.DetectXCheckBox.Size = new System.Drawing.Size(192, 16);
            this.DetectXCheckBox.TabIndex = 6;
            this.DetectXCheckBox.Text = "Detect human X chromosome";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(13, 455);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 10;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(286, 455);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 11;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ExperimentGroupBox
            // 
            this.ExperimentGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ExperimentGroupBox.Controls.Add(this.MarkerRadioButton);
            this.ExperimentGroupBox.Controls.Add(this.AssayRadioButton);
            this.ExperimentGroupBox.Location = new System.Drawing.Point(13, 121);
            this.ExperimentGroupBox.Name = "ExperimentGroupBox";
            this.ExperimentGroupBox.Size = new System.Drawing.Size(337, 85);
            this.ExperimentGroupBox.TabIndex = 5;
            this.ExperimentGroupBox.TabStop = false;
            this.ExperimentGroupBox.Text = "Experiment Report Unit";
            // 
            // MarkerRadioButton
            // 
            this.MarkerRadioButton.AutoSize = true;
            this.MarkerRadioButton.Checked = true;
            this.MarkerRadioButton.Location = new System.Drawing.Point(16, 52);
            this.MarkerRadioButton.Name = "MarkerRadioButton";
            this.MarkerRadioButton.Size = new System.Drawing.Size(58, 17);
            this.MarkerRadioButton.TabIndex = 4;
            this.MarkerRadioButton.TabStop = true;
            this.MarkerRadioButton.Text = "Marker";
            this.MarkerRadioButton.UseVisualStyleBackColor = true;
            // 
            // AssayRadioButton
            // 
            this.AssayRadioButton.AutoSize = true;
            this.AssayRadioButton.Location = new System.Drawing.Point(16, 29);
            this.AssayRadioButton.Name = "AssayRadioButton";
            this.AssayRadioButton.Size = new System.Drawing.Size(53, 17);
            this.AssayRadioButton.TabIndex = 3;
            this.AssayRadioButton.Text = "Assay";
            this.AssayRadioButton.UseVisualStyleBackColor = true;
            // 
            // SessionSettingsForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(370, 491);
            this.Controls.Add(this.ExperimentGroupBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.InheritanceTestGroupBox);
            this.Controls.Add(this.HardyWeinbergGroupBox);
            this.Controls.Add(this.UnitGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SessionSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Session Settings";
            this.UnitGroupBox.ResumeLayout(false);
            this.HardyWeinbergGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HWLimitNumeric)).EndInit();
            this.InheritanceTestGroupBox.ResumeLayout(false);
            this.ExperimentGroupBox.ResumeLayout(false);
            this.ExperimentGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public DialogResult GetSettings(SessionSettings currentSettings)
		{
			//Changes currentSettings and returns DialogResult.OK if the user clicks OK,
			//otherwise returns DialogResult.Cancel.
			this.SetControlsFromSettings(currentSettings);
			this.SetControlEnabledStatus();
			this.ShowDialog();
			if (MyOKFlag)
			{
				GetSettingsFromControls(currentSettings);
				return DialogResult.OK;
			}
			else
			{
				return DialogResult.Cancel;
			}
		}
		
		private void GetSettingsFromControls(SessionSettings outSettings)
		{
			outSettings.Set(SessionSettings.Bools.DoInheritanceTest, DoInheritanceTestCheckBox.Checked);
			outSettings.Set(SessionSettings.Bools.HumanXChr, DetectXCheckBox.Checked);
			outSettings.Set(SessionSettings.Bools.DemandDuplicates, DemandDuplicatesCheckBox.Checked);
			outSettings.Set(SessionSettings.Bools.DoHWTest, DoHWTestCheckBox.Checked);
			outSettings.Set(SessionSettings.Bools.HWSkipChildren, SkipChildrenCheckBox.Checked);
			outSettings.Set(SessionSettings.Doubles.HWLimit, Convert.ToDouble(HWLimitNumeric.Value));
			outSettings.Set(SessionSettings.Strings.ViewMode, SampleRadioButton.Checked ? "SAMPLE" : "SUBJECT");
            outSettings.Set(SessionSettings.Strings.ExperimentMode, AssayRadioButton.Checked ? "ASSAY" : "MARKER");
		}

		private void SetControlsFromSettings(SessionSettings settings)
		{
			DoInheritanceTestCheckBox.Checked = settings.Get(SessionSettings.Bools.DoInheritanceTest);
			DetectXCheckBox.Checked = settings.Get(SessionSettings.Bools.HumanXChr);
			DemandDuplicatesCheckBox.Checked = settings.Get(SessionSettings.Bools.DemandDuplicates);
			DoHWTestCheckBox.Checked = settings.Get(SessionSettings.Bools.DoHWTest);
			SkipChildrenCheckBox.Checked = settings.Get(SessionSettings.Bools.HWSkipChildren);
			HWLimitNumeric.Value = (int)settings.Get(SessionSettings.Doubles.HWLimit);
			if (settings.Get(SessionSettings.Strings.ViewMode) == "SAMPLE")
			{
				SampleRadioButton.Checked = true;
			}
			else if (settings.Get(SessionSettings.Strings.ViewMode) == "SUBJECT")
			{
				IndividualRadioButton.Checked = true;
			}
            if (settings.Get(SessionSettings.Strings.ExperimentMode) == "ASSAY")
            {
                AssayRadioButton.Checked = true;
            }
            else if (settings.Get(SessionSettings.Strings.ExperimentMode) == "MARKER") 
            {
                MarkerRadioButton.Checked = true;
            }
		}

		private void SetControlEnabledStatus()
		{
			DoInheritanceTestCheckBox.Enabled = IndividualRadioButton.Checked;
			SkipChildrenCheckBox.Enabled = IndividualRadioButton.Checked;
			HWLimitNumeric.Enabled = DoHWTestCheckBox.Checked;
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

		private void CheckChangedEventHandler(object sender, System.EventArgs e)
		{
			try
			{
				SetControlEnabledStatus();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when changing control status.", this);
			}
		}



	}
}
