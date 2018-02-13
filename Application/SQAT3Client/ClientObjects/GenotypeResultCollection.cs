using System;
using System.Collections;
using System.Text;


namespace Molmed.SQAT.ClientObjects
{
    public class GenotypeResultCollection : CollectionBase
    {

        public String this[int index]
        {
            get { return ((String)List[index]); }
            set { List[index] = value; }
        }

        public int Add(String key, String result)
        {
            return (List.Add(key + "¤" + result));
        }

        public int Add(String item, String experiment, String result)
        {
            return (List.Add("i" + item + "¤" + "e" + experiment + "¤" + result));
        }

        public override string ToString()
        {
            StringBuilder sb;
            string[] tempStrings;

            sb = new StringBuilder(16 * List.Count);
            if (List.Count < 1)
            {
                sb.Append("<empty>" + Environment.NewLine);
            }
            else
            {
                foreach (string s in List)
                {
                    tempStrings = s.Split(new char[] { '¤' });
                    sb.Append(tempStrings[0].Substring(1) + "\t" + tempStrings[1].Substring(1) + "\t" + tempStrings[2] + Environment.NewLine);
                }

            }
            return sb.ToString();
        }

    }
}