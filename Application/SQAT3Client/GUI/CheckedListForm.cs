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
	/// Summary description for CheckedListForm.
	/// </summary>
	public class CheckedListForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel ListPanel;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Button OKButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private AdvancedCheckedListBox2 MyCheckedListBox;

		private bool MyOKFlag;

		public CheckedListForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public void LoadItems(object [] items)
		{
			this.Cursor = Cursors.WaitCursor;
			MyCheckedListBox.TheCheckedListBox.Items.Clear();
			MyCheckedListBox.TheCheckedListBox.Items.AddRange(items);
            MyCheckedListBox.RefreshCounterText();
			this.Cursor = Cursors.Default;
		}

		public void DisplayItems(string title)
		{
			this.Text = title;
			OKButton.Visible = false;
			CloseButton.Text = "&Close";
			this.ShowDialog();
		}

		public void DisplayItems(string title, object [] items)
		{
			this.Text = title;
			this.LoadItems(items);
			OKButton.Visible = false;
			CloseButton.Text = "&Close";
			this.ShowDialog();
		}

		public DialogResult DisplayItemsDialog(string title, object [] items, out object [] checkedItems)
		{
			this.Text = title;
			this.LoadItems(items);
			OKButton.Visible = true;
			CloseButton.Text = "&Cancel";
			MyOKFlag = false;
			this.ShowDialog();
			checkedItems = GetCheckedItems();

			return MyOKFlag ? DialogResult.OK : DialogResult.Cancel;	
		}

		public void SetAllCheckState(bool checkState)
		{
			this.Cursor = Cursors.WaitCursor;
			MyCheckedListBox.TheCheckedListBox.BeginUpdate();
			for (int i = 0 ; i < MyCheckedListBox.TheCheckedListBox.Items.Count ; i++)
			{
				MyCheckedListBox.TheCheckedListBox.SetItemChecked(i, checkState);
			}
			MyCheckedListBox.TheCheckedListBox.EndUpdate();
			this.Cursor = Cursors.Default;
		}

		public void SetItemChecked(int index, bool checkStatus)
		{
			MyCheckedListBox.TheCheckedListBox.SetItemChecked(index, checkStatus);
		}

		public bool GetItemChecked(int index)
		{
			return MyCheckedListBox.TheCheckedListBox.GetItemChecked(index);
		}

		public object [] GetItems()
		{
			object [] returnArray;

			this.Cursor = Cursors.WaitCursor;
			returnArray = new object[MyCheckedListBox.TheCheckedListBox.Items.Count];
			MyCheckedListBox.TheCheckedListBox.Items.CopyTo(returnArray, 0);
			this.Cursor = Cursors.Default;
			return returnArray;
		}

		public object [] GetCheckedItems()
		{
			object [] checkedItems;

			this.Cursor = Cursors.WaitCursor;
			checkedItems = new object[MyCheckedListBox.TheCheckedListBox.CheckedItems.Count];
			for (int i = 0 ;  i < MyCheckedListBox.TheCheckedListBox.CheckedItems.Count ; i++)
			{
				checkedItems[i] = MyCheckedListBox.TheCheckedListBox.CheckedItems[i];
			}
			this.Cursor = Cursors.Default;
			return checkedItems;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckedListForm));
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.OKButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ListPanel = new System.Windows.Forms.Panel();
            this.MyCheckedListBox = new Molmed.SQAT.GUI.AdvancedCheckedListBox2();
            this.BottomPanel.SuspendLayout();
            this.ListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.OKButton);
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 270);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(392, 40);
            this.BottomPanel.TabIndex = 0;
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(8, 8);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(312, 8);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "&Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ListPanel
            // 
            this.ListPanel.Controls.Add(this.MyCheckedListBox);
            this.ListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListPanel.Location = new System.Drawing.Point(0, 0);
            this.ListPanel.Name = "ListPanel";
            this.ListPanel.Size = new System.Drawing.Size(392, 270);
            this.ListPanel.TabIndex = 1;
            // 
            // MyCheckedListBox
            // 
            this.MyCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyCheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.MyCheckedListBox.Name = "MyCheckedListBox";
            this.MyCheckedListBox.Size = new System.Drawing.Size(392, 270);
            this.MyCheckedListBox.TabIndex = 0;
            // 
            // CheckedListForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(392, 310);
            this.Controls.Add(this.ListPanel);
            this.Controls.Add(this.BottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CheckedListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CheckedListForm";
            this.BottomPanel.ResumeLayout(false);
            this.ListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void CloseButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Unable to close form.", this);
			}
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
				MessageManager.ShowError(ex, "Unable to close form.", this);				
			}
		}
	}
}
