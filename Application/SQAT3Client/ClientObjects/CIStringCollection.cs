using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;


namespace Molmed.SQAT.ClientObjects
{
    public class CIStringCollection
    {
        private System.Collections.Generic.Dictionary<string, string> MyDictionary;

        public CIStringCollection()
        {
            MyDictionary = new System.Collections.Generic.Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        public CIStringCollection(StringCollection template)
        {
            MyDictionary = new System.Collections.Generic.Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (string tempStr in template)
            {
                if (!MyDictionary.ContainsKey(tempStr))
                {
                    MyDictionary.Add(tempStr, tempStr);
                }
            }
        }

        public StringCollection StringCollection
        {
            get
            {
                StringCollection retCol;

                retCol = new StringCollection();
                foreach (string tempStr in MyDictionary.Values)
                {
                    retCol.Add(tempStr);
                }
                return retCol;
            }
        }

        public void Add(String value)
        {
            MyDictionary.Add(value, value);
        }

        public bool Contains(String value)
        {
            return MyDictionary.ContainsKey(value);
        }

        public System.Collections.Generic.IEnumerator<string> GetEnumerator()
        {
            return MyDictionary.Values.GetEnumerator();
        }

        public int Count
        {
            get { return MyDictionary.Count; }
        }

        public override string ToString()
        {
            StringBuilder sb;

            sb = new StringBuilder(10 * MyDictionary.Count);
            if (MyDictionary.Count < 1)
            {
                sb.Append("<empty>" + Environment.NewLine);
            }
            else
            {
                foreach (string tempVal in MyDictionary.Values)
                {
                    sb.Append(tempVal + Environment.NewLine);
                }
            }
            return sb.ToString();
        }

    }


}