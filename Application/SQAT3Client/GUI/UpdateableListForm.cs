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
	public class UpdateableListForm : Molmed.SQAT.GUI.ListForm
	{
		private System.ComponentModel.IContainer components = null;
		private ListFetcher MyListFetcher;

		public UpdateableListForm()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UpdateableListForm));
			this.SuspendLayout();
			// 
			// MyAdvancedListView
			// 
			this.MyAdvancedListView.Visible = true;
			// 
			// OKButton
			// 
			this.OKButton.Visible = true;
			// 
			// CloseButton
			// 
			this.CloseButton.Visible = true;
			// 
			// UpdateableListForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 374);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "UpdateableListForm";
			this.ResumeLayout(false);

		}
		#endregion

		public void ShowInformation(string title, ListFetcher fetcher)
		{
			OKButton.Visible = false;
			this.Text = title;
			MyListFetcher = fetcher;

            this.UpdateList();
			this.ShowDialog();
		}

		public void UpdateList()
		{
            MyAdvancedListView.ShowFormattedTable(MyListFetcher.GetData());
		}

		abstract public class ListFetcher
		{
			public ListFetcher()
			{
			}

			abstract public DataTable GetData();
		}

	}
}

