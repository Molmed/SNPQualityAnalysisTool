using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.GUI
{
	public class GenotypeListForm : Molmed.SQAT.GUI.UpdateableListForm
	{
        private DataServer MyDataServer;
        private ContextMenuStrip MyGenotypeContextMenuStrip;
        private ToolStripMenuItem StatusMenuItem;
        private ToolStripMenuItem StatusLoadMenuItem;
        private ToolStripMenuItem StatusRejectedMenuItem;
        private ToolStripMenuItem HistoryMenuItem;
		private System.ComponentModel.IContainer components = null;

		public GenotypeListForm(DataServer dServer)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			MyDataServer = dServer;
			MyAdvancedListView.TheListView.ContextMenuStrip = MyGenotypeContextMenuStrip;
			MyAdvancedListView.TheListView.MultiSelect = true;
			MyAdvancedListView.TheListView.Click += new EventHandler(ListView_Click);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenotypeListForm));
            this.MyGenotypeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.StatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusRejectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HistoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MyGenotypeContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MyGenotypeContextMenuStrip
            // 
            this.MyGenotypeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusMenuItem,
            this.HistoryMenuItem});
            this.MyGenotypeContextMenuStrip.Name = "MyGenotypeContextMenuStrip";
            this.MyGenotypeContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MyGenotypeContextMenuStrip.Size = new System.Drawing.Size(156, 70);
            this.MyGenotypeContextMenuStrip.Opened += new System.EventHandler(this.MyGenotypeContextMenu_Popup);
            // 
            // StatusMenuItem
            // 
            this.StatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLoadMenuItem,
            this.StatusRejectedMenuItem});
            this.StatusMenuItem.Name = "StatusMenuItem";
            this.StatusMenuItem.Size = new System.Drawing.Size(155, 22);
            this.StatusMenuItem.Text = "Change status";
            // 
            // StatusLoadMenuItem
            // 
            this.StatusLoadMenuItem.Name = "StatusLoadMenuItem";
            this.StatusLoadMenuItem.Size = new System.Drawing.Size(128, 22);
            this.StatusLoadMenuItem.Text = "Load";
            this.StatusLoadMenuItem.Click += new System.EventHandler(this.ChangeStatus_Click);
            // 
            // StatusRejectedMenuItem
            // 
            this.StatusRejectedMenuItem.Name = "StatusRejectedMenuItem";
            this.StatusRejectedMenuItem.Size = new System.Drawing.Size(128, 22);
            this.StatusRejectedMenuItem.Text = "Rejected";
            this.StatusRejectedMenuItem.Click += new System.EventHandler(this.ChangeStatus_Click);
            // 
            // HistoryMenuItem
            // 
            this.HistoryMenuItem.Name = "HistoryMenuItem";
            this.HistoryMenuItem.Size = new System.Drawing.Size(155, 22);
            this.HistoryMenuItem.Text = "Show history";
            this.HistoryMenuItem.Click += new System.EventHandler(this.ShowHistoryMenuItem_Click);
            // 
            // GenotypeListForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 374);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GenotypeListForm";
            this.MyGenotypeContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		private void ChangeStatus_Click(object sender, System.EventArgs e)
		{
			int [] genotypeIDs;

			try
			{
				if (MyAdvancedListView.TheListView.SelectedItems.Count > 0)
				{
					this.Cursor = Cursors.WaitCursor;
					genotypeIDs = new Int32[MyAdvancedListView.TheListView.SelectedItems.Count];
					for (int i = 0; i < MyAdvancedListView.TheListView.SelectedItems.Count; i++)
					{
						genotypeIDs[i] = Convert.ToInt32(MyAdvancedListView.TheListView.SelectedItems[i].Tag);
					}

					if (sender == StatusLoadMenuItem)
					{ 
						MyDataServer.UpdateGenotypeStatus(genotypeIDs, "load"); 
					}
					else if (sender == StatusRejectedMenuItem)
					{ 
						MyDataServer.UpdateGenotypeStatus(genotypeIDs, "rejected"); 
					}

					base.UpdateList();
					this.Cursor = Cursors.Default;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when changing status.", this);
			}
		}
	
		private void ListView_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (MyAdvancedListView.TheListView.SelectedItems.Count >1)
				{
					HistoryMenuItem.Enabled = false;
				}
				else
				{
					HistoryMenuItem.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when updating menu.", this);
			}
		}

		private void ShowHistoryMenuItem_Click(object sender, System.EventArgs e)
		{
			int genotypeID;
			string genotypeName;
			StaticListForm historyForm;
			DataTable historyTable;

			try
			{
				if (MyAdvancedListView.TheListView.SelectedItems.Count == 1)
				{
					genotypeID = Convert.ToInt32(MyAdvancedListView.TheListView.SelectedItems[0].Tag);
					genotypeName = MyAdvancedListView.TheListView.SelectedItems[0].Text;
					this.Cursor = Cursors.WaitCursor;
					historyTable = MyDataServer.GetGenotypeHistory(genotypeID);
					this.Cursor = Cursors.Default;
					if (historyTable.Rows.Count > 0)
					{
						historyForm = new StaticListForm();
						historyForm.ShowInformation("History for " + genotypeName, historyTable);
					}
					else
					{
						MessageManager.ShowInformation("No history information available.", this);
					}

				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing history.", this);
			}
		}

		private void MyGenotypeContextMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				for (int i = 0 ; i <  MyGenotypeContextMenuStrip.Items.Count ; i++)
				{
					MyGenotypeContextMenuStrip.Items[i].Enabled = (MyAdvancedListView.TheListView.SelectedItems.Count > 0) ;
				}

                //Disable history menu if more than one item is selected.
                HistoryMenuItem.Enabled = (MyAdvancedListView.TheListView.SelectedItems.Count == 1);
 
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when showing menu.", this);
			}
		}
	
	
	}
}

