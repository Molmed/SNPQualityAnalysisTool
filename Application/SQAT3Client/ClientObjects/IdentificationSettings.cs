using System;

namespace Molmed.SQAT.ClientObjects
{

    public class IdentificationSettings
    {
        private InvestigationMode MyInvestigationMode;
        private bool MyIsFreeTextCaseSensitive;
        private bool MyIncludeSelfComparison;
        private string MySpecificItem;
        private int MySimilarityCutoffPercent;
        private int MyExperimentsCutoff;

        public InvestigationMode Mode
        {
            get { return MyInvestigationMode; }
            set { MyInvestigationMode = value; }
        }

        public bool IsFreeTextCaseSensitive
        {
            get { return MyIsFreeTextCaseSensitive; }
            set { MyIsFreeTextCaseSensitive = value; }
        }

        public string SpecificItem
        {
            get { return MySpecificItem; }
            set { MySpecificItem = value; }
        }

        public bool IncludeSelfComparison
        {
            get { return MyIncludeSelfComparison; }
            set { MyIncludeSelfComparison = value; }
        }

        public int SimilarityCutoffPercent
        {
            get { return MySimilarityCutoffPercent; }
            set { MySimilarityCutoffPercent = value; }
        }

        public int ExperimentsCutoff
        {
            get { return MyExperimentsCutoff; }
            set { MyExperimentsCutoff = value; }
        }

        override public string ToString()
        {
            string str;

            str = "";
            str += "Comparison type: " + MyInvestigationMode.ToString() + Environment.NewLine;
            if (MyInvestigationMode == InvestigationMode.FreeText)
            {
                str += "Allele case sensitive: " + (MyIsFreeTextCaseSensitive ? "ON" : "OFF") + Environment.NewLine;
            }
            if (SpecificItem.Trim() != "")
            {
                str += "Match item: " + SpecificItem;
            }
            else
            {
                str += "All-to-all comparison.";
            }
            str += Environment.NewLine;
            str += "Include self-comparison: " + (MyIncludeSelfComparison ? "ON" : "OFF") + Environment.NewLine;
            str += "Cutoff: " + MySimilarityCutoffPercent.ToString() + "% similarity for at least " + MyExperimentsCutoff.ToString() + " experiments";

            return str;
        }

    }

}