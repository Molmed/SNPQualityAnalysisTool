using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;


namespace Molmed.SQAT.GUI
{
    public partial class AdvancedCheckedListBox2 : UserControl
    {
        private int MyCheckedCounter;

        public AdvancedCheckedListBox2()
        {
            InitializeComponent();

            MyCheckedCounter = 0;
        }

        public CheckedListBox TheCheckedListBox
        {
            get { return MyCheckedListBox; }
        }

        public void SetAllCheckStatus(bool status)
        {
            base.FindForm().Cursor = Cursors.WaitCursor;
            MyCheckedListBox.BeginUpdate();
            for (int i = 0; i < MyCheckedListBox.Items.Count; i++)
            {
                MyCheckedListBox.SetItemChecked(i, status);
            }
            MyCheckedListBox.EndUpdate();
            base.FindForm().Cursor = Cursors.Default;
        }

        public void ReverseAllCheckStatus()
        {
            base.FindForm().Cursor = Cursors.WaitCursor;
            MyCheckedListBox.BeginUpdate();
            for (int i = 0; i < MyCheckedListBox.Items.Count; i++)
            {
                MyCheckedListBox.SetItemChecked(i, !MyCheckedListBox.GetItemChecked(i));
            }
            MyCheckedListBox.EndUpdate();
            base.FindForm().Cursor = Cursors.Default;
        }

        public string GetListAsString(bool all)
        {
            StringBuilder str;

            base.FindForm().Cursor = Cursors.WaitCursor;
            str = new StringBuilder(20 * TheCheckedListBox.Items.Count);
            for (int i = 0; i < MyCheckedListBox.Items.Count; i++)
            {
                if (MyCheckedListBox.GetItemChecked(i) || all)
                {
                    str.Append(MyCheckedListBox.Items[i].ToString() + Environment.NewLine);
                }
            }
            base.FindForm().Cursor = Cursors.Default;
            return str.ToString();
        }

        public void ClearItems()
        {
            base.FindForm().Cursor = Cursors.WaitCursor;
            MyCheckedListBox.Items.Clear();
            MyCheckedCounter = 0;
            RefreshCounterText();
            base.FindForm().Cursor = Cursors.Default;
        }

        public void RefreshCounterText()
        {
            NumberLabel.Text = TheCheckedListBox.Items.Count.ToString() +
                " (" + MyCheckedCounter.ToString() + ")";
        }

        private void CheckAllMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                SetAllCheckStatus(true);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to check all items.", base.FindForm());
            }
        }

        private void UncheckAllMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                SetAllCheckStatus(false);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to uncheck all items.", base.FindForm());
            }
        }

        private void ReverseMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                ReverseAllCheckStatus();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to reverse the selection.", base.FindForm());
            }
        }

        private void MyCheckedListBox_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            try
            {
                if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
                {
                    MyCheckedCounter++;
                }
                else if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                {
                    MyCheckedCounter--;
                }

                if (MyCheckedCounter < 0)
                {
                    MyCheckedCounter = 0;
                }
                else if (MyCheckedCounter > TheCheckedListBox.Items.Count)
                {
                    MyCheckedCounter = TheCheckedListBox.Items.Count;
                }

                RefreshCounterText();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when updating item status.", base.FindForm());
            }

        }

        private void CopyAllMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                base.FindForm().Cursor = Cursors.WaitCursor;
                Clipboard.SetDataObject(GetListAsString(true), false);
                base.FindForm().Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to copy all items.", base.FindForm());
            }
        }

        private void CopyCheckedMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                base.FindForm().Cursor = Cursors.WaitCursor;
                Clipboard.SetDataObject(GetListAsString(false), false);
                base.FindForm().Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to copy checked items.", base.FindForm());
            }
        }

        private void SaveAllMenuItem_Click(object sender, System.EventArgs e)
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

                //Save file.
                base.FindForm().Cursor = Cursors.WaitCursor;
                fs = new FileServer();
                fs.SaveFile(GetListAsString(true), filename);
                base.FindForm().Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to save all items.", base.FindForm());
            }
        }

        private void SaveCheckedMenuItem_Click(object sender, System.EventArgs e)
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

                //Save file.
                base.FindForm().Cursor = Cursors.WaitCursor;
                fs = new FileServer();
                fs.SaveFile(GetListAsString(false), filename);
                base.FindForm().Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to save checked items.", base.FindForm());
            }
        }

        private void CheckFromFileMenuItem_Click(object sender, System.EventArgs e)
        {
            FileServer fs;
            string filename;
            string[] loadedItems;
            bool isUnknownItem;

            try
            {
                //Get file name.
                MyOpenFileDialog.ShowDialog();
                filename = MyOpenFileDialog.FileName;
                if (filename == "")
                {
                    return;
                }

                base.FindForm().Cursor = Cursors.WaitCursor;
                //Load file.
                fs = new FileServer();
                loadedItems = fs.ReadSingleColumn(filename);
                if (loadedItems.GetLength(0) < 1)
                {
                    base.FindForm().Cursor = Cursors.Default;
                    return;
                }

                //See if there are loaded items which are not
                //in the checked list box.
                for (int i = 0; i < loadedItems.GetLength(0); i++)
                {
                    isUnknownItem = true;
                    for (int j = 0; j < MyCheckedListBox.Items.Count; j++)
                    {
                        if (MyCheckedListBox.Items[j].ToString().ToUpper() == loadedItems[i].ToUpper())
                        {
                            isUnknownItem = false;
                            break; //Exit for.
                        }
                    }
                    if (isUnknownItem)
                    {
                        base.FindForm().Cursor = Cursors.Default;
                        MessageManager.ShowWarning("The item " + loadedItems[i] +
                            " in the file is not present in the list.", base.FindForm());
                        return;
                    }
                }

                //Check the loaded items.
                for (int i = 0; i < loadedItems.GetLength(0); i++)
                {
                    for (int j = 0; j < MyCheckedListBox.Items.Count; j++)
                    {
                        if (MyCheckedListBox.Items[j].ToString().ToUpper() == loadedItems[i].ToUpper())
                        {
                            MyCheckedListBox.SetItemChecked(j, true);
                            break; //Exit for.
                        }
                    }
                }
                base.FindForm().Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to check items from file.", base.FindForm());
            }

        }

    }
}
