using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Molmed.SQAT.ClientObjects
{

	[Serializable] public class Preferences
	{
		private bool MyRecalculate = true;
        private bool MyExperimentFilter = false;

		
		public void Serialize()
		{
			IsolatedStorageFileStream fStream = null;
			BinaryFormatter formatter;

			try
			{
				fStream = new IsolatedStorageFileStream("SQATPreferences.obj", FileMode.Create);
				formatter = new BinaryFormatter();
				formatter.Serialize(fStream, this);
			}
			catch (Exception e)
			{
				throw(e);
			}
			finally
			{
				if (fStream != null)
				{
					fStream.Close();
				}
			}
		}

		public static Preferences Deserialize()
		{
			IsolatedStorageFileStream fStream = null;
			BinaryFormatter formatter;

			try
			{
				fStream = new IsolatedStorageFileStream("SQATPreferences.obj", FileMode.Open);
				formatter = new BinaryFormatter();
				return (Preferences)formatter.Deserialize(fStream);
			}
			catch (FileNotFoundException)
			{
				//Do nothing, no saved preferences.
				return null;
			}
			catch (Exception)
			{
				throw new Exception("Unable to load user preferences.");
			}
			finally
			{
				if (fStream != null)
				{
					fStream.Close();
				}
			}			

		}

		public bool Get(Bools item)
		{
			switch (item)
			{
				case Preferences.Bools.Recalculate:
					return MyRecalculate;
                case Preferences.Bools.ExperimentFilter:
                    return MyExperimentFilter;
				default:
					throw new Exception("Attempting to get unknown preference.");
			}
		}

		public void Set(Bools item, bool val)
		{
			switch (item)
			{
				case Preferences.Bools.Recalculate:
					MyRecalculate = val;
					break;
                case Preferences.Bools.ExperimentFilter:
                    MyExperimentFilter = val;
                    break;
				default:
					throw new Exception("Attempting to set unknown preference.");
			}
		}

		public enum Bools
		{
			Recalculate = 100,
            ExperimentFilter = 101
		}


	}

}
