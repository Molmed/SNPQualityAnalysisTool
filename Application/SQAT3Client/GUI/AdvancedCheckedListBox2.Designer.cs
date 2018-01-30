namespace Molmed.SQAT.GUI
{
    partial class AdvancedCheckedListBox2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedCheckedListBox2));
            this.BottomToolStrip = new System.Windows.Forms.ToolStrip();
            this.NumberLabel = new System.Windows.Forms.ToolStripLabel();
            this.MenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.CheckAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UncheckAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckFromFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReverseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveCheckedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyCheckedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MyCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.MyOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MySaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.BottomToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomToolStrip
            // 
            this.BottomToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.BottomToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NumberLabel,
            this.MenuButton});
            this.BottomToolStrip.Location = new System.Drawing.Point(0, 190);
            this.BottomToolStrip.Name = "BottomToolStrip";
            this.BottomToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.BottomToolStrip.Size = new System.Drawing.Size(280, 25);
            this.BottomToolStrip.TabIndex = 0;
            this.BottomToolStrip.Text = "toolStrip1";
            // 
            // NumberLabel
            // 
            this.NumberLabel.Name = "NumberLabel";
            this.NumberLabel.Size = new System.Drawing.Size(30, 22);
            this.NumberLabel.Text = "0 (0)";
            // 
            // MenuButton
            // 
            this.MenuButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckAllMenuItem,
            this.UncheckAllMenuItem,
            this.CheckFromFileMenuItem,
            this.ReverseMenuItem,
            this.toolStripSeparator1,
            this.SaveAllMenuItem,
            this.SaveCheckedMenuItem,
            this.toolStripSeparator2,
            this.CopyAllMenuItem,
            this.CopyCheckedMenuItem});
            this.MenuButton.Image = ((System.Drawing.Image)(resources.GetObject("MenuButton.Image")));
            this.MenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuButton.Name = "MenuButton";
            this.MenuButton.Size = new System.Drawing.Size(29, 22);
            this.MenuButton.Text = "toolStripDropDownButton1";
            this.MenuButton.ToolTipText = "Show menu";
            // 
            // CheckAllMenuItem
            // 
            this.CheckAllMenuItem.Name = "CheckAllMenuItem";
            this.CheckAllMenuItem.Size = new System.Drawing.Size(191, 22);
            this.CheckAllMenuItem.Text = "Check All";
            this.CheckAllMenuItem.Click += new System.EventHandler(this.CheckAllMenuItem_Click);
            // 
            // UncheckAllMenuItem
            // 
            this.UncheckAllMenuItem.Name = "UncheckAllMenuItem";
            this.UncheckAllMenuItem.Size = new System.Drawing.Size(191, 22);
            this.UncheckAllMenuItem.Text = "Uncheck All";
            this.UncheckAllMenuItem.Click += new System.EventHandler(this.UncheckAllMenuItem_Click);
            // 
            // CheckFromFileMenuItem
            // 
            this.CheckFromFileMenuItem.Name = "CheckFromFileMenuItem";
            this.CheckFromFileMenuItem.Size = new System.Drawing.Size(191, 22);
            this.CheckFromFileMenuItem.Text = "Check from File...";
            this.CheckFromFileMenuItem.Click += new System.EventHandler(this.CheckFromFileMenuItem_Click);
            // 
            // ReverseMenuItem
            // 
            this.ReverseMenuItem.Name = "ReverseMenuItem";
            this.ReverseMenuItem.Size = new System.Drawing.Size(191, 22);
            this.ReverseMenuItem.Text = "Reverse Check Status";
            this.ReverseMenuItem.Click += new System.EventHandler(this.ReverseMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(188, 6);
            // 
            // SaveAllMenuItem
            // 
            this.SaveAllMenuItem.Name = "SaveAllMenuItem";
            this.SaveAllMenuItem.Size = new System.Drawing.Size(191, 22);
            this.SaveAllMenuItem.Text = "Save All...";
            this.SaveAllMenuItem.Click += new System.EventHandler(this.SaveAllMenuItem_Click);
            // 
            // SaveCheckedMenuItem
            // 
            this.SaveCheckedMenuItem.Name = "SaveCheckedMenuItem";
            this.SaveCheckedMenuItem.Size = new System.Drawing.Size(191, 22);
            this.SaveCheckedMenuItem.Text = "Save Checked...";
            this.SaveCheckedMenuItem.Click += new System.EventHandler(this.SaveCheckedMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(188, 6);
            // 
            // CopyAllMenuItem
            // 
            this.CopyAllMenuItem.Name = "CopyAllMenuItem";
            this.CopyAllMenuItem.Size = new System.Drawing.Size(191, 22);
            this.CopyAllMenuItem.Text = "Copy All";
            this.CopyAllMenuItem.Click += new System.EventHandler(this.CopyAllMenuItem_Click);
            // 
            // CopyCheckedMenuItem
            // 
            this.CopyCheckedMenuItem.Name = "CopyCheckedMenuItem";
            this.CopyCheckedMenuItem.Size = new System.Drawing.Size(191, 22);
            this.CopyCheckedMenuItem.Text = "Copy Checked";
            this.CopyCheckedMenuItem.Click += new System.EventHandler(this.CopyCheckedMenuItem_Click);
            // 
            // MyCheckedListBox
            // 
            this.MyCheckedListBox.CheckOnClick = true;
            this.MyCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyCheckedListBox.FormattingEnabled = true;
            this.MyCheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.MyCheckedListBox.Name = "MyCheckedListBox";
            this.MyCheckedListBox.Size = new System.Drawing.Size(280, 184);
            this.MyCheckedListBox.TabIndex = 1;
            this.MyCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.MyCheckedListBox_ItemCheck);
            // 
            // MyOpenFileDialog
            // 
            this.MyOpenFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            this.MyOpenFileDialog.Title = "Open File";
            // 
            // MySaveFileDialog
            // 
            this.MySaveFileDialog.FileName = "FileName";
            this.MySaveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            this.MySaveFileDialog.Title = "Save File";
            // 
            // AdvancedCheckedListBox2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MyCheckedListBox);
            this.Controls.Add(this.BottomToolStrip);
            this.Name = "AdvancedCheckedListBox2";
            this.Size = new System.Drawing.Size(280, 215);
            this.BottomToolStrip.ResumeLayout(false);
            this.BottomToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip BottomToolStrip;
        private System.Windows.Forms.ToolStripLabel NumberLabel;
        private System.Windows.Forms.ToolStripDropDownButton MenuButton;
        private System.Windows.Forms.ToolStripMenuItem CheckAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UncheckAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CheckFromFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReverseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAllMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem SaveCheckedMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem CopyAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyCheckedMenuItem;
        private System.Windows.Forms.CheckedListBox MyCheckedListBox;
        private System.Windows.Forms.OpenFileDialog MyOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog MySaveFileDialog;
    }
}
