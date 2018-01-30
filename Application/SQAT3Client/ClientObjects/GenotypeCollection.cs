using System;
using System.Collections;
using System.Text;


namespace Molmed.SQAT.ClientObjects
{
    public class GenotypeCollection : CollectionBase
    {

        public String this[int index]
        {
            get { return ((String)List[index]); }
            set { List[index] = value; }
        }

        public int Add(String key)
        {
            return (List.Add(key));
        }

        public int Add(String item, String experiment)
        {
            return (List.Add("i" + item + "¤" + "e" + experiment));
        }

        public bool Contains(String item, String experiment)
        {
            string searchValue;

            searchValue = "i" + item + "¤" + "e" + experiment;

            foreach (string tempVal in List)
            {
                if (tempVal.ToUpper() == searchValue.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb;
            string[] tempStrings;

            sb = new StringBuilder(10 * List.Count);
            if (List.Count < 1)
            {
                sb.Append("<empty>" + Environment.NewLine);
            }
            else
            {
                foreach (string s in List)
                {
                    tempStrings = s.Split(new char[] { '¤' });
                    sb.Append(tempStrings[0].Substring(1) + "\t" + tempStrings[1].Substring(1) + Environment.NewLine);
                }
            }

            return sb.ToString();            
        }

    }


}
