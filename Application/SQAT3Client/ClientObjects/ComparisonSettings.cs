using System;

namespace Molmed.SQAT.ClientObjects
{

    public class ComparisonSettings
    {
        private InvestigationMode MyInvestigationMode;
        private bool MyHarmonizePolarities;
        private bool MyIsFreeTextCaseSensitive;
        private bool MyListDuplicateFailures, MyListMissing, MyListCompared, MyListIdentical;
        private bool MyListDifferent, MyListInvalid, MyListNoResults, MyListUnharmonizableSNPs;


        public InvestigationMode Mode
        {
            get { return MyInvestigationMode; }
            set { MyInvestigationMode = value; }
        }

        public bool HarmonizePolarities
        {
            get { return MyHarmonizePolarities; }
            set { MyHarmonizePolarities = value; }
        }

        public bool IsFreeTextCaseSensitive
        {
            get { return MyIsFreeTextCaseSensitive; }
            set { MyIsFreeTextCaseSensitive = value; }
        }

        public bool ListDuplicateFailures
        {
            get { return MyListDuplicateFailures; }
            set { MyListDuplicateFailures = value; }
        }

        public bool ListMissing
        {
            get { return MyListMissing; }
            set { MyListMissing = value; }
        }

        public bool ListInvalid
        {
            get { return MyListInvalid; }
            set { MyListInvalid = value; }
        }

        public bool ListCompared
        {
            get { return MyListCompared; }
            set { MyListCompared = value; }
        }

        public bool ListIdentical
        {
            get { return MyListIdentical; }
            set { MyListIdentical = value; }
        }

        public bool ListDifferent
        {
            get { return MyListDifferent; }
            set { MyListDifferent = value; }
        }

        public bool ListNoResults
        {
            get { return MyListNoResults; }
            set { MyListNoResults = value; }
        }

        public bool ListUnharmonizableSNPs
        {
            get { return MyListUnharmonizableSNPs; }
            set { MyListUnharmonizableSNPs = value; }
        }


        override public string ToString()
        {
            string str;

            str = "";
            str += "Comparison type: " + MyInvestigationMode.ToString() + Environment.NewLine;
            if (MyInvestigationMode == InvestigationMode.Genotype)
            {
                str += "Harmonize polarities: " + (MyHarmonizePolarities ? "ON" : "OFF") + Environment.NewLine;
            }
            else if (MyInvestigationMode == InvestigationMode.FreeText)
            {
                str += "Allele case sensitive: " + (MyIsFreeTextCaseSensitive ? "ON" : "OFF") + Environment.NewLine;
            }
            return str;
        }

    }

}