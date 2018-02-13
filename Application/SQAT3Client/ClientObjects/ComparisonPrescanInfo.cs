using System;
using System.Collections.Specialized;

namespace Molmed.SQAT.ClientObjects
{

    public class ComparisonPrescanInfo
    {
        CIStringCollection MyItemsSource1, MyExperimentsSource1, MyItemsSource2, MyExperimentsSource2;
        CIStringCollection MyItemsUnion, MyExperimentsUnion;
        CIStringCollection MyItemsSource1NotInSource2, MyExperimentsSource1NotInSource2;
        CIStringCollection MyItemsSource2NotInSource1, MyExperimentsSource2NotInSource1;

        public CIStringCollection ItemsSource1
        {
            get { return MyItemsSource1; }
        }

        public CIStringCollection ItemsSource2
        {
            get { return MyItemsSource2; }
        }

        public CIStringCollection ExperimentsSource1
        {
            get { return MyExperimentsSource1; }
        }

        public CIStringCollection ExperimentsSource2
        {
            get { return MyExperimentsSource2; }
        }

        public CIStringCollection ItemsUnion
        {
            get { return MyItemsUnion; }
        }

        public CIStringCollection ExperimentsUnion
        {
            get { return MyExperimentsUnion; }
        }

        public CIStringCollection ItemsSource1NotInSource2
        {
            get { return MyItemsSource1NotInSource2; }
        }

        public CIStringCollection ItemsSource2NotInSource1
        {
            get { return MyItemsSource2NotInSource1; }
        }

        public CIStringCollection ExperimentsSource1NotInSource2
        {
            get { return MyExperimentsSource1NotInSource2; }
        }

        public CIStringCollection ExperimentsSource2NotInSource1
        {
            get { return MyExperimentsSource2NotInSource1; }
        }


        public ComparisonPrescanInfo(CIStringCollection itemsSource1, CIStringCollection itemsSource2, CIStringCollection experimentsSource1, CIStringCollection experimentsSource2)
        {
            MyItemsSource1 = itemsSource1;
            MyItemsSource2 = itemsSource2;
            MyExperimentsSource1 = experimentsSource1;
            MyExperimentsSource2 = experimentsSource2;

            //Get items which are in both sources and the ones which are only in either source.
            MyItemsUnion = Union(MyItemsSource1, MyItemsSource2, out MyItemsSource1NotInSource2, out MyItemsSource2NotInSource1);

            //Get experiments which are in both sources and the ones which are only in either source.
            MyExperimentsUnion = Union(MyExperimentsSource1, MyExperimentsSource2, out MyExperimentsSource1NotInSource2, out MyExperimentsSource2NotInSource1);
        }

        private CIStringCollection Union(CIStringCollection values1, CIStringCollection values2, out CIStringCollection values1NotIn2, out CIStringCollection values2NotIn1)
        {
            //Sort out values which are both in source 1 and source 2, and also save 
            //those which are only in source 1.
            CIStringCollection valuesUnion;

            valuesUnion = new CIStringCollection();
            values1NotIn2 = new CIStringCollection();
            values2NotIn1 = new CIStringCollection();

            foreach (string tempValue in values1)
            {
                if (values2.Contains(tempValue))
                {
                    if (!valuesUnion.Contains(tempValue))
                    {
                        valuesUnion.Add(tempValue);
                    }
                }
                else
                {
                    if (!values1NotIn2.Contains(tempValue))
                    {
                        values1NotIn2.Add(tempValue);
                    }
                }
            }

            //Save values which are only in source 2.
            foreach (string tempValue in values2)
            {
                if (!values1.Contains(tempValue))
                {
                    if (!values2NotIn1.Contains(tempValue))
                    {
                        values2NotIn1.Add(tempValue);
                    }
                }
            }

            return valuesUnion;
        }

        override public string ToString()
        {
            string str;
            int totalItems1, totalItems2, totalExperiments1, totalExperiments2;

            totalItems1 = MyItemsUnion.Count + MyItemsSource1NotInSource2.Count;
            totalItems2 = MyItemsUnion.Count + MyItemsSource2NotInSource1.Count;
            totalExperiments1 = MyExperimentsUnion.Count + MyExperimentsSource1NotInSource2.Count;
            totalExperiments2 = MyExperimentsUnion.Count + MyExperimentsSource2NotInSource1.Count;


            str = "";
            str += "No. items in source 1: " + totalItems1.ToString() + Environment.NewLine;
            str += "No. items in source 2: " + totalItems2.ToString() + Environment.NewLine;
            str += "No. items in both sources: " + MyItemsUnion.Count + Environment.NewLine;
            str += "No. items in source 1 but not in source 2: " + MyItemsSource1NotInSource2.Count + Environment.NewLine;
            str += "No. items in source 2 but not in source 1: " + MyItemsSource2NotInSource1.Count + Environment.NewLine;
            str += Environment.NewLine;
            str += "No. experiments in source 1: " + totalExperiments1.ToString() + Environment.NewLine;
            str += "No. experiments in source 2: " + totalExperiments2.ToString() +Environment.NewLine;            
            str += "No. experiments in both sources: " + MyExperimentsUnion.Count + Environment.NewLine;
            str += "No. experiments in source 1 but not in source 2: " + MyExperimentsSource1NotInSource2.Count + Environment.NewLine;
            str += "No. experiments in source 2 but not in source 1: " + MyExperimentsSource2NotInSource1.Count + Environment.NewLine;

            return str;
        }

    }
}