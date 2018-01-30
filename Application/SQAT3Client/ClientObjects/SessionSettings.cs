using System;

namespace Molmed.SQAT.ClientObjects
{

	[Serializable] public class SessionSettings
	{
		//Declare setting variables and set default values.
		private string MyViewMode = "SAMPLE";
        private string MyExperimentMode = "MARKER";
		private bool MyHumanXChr = true, MyHWSkipChildren = false, MyDemandDuplicates = false;
		private bool MyDoInheritanceTest = true, MyDoHWTest = false;
		private double MyHWLimit = 4;

		//Declare event which fires when some change has occurred with
		//the settings.
		public event EventHandler SettingsChanged;

		public bool Get(Bools item)
		{
			switch (item)
			{
				case SessionSettings.Bools.HumanXChr:
					return MyHumanXChr;
				case SessionSettings.Bools.HWSkipChildren:
					return MyHWSkipChildren;
				case SessionSettings.Bools.DemandDuplicates:
					return MyDemandDuplicates;
				case SessionSettings.Bools.DoInheritanceTest:
					return MyDoInheritanceTest;
				case SessionSettings.Bools.DoHWTest:
					return MyDoHWTest;
				default:
					throw new Exception("Attempting to get unknown session setting.");
			}
		}

		public void Set(Bools item, bool val)
		{
			switch (item)
			{
				case SessionSettings.Bools.HumanXChr:
					if (val != MyHumanXChr) { OnSettingsChanged(); }
					MyHumanXChr = val;
					break;
				case SessionSettings.Bools.HWSkipChildren:
					if (val != MyHWSkipChildren) { OnSettingsChanged(); }
					MyHWSkipChildren = val;
					break;
				case SessionSettings.Bools.DemandDuplicates:
					if (val != MyDemandDuplicates) { OnSettingsChanged(); }
					MyDemandDuplicates = val;
					break;
				case SessionSettings.Bools.DoInheritanceTest:
					if (val != MyDoInheritanceTest) { OnSettingsChanged(); }
					MyDoInheritanceTest = val;
					break;
				case SessionSettings.Bools.DoHWTest:
					if (val != MyDoHWTest) { OnSettingsChanged(); }
					MyDoHWTest = val;
					break;
				default:
					throw new Exception("Attempting to set unknown session setting.");
			}
		}

		public double Get(Doubles item)
		{
			switch (item)
			{
				case SessionSettings.Doubles.HWLimit:
					return MyHWLimit;
				default:
					throw new Exception("Attempting to get unknown session setting.");
			}
		}

		public void Set(Doubles item, double val)
		{
			switch (item)
			{
				case SessionSettings.Doubles.HWLimit:
					if (val != MyHWLimit) { OnSettingsChanged(); }
					MyHWLimit = val;
					break;
				default:
					throw new Exception("Attempting to set unknown session setting.");
			}
		}

		public string Get(Strings item)
		{
			switch (item)
			{
				case SessionSettings.Strings.ViewMode:
					return MyViewMode;
                case SessionSettings.Strings.ExperimentMode:
                    return MyExperimentMode;
				default:
					throw new Exception("Attempting to get unknown session setting.");
			}
		}

		public void Set(Strings item, string val)
		{
			switch (item)
			{
				case SessionSettings.Strings.ViewMode:
					if (val != MyViewMode) { OnSettingsChanged(); }
					MyViewMode = val;
					break;
                case SessionSettings.Strings.ExperimentMode:
                    if (val != MyExperimentMode) { OnSettingsChanged(); }
                    MyExperimentMode = val;
                    break;
				default:
					throw new Exception("Attempting to set unknown session setting.");
			}
		}

		public string GetString(Bools item)
		{
			return BoolToString(Get(item));
		}

		public string GetString(Doubles item)
		{
			return DoubleToString(Get(item));
		}

		public string GetString(Strings item)
		{
			return Get(item);
		}

		private void OnSettingsChanged()
		{
			//Raise event if we have any subscribers.
			if (SettingsChanged != null)
			{
				SettingsChanged(this, EventArgs.Empty);
			}			
		}

		private string BoolToString(bool val)
		{
			return (val ? "1" : "0");
		}

		private string DoubleToString(double val)
		{
			return val.ToString().Replace(",", ".");
		}

		public enum Bools
		{
			HumanXChr = 100,
			HWSkipChildren = 101,
			DemandDuplicates = 102,
			DoInheritanceTest = 103,
			DoHWTest = 104
		}

		public enum Doubles
		{
			HWLimit = 200
		}

		public enum Strings
		{
			ViewMode = 300,
            ExperimentMode = 301
		}

	}

}