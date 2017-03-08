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
	public class StaticListForm : Molmed.SQAT.GUI.ListForm
	{
		private System.ComponentModel.IContainer components = null;

		public StaticListForm()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(StaticListForm));
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
			// StaticListForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 374);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "StaticListForm";
			this.ResumeLayout(false);

		}
		#endregion


		public void ShowInformation(string title, DataTable table)
		{
			OKButton.Visible = false;
			this.Text = title;
			MyAdvancedListView.ShowFormattedTable(table);
			this.ShowDialog();
		}

        public void ShowMatrix(string title, string[,] matrix)
        {
            OKButton.Visible = false;
            this.Text = title;
            MyAdvancedListView.ShowMatrix(matrix);
            this.ShowDialog();
        }

		public bool ShowInformationConfirm(string title, DataTable table)
		{
			CloseButton.Text = "&Cancel";
			this.Text = title;
			MyAdvancedListView.ShowFormattedTable(table);
            this.ShowDialog();

			return base.MyOKFlag;
		}


	}
}

