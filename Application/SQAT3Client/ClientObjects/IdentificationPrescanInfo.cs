using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ClientObjects
{
    public class IdentificationPrescanInfo
    {
        CIStringCollection MyItems, MyExperiments;

        public CIStringCollection Items
        {
            get { return MyItems; }
        }

        public CIStringCollection Experiments
        {
            get { return MyExperiments; }
        }

        public IdentificationPrescanInfo(CIStringCollection items, CIStringCollection experiments)
        {
            MyItems = items;
            MyExperiments = experiments;
        }

        override public string ToString()
        {
            string str;

            str = "";
            str += "No. items in source: " + MyItems.Count.ToString() + Environment.NewLine;
            str += "No. experiments in source: " + MyExperiments.Count.ToString() + Environment.NewLine;

            return str;
        }
    }
}
