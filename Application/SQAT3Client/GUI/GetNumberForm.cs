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
	/// Summary description for GetNumberForm.
	/// </summary>
	public class GetNumberForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Label PromptLabel;
		private System.Windows.Forms.NumericUpDown NumberSelector;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private bool MyOKFlag = false;

		public GetNumberForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetNumberForm));
            this.PromptLabel = new System.Windows.Forms.Label();
            this.NumberSelector = new System.Windows.Forms.NumericUpDown();
            this.OKButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NumberSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // PromptLabel
            // 
            this.PromptLabel.Location = new System.Drawing.Point(24, 16);
            this.PromptLabel.Name = "PromptLabel";
            this.PromptLabel.Size = new System.Drawing.Size(296, 40);
            this.PromptLabel.TabIndex = 0;
            this.PromptLabel.Text = "<Prompt>";
            this.PromptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumberSelector
            // 
            this.NumberSelector.Location = new System.Drawing.Point(136, 72);
            this.NumberSelector.Name = "NumberSelector";
            this.NumberSelector.Size = new System.Drawing.Size(72, 20);
            this.NumberSelector.TabIndex = 0;
            this.NumberSelector.Validated += new System.EventHandler(this.NumberSelector_Validated);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(8, 114);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(258, 114);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // GetNumberForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(338, 144);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.NumberSelector);
            this.Controls.Add(this.PromptLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GetNumberForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GetNumberForm";
            ((System.ComponentModel.ISupportInitialize)(this.NumberSelector)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		public bool GetNumber(IWin32Window parent, string title, int min, int max, int defaultVal, string prompt, out int returnedNumber)
		{
			PrepareControls(title, (decimal)min, (decimal)max, (decimal)defaultVal, prompt);
			this.ShowDialog(parent);
			if (MyOKFlag)
			{
				returnedNumber = (int)NumberSelector.Value;
				return true;
			}
			else
			{
				returnedNumber = -1;
				return false;
			}
		}

		public bool GetNumber(IWin32Window parent, string title, decimal min, decimal max, decimal defaultVal, string prompt, out decimal returnedNumber)
		{
			PrepareControls(title, min, max, defaultVal, prompt);
			this.ShowDialog(parent);
			if (MyOKFlag)
			{
				returnedNumber = NumberSelector.Value;
				return true;
			}
			else
			{
				returnedNumber = (decimal)-1;
				return false;
			}			
		}

		private void PrepareControls(string title, decimal min, decimal max, decimal defaultVal, string prompt)
		{
			this.Text = title;
			PromptLabel.Text = prompt;
			NumberSelector.Minimum = min;
			NumberSelector.Maximum = max;
			NumberSelector.Value = defaultVal;
		}

		private void NumberSelector_Validated(object sender, System.EventArgs e)
		{
			if (NumberSelector.Value > NumberSelector.Maximum)
			{
				NumberSelector.Value = NumberSelector.Maximum;
			}
			else if (NumberSelector.Value < NumberSelector.Minimum)
			{
				NumberSelector.Value = NumberSelector.Minimum;
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
	
	}
}
