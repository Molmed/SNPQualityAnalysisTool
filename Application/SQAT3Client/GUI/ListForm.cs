using System;
using System.Data;
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
	/// Summary description for ListForm.
	/// </summary>
	public class ListForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel ListPanel;
        private System.Windows.Forms.Panel BottomPanel;
		protected System.Windows.Forms.Button OKButton;
		protected System.Windows.Forms.Button CloseButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        protected AdvancedListView2 MyAdvancedListView;

		protected bool MyOKFlag = false;

		public ListForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListForm));
            this.ListPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.MyAdvancedListView = new Molmed.SQAT.GUI.AdvancedListView2();
            this.ListPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListPanel
            // 
            this.ListPanel.Controls.Add(this.MyAdvancedListView);
            this.ListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListPanel.Location = new System.Drawing.Point(0, 0);
            this.ListPanel.Name = "ListPanel";
            this.ListPanel.Size = new System.Drawing.Size(632, 334);
            this.ListPanel.TabIndex = 0;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.OKButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 334);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(632, 40);
            this.BottomPanel.TabIndex = 1;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(552, 8);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(64, 24);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "&Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(16, 8);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(64, 24);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // MyAdvancedListView
            // 
            this.MyAdvancedListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyAdvancedListView.Location = new System.Drawing.Point(0, 0);
            this.MyAdvancedListView.Name = "MyAdvancedListView";
            this.MyAdvancedListView.Size = new System.Drawing.Size(632, 334);
            this.MyAdvancedListView.TabIndex = 0;
            // 
            // ListForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(632, 374);
            this.Controls.Add(this.ListPanel);
            this.Controls.Add(this.BottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ListForm";
            this.ListPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion



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


	}



}
