using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;


namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for RerunListForm.
	/// </summary>
	public class RerunListForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Button CloseButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private IdentifiableDictionary MyAllItems, MyCriteriaSelectedItems; 
		private IdentifiableDictionary MyManuallyAddedItems, MyManuallyRemovedItems;
		private string MySelectedExperimentList;
		private RerunSettings MyRerunSettings;
		private System.Windows.Forms.GroupBox LogGroupBox;
		private System.Windows.Forms.TextBox CriteriaTextBox;
		private System.Windows.Forms.Button SaveLogButton;
        private System.Windows.Forms.SaveFileDialog MySaveFileDialog;
        private Splitter VerticalSplitter;
        private AdvancedCheckedListBox2 MyCheckedListBox;
		private StringBuilder MyText;

		public RerunListForm(RerunSettings criteria, Identifiable [] selectedExperiments, 
			Identifiable [] allItems, Identifiable [] criteriaSelectedItems)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			StringBuilder str;

			str = new StringBuilder("", selectedExperiments.GetLength(0) * 8);

			//Save rerun settings.
			MyRerunSettings = criteria;
			
			//Make a text list of the selected experiments.
			foreach(Identifiable tempExperiment in selectedExperiments)
			{
				str.Append(tempExperiment.Name + Environment.NewLine);
			}
			MySelectedExperimentList = str.ToString();

			//Create a dictionary of all items.
			MyAllItems = new IdentifiableDictionary();
			foreach(Identifiable tempItem in allItems)
			{
				MyAllItems.Add(tempItem.ID, tempItem);
			}

			//Create a dictionary of the automatically selected items.
			MyCriteriaSelectedItems = new IdentifiableDictionary();
			foreach(Identifiable tempItem in criteriaSelectedItems)
			{
				MyCriteriaSelectedItems.Add(tempItem.ID, tempItem);
			}

			//Initialize dictionaries for manually added and removed items.
			MyManuallyAddedItems = new IdentifiableDictionary();
			MyManuallyRemovedItems = new IdentifiableDictionary();

			MyCheckedListBox.TheCheckedListBox.Items.AddRange(allItems);

			//Check the automatically selected items.
			for (int i = 0 ; i < MyCheckedListBox.TheCheckedListBox.Items.Count ; i++)
			{
				if (MyCriteriaSelectedItems.Contains(((Identifiable)MyCheckedListBox.TheCheckedListBox.Items[i]).ID))
				{
					MyCheckedListBox.TheCheckedListBox.SetItemChecked(i, true);
				}				
			}
            MyCheckedListBox.RefreshCounterText();

			//Listen to the ItemCheck event inside the actual checked list box.
			MyCheckedListBox.TheCheckedListBox.ItemCheck += new ItemCheckEventHandler(MyCheckedListBox_ItemCheck);

			//Initialize the global string builder which holds the log text.
			//(Need space for all the experiments plus some extra text.)
			MyText = new StringBuilder("", MySelectedExperimentList.Length + 1000);

			RefreshText();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RerunListForm));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.LogGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveLogButton = new System.Windows.Forms.Button();
            this.CriteriaTextBox = new System.Windows.Forms.TextBox();
            this.VerticalSplitter = new System.Windows.Forms.Splitter();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.MySaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MyCheckedListBox = new Molmed.SQAT.GUI.AdvancedCheckedListBox2();
            this.TopPanel.SuspendLayout();
            this.LogGroupBox.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.LogGroupBox);
            this.TopPanel.Controls.Add(this.VerticalSplitter);
            this.TopPanel.Controls.Add(this.MyCheckedListBox);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(544, 398);
            this.TopPanel.TabIndex = 0;
            // 
            // LogGroupBox
            // 
            this.LogGroupBox.Controls.Add(this.SaveLogButton);
            this.LogGroupBox.Controls.Add(this.CriteriaTextBox);
            this.LogGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogGroupBox.Location = new System.Drawing.Point(208, 0);
            this.LogGroupBox.Name = "LogGroupBox";
            this.LogGroupBox.Size = new System.Drawing.Size(336, 398);
            this.LogGroupBox.TabIndex = 3;
            this.LogGroupBox.TabStop = false;
            this.LogGroupBox.Text = "Log";
            // 
            // SaveLogButton
            // 
            this.SaveLogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveLogButton.Location = new System.Drawing.Point(8, 360);
            this.SaveLogButton.Name = "SaveLogButton";
            this.SaveLogButton.Size = new System.Drawing.Size(72, 24);
            this.SaveLogButton.TabIndex = 2;
            this.SaveLogButton.Text = "&Save...";
            this.SaveLogButton.Click += new System.EventHandler(this.SaveLogButton_Click);
            // 
            // CriteriaTextBox
            // 
            this.CriteriaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CriteriaTextBox.Location = new System.Drawing.Point(8, 16);
            this.CriteriaTextBox.Multiline = true;
            this.CriteriaTextBox.Name = "CriteriaTextBox";
            this.CriteriaTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CriteriaTextBox.Size = new System.Drawing.Size(320, 336);
            this.CriteriaTextBox.TabIndex = 1;
            // 
            // VerticalSplitter
            // 
            this.VerticalSplitter.Location = new System.Drawing.Point(198, 0);
            this.VerticalSplitter.Name = "VerticalSplitter";
            this.VerticalSplitter.Size = new System.Drawing.Size(10, 398);
            this.VerticalSplitter.TabIndex = 5;
            this.VerticalSplitter.TabStop = false;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 398);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(544, 48);
            this.BottomPanel.TabIndex = 1;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(456, 16);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "&Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MySaveFileDialog
            // 
            this.MySaveFileDialog.FileName = "FileName";
            this.MySaveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*) |*.*";
            this.MySaveFileDialog.Title = "Save file";
            // 
            // MyCheckedListBox
            // 
            this.MyCheckedListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.MyCheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.MyCheckedListBox.Name = "MyCheckedListBox";
            this.MyCheckedListBox.Size = new System.Drawing.Size(198, 398);
            this.MyCheckedListBox.TabIndex = 0;
            // 
            // RerunListForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(544, 446);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.BottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RerunListForm";
            this.Text = "Rerun List";
            this.TopPanel.ResumeLayout(false);
            this.LogGroupBox.ResumeLayout(false);
            this.LogGroupBox.PerformLayout();
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void RefreshText()
		{
			string nl;

			//Clear the text.
			MyText.Remove(0, MyText.Length);
 
			nl = Environment.NewLine;

			//Add setting description.
			MyText.Append("Settings" + nl);
			MyText.Append("--------" + nl);
			MyText.Append(MyRerunSettings.ToString());
			MyText.Append(nl);

			//Add manually checked and unchecked items.
			MyText.Append("Manual changes" + nl);
			MyText.Append("--------------" + nl);
			foreach(Identifiable tempItem in MyManuallyAddedItems.Values)
			{
				MyText.Append("Added " + tempItem.Name + nl);
			}
			foreach(Identifiable tempItem in MyManuallyRemovedItems.Values)
			{
				MyText.Append("Removed " + tempItem.Name + nl);
			}
			MyText.Append(nl);

			//Add selected experiments.
			MyText.Append("Selected experiments" + nl);
			MyText.Append("----------------" + nl);
			MyText.Append(MySelectedExperimentList);

			CriteriaTextBox.Text = MyText.ToString();

            //Reset text selection.
            CriteriaTextBox.Select(CriteriaTextBox.Text.Length, 0);

		}


		private void MyCheckedListBox_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			int itemID;

			//Note: Intentionally no try-catch in this routine, because will be fired
			//many times when using the check box menu to check multiple items. Exceptions
			//will be catched higher up in the hierarchy.

			itemID = ((Identifiable)MyCheckedListBox.TheCheckedListBox.Items[e.Index]).ID;

			//Keep track of which items have been automatically and manually selected.
			if (MyCriteriaSelectedItems.Contains(itemID))
			{
				if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
				{
					//An automatically selected item is now being unchecked.
					MyManuallyRemovedItems.Add(itemID, new Identifiable(itemID, MyAllItems[itemID].Name));
				}
				else if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
				{
					//An automatically selected item which had been unchecked is now being checked again.
					MyManuallyRemovedItems.Remove(itemID);
				}

			}
			else
			{
				if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
				{
					//A manually checked item is now being unchecked again.
					MyManuallyAddedItems.Remove(itemID);
				}
				else if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
				{
					//An item is being manually checked.
					MyManuallyAddedItems.Add(itemID, new Identifiable(itemID, MyAllItems[itemID].Name));
				}
			}
			RefreshText();

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

		private void SaveLogButton_Click(object sender, System.EventArgs e)
		{
			FileServer fs;
			string filename;
		
			try
			{
				//Get file name.
				if (MySaveFileDialog.ShowDialog() == DialogResult.Cancel)
				{
					return;
				}
				filename = MySaveFileDialog.FileName;

				this.Cursor = Cursors.WaitCursor;

				//Save file.
				fs = new FileServer();
				fs.SaveFile(CriteriaTextBox.Text, filename);
				this.Cursor = Cursors.Default;
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Unable to save file.", this);
			}		
		}

		public class IdentifiableDictionary : DictionaryBase
		{

			public Identifiable this[int key]  
			{
				get  
				{
					return((Identifiable)Dictionary[key]);
				}
				set  
				{
					Dictionary[key] = value;
				}
			}

			public void Add(int key, Identifiable value)  
			{
				Dictionary.Add(key, value);
			}

			public bool Contains(int key)  
			{
				return Dictionary.Contains(key);
			}

			public void Remove(int key)  
			{
				Dictionary.Remove(key);
			}

			public ICollection Values
			{
				get
				{
					return Dictionary.Values;
				}
			}

		}

	}
}
