using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.GUI
{
    public partial class AdvancedListView2 : UserControl
    {
        private DataTable MyDataTable;
        private Form MyParentForm;
        private bool MySortOrderAsc;
        private int MySortIndex;


        public AdvancedListView2()
        {
            InitializeComponent();

            MyParentForm = base.FindForm();
        }

        public ListView TheListView
        {
            get { return MyExtListView; }
        }


        public void ShowFormattedTable(DataTable dTable)
        {
            ShowFormattedTable(dTable, null);
        }

        public void ShowFormattedTable(DataTable dTable, string sortExpression)
        {
            ListViewItem tempListViewItem;
            string columnFormatCode, columnDisplayName;
            int columnNameLength, colLimit;
            long rowLimit, rowCounter;
            System.Globalization.NumberFormatInfo numFrmter;
            IComparer tempComparer;

            rowLimit = long.MaxValue;
            //rowLimit = Convert.ToInt32(Configuration.GetFromFile("ListRowLimit"));
            colLimit = Convert.ToInt32(Configuration.GetFromFile("ListColumnLimit"));

            if (dTable.Rows.Count > rowLimit)
            {
                throw new Exception("Cannot show list. The number of rows exceeds the maximum limit " + rowLimit.ToString() + ".");
            }
            else if (dTable.Columns.Count > colLimit)
            {
                throw new Exception("Cannot show list. The number of columns exceeds the maximum limit " + colLimit.ToString() + ".");
            }

            MyDataTable = dTable;

            numFrmter = new System.Globalization.NumberFormatInfo();
            numFrmter.PercentDecimalDigits = 2;
            numFrmter.PercentDecimalSeparator = ".";
            numFrmter.NumberDecimalDigits = 2;
            numFrmter.NumberDecimalSeparator = ".";

            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.WaitCursor;
            }

            //Clear the list view.
            MyExtListView.Items.Clear();
            MyExtListView.Columns.Clear();

            MyExtListView.BeginUpdate();
            //Turn off sorting before adding new rows,
            //but remember the previous sorter.
            tempComparer = MyExtListView.ListViewItemSorter;
            MyExtListView.ListViewItemSorter = null;

            //Add the column names, all except for the ID column.
            for (int i = 1; i < dTable.Columns.Count; i++)
            {
                columnNameLength = dTable.Columns[i].Caption.Length;
                //The column format code is stored in the extended properties.
                if (dTable.Columns[i].ExtendedProperties.ContainsKey("FormatCode"))
                {
                    columnFormatCode = (string)dTable.Columns[i].ExtendedProperties["FormatCode"];
                }
                else
                {
                    columnFormatCode = "";
                }
                columnDisplayName = dTable.Columns[i].ColumnName;
                if (columnFormatCode.StartsWith("I") || columnFormatCode.StartsWith("R") || columnFormatCode.StartsWith("P"))
                {
                    MyExtListView.Columns.Add(columnDisplayName, 100, HorizontalAlignment.Right);
                }
                else
                {
                    MyExtListView.Columns.Add(columnDisplayName, 100, HorizontalAlignment.Left);
                }

            }

            //Add the rows.
            rowCounter = 0;
            foreach (DataRow tempRow in dTable.Select(null, sortExpression))
            {
                tempListViewItem = MyExtListView.Items.Add(tempRow[1].ToString());
                tempListViewItem.Tag = tempRow[0].ToString();
                tempListViewItem.UseItemStyleForSubItems = false;
                for (int i = 2; i < dTable.Columns.Count; i++)
                {
                    if (dTable.Columns[i].ExtendedProperties.ContainsKey("FormatCode"))
                    {
                        columnFormatCode = (string)dTable.Columns[i].ExtendedProperties["FormatCode"];
                    }
                    else
                    {
                        columnFormatCode = "";
                    }

                    if (tempRow.IsNull(i))
                    {
                        tempListViewItem.SubItems.Add(tempRow[i].ToString());
                    }
                    else
                    {
                        switch (columnFormatCode)
                        {
                            case "IN":
                                tempListViewItem.SubItems.Add(new AdvancedListView2.IntListViewSubItem(tempListViewItem, Convert.ToInt32(tempRow[i])));
                                break;
                            case "RN":
                                tempListViewItem.SubItems.Add(new AdvancedListView2.DoubleListViewSubItem(tempListViewItem, Convert.ToDouble(tempRow[i]), "N", numFrmter));
                                break;
                            case "PN":
                                tempListViewItem.SubItems.Add(new AdvancedListView2.DoubleListViewSubItem(tempListViewItem, Convert.ToDouble(tempRow[i]), "P", numFrmter));
                                break;
                            case "IF":
                                tempListViewItem.SubItems.Add(new AdvancedListView2.IntFailureListViewSubItem(tempListViewItem, Convert.ToInt32(tempRow[i])));
                                break;
                            case "RF":
                                tempListViewItem.SubItems.Add(new AdvancedListView2.DoubleFailureListViewSubItem(tempListViewItem, Convert.ToDouble(tempRow[i]), "N", numFrmter));
                                break;
                            case "PF":
                                tempListViewItem.SubItems.Add(new AdvancedListView2.DoubleFailureListViewSubItem(tempListViewItem, Convert.ToDouble(tempRow[i]), "P", numFrmter));
                                break;
                            case "CF":
                                tempListViewItem.SubItems.Add(new FailureListViewSubItem(tempListViewItem, tempRow[i].ToString()));
                                break;
                            default:
                                tempListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(tempListViewItem, tempRow[i].ToString()));
                                break;
                        }
                    }

                }
                rowCounter++;
                if (rowCounter >= rowLimit)
                {
                    break;
                }
            }

            //Now after we have added values, it's time to set the column header width
            //to that of the widest text in the column.
            for (int i = 0; i < MyExtListView.Columns.Count; i++)
            {
                MyExtListView.Columns[i].Width = -2;
            }

            //Turn on sorting again (if the sorting is not performed by the sortExpression).
            MyExtListView.ListViewItemSorter = (sortExpression != null ? null : tempComparer);
            MyExtListView.EndUpdate();

            //Select the first item
            if (MyExtListView.Items.Count > 0)
            {
                MyExtListView.Items[0].Selected = true;
            }
            RefreshCounterText();

            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.Default;
            }

        }


        public void ShowMatrix(string[,] matrix)
        {
            ColumnHeader[] colHeaderObjects;
            ListViewItem tempRow;
            int rowLimit, colLimit;

            rowLimit = Convert.ToInt32(Configuration.GetFromFile("ListRowLimit"));
            colLimit = Convert.ToInt32(Configuration.GetFromFile("ListColumnLimit"));

            if (matrix.GetLength(0) > rowLimit)
            {
                throw new Exception("Cannot show list. The number of rows exceeds the maximum limit " + rowLimit.ToString() + ".");
            }
            else if (matrix.GetLength(1) > colLimit)
            {
                throw new Exception("Cannot show list. The number of columns exceeds the maximum limit " + colLimit.ToString() + ".");
            }

            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.WaitCursor;
            }

            MyExtListView.BeginUpdate();

            colHeaderObjects = new ColumnHeader[matrix.GetLength(1)];

            for (int i = 0; i < colHeaderObjects.GetLength(0); i++)
            {
                colHeaderObjects[i] = new ColumnHeader();
                colHeaderObjects[i].Text = matrix[0, i];
            }
            MyExtListView.Columns.AddRange(colHeaderObjects);

            for (int j = 1; j < matrix.GetLength(0); j++)
            {
                tempRow = new ListViewItem(matrix[j, 0]);
                for (int i = 1; i < matrix.GetLength(1); i++)
                {
                    tempRow.SubItems.Add(matrix[j, i]);
                }
                MyExtListView.Items.Add(tempRow);
            }

            //Now after we have added values, it's time to set the column header width
            //to that of the widest text in the column.
            for (int i = 0; i < MyExtListView.Columns.Count; i++)
            {
                MyExtListView.Columns[i].Width = -2;
            }

            MyExtListView.EndUpdate();
            RefreshCounterText();

            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.Default;
            }
        }


        public DataTable Table
        {
            get
            {
                return MyDataTable;
            }
        }


        public string GetListAsString(bool all, bool includeHeaders)
        {
            //Inlcudes all columns.
            bool[] includeColumnIndex;

            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.WaitCursor;
            }
            includeColumnIndex = new Boolean[MyExtListView.Columns.Count];
            for (int i = 0; i < MyExtListView.Columns.Count; i++)
            {
                includeColumnIndex[i] = true;
            }
            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.Default;
            }

            return GetListAsString(all, includeHeaders, includeColumnIndex);
        }

        public string GetListAsString(bool all, bool includeHeaders, bool[] includeColumnIndex)
        {
            //includeColumnIndex must be an array of the same size as the number of columns in the list view.
            StringBuilder str;
            string tempStr;

            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.WaitCursor;
            }
            str = new StringBuilder(40 * MyExtListView.Items.Count);

            if (includeHeaders)
            {
                tempStr = "";
                for (int j = 0; j < MyExtListView.Columns.Count; j++)
                {
                    if (includeColumnIndex[j])
                    {
                        tempStr += MyExtListView.Columns[j].Text + "\t";
                    }
                }
                str.Append(tempStr.TrimEnd(null) + Environment.NewLine);
            }

            for (int i = 0; i < MyExtListView.Items.Count; i++)
            {
                if (MyExtListView.Items[i].Selected || all)
                {
                    tempStr = "";
                    for (int j = 0; j < MyExtListView.Items[i].SubItems.Count; j++)
                    {
                        if (includeColumnIndex[j])
                        {
                            if (j > 0)
                            {
                                tempStr += "\t";
                            }
                            tempStr += MyExtListView.Items[i].SubItems[j].Text;
                        }
                    }
                    str.Append(tempStr + Environment.NewLine);
                }
            }
            if (MyParentForm != null)
            {
                MyParentForm.Cursor = Cursors.Default;
            }

            return str.ToString();
        }

        public void RefreshCounterText()
        {
            //Update the item count label.
            NumberLabel.Text = TheListView.Items.Count.ToString() +
                " (" + TheListView.SelectedItems.Count.ToString() + ")";
        }

        private void ColumnClicked(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                if (e.Column == MySortIndex)
                {
                    //Flip sort order if same column.
                    MySortOrderAsc = !MySortOrderAsc;
                }
                else
                {
                    //Only remember the column if new column.
                    MySortIndex = e.Column;
                }
                MyExtListView.ListViewItemSorter = new ListViewItemComparer(e.Column, MySortOrderAsc);
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to change sort order.", MyParentForm);
            }
        }

        private void MenuButton_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                //Set menu items which cannot be selected for
                //single-select lists to false if not multi-select mode is on.
                SelectAllMenuItem.Enabled = MyExtListView.MultiSelect;
                ReverseMenuItem.Enabled = MyExtListView.MultiSelect;
                SelectFromFileMenuItem.Enabled = MyExtListView.MultiSelect;
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when opening menu.", MyParentForm);
            }
        }

        private void MyListView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                RefreshCounterText();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to update selection status.", MyParentForm);
            }

        }

        private void SelectAllMenuItem_Click(object sender, System.EventArgs e)
        {
            ListViewItem tempItem;

            try
            {
                //Exit if the list view is not in multi-select mode.
                if (!MyExtListView.MultiSelect)
                {
                    return;
                }
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                MyExtListView.BeginUpdate();
                for (int i = 0; i < MyExtListView.Items.Count; i++)
                {
                    tempItem = MyExtListView.Items[i];
                    tempItem.Selected = true;
                }
                MyExtListView.EndUpdate();
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to select all items.", MyParentForm);
            }
        }

        private void ReverseMenuItem_Click(object sender, System.EventArgs e)
        {
            ListViewItem tempItem;

            try
            {
                //Exit if the list view is not in multi-select mode.
                if (!MyExtListView.MultiSelect)
                {
                    return;
                }
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                MyExtListView.BeginUpdate();
                for (int i = 0; i < MyExtListView.Items.Count; i++)
                {
                    tempItem = MyExtListView.Items[i];
                    tempItem.Selected = !tempItem.Selected;
                }
                MyExtListView.EndUpdate();
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to reverse selection.", MyParentForm);
            }
        }

        private void SelectFromFileMenuItem_Click(object sender, System.EventArgs e)
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

                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                //Load file.
                fs = new FileServer();
                loadedItems = fs.ReadSingleColumn(filename);
                if (loadedItems.GetLength(0) < 1)
                {
                    if (MyParentForm != null)
                    {
                        MyParentForm.Cursor = Cursors.Default;
                    }
                    return;
                }

                //See if there are loaded items which are not
                //in the list view.
                for (int i = 0; i < loadedItems.GetLength(0); i++)
                {
                    isUnknownItem = true;
                    for (int j = 0; j < MyExtListView.Items.Count; j++)
                    {
                        if (MyExtListView.Items[j].Text.ToUpper() == loadedItems[i].ToUpper())
                        {
                            isUnknownItem = false;
                            break; //Exit for.
                        }
                    }
                    if (isUnknownItem)
                    {
                        if (MyParentForm != null)
                        {
                            MyParentForm.Cursor = Cursors.Default;
                        }
                        MessageManager.ShowWarning("The item " + loadedItems[i] +
                            " in the file is not present in the list.", MyParentForm);
                        return;
                    }
                }

                //Clear selection.
                for (int j = 0; j < MyExtListView.Items.Count; j++)
                {
                    ((ListViewItem)MyExtListView.Items[j]).Selected = false;
                }

                //Select the loaded items.
                for (int i = 0; i < loadedItems.GetLength(0); i++)
                {
                    for (int j = 0; j < MyExtListView.Items.Count; j++)
                    {
                        if (MyExtListView.Items[j].Text.ToUpper() == loadedItems[i].ToUpper())
                        {
                            ((ListViewItem)MyExtListView.Items[j]).Selected = true;
                            break; //Exit for.
                        }
                    }
                }
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to select items from file.", MyParentForm);
            }
        }

        private void CopyAllWithHeadersMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                Clipboard.SetDataObject(GetListAsString(true, true), false);
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to copy all items.", MyParentForm);
            }
        }

        private void CopyAllWithoutHeadersMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                Clipboard.SetDataObject(GetListAsString(true, false), false);
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to copy all items.", MyParentForm);
            }
        }

        private void CopySelectionWithHeadersMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                Clipboard.SetDataObject(GetListAsString(false, true), false);
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to copy selected items.", MyParentForm);
            }
        }

        private void CopySelectionWithoutHeadersMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                Clipboard.SetDataObject(GetListAsString(false, false), false);
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to copy selected items.", MyParentForm);
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

                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                //Save file.
                fs = new FileServer();
                fs.SaveFile(GetListAsString(true, true), filename);
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to save all items.", MyParentForm);
            }
        }

        private void SaveSelectionMenuItem_Click(object sender, System.EventArgs e)
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

                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.WaitCursor;
                }
                //Save file.
                fs = new FileServer();
                fs.SaveFile(GetListAsString(false, true), filename);
                if (MyParentForm != null)
                {
                    MyParentForm.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Unable to save selected items.", MyParentForm);
            }
        }



        private class ListViewItemComparer : System.Collections.IComparer
        {
            private int MyColumnIndex;
            private bool MySortOrderAsc;

            public ListViewItemComparer(int sortIndex, bool ascending)
            {
                MyColumnIndex = sortIndex;
                MySortOrderAsc = ascending;
            }

            public int Compare(object x, object y)
            {
                ListViewItem l1, l2;
                ListViewItem.ListViewSubItem s1, s2;
                DoubleListViewSubItem d1, d2;
                IntListViewSubItem i1, i2;
                int outcome;
                double diff;

                if ((x is ListViewItem) && (y is ListViewItem))
                {
                    l1 = (ListViewItem)x;
                    l2 = (ListViewItem)y;
                    s1 = l1.SubItems[MyColumnIndex];
                    s2 = l2.SubItems[MyColumnIndex];
                    if (s1 is DoubleListViewSubItem && s2 is DoubleListViewSubItem)
                    {
                        //Compare double values.
                        d1 = (DoubleListViewSubItem)s1;
                        d2 = (DoubleListViewSubItem)s2;
                        diff = d1.NumericValue - d2.NumericValue;
                        outcome = (int)(diff > 0 ? Math.Ceiling(diff) : Math.Floor(diff)); //e.g. 0.1 -> 1, -0.1 -> -1
                    }
                    else if (s1 is IntListViewSubItem && s2 is IntListViewSubItem)
                    {
                        //Compare integer values.
                        i1 = (IntListViewSubItem)s1;
                        i2 = (IntListViewSubItem)s2;
                        outcome = i1.NumericValue - i2.NumericValue;
                    }
                    else
                    {
                        //Compare strings.
                        outcome = String.Compare(l1.SubItems[MyColumnIndex].ToString(), l2.SubItems[MyColumnIndex].ToString());
                    }

                    outcome = (MySortOrderAsc ? outcome : -outcome);
                }
                else
                {
                    throw (new ArgumentException("Cannot compare objects in list view."));
                }
                return outcome;
            }

        }

        public class DoubleListViewSubItem : ListViewItem.ListViewSubItem
        {
            private double MyValue;

            public DoubleListViewSubItem()
                : base()
            {

            }

            public DoubleListViewSubItem(ListViewItem owner, double val, string format, NumberFormatInfo numberFrmter)
                : base(owner, val.ToString(format, numberFrmter))
            {
                MyValue = val;
            }

            public double NumericValue
            {
                get { return MyValue; }
                set { MyValue = value; }
            }
        }

        //A DoubleListViewSubItem which gets a red fore color if its numeric value is greater than zero.
        public class DoubleFailureListViewSubItem : DoubleListViewSubItem
        {
            public DoubleFailureListViewSubItem()
                : base()
            {

            }

            public DoubleFailureListViewSubItem(ListViewItem owner, double val, string format, NumberFormatInfo numberFrmter)
                : base(owner, val, format, numberFrmter)
            {
                if (base.NumericValue > 0)
                {
                    this.ForeColor = System.Drawing.Color.Red;
                }
            }

        }

        public class IntListViewSubItem : ListViewItem.ListViewSubItem
        {
            private int MyValue;

            public IntListViewSubItem()
                : base()
            {

            }

            public IntListViewSubItem(ListViewItem owner, int val)
                : base(owner, val.ToString())
            {
                MyValue = val;
            }

            public int NumericValue
            {
                get { return MyValue; }
                set { MyValue = value; }
            }
        }

        //An IntListViewSubItem which gets a red fore color if its numeric value is greater than zero.
        public class IntFailureListViewSubItem : IntListViewSubItem
        {

            public IntFailureListViewSubItem()
                : base()
            {

            }

            public IntFailureListViewSubItem(ListViewItem owner, int val)
                : base(owner, val)
            {
                if (base.NumericValue > 0)
                {
                    base.ForeColor = System.Drawing.Color.Red;
                }
            }

        }


        //A ListViewSubItem which gets a red fore color if its text equals "failed".
        public class FailureListViewSubItem : ListViewItem.ListViewSubItem
        {

            public FailureListViewSubItem()
                : base()
            {

            }

            public FailureListViewSubItem(ListViewItem owner, string text)
                : base(owner, text)
            {
                if (base.Text.ToUpper() == "FAILED")
                {
                    base.ForeColor = System.Drawing.Color.Red;
                }
            }

        }


    }
}
