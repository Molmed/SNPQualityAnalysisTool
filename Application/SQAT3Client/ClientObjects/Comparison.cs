using System;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;

namespace Molmed.SQAT.ClientObjects
{

    public class Comparison : Investigation
    {

        GenotypeDictionary MyMissingIn1, MyMissingIn2;
        GenotypeResultPairCollection MyCompared, MyDifferent, MyIdentical;
        InvestigationSourceInfo MySourceInfo1, MySourceInfo2;
        ComparisonSettings MySettings;
        ComparisonPrescanInfo MyPrescanInfo;
        ComparisonHarmonizer MyHarmonizer;


        public Comparison(InvestigationSourceInfo sourceInfo1, InvestigationSourceInfo sourceInfo2, ComparisonSettings settings, ComparisonPrescanInfo prescanInfo)
        {
            MySourceInfo1 = sourceInfo1;
            MySourceInfo2 = sourceInfo2;
            MySettings = settings;
            MyPrescanInfo = prescanInfo;
        }


        public void Compare()
        {
            GenotypeDictionary unionDict1, unionDict2;
            GenotypeDictionary harmonizedDict1, harmonizedDict2;
            string tempValue1, tempValue2;
            int counter;

            MyMissingIn1 = new GenotypeDictionary();
            MyMissingIn2 = new GenotypeDictionary();

            MyCompared = new GenotypeResultPairCollection();
            MyIdentical = new GenotypeResultPairCollection();
            MyDifferent = new GenotypeResultPairCollection();

            unionDict1 = new GenotypeDictionary();
            unionDict2 = new GenotypeDictionary();

            MyHarmonizer = new ComparisonHarmonizer();

            //Sort out keys from results1 which are also in results2.
            counter = 0;
            OnStatusChange("Find common genotypes, step 1");
            foreach (string key1 in MySourceInfo1.ValidGenotypes.Keys)
            {
                if (MySourceInfo2.ValidGenotypes.Contains(key1))
                {
                    unionDict1.Add(key1, MySourceInfo1.ValidGenotypes[key1]);
                }
                else
                {
                    MyMissingIn2.Add(key1);
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Find common genotypes, step 1 (processed genotype " + counter + ")");
                }
            }

            //Sort out keys from results2 which are also in results1.
            counter = 0;
            OnStatusChange("Find common genotypes, step 2");
            foreach (string key2 in MySourceInfo2.ValidGenotypes.Keys)
            {
                if (MySourceInfo1.ValidGenotypes.Contains(key2))
                {
                    unionDict2.Add(key2, MySourceInfo2.ValidGenotypes[key2]);
                }
                else
                {
                    MyMissingIn1.Add(key2);
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Find common genotypes, step 2 (processed genotype " + counter + ")");
                }
            }

            //Make sure the two union dictionaries are of the same length.
            if (unionDict1.Count != unionDict2.Count)
            {
                throw new Exception("Comparison error. Lists are of unequal length.");
            }

            //Harmonize polarities if the settings say so.
            if (MySettings.Mode == InvestigationMode.Genotype && MySettings.HarmonizePolarities)
            {
                OnStatusChange("Harmonizing polarities");
                MyHarmonizer.MyStatusChangeHandler += new InvestigationStatusChangeHandler(MyHarmonizer_StatusChanged);
                MyHarmonizer.HarmonizePolarities(unionDict1, unionDict2, out harmonizedDict1, out harmonizedDict2);
                unionDict1 = harmonizedDict1;
                unionDict2 = harmonizedDict2;
            }

            //Perform comparison.
            counter = 0;
            OnStatusChange("Comparing genotypes");
            foreach (string key in unionDict1.Keys)
            {
                tempValue1 = unionDict1[key];
                tempValue2 = unionDict2[key];

                if (this.IsEqual(tempValue1, tempValue2))
                {
                    MyIdentical.Add(key, MySourceInfo1.ValidGenotypes[key], MySourceInfo2.ValidGenotypes[key]);
                }
                else
                {
                    MyDifferent.Add(key, MySourceInfo1.ValidGenotypes[key], MySourceInfo2.ValidGenotypes[key]);
                }
                MyCompared.Add(key, MySourceInfo1.ValidGenotypes[key], MySourceInfo2.ValidGenotypes[key]);
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Comparing genotypes (processed genotype " + counter + ")");
                }
            }
        }

        private bool IsEqual(string value1, string value2)
        {
            if (MySettings.Mode == InvestigationMode.Zygosity)
            {
                return ((IsHomozygote(value1) && IsHomozygote(value2)) || (!IsHomozygote(value1) && !IsHomozygote(value2)));
            }
            else if (MySettings.Mode == InvestigationMode.FreeText)
            {
                if (MySettings.IsFreeTextCaseSensitive)
                {
                    return (value1 == value2);
                }
                else
                {
                    return (value1.ToUpper() == value2.ToUpper());
                }
            }
            else if (MySettings.Mode == InvestigationMode.Genotype)
            {
                return (
                        (
                        value1.Substring(0, 1).ToUpper() == value2.Substring(0, 1).ToUpper() &&
                        value1.Substring(2, 1).ToUpper() == value2.Substring(2, 1).ToUpper()
                        ) ||
                        (
                        value1.Substring(2, 1).ToUpper() == value2.Substring(0, 1).ToUpper() &&
                        value1.Substring(0, 1).ToUpper() == value2.Substring(2, 1).ToUpper()
                        )
                        );
            }
            else
            {
                throw new Exception("Unknown comparison type.");
            }
        }

        private bool IsHomozygote(string genotype)
        {
            return (genotype.Substring(0, 1).ToUpper() == genotype.Substring(2, 1).ToUpper());
        }

        override public string ToString()
        {
            string str;
            double difference;
            string appVersion, appName;
            string NEW_SECTION = Environment.NewLine + Environment.NewLine;

            appVersion = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Revision;

            appName = Assembly.GetExecutingAssembly().GetName().Name;
            str = "";

            str += "Generated using " + appName + " " + appVersion + Environment.NewLine;
            str += Environment.NewLine;

            str += "COMPARISON" + Environment.NewLine;
            str += "No. compared genotypes: " + MyCompared.Count.ToString() + Environment.NewLine;
            str += "No. identical genotypes: " + MyIdentical.Count.ToString() + Environment.NewLine;
            str += "No. different genotypes: " + MyDifferent.Count.ToString() + Environment.NewLine;
            if (MyCompared.Count > 0)
            {
                difference = Convert.ToDouble(MyDifferent.Count) / Convert.ToDouble(MyCompared.Count);
                str += "Difference: " + difference.ToString("P") + Environment.NewLine;
            }
            else
            {
                str += "Difference: -" + Environment.NewLine; 
            }

            str += NEW_SECTION;

            str += "PRE-SCAN INFORMATION" + Environment.NewLine;
            str += MyPrescanInfo.ToString();

            str += NEW_SECTION;

            str += "SOURCE 1 INFORMATION" + Environment.NewLine;
            str += MySourceInfo1.ToString();

            str += NEW_SECTION;

            str += "SOURCE 2 INFORMATION" + Environment.NewLine;
            str += MySourceInfo2.ToString();

            str += NEW_SECTION;

            str += "MISSING COMBINATIONS" + Environment.NewLine;
            str += "No. genotypes in source 1 but not in source 2 (after pre-scan): " + MyMissingIn2.Count.ToString() + Environment.NewLine;
            str += "No. genotypes in source 2 but not in source 1 (after pre-scan): " + MyMissingIn1.Count.ToString() + Environment.NewLine;

            str += NEW_SECTION;

            str += "SETTINGS" + Environment.NewLine;
            str += MySettings.ToString();

            if (MySettings.Mode == InvestigationMode.Genotype && MySettings.HarmonizePolarities)
            {
                str += NEW_SECTION;
                str += "POLARITY HARMONIZATION" + Environment.NewLine;
                str += MyHarmonizer.GetCountInformation();
            }

            if (MySettings.ListDifferent || MySettings.ListIdentical || MySettings.ListCompared || MySettings.ListMissing
                || MySettings.ListInvalid || MySettings.ListNoResults || MySettings.ListDuplicateFailures || MySettings.ListUnharmonizableSNPs)
            {
                str += NEW_SECTION;
                str += "LISTINGS";
            }

            if (MySettings.ListDifferent)
            {
                str += Environment.NewLine;
                str += "Different genotypes" + Environment.NewLine;
                str += MyDifferent.ToString() + Environment.NewLine;
            }
            if (MySettings.ListIdentical)
            {
                str += Environment.NewLine;
                str += "Identical genotypes" + Environment.NewLine;
                str += MyIdentical.ToString() + Environment.NewLine;
            }
            if (MySettings.ListCompared)
            {
                str += Environment.NewLine;
                str += "Compared genotypes" + Environment.NewLine;
                str += MyCompared.ToString() + Environment.NewLine;
            }
            if (MySettings.ListMissing)
            {
                str += Environment.NewLine;
                str += "Items in source 1 but not in source 2" + Environment.NewLine;
                str += MyPrescanInfo.ItemsSource1NotInSource2.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Items in source 2 but not in source 1" + Environment.NewLine;
                str += MyPrescanInfo.ItemsSource2NotInSource1.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Experiments in source 1 but not in source 2" + Environment.NewLine;
                str += MyPrescanInfo.ExperimentsSource1NotInSource2.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Experiments in source 2 but not in source 1" + Environment.NewLine;
                str += MyPrescanInfo.ExperimentsSource2NotInSource1.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Genotypes in source 1 but not in source 2 (after pre-scan)" + Environment.NewLine;
                str += MyMissingIn2.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Genotypes in source 2 but not in source 1 (after pre-scan)" + Environment.NewLine;
                str += MyMissingIn1.ToString() + Environment.NewLine;
            }
            if (MySettings.ListInvalid)
            {
                str += Environment.NewLine;
                str += "Invalid allele codes in source 1" + Environment.NewLine;
                str += MySourceInfo1.InvalidAlleles.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Invalid allele codes in source 2" + Environment.NewLine;
                str += MySourceInfo2.InvalidAlleles.ToString() + Environment.NewLine;
            }
            if (MySettings.ListNoResults)
            {
                str += Environment.NewLine;
                str += "Missing value codes in source 1" + Environment.NewLine;
                str += MySourceInfo1.NoResults.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Missing value codes in source 2" + Environment.NewLine;
                str += MySourceInfo2.NoResults.ToString() + Environment.NewLine;
            }
            if (MySettings.ListDuplicateFailures)
            {
                str += Environment.NewLine;
                str += "Duplicate failures in source 1" + Environment.NewLine;
                str += MySourceInfo1.DuplicateFailures.ToString() + Environment.NewLine;
                str += Environment.NewLine;
                str += "Duplicate failures in source 2" + Environment.NewLine;
                str += MySourceInfo2.DuplicateFailures.ToString() + Environment.NewLine;
            }
            if (MySettings.ListUnharmonizableSNPs)
            {
                str += Environment.NewLine;
                str += MyHarmonizer.ToString();
            }

            return str;
        }

        private void MyHarmonizer_StatusChanged(InvestigationStatusEventArgs status)
        {
            OnStatusChange(status.Status);
        }

    }
}