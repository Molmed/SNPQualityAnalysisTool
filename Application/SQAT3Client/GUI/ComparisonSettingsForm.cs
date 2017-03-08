using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.GUI
{
    public partial class ComparisonSettingsForm : Form
    {
        private SessionForm[] MySessions;
        private string MyConnectionString;

        public ComparisonSettingsForm(string connectionString)
        {
            InitializeComponent();
            MyConnectionString = connectionString;
            SetFormStatus();
        }


        public void InitSessions(SessionForm[] sessions, StringCollection individualTypes, StringCollection genotypeStatuses)
        {
            string[] names;

            MySessions = sessions;

            if (MySessions != null)
            {
                names = new string[MySessions.GetLength(0)];
                for (int i = 0; i < MySessions.GetLength(0); i++)
                {
                    names[i] = MySessions[i].Text;
                }

                ComparisonSource1.SetSessionNames(names);
                ComparisonSource2.SetSessionNames(names);
            }
            if (individualTypes != null)
            {
                ComparisonSource1.SetItemTypes(individualTypes);
                ComparisonSource2.SetItemTypes(individualTypes);
            }
            if (genotypeStatuses != null)
            {
                ComparisonSource1.SetGenotypeStatuses(genotypeStatuses) ;
                ComparisonSource2.SetGenotypeStatuses(genotypeStatuses);
            }
        }

        public void InitReferenceSets(StringCollection referenceSetNames)
        {
            string[] names;
            int i = 0;

            names = new string[referenceSetNames.Count];
            foreach (string tempName in referenceSetNames)
            {
                names[i++] = tempName;
            }

            ComparisonSource1.SetReferenceSetNames(names);
            ComparisonSource2.SetReferenceSetNames(names);
        }

        private void SetFormStatus()
        {
            ConvertPolarityCheckBox.Enabled = CompareGenotypesRadioButton.Checked;
            CaseSensitiveCheckBox.Enabled = CompareFreeTextRadioButton.Checked;
            ListUnharmonizableCheckBox.Enabled = CompareGenotypesRadioButton.Checked && ConvertPolarityCheckBox.Checked;

            if (!ListUnharmonizableCheckBox.Enabled)
            {
                ListUnharmonizableCheckBox.Checked = false;
            }
        }

        private ComparisonSettings GetSettings()
        {
            ComparisonSettings settings;

            settings = new ComparisonSettings();

            //Extract comparison mode setting.
            if (CompareZygosityRadioButton.Checked)
            {
                settings.Mode = InvestigationMode.Zygosity;
            }
            else if (CompareGenotypesRadioButton.Checked)
            {
                settings.Mode = InvestigationMode.Genotype;
            }
            else if (CompareFreeTextRadioButton.Checked)
            {
                settings.Mode = InvestigationMode.FreeText;
            }
            settings.HarmonizePolarities = ConvertPolarityCheckBox.Checked;
            settings.IsFreeTextCaseSensitive = CaseSensitiveCheckBox.Checked;

            //Extract list settings.
            settings.ListUnharmonizableSNPs = ListUnharmonizableCheckBox.Checked;
            settings.ListCompared = ListComparedCheckBox.Checked;
            settings.ListDifferent = ListDifferentCheckBox.Checked;
            settings.ListDuplicateFailures = ListDuplicateFailuresCheckBox.Checked;
            settings.ListIdentical = ListIdenticalCheckBox.Checked;
            settings.ListMissing = ListMissingCheckBox.Checked;
            settings.ListNoResults = ListMissingValueCheckBox.Checked;
            settings.ListInvalid = ListInvalidAllelesCheckBox.Checked;

            return settings;
        }

        private InvestigationSourceInfo GetSourceInfo(ComparisonSource source)
        {
            SessionForm session;
            string missingValueCode;
            if (source.GetSourceType() == InvestigationSourceType.Session)
            {
                session = GetSession(source.GetSourceName());
                missingValueCode = "N/A";
            }
            else if (source.GetSourceType() == InvestigationSourceType.ReferenceSet)
            {
                session = null;
                missingValueCode = "N/A";
            }
            else
            {
                session = null;
                missingValueCode = source.GetSourceMissingValue();
            }
            return new InvestigationSourceInfo(source.GetSourceName(), missingValueCode, source.GetSourceType(), source.GetIncludedGenotypeStatuses(), source.GetIncludedItemTypes(), session);
        }

        private SessionForm GetSession(string sessionName)
        {
            SessionForm selectedSession = null;

            //Get the selected session.
            for (int i = 0; i < MySessions.GetLength(0); i++)
            {
                if (MySessions[i].Text == sessionName)
                {
                    selectedSession = MySessions[i];
                    break;
                }
            }
            if (selectedSession == null)
            {
                throw new Exception("Unable to find the selected session " + sessionName + ".");
            }
            return selectedSession;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when trying to close form.", this);
            }
        }

        private void CompareZygosityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetFormStatus();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing setting.", this);
            }
        }

        private void CompareGenotypesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetFormStatus();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing setting.", this);
            }
        }

        private void CompareFreeTextRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetFormStatus();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing setting.", this);
            }
        }

        private void ConvertPolarityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetFormStatus();
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when changing setting.", this);
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            ComparisonSettings settings;
            InvestigationSourceInfo sourceInfo1, sourceInfo2;
            InvestigationReportForm repForm;
            
            try
            {
                //Check that both sources have been selected.
                if (ComparisonSource1.GetSourceName().Trim() == "" || ComparisonSource2.GetSourceName().Trim() == "")
                {
                    MessageManager.ShowInformation("Please select two sources.", this);
                    return;
                }

                settings = GetSettings();
                sourceInfo1 = GetSourceInfo(ComparisonSource1);
                sourceInfo2 = GetSourceInfo(ComparisonSource2);

                repForm = new InvestigationReportForm(MyConnectionString);
                repForm.GenerateComparisonReport(sourceInfo1, sourceInfo2, settings);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when generating comparison report.", this);
            }
        }



    }
}