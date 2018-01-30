using System;
using System.Collections.Specialized;
using System.Data;

namespace Molmed.SQAT.ClientObjects
{

    public enum InvestigationSourceType
    {
        Session = 1,
        ReferenceSet = 2,
        File = 3,
    }

    public class InvestigationSourceInfo
    {
        GenotypeCollection MyNoResults;
        GenotypeResultCollection MyInvalidAlleles;
        GenotypeDictionary MyDuplFailures, MyValidGenotypes;
        StringCollection MyGenotypeStatuses, MyItemTypes;
        int MyTotalGenotypesCount;
        string MySourceName, MyMissingValueCode;
        InvestigationSourceType MySourceType;
        Molmed.SQAT.GUI.SessionForm MySession;


        public Molmed.SQAT.GUI.SessionForm Session
        {
            get { return MySession; }
        }

        public int TotalGenotypesCount
        {
            get { return MyTotalGenotypesCount; }
        }

        public GenotypeCollection NoResults
        {
            get { return MyNoResults; }
        }

        public GenotypeDictionary DuplicateFailures
        {
            get { return MyDuplFailures; }
        }

        public GenotypeResultCollection InvalidAlleles
        {
            get { return MyInvalidAlleles; }
        }

        public StringCollection GenotypeStatuses
        {
            get { return MyGenotypeStatuses; }
        }

        public StringCollection ItemTypes
        {
            get { return MyItemTypes; }
        }

        public GenotypeDictionary ValidGenotypes
        {
            get { return MyValidGenotypes; }
        }

        public string SourceName
        {
            get { return MySourceName; }
        }

        public string MissingValueCode
        {
            get { return MyMissingValueCode; }
        }

        public InvestigationSourceType SourceType
        {
            get { return MySourceType; }
        }

        public InvestigationSourceInfo(string sourceName, string noResultCode, InvestigationSourceType sourceType, StringCollection genotypeStatuses, StringCollection itemTypes, Molmed.SQAT.GUI.SessionForm session)
        {
            MyMissingValueCode = noResultCode;
            MySourceType = sourceType;
            MySourceName = sourceName;
            MyGenotypeStatuses = genotypeStatuses;
            MyItemTypes = itemTypes;
            MySession = session;
        }

        public void Load(DataTable results, InvestigationMode cmpType)
        {
            string tempItem, tempExp, tempAlleles;
            GenotypeDictionary tempDuplFailures, tempGenotypeDict, tempDuplTestDict;
            GenotypeCollection tempNoResults;
            GenotypeResultCollection tempInvalidAlleles;

            MyTotalGenotypesCount = results.Rows.Count;

            //Initiate dictionaries and collections.
            tempGenotypeDict = new GenotypeDictionary();
            tempDuplTestDict = new GenotypeDictionary();
            tempDuplFailures = new GenotypeDictionary();

            tempInvalidAlleles = new GenotypeResultCollection();

            tempNoResults = new GenotypeCollection();

            //First perform duplicate test.
            foreach (DataRow row in results.Rows)
            {
                //Read values.
                tempItem = row["Item"].ToString();
                tempExp = row["Experiment"].ToString();
                tempAlleles = row["Alleles"].ToString();

                if (tempAlleles.ToUpper() != MyMissingValueCode.ToUpper())
                {
                    if (this.IsValidAlleleCombination(tempAlleles, cmpType))
                    {
                        if (tempDuplTestDict.Contains(tempItem, tempExp))
                        {
                            //This key already exists, check if the alleles are different.
                            if (tempDuplTestDict[tempItem, tempExp].ToUpper() != tempAlleles.ToUpper())
                            {
                                //Remember this key as having a duplicate failure.
                                if (!tempDuplFailures.Contains(tempItem, tempExp))
                                {
                                    tempDuplFailures.Add(tempItem, tempExp, "");
                                }
                            }
                        }
                        else
                        {
                            //Did not exist already.
                            tempDuplTestDict.Add(tempItem, tempExp, tempAlleles);
                        }
                    }
                }
            }

            //Now go through the values again to avoid those with duplicate failures.
            foreach (DataRow row in results.Rows)
            {
                //Read values.
                tempItem = row["Item"].ToString();
                tempExp = row["Experiment"].ToString();
                tempAlleles = row["Alleles"].ToString();

                if (tempAlleles.ToUpper() == MyMissingValueCode.ToUpper())
                {
                    //Skipped because of no result.
                    tempNoResults.Add(tempItem, tempExp);
                }
                else if (!this.IsValidAlleleCombination(tempAlleles, cmpType))
                {
                    //Skipped because invalid alleles.
                    tempInvalidAlleles.Add(tempItem, tempExp, tempAlleles);
                }
                else if (!tempDuplFailures.Contains(tempItem, tempExp) && !tempGenotypeDict.Contains(tempItem, tempExp))
                {
                    //Neither a duplicate failure nor already existing, go ahead and add.
                    tempGenotypeDict.Add(tempItem, tempExp, tempAlleles);
                }
            }

            MyValidGenotypes = tempGenotypeDict;
            MyDuplFailures = tempDuplFailures;
            MyNoResults = tempNoResults;
            MyInvalidAlleles = tempInvalidAlleles;
        }

        private bool IsValidAlleleCombination(string alleles, InvestigationMode cmpType)
        {
            if (cmpType == InvestigationMode.Zygosity || cmpType == InvestigationMode.Genotype)
            {
                //For the zygosity and genotype comparison modes only accept two characters
                //separated by a slash.
                if (alleles.Length != 3)
                {
                    return false;
                }
                if (alleles.Substring(1, 1) != "/")
                {
                    return false;
                }
            }
            if (cmpType == InvestigationMode.Genotype)
            {
                //For the genotype comparison mode, also require A, C, G or T alleles.
                if (alleles.Substring(0, 1).ToUpper() != "A" &&
                    alleles.Substring(0, 1).ToUpper() != "C" &&
                    alleles.Substring(0, 1).ToUpper() != "G" &&
                    alleles.Substring(0, 1).ToUpper() != "T")
                {
                    return false;
                }
                if (alleles.Substring(2, 1).ToUpper() != "A" &&
                    alleles.Substring(2, 1).ToUpper() != "C" &&
                    alleles.Substring(2, 1).ToUpper() != "G" &&
                    alleles.Substring(2, 1).ToUpper() != "T")
                {
                    return false;
                }                
            }

            return true;
        }

        override public string ToString()
        {
            string str;

            str = "";
            str += "Name: " + MySourceName + Environment.NewLine;
            str += "Type: " + MySourceType.ToString() + Environment.NewLine;
            str += "Missing value code: " + MyMissingValueCode + Environment.NewLine;
            str += "Total number of genotypes (after pre-scan): " + MyTotalGenotypesCount + Environment.NewLine;
            str += "No. valid unique item/experiment combinations: " + MyValidGenotypes.Count + Environment.NewLine;
            str += "No. missing value codes: " + MyNoResults.Count + Environment.NewLine;
            str += "No. invalid allele codes: " + MyInvalidAlleles.Count + Environment.NewLine;
            str += "No. item/experiment combinations with duplicate failures: " + MyDuplFailures.Count + Environment.NewLine;
            if (MySourceType == InvestigationSourceType.Session)
            {
                str += Environment.NewLine + "Included item types: " + Environment.NewLine;
                foreach (string type in MyItemTypes)
                {
                    str += "\t" + type + Environment.NewLine;
                }
                str += Environment.NewLine;
                str += "Included genotype statuses: " + Environment.NewLine;
                foreach (string status in MyGenotypeStatuses)
                {
                    str += "\t" + status + Environment.NewLine;
                }
            }

            return str;
        }

    }
}