namespace Molmed.SQAT.GUI
{
    partial class ComparisonSource
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComparisonSource));
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.FileRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionRadioButton = new System.Windows.Forms.RadioButton();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.SessionInclusionToolStrip = new System.Windows.Forms.ToolStrip();
            this.ItemTypesDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.GenotypeStatusesDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.MissingValueTextBox = new System.Windows.Forms.TextBox();
            this.MissingValueLabel = new System.Windows.Forms.Label();
            this.FileBrowseButton = new System.Windows.Forms.Button();
            this.FileTextBox = new System.Windows.Forms.TextBox();
            this.SessionList = new System.Windows.Forms.ComboBox();
            this.MyOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ReferenceSetRadioButton = new System.Windows.Forms.RadioButton();
            this.ReferenceSetList = new System.Windows.Forms.ComboBox();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.SessionInclusionToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.ReferenceSetRadioButton);
            this.LeftPanel.Controls.Add(this.FileRadioButton);
            this.LeftPanel.Controls.Add(this.SessionRadioButton);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(96, 155);
            this.LeftPanel.TabIndex = 6;
            // 
            // FileRadioButton
            // 
            this.FileRadioButton.AutoSize = true;
            this.FileRadioButton.Location = new System.Drawing.Point(3, 103);
            this.FileRadioButton.Name = "FileRadioButton";
            this.FileRadioButton.Size = new System.Drawing.Size(41, 17);
            this.FileRadioButton.TabIndex = 3;
            this.FileRadioButton.Text = "File";
            this.FileRadioButton.UseVisualStyleBackColor = true;
            this.FileRadioButton.CheckedChanged += new System.EventHandler(this.FileRadioButton_CheckedChanged);
            // 
            // SessionRadioButton
            // 
            this.SessionRadioButton.AutoSize = true;
            this.SessionRadioButton.Checked = true;
            this.SessionRadioButton.Location = new System.Drawing.Point(3, 3);
            this.SessionRadioButton.Name = "SessionRadioButton";
            this.SessionRadioButton.Size = new System.Drawing.Size(62, 17);
            this.SessionRadioButton.TabIndex = 1;
            this.SessionRadioButton.TabStop = true;
            this.SessionRadioButton.Text = "Session";
            this.SessionRadioButton.UseVisualStyleBackColor = true;
            this.SessionRadioButton.CheckedChanged += new System.EventHandler(this.SessionRadioButton_CheckedChanged);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.ReferenceSetList);
            this.RightPanel.Controls.Add(this.SessionInclusionToolStrip);
            this.RightPanel.Controls.Add(this.MissingValueTextBox);
            this.RightPanel.Controls.Add(this.MissingValueLabel);
            this.RightPanel.Controls.Add(this.FileBrowseButton);
            this.RightPanel.Controls.Add(this.FileTextBox);
            this.RightPanel.Controls.Add(this.SessionList);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(96, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(296, 155);
            this.RightPanel.TabIndex = 7;
            // 
            // SessionInclusionToolStrip
            // 
            this.SessionInclusionToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.SessionInclusionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SessionInclusionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemTypesDropDownButton,
            this.GenotypeStatusesDropDownButton});
            this.SessionInclusionToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.SessionInclusionToolStrip.Location = new System.Drawing.Point(3, 27);
            this.SessionInclusionToolStrip.Name = "SessionInclusionToolStrip";
            this.SessionInclusionToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.SessionInclusionToolStrip.Size = new System.Drawing.Size(184, 20);
            this.SessionInclusionToolStrip.TabIndex = 8;
            this.SessionInclusionToolStrip.Text = "Session inclusion criteria";
            // 
            // ItemTypesDropDownButton
            // 
            this.ItemTypesDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ItemTypesDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("ItemTypesDropDownButton.Image")));
            this.ItemTypesDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ItemTypesDropDownButton.Name = "ItemTypesDropDownButton";
            this.ItemTypesDropDownButton.Size = new System.Drawing.Size(72, 17);
            this.ItemTypesDropDownButton.Text = "Item types";
            this.ItemTypesDropDownButton.ToolTipText = "Set included individual types";
            // 
            // GenotypeStatusesDropDownButton
            // 
            this.GenotypeStatusesDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GenotypeStatusesDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("GenotypeStatusesDropDownButton.Image")));
            this.GenotypeStatusesDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GenotypeStatusesDropDownButton.Name = "GenotypeStatusesDropDownButton";
            this.GenotypeStatusesDropDownButton.Size = new System.Drawing.Size(111, 17);
            this.GenotypeStatusesDropDownButton.Text = "Genotype statuses";
            this.GenotypeStatusesDropDownButton.ToolTipText = "Set included genotype statuses";
            // 
            // MissingValueTextBox
            // 
            this.MissingValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MissingValueTextBox.Location = new System.Drawing.Point(168, 128);
            this.MissingValueTextBox.Name = "MissingValueTextBox";
            this.MissingValueTextBox.Size = new System.Drawing.Size(82, 20);
            this.MissingValueTextBox.TabIndex = 7;
            this.MissingValueTextBox.Text = "-/-";
            // 
            // MissingValueLabel
            // 
            this.MissingValueLabel.AutoSize = true;
            this.MissingValueLabel.Location = new System.Drawing.Point(88, 131);
            this.MissingValueLabel.Name = "MissingValueLabel";
            this.MissingValueLabel.Size = new System.Drawing.Size(74, 13);
            this.MissingValueLabel.TabIndex = 6;
            this.MissingValueLabel.Text = "Missing value:";
            // 
            // FileBrowseButton
            // 
            this.FileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileBrowseButton.Location = new System.Drawing.Point(256, 98);
            this.FileBrowseButton.Name = "FileBrowseButton";
            this.FileBrowseButton.Size = new System.Drawing.Size(34, 27);
            this.FileBrowseButton.TabIndex = 5;
            this.FileBrowseButton.Text = "...";
            this.FileBrowseButton.UseVisualStyleBackColor = true;
            this.FileBrowseButton.Click += new System.EventHandler(this.FileBrowseButton_Click);
            // 
            // FileTextBox
            // 
            this.FileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FileTextBox.Location = new System.Drawing.Point(3, 102);
            this.FileTextBox.Name = "FileTextBox";
            this.FileTextBox.Size = new System.Drawing.Size(247, 20);
            this.FileTextBox.TabIndex = 4;
            // 
            // SessionList
            // 
            this.SessionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SessionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SessionList.FormattingEnabled = true;
            this.SessionList.Location = new System.Drawing.Point(3, 3);
            this.SessionList.Name = "SessionList";
            this.SessionList.Size = new System.Drawing.Size(290, 21);
            this.SessionList.TabIndex = 2;
            // 
            // MyOpenFileDialog
            // 
            this.MyOpenFileDialog.FileName = "FileName";
            // 
            // ReferenceSetRadioButton
            // 
            this.ReferenceSetRadioButton.AutoSize = true;
            this.ReferenceSetRadioButton.Location = new System.Drawing.Point(3, 53);
            this.ReferenceSetRadioButton.Name = "ReferenceSetRadioButton";
            this.ReferenceSetRadioButton.Size = new System.Drawing.Size(92, 17);
            this.ReferenceSetRadioButton.TabIndex = 4;
            this.ReferenceSetRadioButton.TabStop = true;
            this.ReferenceSetRadioButton.Text = "Reference set";
            this.ReferenceSetRadioButton.UseVisualStyleBackColor = true;
            this.ReferenceSetRadioButton.CheckedChanged += new System.EventHandler(this.ReferenceSetRadioButton_CheckedChanged);
            // 
            // ReferenceSetList
            // 
            this.ReferenceSetList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceSetList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReferenceSetList.FormattingEnabled = true;
            this.ReferenceSetList.Location = new System.Drawing.Point(3, 53);
            this.ReferenceSetList.Name = "ReferenceSetList";
            this.ReferenceSetList.Size = new System.Drawing.Size(290, 21);
            this.ReferenceSetList.TabIndex = 9;
            // 
            // ComparisonSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.Name = "ComparisonSource";
            this.Size = new System.Drawing.Size(392, 155);
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.RightPanel.ResumeLayout(false);
            this.RightPanel.PerformLayout();
            this.SessionInclusionToolStrip.ResumeLayout(false);
            this.SessionInclusionToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.RadioButton FileRadioButton;
        private System.Windows.Forms.RadioButton SessionRadioButton;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Button FileBrowseButton;
        private System.Windows.Forms.TextBox FileTextBox;
        private System.Windows.Forms.ComboBox SessionList;
        private System.Windows.Forms.OpenFileDialog MyOpenFileDialog;
        private System.Windows.Forms.TextBox MissingValueTextBox;
        private System.Windows.Forms.Label MissingValueLabel;
        private System.Windows.Forms.ToolStrip SessionInclusionToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton ItemTypesDropDownButton;
        private System.Windows.Forms.ToolStripDropDownButton GenotypeStatusesDropDownButton;
        private System.Windows.Forms.RadioButton ReferenceSetRadioButton;
        private System.Windows.Forms.ComboBox ReferenceSetList;
    }
}
