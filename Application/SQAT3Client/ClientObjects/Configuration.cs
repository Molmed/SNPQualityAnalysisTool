using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;

namespace Molmed.SQAT.ClientObjects
{
	[Serializable] public class Configuration
	{
		private StringDictionary MyItems;
		private const string ConfigFileName = "SQATconfig.XML";

		public Configuration()
		{
			MyItems = new StringDictionary();

//			SAVED FOR CREATING THE CONFIGURATION FILE.
//			MyItems.Add("VersionControlFlag", "true");
//			MyItems.Add("AsyncTimeout", "0");
//			MyItems.Add("SyncTimeout", "0");
//			MyItems.Add("IsolationLevel", "read committed");
//          MyItems.Add("AllowUnlockFlag", "false");
//          MyItems.Add("ExperimentBatchSize", "100");
//          MyItems.Add("ListRowLimit", "65536");
//          MyItems.Add("ListColumnLimit", "256");
        }

		public void Serialize()
		{
			DataSet dSet;
			object [] rowValues;

			dSet = Configuration.CreateSerializeDataSet();

			foreach(string tempKey in MyItems.Keys)
			{
				rowValues = new object[2];
				rowValues[0] = tempKey;
				rowValues[1] = MyItems[tempKey];
				dSet.Tables["Item"].Rows.Add(rowValues);
			}

            

			dSet.WriteXml(System.Windows.Forms.Application.StartupPath + "\\" + ConfigFileName, XmlWriteMode.IgnoreSchema);
		}
	
		public static Configuration Deserialize()
		{
			DataSet dSet;
			Configuration config;
            string configFilePath;

            configFilePath = System.Windows.Forms.Application.StartupPath + "\\" + ConfigFileName;

			if (!File.Exists(configFilePath))
			{
				throw new Exception("Unable to find configuration file " + configFilePath + ".");
			}

			config = new Configuration();
			dSet = Configuration.CreateSerializeDataSet();

			dSet.ReadXml(configFilePath, XmlReadMode.IgnoreSchema);
			foreach(DataRow tempRow in dSet.Tables["Item"].Rows)
			{
				config.MyItems.Add(tempRow["Key"].ToString(), tempRow["Value"].ToString());
			}

			return config;
		}

        public static object GetFromFile(string key)
        {
            Configuration tempConfig;

            tempConfig = Configuration.Deserialize();
            return tempConfig.Get(key);
        }

		public object Get(string key)
		{
			if (MyItems.ContainsKey(key))
			{
				return MyItems[key];
			}
			else
			{
				throw new Exception("Attempting to retrieve unknown configuration item " + key + ".");
			}
		}

		public static DataSet CreateSerializeDataSet()
		{
			DataSet dSet;
			DataTable table;

			table = new DataTable("Item");
			table.Columns.Add("Key", typeof(System.String));
			table.Columns.Add("Value", typeof(System.String));

			dSet = new DataSet("Configuration");
			dSet.Tables.Add(table);

			return dSet;
		}

	}

}