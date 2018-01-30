using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Molmed.SQAT.GUI
{
    public partial class ComparisonSource : UserControl
    {
        public ComparisonSource()
        {
            InitializeComponent();
            SetFormStatus();
        }

        public void SetSessionNames(string[] names)
        {
            SortedList<string, string> sl;

            //Sort the session names by using a SortedList before displaying them.
            sl = new SortedList<string, string>();
            for (int i = 0; i < names.GetLength(0); i++)
            {
                sl.Add(names[i], "");
            }

            SessionList.Items.Clear();
            for (int i = 0; i < sl.Count; i++)
            {
                SessionList.Items.Add(sl.Keys[i]);
            }
            if (SessionList.Items.Count > 0)
            {
                SessionList.SelectedIndex = 0;
            }
        }

        public void SetReferenceSetNames(string[] names)
        {
            SortedList<string, string> sl;

            //Sort the names by using a SortedList before displaying them.
            sl = new SortedList<string, string>();
            for (int i = 0; i < names.GetLength(0); i++)
            {
                sl.Add(names[i], "");
            }

            ReferenceSetList.Items.Clear();
            for (int i = 0; i < sl.Count; i++)
            {
                ReferenceSetList.Items.Add(sl.Keys[i]);
            }
            if (ReferenceSetList.Items.Count > 0)
            {
                ReferenceSetList.SelectedIndex = 0;
            }
        }

        public void SetItemTypes(StringCollection types)
        {
            foreach (string tempType in types)
            {
                ItemTypesDropDownButton.DropDownItems.Add(tempType);
            }
            for (int i = 0; i < ItemTypesDropDownButton.DropDownItems.Count; i++)
            {
                ((ToolStripMenuItem)ItemTypesDropDownButton.DropDownItems[i]).CheckOnClick = true;
                ((ToolStripMenuItem)ItemTypesDropDownButton.DropDownItems[i]).Checked = true;
            }

        }

        public void SetGenotypeStatuses(StringCollection statuses)
        {
            foreach (string tempStatus in statuses)
            {
                GenotypeStatusesDropDownButton.DropDownItems.Add(tempStatus);
            }
            for (int i = 0; i < GenotypeStatusesDropDownButton.DropDownItems.Count; i++)
            {
                ((ToolStripMenuItem)GenotypeStatusesDropDownButton.DropDownItems[i]).CheckOnClick = true;
                ((ToolStripMenuItem)GenotypeStatusesDropDownButton.DropDownItems[i]).Checked = true;
            }

        }

        public Molmed.SQAT.ClientObjects.InvestigationSourceType GetSourceType()
        {
            if (SessionRadioButton.Checked)
            {
                return Molmed.SQAT.ClientObjects.InvestigationSourceType.Session;
            }
            else if (ReferenceSetRadioButton.Checked)
            {
                return Molmed.SQAT.ClientObjects.InvestigationSourceType.ReferenceSet;
            }
            else
            {
                return Molmed.SQAT.ClientObjects.InvestigationSourceType.File;
            }
        }

        public string GetSourceName()
        {
            if (SessionRadioButton.Checked)
            {
                return SessionList.Text;
            }
            else if (ReferenceSetRadioButton.Checked)
            {
                return ReferenceSetList.Text;
            }
            else
            {
                return FileTextBox.Text;
            }
        }

        public string GetSourceMissingValue()
        {
            return MissingValueTextBox.Text;
        }

        public StringCollection GetIncludedItemTypes()
        {
            StringCollection itemTypes;

            itemTypes = new StringCollection();
            foreach (ToolStripMenuItem m in ItemTypesDropDownButton.DropDownItems)
            {
                if (m.Checked)
                {
                    itemTypes.Add(m.Text);
                }
            }
            return itemTypes;
        }

        public StringCollection GetIncludedGenotypeStatuses()
        {
            StringCollection genotypeStatuses;

            genotypeStatuses = new StringCollection();
            foreach (ToolStripMenuItem m in GenotypeStatusesDropDownButton.DropDownItems)
            {
                if (m.Checked)
                {
                    genotypeStatuses.Add(m.Text);
                }
            }
            return genotypeStatuses;
        }

        private void SetFormStatus()
        {
            SessionList.Enabled = SessionRadioButton.Checked;
            SessionInclusionToolStrip.Enabled = SessionRadioButton.Checked;
            ReferenceSetList.Enabled = ReferenceSetRadioButton.Checked;
            FileTextBox.Enabled = FileRadioButton.Checked;
            FileBrowseButton.Enabled = FileRadioButton.Checked;
            MissingValueLabel.Enabled = FileRadioButton.Checked;
            MissingValueTextBox.Enabled = FileRadioButton.Checked;
        }

        private void SessionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetFormStatus();
        }

        private void ReferenceSetRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetFormStatus();
        }

        private void FileRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetFormStatus();
        }

        private void FileBrowseButton_Click(object sender, EventArgs e)
        {
            if (MyOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileTextBox.Text = MyOpenFileDialog.FileName;
            }
        }


    }
}
