using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ClientObjects
{

    public class GenotypeDictionary
    {

        Dictionary<string, string> MyDictionary;
        Dictionary<string, List<string>> MyExperimentKeys;  //Contains the experiments as key and a list of the
                                                            //real keys as value. Used to be able to remove all
                                                            //entries from a certain experiment quickly.

        public GenotypeDictionary()
        {
            MyDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            MyExperimentKeys = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
        }

        public String GetExperiment(String key)
        {
            //Returns the string starting at the second position after the delimiter.
            string[] tempStrings;

            tempStrings = key.Split(new char[] { '¤' });

            return tempStrings[1].Substring(1);
        }

        public String GetItem(String key)
        {
            //Returns the string starting at the second position before the delimiter.
            string[] tempStrings;

            tempStrings = key.Split(new char[] { '¤' });

            return tempStrings[0].Substring(1);
        }

        public void RemoveExperiment(string experiment)
        {
            List<string> keysForExperiment;

            //Retrieve the keys for this particular experiment.
            keysForExperiment = MyExperimentKeys[experiment];
            //Remove those keys only.
            foreach (string tempKey in keysForExperiment)
            {
                MyDictionary.Remove(tempKey);
            }

        }

        public String this[String key]  
        {
            get
            {
                return MyDictionary[key];
            }
            set
            { 
                MyDictionary[key] = value; 
            }
        }

        public String this[String item, String experiment]
        {
            get { return MyDictionary["i" + item + "¤" + "e" + experiment]; }
            set { MyDictionary["i" + item + "¤" + "e" + experiment] = value; }
        }    

        public ICollection<string> Keys
        {
            get { return MyDictionary.Keys; }
        }

        public ICollection<string> Values
        {
            get { return MyDictionary.Values; }
        }

        public void Add(String key)
        {
            this.Add(key, key);
        }

        public void Add(String key, String value)
        {
            string experiment;
            List<string> dictionaryKeys;

            //First maintain the catalog of experiments and keys.
            experiment = this.GetExperiment(key);
            if (MyExperimentKeys.ContainsKey(experiment))
            {
                dictionaryKeys = MyExperimentKeys[experiment];
                dictionaryKeys.Add(key);
            }
            else
            {
                dictionaryKeys = new List<string>();
                dictionaryKeys.Add(key);
                MyExperimentKeys.Add(experiment, dictionaryKeys);
            }

            //Now add the key-value pair to the main dictionary.
            MyDictionary.Add(key, value);
        }

        public void Add(String item, String experiment, String value)
        {
            string key;

            key = "i" + item + "¤" + "e" + experiment;

            this.Add("i" + item + "¤" + "e" + experiment, value);
        }

        public bool Contains(String key)
        {
            return MyDictionary.ContainsKey(key);
        }

        public bool Contains(String item, String experiment)
        {
            return MyDictionary.ContainsKey("i" + item + "¤" + "e" + experiment);
        }

        public int Count
        {
            get { return MyDictionary.Count; }
        }

        public override string ToString()
        {
            StringBuilder sb;
            string[] tempStrings;

            sb = new StringBuilder(16 * MyDictionary.Keys.Count);
            if (MyDictionary.Keys.Count < 1)
            {
                sb.Append("<empty>" + Environment.NewLine);
            }
            else
            {
                foreach (string s in MyDictionary.Keys)
                {
                    tempStrings = s.Split(new char[] { '¤' });
                    sb.Append(tempStrings[0].Substring(1) + "\t" + tempStrings[1].Substring(1) + Environment.NewLine);
                }

            }
            return sb.ToString();
        }

    }


}