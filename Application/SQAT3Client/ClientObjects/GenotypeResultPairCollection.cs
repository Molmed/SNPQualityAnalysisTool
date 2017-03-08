using System;
using System.Collections;
using System.Text;


namespace Molmed.SQAT.ClientObjects
{
    public class GenotypeResultPairCollection : CollectionBase
    {

        public String this[int index]
        {
            get { return ((String)List[index]); }
            set { List[index] = value; }
        }

        public int Add(String key, String result1, String result2)
        {
            return (List.Add(key + "¤" + result1 + "¤" + result2));
        }

        public int Add(String item, String experiment, String result1, String result2)
        {
            return (List.Add("i" + item + "¤" + "e" + experiment + "¤" + result1 + "¤" + result2));
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
                    sb.Append(tempStrings[0].Substring(1) + "\t" + tempStrings[1].Substring(1) + "\t" + tempStrings[2] + "\t" + tempStrings[3] + Environment.NewLine);
                }
            }
            return sb.ToString();
        }

    }
}