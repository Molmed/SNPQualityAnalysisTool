namespace Molmed.SQAT.GUI
{
    partial class ComparisonSettingsForm
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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.ListInvalidAllelesCheckBox = new System.Windows.Forms.CheckBox();
            this.ListUnharmonizableCheckBox = new System.Windows.Forms.CheckBox();
            this.ListMissingValueCheckBox = new System.Windows.Forms.CheckBox();
            this.ListDifferentCheckBox = new System.Windows.Forms.CheckBox();
            this.ListIdenticalCheckBox = new System.Windows.Forms.CheckBox();
            this.ListComparedCheckBox = new System.Windows.Forms.CheckBox();
            this.ListMissingCheckBox = new System.Windows.Forms.CheckBox();
            this.ListDuplicateFailuresCheckBox = new System.Windows.Forms.CheckBox();
            this.CaseSensitiveCheckBox = new System.Windows.Forms.CheckBox();
            this.CompareFreeTextRadioButton = new System.Windows.Forms.RadioButton();
            this.ConvertPolarityCheckBox = new System.Windows.Forms.CheckBox();
            this.CompareGenotypesRadioButton = new System.Windows.Forms.RadioButton();
            this.CompareZygosityRadioButton = new System.Windows.Forms.RadioButton();
            this.Source2GroupBox = new System.Windows.Forms.GroupBox();
            this.Source1GroupBox = new System.Windows.Forms.GroupBox();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.ComparisonSource2 = new Molmed.SQAT.GUI.ComparisonSource();
            this.ComparisonSource1 = new Molmed.SQAT.GUI.ComparisonSource();
            this.MainPanel.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            this.Source2GroupBox.SuspendLayout();
            this.Source1GroupBox.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.SettingsGroupBox);
            this.MainPanel.Controls.Add(this.Source2GroupBox);
            this.MainPanel.Controls.Add(this.Source1GroupBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(432, 696);
            this.MainPanel.TabIndex = 0;
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsGroupBox.Controls.Add(this.ListInvalidAllelesCheckBox);
            this.SettingsGroupBox.Controls.Add(this.ListUnharmonizableCheckBox);
            this.SettingsGroupBox.Controls.Add(this.ListMissingValueCheckBox);
            this.SettingsGroupBox.Controls.Add(this.ListDifferentCheckBox);
            this.SettingsGroupBox.Controls.Add(this.ListIdenticalCheckBox);
            this.SettingsGroupBox.Controls.Add(this.ListComparedCheckBox);
            this.SettingsGroupBox.Controls.Add(this.ListMissingCheckBox);
            this.SettingsGroupBox.Controls.Add(this.ListDuplicateFailuresCheckBox);
            this.SettingsGroupBox.Controls.Add(this.CaseSensitiveCheckBox);
            this.SettingsGroupBox.Controls.Add(this.CompareFreeTextRadioButton);
            this.SettingsGroupBox.Controls.Add(this.ConvertPolarityCheckBox);
            this.SettingsGroupBox.Controls.Add(this.CompareGenotypesRadioButton);
            this.SettingsGroupBox.Controls.Add(this.CompareZygosityRadioButton);
            this.SettingsGroupBox.Location = new System.Drawing.Point(12, 373);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(408, 271);
            this.SettingsGroupBox.TabIndex = 2;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Settings";
            // 
            // ListInvalidAllelesCheckBox
            // 
            this.ListInvalidAllelesCheckBox.AutoSize = true;
            this.ListInvalidAllelesCheckBox.Location = new System.Drawing.Point(216, 175);
            this.ListInvalidAllelesCheckBox.Name = "ListInvalidAllelesCheckBox";
            this.ListInvalidAllelesCheckBox.Size = new System.Drawing.Size(107, 17);
            this.ListInvalidAllelesCheckBox.TabIndex = 13;
            this.ListInvalidAllelesCheckBox.Text = "List invalid alleles";
            this.ListInvalidAllelesCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListUnharmonizableCheckBox
            // 
            this.ListUnharmonizableCheckBox.AutoSize = true;
            this.ListUnharmonizableCheckBox.Location = new System.Drawing.Point(216, 244);
            this.ListUnharmonizableCheckBox.Name = "ListUnharmonizableCheckBox";
            this.ListUnharmonizableCheckBox.Size = new System.Drawing.Size(149, 17);
            this.ListUnharmonizableCheckBox.TabIndex = 12;
            this.ListUnharmonizableCheckBox.Text = "List unharmonizable SNPs";
            this.ListUnharmonizableCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListMissingValueCheckBox
            // 
            this.ListMissingValueCheckBox.AutoSize = true;
            this.ListMissingValueCheckBox.Location = new System.Drawing.Point(216, 198);
            this.ListMissingValueCheckBox.Name = "ListMissingValueCheckBox";
            this.ListMissingValueCheckBox.Size = new System.Drawing.Size(183, 17);
            this.ListMissingValueCheckBox.TabIndex = 10;
            this.ListMissingValueCheckBox.Text = "List \"missing value\" combinations";
            this.ListMissingValueCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListDifferentCheckBox
            // 
            this.ListDifferentCheckBox.AutoSize = true;
            this.ListDifferentCheckBox.Location = new System.Drawing.Point(6, 175);
            this.ListDifferentCheckBox.Name = "ListDifferentCheckBox";
            this.ListDifferentCheckBox.Size = new System.Drawing.Size(148, 17);
            this.ListDifferentCheckBox.TabIndex = 9;
            this.ListDifferentCheckBox.Text = "List different combinations";
            this.ListDifferentCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListIdenticalCheckBox
            // 
            this.ListIdenticalCheckBox.AutoSize = true;
            this.ListIdenticalCheckBox.Location = new System.Drawing.Point(6, 198);
            this.ListIdenticalCheckBox.Name = "ListIdenticalCheckBox";
            this.ListIdenticalCheckBox.Size = new System.Drawing.Size(149, 17);
            this.ListIdenticalCheckBox.TabIndex = 8;
            this.ListIdenticalCheckBox.Text = "List identical combinations";
            this.ListIdenticalCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListComparedCheckBox
            // 
            this.ListComparedCheckBox.AutoSize = true;
            this.ListComparedCheckBox.Location = new System.Drawing.Point(6, 221);
            this.ListComparedCheckBox.Name = "ListComparedCheckBox";
            this.ListComparedCheckBox.Size = new System.Drawing.Size(157, 17);
            this.ListComparedCheckBox.TabIndex = 7;
            this.ListComparedCheckBox.Text = "List compared combinations";
            this.ListComparedCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListMissingCheckBox
            // 
            this.ListMissingCheckBox.AutoSize = true;
            this.ListMissingCheckBox.Location = new System.Drawing.Point(6, 244);
            this.ListMissingCheckBox.Name = "ListMissingCheckBox";
            this.ListMissingCheckBox.Size = new System.Drawing.Size(144, 17);
            this.ListMissingCheckBox.TabIndex = 6;
            this.ListMissingCheckBox.Text = "List missing combinations";
            this.ListMissingCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListDuplicateFailuresCheckBox
            // 
            this.ListDuplicateFailuresCheckBox.AutoSize = true;
            this.ListDuplicateFailuresCheckBox.Location = new System.Drawing.Point(216, 221);
            this.ListDuplicateFailuresCheckBox.Name = "ListDuplicateFailuresCheckBox";
            this.ListDuplicateFailuresCheckBox.Size = new System.Drawing.Size(124, 17);
            this.ListDuplicateFailuresCheckBox.TabIndex = 5;
            this.ListDuplicateFailuresCheckBox.Text = "List duplicate failures";
            this.ListDuplicateFailuresCheckBox.UseVisualStyleBackColor = true;
            // 
            // CaseSensitiveCheckBox
            // 
            this.CaseSensitiveCheckBox.AutoSize = true;
            this.CaseSensitiveCheckBox.Location = new System.Drawing.Point(33, 135);
            this.CaseSensitiveCheckBox.Name = "CaseSensitiveCheckBox";
            this.CaseSensitiveCheckBox.Size = new System.Drawing.Size(126, 17);
            this.CaseSensitiveCheckBox.TabIndex = 4;
            this.CaseSensitiveCheckBox.Text = "Result case sensitive";
            this.CaseSensitiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // CompareFreeTextRadioButton
            // 
            this.CompareFreeTextRadioButton.AutoSize = true;
            this.CompareFreeTextRadioButton.Location = new System.Drawing.Point(4, 112);
            this.CompareFreeTextRadioButton.Name = "CompareFreeTextRadioButton";
            this.CompareFreeTextRadioButton.Size = new System.Drawing.Size(155, 17);
            this.CompareFreeTextRadioButton.TabIndex = 3;
            this.CompareFreeTextRadioButton.Text = "Compare results as free text";
            this.CompareFreeTextRadioButton.UseVisualStyleBackColor = true;
            this.CompareFreeTextRadioButton.CheckedChanged += new System.EventHandler(this.CompareFreeTextRadioButton_CheckedChanged);
            // 
            // ConvertPolarityCheckBox
            // 
            this.ConvertPolarityCheckBox.AutoSize = true;
            this.ConvertPolarityCheckBox.Location = new System.Drawing.Point(33, 80);
            this.ConvertPolarityCheckBox.Name = "ConvertPolarityCheckBox";
            this.ConvertPolarityCheckBox.Size = new System.Drawing.Size(313, 17);
            this.ConvertPolarityCheckBox.TabIndex = 2;
            this.ConvertPolarityCheckBox.Text = "Harmonize polarities (skip monomorphic, A/T and C/G SNPs)";
            this.ConvertPolarityCheckBox.UseVisualStyleBackColor = true;
            this.ConvertPolarityCheckBox.CheckedChanged += new System.EventHandler(this.ConvertPolarityCheckBox_CheckedChanged);
            // 
            // CompareGenotypesRadioButton
            // 
            this.CompareGenotypesRadioButton.AutoSize = true;
            this.CompareGenotypesRadioButton.Location = new System.Drawing.Point(6, 57);
            this.CompareGenotypesRadioButton.Name = "CompareGenotypesRadioButton";
            this.CompareGenotypesRadioButton.Size = new System.Drawing.Size(119, 17);
            this.CompareGenotypesRadioButton.TabIndex = 1;
            this.CompareGenotypesRadioButton.Text = "Compare genotypes";
            this.CompareGenotypesRadioButton.UseVisualStyleBackColor = true;
            this.CompareGenotypesRadioButton.CheckedChanged += new System.EventHandler(this.CompareGenotypesRadioButton_CheckedChanged);
            // 
            // CompareZygosityRadioButton
            // 
            this.CompareZygosityRadioButton.AutoSize = true;
            this.CompareZygosityRadioButton.Checked = true;
            this.CompareZygosityRadioButton.Location = new System.Drawing.Point(6, 19);
            this.CompareZygosityRadioButton.Name = "CompareZygosityRadioButton";
            this.CompareZygosityRadioButton.Size = new System.Drawing.Size(115, 17);
            this.CompareZygosityRadioButton.TabIndex = 0;
            this.CompareZygosityRadioButton.TabStop = true;
            this.CompareZygosityRadioButton.Text = "Compare zygosities";
            this.CompareZygosityRadioButton.UseVisualStyleBackColor = true;
            this.CompareZygosityRadioButton.CheckedChanged += new System.EventHandler(this.CompareZygosityRadioButton_CheckedChanged);
            // 
            // Source2GroupBox
            // 
            this.Source2GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Source2GroupBox.Controls.Add(this.ComparisonSource2);
            this.Source2GroupBox.Location = new System.Drawing.Point(12, 190);
            this.Source2GroupBox.Name = "Source2GroupBox";
            this.Source2GroupBox.Size = new System.Drawing.Size(408, 172);
            this.Source2GroupBox.TabIndex = 1;
            this.Source2GroupBox.TabStop = false;
            this.Source2GroupBox.Text = "Source 2";
            // 
            // Source1GroupBox
            // 
            this.Source1GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Source1GroupBox.Controls.Add(this.ComparisonSource1);
            this.Source1GroupBox.Location = new System.Drawing.Point(12, 12);
            this.Source1GroupBox.Name = "Source1GroupBox";
            this.Source1GroupBox.Size = new System.Drawing.Size(408, 172);
            this.Source1GroupBox.TabIndex = 0;
            this.Source1GroupBox.TabStop = false;
            this.Source1GroupBox.Text = "Source 1";
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.OKButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 650);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(432, 46);
            this.BottomPanel.TabIndex = 1;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(342, 12);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(12, 12);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // ComparisonSource2
            // 
            this.ComparisonSource2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComparisonSource2.Location = new System.Drawing.Point(3, 16);
            this.ComparisonSource2.Name = "ComparisonSource2";
            this.ComparisonSource2.Size = new System.Drawing.Size(402, 153);
            this.ComparisonSource2.TabIndex = 0;
            // 
            // ComparisonSource1
            // 
            this.ComparisonSource1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComparisonSource1.Location = new System.Drawing.Point(3, 16);
            this.ComparisonSource1.Name = "ComparisonSource1";
            this.ComparisonSource1.Size = new System.Drawing.Size(402, 153);
            this.ComparisonSource1.TabIndex = 0;
            // 
            // ComparisonSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 696);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.MainPanel);
            this.Name = "ComparisonSettingsForm";
            this.Text = "Compare Results";
            this.MainPanel.ResumeLayout(false);
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            this.Source2GroupBox.ResumeLayout(false);
            this.Source1GroupBox.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.GroupBox Source2GroupBox;
        private System.Windows.Forms.GroupBox Source1GroupBox;
        private System.Windows.Forms.Panel BottomPanel;
        private ComparisonSource ComparisonSource1;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CloseButton;
        private ComparisonSource ComparisonSource2;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.RadioButton CompareGenotypesRadioButton;
        private System.Windows.Forms.RadioButton CompareZygosityRadioButton;
        private System.Windows.Forms.CheckBox CaseSensitiveCheckBox;
        private System.Windows.Forms.RadioButton CompareFreeTextRadioButton;
        private System.Windows.Forms.CheckBox ConvertPolarityCheckBox;
        private System.Windows.Forms.CheckBox ListMissingCheckBox;
        private System.Windows.Forms.CheckBox ListDuplicateFailuresCheckBox;
        private System.Windows.Forms.CheckBox ListDifferentCheckBox;
        private System.Windows.Forms.CheckBox ListIdenticalCheckBox;
        private System.Windows.Forms.CheckBox ListComparedCheckBox;
        private System.Windows.Forms.CheckBox ListMissingValueCheckBox;
        private System.Windows.Forms.CheckBox ListUnharmonizableCheckBox;
        private System.Windows.Forms.CheckBox ListInvalidAllelesCheckBox;

    }
}