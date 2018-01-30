namespace Molmed.SQAT.GUI
{
    partial class IdentificationSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.ExperimentsLabel = new System.Windows.Forms.Label();
            this.ExperimentsCutoffComboBox = new System.Windows.Forms.ComboBox();
            this.SimilarityLabel = new System.Windows.Forms.Label();
            this.SimilarityCutoffComboBox = new System.Windows.Forms.ComboBox();
            this.CutoffLabel = new System.Windows.Forms.Label();
            this.SpecificItemTextBox = new System.Windows.Forms.TextBox();
            this.SelfComparisonCheckBox = new System.Windows.Forms.CheckBox();
            this.SpecificItemCheckBox = new System.Windows.Forms.CheckBox();
            this.CaseSensitiveCheckBox = new System.Windows.Forms.CheckBox();
            this.CompareFreeTextRadioButton = new System.Windows.Forms.RadioButton();
            this.CompareGenotypesRadioButton = new System.Windows.Forms.RadioButton();
            this.CompareZygosityRadioButton = new System.Windows.Forms.RadioButton();
            this.SourceGroupBox = new System.Windows.Forms.GroupBox();
            this.IdentificationComparisonSource = new Molmed.SQAT.GUI.ComparisonSource();
            this.BottomPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            this.SourceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.OKButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 502);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(441, 50);
            this.BottomPanel.TabIndex = 2;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(354, 14);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(77, 25);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(12, 14);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.SettingsGroupBox);
            this.MainPanel.Controls.Add(this.SourceGroupBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(441, 502);
            this.MainPanel.TabIndex = 3;
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsGroupBox.Controls.Add(this.ExperimentsLabel);
            this.SettingsGroupBox.Controls.Add(this.ExperimentsCutoffComboBox);
            this.SettingsGroupBox.Controls.Add(this.SimilarityLabel);
            this.SettingsGroupBox.Controls.Add(this.SimilarityCutoffComboBox);
            this.SettingsGroupBox.Controls.Add(this.CutoffLabel);
            this.SettingsGroupBox.Controls.Add(this.SpecificItemTextBox);
            this.SettingsGroupBox.Controls.Add(this.SelfComparisonCheckBox);
            this.SettingsGroupBox.Controls.Add(this.SpecificItemCheckBox);
            this.SettingsGroupBox.Controls.Add(this.CaseSensitiveCheckBox);
            this.SettingsGroupBox.Controls.Add(this.CompareFreeTextRadioButton);
            this.SettingsGroupBox.Controls.Add(this.CompareGenotypesRadioButton);
            this.SettingsGroupBox.Controls.Add(this.CompareZygosityRadioButton);
            this.SettingsGroupBox.Location = new System.Drawing.Point(12, 190);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(416, 292);
            this.SettingsGroupBox.TabIndex = 2;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Settings";
            // 
            // ExperimentsLabel
            // 
            this.ExperimentsLabel.AutoSize = true;
            this.ExperimentsLabel.Location = new System.Drawing.Point(347, 251);
            this.ExperimentsLabel.Name = "ExperimentsLabel";
            this.ExperimentsLabel.Size = new System.Drawing.Size(63, 13);
            this.ExperimentsLabel.TabIndex = 11;
            this.ExperimentsLabel.Text = "experiments";
            // 
            // ExperimentsCutoffComboBox
            // 
            this.ExperimentsCutoffComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ExperimentsCutoffComboBox.FormattingEnabled = true;
            this.ExperimentsCutoffComboBox.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100",
            "200",
            "300",
            "400",
            "500",
            "1000",
            "5000",
            "10000",
            "50000",
            "100000"});
            this.ExperimentsCutoffComboBox.Location = new System.Drawing.Point(263, 248);
            this.ExperimentsCutoffComboBox.Name = "ExperimentsCutoffComboBox";
            this.ExperimentsCutoffComboBox.Size = new System.Drawing.Size(80, 21);
            this.ExperimentsCutoffComboBox.TabIndex = 10;
            // 
            // SimilarityLabel
            // 
            this.SimilarityLabel.AutoSize = true;
            this.SimilarityLabel.Location = new System.Drawing.Point(149, 251);
            this.SimilarityLabel.Name = "SimilarityLabel";
            this.SimilarityLabel.Size = new System.Drawing.Size(108, 13);
            this.SimilarityLabel.TabIndex = 9;
            this.SimilarityLabel.Text = "% similarity for at least";
            // 
            // SimilarityCutoffComboBox
            // 
            this.SimilarityCutoffComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SimilarityCutoffComboBox.FormattingEnabled = true;
            this.SimilarityCutoffComboBox.Items.AddRange(new object[] {
            "100",
            "95",
            "90",
            "85",
            "80",
            "75",
            "70",
            "65",
            "60",
            "55",
            "50",
            "40",
            "30",
            "20",
            "10",
            "0"});
            this.SimilarityCutoffComboBox.Location = new System.Drawing.Point(47, 248);
            this.SimilarityCutoffComboBox.Name = "SimilarityCutoffComboBox";
            this.SimilarityCutoffComboBox.Size = new System.Drawing.Size(96, 21);
            this.SimilarityCutoffComboBox.TabIndex = 8;
            // 
            // CutoffLabel
            // 
            this.CutoffLabel.AutoSize = true;
            this.CutoffLabel.Location = new System.Drawing.Point(3, 251);
            this.CutoffLabel.Name = "CutoffLabel";
            this.CutoffLabel.Size = new System.Drawing.Size(38, 13);
            this.CutoffLabel.TabIndex = 7;
            this.CutoffLabel.Text = "Cutoff:";
            // 
            // SpecificItemTextBox
            // 
            this.SpecificItemTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SpecificItemTextBox.Location = new System.Drawing.Point(193, 184);
            this.SpecificItemTextBox.Name = "SpecificItemTextBox";
            this.SpecificItemTextBox.Size = new System.Drawing.Size(217, 20);
            this.SpecificItemTextBox.TabIndex = 6;
            // 
            // SelfComparisonCheckBox
            // 
            this.SelfComparisonCheckBox.AutoSize = true;
            this.SelfComparisonCheckBox.Location = new System.Drawing.Point(6, 218);
            this.SelfComparisonCheckBox.Name = "SelfComparisonCheckBox";
            this.SelfComparisonCheckBox.Size = new System.Drawing.Size(137, 17);
            this.SelfComparisonCheckBox.TabIndex = 5;
            this.SelfComparisonCheckBox.Text = "Include self-comparison";
            this.SelfComparisonCheckBox.UseVisualStyleBackColor = true;
            // 
            // SpecificItemCheckBox
            // 
            this.SpecificItemCheckBox.AutoSize = true;
            this.SpecificItemCheckBox.Location = new System.Drawing.Point(6, 186);
            this.SpecificItemCheckBox.Name = "SpecificItemCheckBox";
            this.SpecificItemCheckBox.Size = new System.Drawing.Size(181, 17);
            this.SpecificItemCheckBox.TabIndex = 4;
            this.SpecificItemCheckBox.Text = "Limit search to the following item:";
            this.SpecificItemCheckBox.UseVisualStyleBackColor = true;
            this.SpecificItemCheckBox.CheckedChanged += new System.EventHandler(this.SpecificItemCheckBox_CheckedChanged);
            // 
            // CaseSensitiveCheckBox
            // 
            this.CaseSensitiveCheckBox.AutoSize = true;
            this.CaseSensitiveCheckBox.Location = new System.Drawing.Point(26, 119);
            this.CaseSensitiveCheckBox.Name = "CaseSensitiveCheckBox";
            this.CaseSensitiveCheckBox.Size = new System.Drawing.Size(126, 17);
            this.CaseSensitiveCheckBox.TabIndex = 3;
            this.CaseSensitiveCheckBox.Text = "Result case sensitive";
            this.CaseSensitiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // CompareFreeTextRadioButton
            // 
            this.CompareFreeTextRadioButton.AutoSize = true;
            this.CompareFreeTextRadioButton.Location = new System.Drawing.Point(6, 96);
            this.CompareFreeTextRadioButton.Name = "CompareFreeTextRadioButton";
            this.CompareFreeTextRadioButton.Size = new System.Drawing.Size(155, 17);
            this.CompareFreeTextRadioButton.TabIndex = 2;
            this.CompareFreeTextRadioButton.Text = "Compare results as free text";
            this.CompareFreeTextRadioButton.UseVisualStyleBackColor = true;
            this.CompareFreeTextRadioButton.CheckedChanged += new System.EventHandler(this.CompareFreeTextRadioButton_CheckedChanged);
            // 
            // CompareGenotypesRadioButton
            // 
            this.CompareGenotypesRadioButton.AutoSize = true;
            this.CompareGenotypesRadioButton.Location = new System.Drawing.Point(6, 64);
            this.CompareGenotypesRadioButton.Name = "CompareGenotypesRadioButton";
            this.CompareGenotypesRadioButton.Size = new System.Drawing.Size(119, 17);
            this.CompareGenotypesRadioButton.TabIndex = 1;
            this.CompareGenotypesRadioButton.Text = "Compare genotypes";
            this.CompareGenotypesRadioButton.UseVisualStyleBackColor = true;
            // 
            // CompareZygosityRadioButton
            // 
            this.CompareZygosityRadioButton.AutoSize = true;
            this.CompareZygosityRadioButton.Checked = true;
            this.CompareZygosityRadioButton.Location = new System.Drawing.Point(6, 29);
            this.CompareZygosityRadioButton.Name = "CompareZygosityRadioButton";
            this.CompareZygosityRadioButton.Size = new System.Drawing.Size(115, 17);
            this.CompareZygosityRadioButton.TabIndex = 0;
            this.CompareZygosityRadioButton.TabStop = true;
            this.CompareZygosityRadioButton.Text = "Compare zygosities";
            this.CompareZygosityRadioButton.UseVisualStyleBackColor = true;
            // 
            // SourceGroupBox
            // 
            this.SourceGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceGroupBox.Controls.Add(this.IdentificationComparisonSource);
            this.SourceGroupBox.Location = new System.Drawing.Point(12, 12);
            this.SourceGroupBox.Name = "SourceGroupBox";
            this.SourceGroupBox.Size = new System.Drawing.Size(416, 172);
            this.SourceGroupBox.TabIndex = 1;
            this.SourceGroupBox.TabStop = false;
            this.SourceGroupBox.Text = "Source";
            // 
            // IdentificationComparisonSource
            // 
            this.IdentificationComparisonSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.IdentificationComparisonSource.Location = new System.Drawing.Point(3, 16);
            this.IdentificationComparisonSource.Name = "IdentificationComparisonSource";
            this.IdentificationComparisonSource.Size = new System.Drawing.Size(410, 153);
            this.IdentificationComparisonSource.TabIndex = 0;
            this.IdentificationComparisonSource.Load += new System.EventHandler(this.IdentificationComparisonSource_Load);
            // 
            // IdentificationSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 552);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.BottomPanel);
            this.Name = "IdentificationSettingsForm";
            this.Text = "Identify Items";
            this.BottomPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            this.SourceGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.GroupBox SourceGroupBox;
        private Molmed.SQAT.GUI.ComparisonSource IdentificationComparisonSource;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.RadioButton CompareZygosityRadioButton;
        private System.Windows.Forms.RadioButton CompareGenotypesRadioButton;
        private System.Windows.Forms.RadioButton CompareFreeTextRadioButton;
        private System.Windows.Forms.CheckBox CaseSensitiveCheckBox;
        private System.Windows.Forms.TextBox SpecificItemTextBox;
        private System.Windows.Forms.CheckBox SelfComparisonCheckBox;
        private System.Windows.Forms.CheckBox SpecificItemCheckBox;
        private System.Windows.Forms.ComboBox SimilarityCutoffComboBox;
        private System.Windows.Forms.Label CutoffLabel;
        private System.Windows.Forms.Label SimilarityLabel;
        private System.Windows.Forms.Label ExperimentsLabel;
        private System.Windows.Forms.ComboBox ExperimentsCutoffComboBox;


    }
}