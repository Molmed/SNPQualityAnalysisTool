using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ClientObjects
{

    public class ComparisonHarmonizer
    {
        CIStringCollection MyRemovedExpTooManyAlleles1, MyRemovedExpTooManyAlleles2;
        CIStringCollection MyRemovedExpMonomorphicInBoth, MyRemovedExpIncompatibleSNPTypes;
        CIStringCollection MyRemovedExpATCG;

        public event Investigation.InvestigationStatusChangeHandler MyStatusChangeHandler;

        public void HarmonizePolarities(GenotypeDictionary dict1, GenotypeDictionary dict2, out GenotypeDictionary harmonizedDict1, out GenotypeDictionary harmonizedDict2)
        {
            //Harmonizes dict2 after dict1 and removes all entries from A/T or G/C experiments and incompatible
            //SNP types and items from SNPs which are monomorphic in both source.
            string tempExp, tempAlleles1, tempAlleles2;
            CIStringCollection uniqueExps; 
            Dictionary<string,string> allelesDict1, allelesDict2;
            int counter;

            //Find all experiments and their SNP types (the experiment names should be the same in both dictionaries, use dict1 here).
            uniqueExps = new CIStringCollection();
            allelesDict1 = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            allelesDict2 = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            OnStatusChange("Extracting experiments and SNP types");
            counter = 0;
            foreach (string tempKey in dict1.Keys)
            {
                tempExp = dict1.GetExperiment(tempKey);
                tempAlleles1 = dict1[tempKey].ToUpper();
                tempAlleles2 = dict2[tempKey].ToUpper();
                if (!uniqueExps.Contains(tempExp))
                {
                    //This experiment has not been processed before.
                    uniqueExps.Add(tempExp);
                    //Get the unique alleles from dictionary 1.
                    if (tempAlleles1.Substring(0, 1) != tempAlleles1.Substring(2, 1))
                    {
                        //The alleles are different, add both.
                        allelesDict1.Add(tempExp, tempAlleles1.Substring(0, 1) + tempAlleles1.Substring(2, 1));
                    }
                    else
                    {
                        //The alleles are the same, add only the first.
                        allelesDict1.Add(tempExp, tempAlleles1.Substring(0, 1));
                    }
                    //Get the unique alleles from dictionary 2.
                    if (tempAlleles2.Substring(0, 1) != tempAlleles2.Substring(2, 1))
                    {
                        //The alleles are different, add both.
                        allelesDict2.Add(tempExp, tempAlleles2.Substring(0, 1) + tempAlleles2.Substring(2, 1));
                    }
                    else
                    {
                        //The alleles are the same, add only the first.
                        allelesDict2.Add(tempExp, tempAlleles2.Substring(0, 1));
                    }
                }
                else
                {
                    //This experiment has been processed before.
                    //Add the first allele from dictionary one if it does not already exist in the string.
                    if (!allelesDict1[tempExp].Contains(tempAlleles1.Substring(0, 1)))
                    {
                        allelesDict1[tempExp] = allelesDict1[tempExp] + tempAlleles1.Substring(0, 1);
                    }
                    //Add the second allele from dictionary one if it does not already exist in the string.
                    if (!allelesDict1[tempExp].Contains(tempAlleles1.Substring(2, 1)))
                    {
                        allelesDict1[tempExp] = allelesDict1[tempExp] + tempAlleles1.Substring(2, 1);
                    }

                    //Add the first allele from dictionary two if it does not already exist in the string.
                    if (!allelesDict2[tempExp].Contains(tempAlleles2.Substring(0, 1)))
                    {
                        allelesDict2[tempExp] = allelesDict2[tempExp] + tempAlleles2.Substring(0, 1);
                    }
                    //Add the second allele from dictionary one if it does not already exist in the string.
                    if (!allelesDict2[tempExp].Contains(tempAlleles2.Substring(2, 1)))
                    {
                        allelesDict2[tempExp] = allelesDict2[tempExp] + tempAlleles2.Substring(2, 1);
                    }
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Extracting experiments and SNP types (processed genotype " + counter + ")");
                }
            }

            //Remove data from SNPs with more than two alleles in source 1 from both dictionaries and save.
            MyRemovedExpTooManyAlleles1 = new CIStringCollection();
            OnStatusChange("Extracting harmonizable data step 1/6");
            counter = 0;
            foreach (string exp in allelesDict1.Keys)
            {
                if (allelesDict1[exp].Length > 2)
                {
                    dict1.RemoveExperiment(exp);
                    dict2.RemoveExperiment(exp);
                    MyRemovedExpTooManyAlleles1.Add(exp);
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Extracting harmonizable data step 1/6 (processed experiment " + counter + ")");
                }
            }

            //Remove data from SNPs with more than two alleles in source 2 from both dictionaries and save.
            MyRemovedExpTooManyAlleles2 = new CIStringCollection();
            OnStatusChange("Extracting harmonizable data step 2/6");
            counter = 0;
            foreach (string exp in allelesDict2.Keys)
            {
                if (allelesDict2[exp].Length > 2)
                {
                    dict1.RemoveExperiment(exp);
                    dict2.RemoveExperiment(exp);
                    MyRemovedExpTooManyAlleles2.Add(exp);
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Extracting harmonizable data step 2/6 (processed experiment " + counter + ")");
                }
            }

            //Remove data from SNPs which are monomorphic in both sources and save.
            MyRemovedExpMonomorphicInBoth = new CIStringCollection();
            OnStatusChange("Extracting harmonizable data step 3/6");
            counter = 0;
            foreach (string exp in allelesDict1.Keys)
            {
                if (allelesDict1[exp].Length == 1 && allelesDict2[exp].Length == 1)
                {
                    dict1.RemoveExperiment(exp);
                    dict2.RemoveExperiment(exp);
                    MyRemovedExpMonomorphicInBoth.Add(exp);
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Extracting harmonizable data step 3/6 (processed experiment " + counter + ")");
                }
            }

            //Remove data from SNPs with incompatible SNP types from both dictionaries and save.
            MyRemovedExpIncompatibleSNPTypes = new CIStringCollection();
            OnStatusChange("Extracting harmonizable data step 4/6");
            counter = 0;
            foreach (string exp in allelesDict1.Keys)
            {
                if (!MyRemovedExpTooManyAlleles1.Contains(exp) && !MyRemovedExpTooManyAlleles2.Contains(exp)
                    && !MyRemovedExpMonomorphicInBoth.Contains(exp))
                {
                    if (!this.IsCompatibleSNPTypes(allelesDict1[exp], allelesDict2[exp]))
                    {
                        dict1.RemoveExperiment(exp);
                        dict2.RemoveExperiment(exp);
                        MyRemovedExpIncompatibleSNPTypes.Add(exp);
                    }
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Extracting harmonizable data step 4/6 (processed experiment " + counter + ")");
                }
            }

            //Remove data from SNPs with A/T or G/C SNPs from both dictionaries and save.
            MyRemovedExpATCG = new CIStringCollection();
            OnStatusChange("Extracting harmonizable data step 5/6");
            counter = 0;
            foreach (string exp in allelesDict1.Keys)
            {
                if (!MyRemovedExpTooManyAlleles1.Contains(exp) && !MyRemovedExpTooManyAlleles2.Contains(exp)
                    && !MyRemovedExpMonomorphicInBoth.Contains(exp) && !MyRemovedExpIncompatibleSNPTypes.Contains(exp))
                {
                    if (allelesDict1[exp].Length > 1)
                    {
                        if (this.GetNumberOfATalleles(allelesDict1[exp]) != 1)
                        {
                            //Either 0 or 2 A or T alleles.
                            dict1.RemoveExperiment(exp);
                            dict2.RemoveExperiment(exp);
                            MyRemovedExpATCG.Add(exp);
                        }
                    }
                    else
                    {
                        if (this.GetNumberOfATalleles(allelesDict2[exp]) != 1)
                        {
                            dict1.RemoveExperiment(exp);
                            dict2.RemoveExperiment(exp);
                            MyRemovedExpATCG.Add(exp);
                        }
                    }

                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Extracting harmonizable data step 5/6 (processed experiment " + counter + ")");
                }
            }

            //Store only data from experiments passing all tests.
            harmonizedDict1 = new GenotypeDictionary();
            harmonizedDict2 = new GenotypeDictionary();
            OnStatusChange("Extracting harmonizable data step 6/6");
            counter = 0;
            foreach (string tempKey in dict1.Keys)
            {
                tempExp = dict1.GetExperiment(tempKey);
                if (!MyRemovedExpTooManyAlleles1.Contains(tempExp) &&
                    !MyRemovedExpTooManyAlleles2.Contains(tempExp) &&
                    !MyRemovedExpMonomorphicInBoth.Contains(tempExp) &&
                    !MyRemovedExpIncompatibleSNPTypes.Contains(tempExp) &&
                    !MyRemovedExpATCG.Contains(tempExp))
                {
                    harmonizedDict1.Add(tempKey, dict1[tempKey]);
                    harmonizedDict2.Add(tempKey, dict2[tempKey]);
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Extracting harmonizable data step 6/6 (processed genotype " + counter + ")");
                }
            }


            //Go through dict1. If the SNP type for the current row is different than
            //the SNP type in dict2, convert the row in dict2.
            OnStatusChange("Harmonizing");
            counter = 0;
            foreach (string tempKey in harmonizedDict1.Keys)
            {
                tempExp = harmonizedDict1.GetExperiment(tempKey);
                tempAlleles1 = allelesDict1[tempExp];
                tempAlleles2 = allelesDict2[tempExp];
                if (tempAlleles1.Length == 2 && tempAlleles2.Length == 2)
                {
                    if ((
                        (tempAlleles1.Substring(0, 1) == tempAlleles2.Substring(0, 1)) &&
                        (tempAlleles1.Substring(1, 1) == tempAlleles2.Substring(1, 1))
                        ) ||
                        (
                        (tempAlleles1.Substring(0, 1) == tempAlleles2.Substring(1, 1)) &&
                        (tempAlleles1.Substring(1, 1) == tempAlleles2.Substring(0, 1))
                        )
                        )
                    {
                        //They are the same.
                    }
                    else
                    {
                        harmonizedDict2[tempKey] = this.ReverseComplement(harmonizedDict2[tempKey]);
                    }
                }
                else if (tempAlleles1.Length == 1 && tempAlleles2.Length == 2)
                {
                    if (tempAlleles1 != tempAlleles2.Substring(0, 1) && tempAlleles1 != tempAlleles2.Substring(1, 1))
                    {
                        harmonizedDict2[tempKey] = this.ReverseComplement(harmonizedDict2[tempKey]);
                    }
                }
                else if (tempAlleles1.Length == 2 && tempAlleles2.Length == 1)
                {
                    if (tempAlleles2 != tempAlleles1.Substring(0, 1) && tempAlleles2 != tempAlleles1.Substring(1, 1))
                    {
                        harmonizedDict2[tempKey] = this.ReverseComplement(harmonizedDict2[tempKey]);
                    }
                }
                else
                {
                    throw new Exception("Inappropriate number of alleles detected when converting SNP types");
                }
                counter++;
                if (counter % 100 == 0)
                {
                    OnStatusChange("Harmonizing (processed genotype " + counter + ")");
                }
            }

        }

        private void OnStatusChange(string status)
        {
            if (MyStatusChangeHandler != null)
            {
                MyStatusChangeHandler(new InvestigationStatusEventArgs(status));
            }
        }

        public string GetCountInformation()
        {
            string str;

            str = "";
            str += "No. experiments with too many alleles in source 1: " + MyRemovedExpTooManyAlleles1.Count;
            str += Environment.NewLine;

            str += "No. experiments with too many alleles in source 2: " + MyRemovedExpTooManyAlleles2.Count;
            str += Environment.NewLine;

            str += "No. experiments monomorphic in both sources: " + MyRemovedExpMonomorphicInBoth.Count;
            str += Environment.NewLine;

            str += "No. experiments with incompatible SNP types: " + MyRemovedExpIncompatibleSNPTypes.Count;
            str += Environment.NewLine;

            str += "No. experiments with A/T and C/G SNP types: " + MyRemovedExpATCG.Count;
            str += Environment.NewLine;

            return str;
        }

        private string ReverseComplement(string result)
        {

            switch (result.ToUpper())
            {
                case "A/A":
                    return "T/T";
                case "A/C":
                    return "T/G";
                case "A/G":
                    return "T/C";
                case "A/T":
                    return "T/A";

                case "C/C":
                    return "G/G";
                case "C/G":
                    return "G/C";
                case "C/T":
                    return "G/A";

                case "G/G":
                    return "C/C";
                case "G/T":
                    return "C/A";

                case "T/T":
                    return "A/A";

                case "T/G":
                    return "C/A";

                case "G/C":
                    return "G/C";
                case "T/C":
                    return "G/A";

                case "C/A":
                    return "T/G";
                case "G/A":
                    return "T/C";
                case "T/A":
                    return "T/A";

                default:
                    throw new Exception("Invalid result " + result + " when doing reverse complement.");

            }

        }

        private bool IsCompatibleSNPTypes(string alleles1, string alleles2)
        {
            string type1, type2, type2RevComp;

            if (alleles1.Length == 2 && alleles2.Length == 2)
            {
                type1 = alleles1.Substring(0,1).ToUpper() + "/" + alleles1.Substring(1,1).ToUpper();
                type2 = alleles2.Substring(0,1).ToUpper() + "/" + alleles2.Substring(1,1).ToUpper();
                type2RevComp = this.ReverseComplement(type2).ToUpper();

                return (type2.Contains(type1.Substring(0, 1)) && type2.Contains(type1.Substring(2, 1))) ||
                    (type2RevComp.Contains(type1.Substring(0, 1)) && type2RevComp.Contains(type1.Substring(2, 1)));
            }
            else if (alleles1.Length == 1 && alleles2.Length == 2)
            {
                type2 = alleles2.Substring(0,1) + "/" + alleles2.Substring(1,1);
                return (type2.ToUpper().Contains(alleles1.ToUpper()) || (this.ReverseComplement(type2).ToUpper().Contains(alleles1.ToUpper())));   
            }
            else if (alleles2.Length == 1 && alleles1.Length == 2)
            {
                type1 = alleles1.Substring(0, 1) + "/" + alleles1.Substring(1, 1);
                return (type1.ToUpper().Contains(alleles2.ToUpper()) || (this.ReverseComplement(type1).ToUpper().Contains(alleles2.ToUpper()))); 
            }
            else
            {
                throw new Exception("Inappropriate number of alleles when detecting incompatible SNP types");
            }
        }

        private int GetNumberOfATalleles(string genotype)
        {
            int counter = 0;

            for (int i = 0; i < genotype.Length; i++)
            {
                if (genotype.Substring(i, 1).ToUpper() == "A" || genotype.Substring(i, 1).ToUpper() == "T")
                {
                    counter++;
                }
            }

            return counter;
        }


        override public string ToString()
        {
            string str;

            str = "";
            str += "Experiments with too many alleles in source 1" + Environment.NewLine;
            str += MyRemovedExpTooManyAlleles1.ToString() + Environment.NewLine + Environment.NewLine;

            str += "Experiments with too many alleles in source 2" + Environment.NewLine;
            str += MyRemovedExpTooManyAlleles2.ToString() + Environment.NewLine + Environment.NewLine;

            str += "Experiments monomorphic in both sources" + Environment.NewLine;
            str += MyRemovedExpMonomorphicInBoth.ToString() + Environment.NewLine + Environment.NewLine;

            str += "Experiments with incompatible SNP types" + Environment.NewLine;
            str += MyRemovedExpIncompatibleSNPTypes.ToString() + Environment.NewLine + Environment.NewLine;

            str += "Experiments with A/T and C/G SNP types" + Environment.NewLine;
            str += MyRemovedExpATCG.ToString() + Environment.NewLine;

            return str;
        }

    }
}