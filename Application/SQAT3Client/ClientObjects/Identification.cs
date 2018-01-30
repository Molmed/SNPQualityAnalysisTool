using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Molmed.SQAT.ClientObjects
{

    public class Identification : Investigation
    {

        private struct IdentificationResults : IComparable<IdentificationResults>
        {
            public string MyItem1, MyItem2;
            public int MyEqual, MyCompared;

            public IdentificationResults(string item1, string item2, int equal, int compared)
            {
                MyItem1 = item1;
                MyItem2 = item2;
                MyEqual = equal;
                MyCompared = compared;
            }

            public int CompareTo(IdentificationResults r)
            {
                double thisFraction, otherFraction;

                if ((MyCompared < 1) && (r.MyCompared < 1))
                {
                    return 0;
                }
                else if ((MyCompared > 0) && (r.MyCompared < 1))
                {
                    return 1;
                }
                else if ((MyCompared < 1) && (r.MyCompared > 0))
                {
                    return -1;
                }
                else
                {
                    thisFraction = (double)MyEqual / (double)MyCompared;
                    otherFraction = (double)r.MyEqual / (double)r.MyCompared;

                    if (thisFraction > otherFraction)
                    {
                        return 1;
                    }
                    else if (thisFraction < otherFraction)
                    {
                        return -1;
                    }
                    else
                    {
                        //The fractions are equal, let the number of compared items decide.
                        if (MyCompared > r.MyCompared)
                        {
                            return 1;
                        }
                        else if (MyCompared < r.MyCompared)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
        }


        InvestigationSourceInfo MySourceInfo;
        IdentificationSettings MySettings;
        IdentificationPrescanInfo MyPrescanInfo;
        List<IdentificationResults> MyResultList;
        


        public Identification(InvestigationSourceInfo sourceInfo, IdentificationSettings settings, IdentificationPrescanInfo prescanInfo)
        {
            MySourceInfo = sourceInfo;
            MySettings = settings;
            MyPrescanInfo = prescanInfo;
            MyResultList = new List<IdentificationResults>();
        }


        public void Identify()
        {
            string specificItem;
            Dictionary<string, bool> comparedItems;
            int itemCounter;

            comparedItems = new Dictionary<string, bool>();
            itemCounter = 0;


            //Perform identification.
            OnStatusChange("Comparing items");

            if (MySettings.SpecificItem.Trim() != "")
            {
                //Compare one item against the rest.
                specificItem = MySettings.SpecificItem.Trim();
                if (!MyPrescanInfo.Items.Contains(specificItem))
                {
                    throw new Exception("The specified item was not found in the data set.");
                }
                foreach (string item in MyPrescanInfo.Items)
                {
                    if (specificItem != item || MySettings.IncludeSelfComparison)
                    {
                        CompareItems(specificItem, item);
                        itemCounter++;
                        OnStatusChange("Comparing items (" + itemCounter.ToString() + "/" + MyPrescanInfo.Items.Count.ToString() + ")");
                    }
                }
            }
            else
            {
                //Compare all items against each other.
                foreach (string item1 in MyPrescanInfo.Items)
                {
                    foreach (string item2 in MyPrescanInfo.Items)
                    {
                        if (item1 != item2 || MySettings.IncludeSelfComparison)
                        {
                            if (!comparedItems.ContainsKey(item1 + "¤" + item2) && !comparedItems.ContainsKey(item2 + "¤" + item1))
                            {
                                CompareItems(item1, item2);
                                comparedItems.Add(item1 + "¤" + item2, true);
                            }
                        }
                    }
                    itemCounter++;
                    OnStatusChange("Comparing items (" + itemCounter.ToString() + "/" + MyPrescanInfo.Items.Count.ToString() + ")");
                }
            }

            OnStatusChange("Sorting results");
            MyResultList.Sort();
            MyResultList.Reverse();  //Arrange the list in descending order.
        }


        private void CompareItems(string item1, string item2)
        {
            string tempValue1, tempValue2;
            int tempCompared, tempEqual;
            IdentificationResults results;

            tempEqual = 0;
            tempCompared = 0;
            foreach (string exp in MyPrescanInfo.Experiments)
            {
                if (MySourceInfo.ValidGenotypes.Contains(item1, exp) &&
                    MySourceInfo.ValidGenotypes.Contains(item2, exp))
                {
                    tempValue1 = MySourceInfo.ValidGenotypes[item1, exp];
                    tempValue2 = MySourceInfo.ValidGenotypes[item2, exp];
                    if (IsEqual(tempValue1, tempValue2))
                    {
                        tempEqual++;
                    }
                    tempCompared++;
                }

            }
            if (tempCompared > 0)
            {
                if (((100.0 * (double)tempEqual / (double)tempCompared) >= MySettings.SimilarityCutoffPercent) && (tempCompared >= MySettings.ExperimentsCutoff))
                {
                    results = new IdentificationResults(item1, item2, tempEqual, tempCompared);
                    MyResultList.Add(results);
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

            str += "IDENTIFICATION" + Environment.NewLine;
            str += "Item 1\tItem 2\tIdentical experiments\tCompared experiments" + Environment.NewLine;
            foreach (IdentificationResults r in MyResultList)
            {
                str += r.MyItem1 + "\t" + r.MyItem2 + "\t" + r.MyEqual.ToString() + "\t" + r.MyCompared + Environment.NewLine;
            }

            str += NEW_SECTION;

            str += "PRE-SCAN INFORMATION" + Environment.NewLine;
            str += MyPrescanInfo.ToString();

            str += NEW_SECTION;

            str += "SOURCE INFORMATION" + Environment.NewLine;
            str += MySourceInfo.ToString();

            str += NEW_SECTION;

            str += "SETTINGS" + Environment.NewLine;
            str += MySettings.ToString();



            return str;
        }

        private void MyHarmonizer_StatusChanged(InvestigationStatusEventArgs status)
        {
            OnStatusChange(status.Status);
        }

    }
}