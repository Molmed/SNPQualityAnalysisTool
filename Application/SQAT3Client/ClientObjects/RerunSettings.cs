using System;

namespace Molmed.SQAT.ClientObjects
{

	public class RerunSettings
	{
		private bool MyMissingGenotype;
		private int MyMissingGenotypePercent;
		private bool MyLowAlleleFrq;
		private int MyLowAlleleFrqPercent;
		private bool MyDuplError;

		public bool MissingGenotype
		{
			get { return MyMissingGenotype; }
			set { MyMissingGenotype = value; }
		}

		public string GetMissingGenotypeString()
		{
			return MyMissingGenotype ? "1" : "0";
		}

		public int MissingGenotypePercent
		{
			get { return MyMissingGenotypePercent; }
			set { MyMissingGenotypePercent = value; }
		}

		public bool LowAlleleFrq
		{
			get { return MyLowAlleleFrq; }
			set { MyLowAlleleFrq = value; }
		}

		public string GetLowAlleleFrqString()
		{
			return MyLowAlleleFrq ? "1" : "0";
		}

		public int LowAlleleFrqPercent
		{
			get { return MyLowAlleleFrqPercent; }
			set { MyLowAlleleFrqPercent = value; }
		}

		public bool DuplicateErrors
		{
			get { return MyDuplError; }
			set { MyDuplError = value; }
		}

		public string GetDuplicateErrorsString()
		{
			return MyDuplError ? "1" : "0";
		}
		
		override public string ToString()
		{
			string str;
			
			str = "";
			str += "SNP success rate limit: " + (MyMissingGenotype ? (100 - MyMissingGenotypePercent) + "%" : "OFF");
			str += Environment.NewLine;
			str += "Allele frequency limit: " + (MyLowAlleleFrq ? MyLowAlleleFrqPercent.ToString() + "%" : "OFF");
			str += Environment.NewLine;
			str += "Duplicate errors: " + (MyDuplError ? "ON" : "OFF");
			str += Environment.NewLine;

			return str;
		}

	}

}