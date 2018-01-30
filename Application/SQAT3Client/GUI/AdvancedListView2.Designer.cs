namespace Molmed.SQAT.GUI
{
    partial class AdvancedListView2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedListView2));
            this.BottomToolStrip = new System.Windows.Forms.ToolStrip();
            this.NumberLabel = new System.Windows.Forms.ToolStripLabel();
            this.MenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SelectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectFromFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReverseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveSelectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyAllWithHeadersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyAllWithoutHeadersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopySelectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopySelectionWithHeadersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopySelectionWithoutHeadersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MyOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MySaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MyExtListView = new Molmed.SQAT.GUI.GridlineListView(this.components);
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
            this.BottomToolStrip.Location = new System.Drawing.Point(0, 194);
            this.BottomToolStrip.Name = "BottomToolStrip";
            this.BottomToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.BottomToolStrip.Size = new System.Drawing.Size(269, 25);
            this.BottomToolStrip.TabIndex = 2;
            this.BottomToolStrip.Text = "Tool strip";
            // 
            // NumberLabel
            // 
            this.NumberLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.NumberLabel.Name = "NumberLabel";
            this.NumberLabel.Size = new System.Drawing.Size(30, 22);
            this.NumberLabel.Text = "0 (0)";
            this.NumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MenuButton
            // 
            this.MenuButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectAllMenuItem,
            this.SelectFromFileMenuItem,
            this.ReverseMenuItem,
            this.toolStripSeparator1,
            this.SaveAllMenuItem,
            this.SaveSelectionMenuItem,
            this.toolStripSeparator2,
            this.CopyAllMenuItem,
            this.CopySelectionMenuItem});
            this.MenuButton.Image = ((System.Drawing.Image)(resources.GetObject("MenuButton.Image")));
            this.MenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuButton.Name = "MenuButton";
            this.MenuButton.Size = new System.Drawing.Size(29, 22);
            this.MenuButton.Text = "toolStripDropDownButton1";
            this.MenuButton.ToolTipText = "Show menu";
            this.MenuButton.DropDownOpening += new System.EventHandler(this.MenuButton_DropDownOpening);
            // 
            // SelectAllMenuItem
            // 
            this.SelectAllMenuItem.Name = "SelectAllMenuItem";
            this.SelectAllMenuItem.Size = new System.Drawing.Size(171, 22);
            this.SelectAllMenuItem.Text = "Select All";
            this.SelectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItem_Click);
            // 
            // SelectFromFileMenuItem
            // 
            this.SelectFromFileMenuItem.Name = "SelectFromFileMenuItem";
            this.SelectFromFileMenuItem.Size = new System.Drawing.Size(171, 22);
            this.SelectFromFileMenuItem.Text = "Select from File...";
            this.SelectFromFileMenuItem.Click += new System.EventHandler(this.SelectFromFileMenuItem_Click);
            // 
            // ReverseMenuItem
            // 
            this.ReverseMenuItem.Name = "ReverseMenuItem";
            this.ReverseMenuItem.Size = new System.Drawing.Size(171, 22);
            this.ReverseMenuItem.Text = "Reverse Selection";
            this.ReverseMenuItem.Click += new System.EventHandler(this.ReverseMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // SaveAllMenuItem
            // 
            this.SaveAllMenuItem.Name = "SaveAllMenuItem";
            this.SaveAllMenuItem.Size = new System.Drawing.Size(171, 22);
            this.SaveAllMenuItem.Text = "Save All...";
            this.SaveAllMenuItem.Click += new System.EventHandler(this.SaveAllMenuItem_Click);
            // 
            // SaveSelectionMenuItem
            // 
            this.SaveSelectionMenuItem.Name = "SaveSelectionMenuItem";
            this.SaveSelectionMenuItem.Size = new System.Drawing.Size(171, 22);
            this.SaveSelectionMenuItem.Text = "Save Selection...";
            this.SaveSelectionMenuItem.Click += new System.EventHandler(this.SaveSelectionMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(168, 6);
            // 
            // CopyAllMenuItem
            // 
            this.CopyAllMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyAllWithHeadersMenuItem,
            this.CopyAllWithoutHeadersMenuItem});
            this.CopyAllMenuItem.Name = "CopyAllMenuItem";
            this.CopyAllMenuItem.Size = new System.Drawing.Size(171, 22);
            this.CopyAllMenuItem.Text = "Copy All";
            // 
            // CopyAllWithHeadersMenuItem
            // 
            this.CopyAllWithHeadersMenuItem.Name = "CopyAllWithHeadersMenuItem";
            this.CopyAllWithHeadersMenuItem.Size = new System.Drawing.Size(194, 22);
            this.CopyAllWithHeadersMenuItem.Text = "with Column Names";
            this.CopyAllWithHeadersMenuItem.Click += new System.EventHandler(this.CopyAllWithHeadersMenuItem_Click);
            // 
            // CopyAllWithoutHeadersMenuItem
            // 
            this.CopyAllWithoutHeadersMenuItem.Name = "CopyAllWithoutHeadersMenuItem";
            this.CopyAllWithoutHeadersMenuItem.Size = new System.Drawing.Size(194, 22);
            this.CopyAllWithoutHeadersMenuItem.Text = "without Column Names";
            this.CopyAllWithoutHeadersMenuItem.Click += new System.EventHandler(this.CopyAllWithoutHeadersMenuItem_Click);
            // 
            // CopySelectionMenuItem
            // 
            this.CopySelectionMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopySelectionWithHeadersMenuItem,
            this.CopySelectionWithoutHeadersMenuItem});
            this.CopySelectionMenuItem.Name = "CopySelectionMenuItem";
            this.CopySelectionMenuItem.Size = new System.Drawing.Size(171, 22);
            this.CopySelectionMenuItem.Text = "Copy Selection";
            // 
            // CopySelectionWithHeadersMenuItem
            // 
            this.CopySelectionWithHeadersMenuItem.Name = "CopySelectionWithHeadersMenuItem";
            this.CopySelectionWithHeadersMenuItem.Size = new System.Drawing.Size(194, 22);
            this.CopySelectionWithHeadersMenuItem.Text = "with Column Names";
            this.CopySelectionWithHeadersMenuItem.Click += new System.EventHandler(this.CopySelectionWithHeadersMenuItem_Click);
            // 
            // CopySelectionWithoutHeadersMenuItem
            // 
            this.CopySelectionWithoutHeadersMenuItem.Name = "CopySelectionWithoutHeadersMenuItem";
            this.CopySelectionWithoutHeadersMenuItem.Size = new System.Drawing.Size(194, 22);
            this.CopySelectionWithoutHeadersMenuItem.Text = "without Column Names";
            this.CopySelectionWithoutHeadersMenuItem.Click += new System.EventHandler(this.CopySelectionWithoutHeadersMenuItem_Click);
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
            // 
            // MyExtListView
            // 
            this.MyExtListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyExtListView.FullRowSelect = true;
            this.MyExtListView.GridLines = true;
            this.MyExtListView.HideSelection = false;
            this.MyExtListView.LabelWrap = false;
            this.MyExtListView.Location = new System.Drawing.Point(0, 0);
            this.MyExtListView.Name = "MyExtListView";
            this.MyExtListView.Size = new System.Drawing.Size(269, 194);
            this.MyExtListView.TabIndex = 3;
            this.MyExtListView.UseCompatibleStateImageBehavior = false;
            this.MyExtListView.View = System.Windows.Forms.View.Details;
            this.MyExtListView.SelectedIndexChanged += new System.EventHandler(this.MyListView_SelectedIndexChanged);
            this.MyExtListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClicked);
            // 
            // AdvancedListView2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MyExtListView);
            this.Controls.Add(this.BottomToolStrip);
            this.Name = "AdvancedListView2";
            this.Size = new System.Drawing.Size(269, 219);
            this.BottomToolStrip.ResumeLayout(false);
            this.BottomToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip BottomToolStrip;
        private System.Windows.Forms.ToolStripLabel NumberLabel;
        private System.Windows.Forms.ToolStripDropDownButton MenuButton;
        private System.Windows.Forms.ToolStripMenuItem SelectAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SelectFromFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReverseMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem SaveAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveSelectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyAllMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem CopySelectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyAllWithHeadersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyAllWithoutHeadersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopySelectionWithHeadersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopySelectionWithoutHeadersMenuItem;
        private System.Windows.Forms.OpenFileDialog MyOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog MySaveFileDialog;
        private GridlineListView MyExtListView;
    }
}
