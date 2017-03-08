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
    public partial class IdentificationSettingsForm : Form
    {
        private SessionForm[] MySessions;
        private string MyConnectionString;

        public IdentificationSettingsForm(string connectionString)
        {
            InitializeComponent();
            MyConnectionString = connectionString;
            SetFormStatus();
            SimilarityCutoffComboBox.SelectedIndex = 4;
            ExperimentsCutoffComboBox.SelectedIndex = 4;
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

                IdentificationComparisonSource.SetSessionNames(names);
            }
            if (individualTypes != null)
            {
                IdentificationComparisonSource.SetItemTypes(individualTypes);
            }
            if (genotypeStatuses != null)
            {
                IdentificationComparisonSource.SetGenotypeStatuses(genotypeStatuses);
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

            IdentificationComparisonSource.SetReferenceSetNames(names);
        }


        private void SetFormStatus()
        {
            CaseSensitiveCheckBox.Enabled = CompareFreeTextRadioButton.Checked;
            SpecificItemTextBox.Enabled = SpecificItemCheckBox.Checked;
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

        private IdentificationSettings GetSettings()
        {
            IdentificationSettings settings;

            settings = new IdentificationSettings();

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
            settings.IsFreeTextCaseSensitive = CaseSensitiveCheckBox.Checked;

            if (SpecificItemCheckBox.Checked)
            {
                settings.SpecificItem = SpecificItemTextBox.Text;
            }
            else
            {
                settings.SpecificItem = "";
            }
            settings.IncludeSelfComparison = SelfComparisonCheckBox.Checked;
            settings.SimilarityCutoffPercent = Convert.ToInt32(SimilarityCutoffComboBox.Text);
            settings.ExperimentsCutoff = Convert.ToInt32(ExperimentsCutoffComboBox.Text);

            return settings;
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

        private void SpecificItemCheckBox_CheckedChanged(object sender, EventArgs e)
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

        private void OKButton_Click(object sender, EventArgs e)
        {
            IdentificationSettings settings;
            InvestigationSourceInfo sourceInfo;
            InvestigationReportForm repForm;

            try
            {
                //Check that a source has been selected.
                if (IdentificationComparisonSource.GetSourceName().Trim() == "")
                {
                    MessageManager.ShowInformation("Please select a valid source.", this);
                    return;
                }

                settings = GetSettings();
                sourceInfo = GetSourceInfo(IdentificationComparisonSource);

                repForm = new InvestigationReportForm(MyConnectionString);
                repForm.GenerateIdentificationReport(sourceInfo, settings);
            }
            catch (Exception ex)
            {
                MessageManager.ShowError(ex, "Error when generating comparison report.", this);
            }
        }

        private void IdentificationComparisonSource_Load(object sender, EventArgs e)
        {

        }


    }
}