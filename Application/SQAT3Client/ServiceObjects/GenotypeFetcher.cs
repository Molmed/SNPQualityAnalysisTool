using System.Data;


namespace Molmed.SQAT.ServiceObjects
{
	public class GenotypeFetcher : GUI.UpdateableListForm.ListFetcher
	{
		private int[] MyExperimentID;
		private DataServer.TestSuccess MyTestStatus;
		private string MyWhereClause;
		TestRetrievalMethodDelegate MyTestRetrievalMethod;
		StringRetrievalMethodDelegate MyStringRetrievalMethod;

		public delegate DataTable TestRetrievalMethodDelegate(int[] experimentID, DataServer.TestSuccess successStatus);
		public delegate DataTable StringRetrievalMethodDelegate(int[] experimentID, string whereClause);

		public GenotypeFetcher(TestRetrievalMethodDelegate retrievalMethod, int[] experimentID, DataServer.TestSuccess testStatus)
		{
			MyExperimentID = experimentID;
			MyTestStatus = testStatus;
			MyTestRetrievalMethod = retrievalMethod;
		}

		public GenotypeFetcher(StringRetrievalMethodDelegate retrievalMethod, int [] experimentID, string whereClause)
		{
			MyExperimentID = experimentID;
			MyWhereClause = whereClause;
			MyStringRetrievalMethod = retrievalMethod;
		}


		override public DataTable GetData()
		{
			if (MyTestRetrievalMethod != null)
			{
				return MyTestRetrievalMethod(MyExperimentID, MyTestStatus);
			}
			else
			{
				return MyStringRetrievalMethod(MyExperimentID, MyWhereClause);
			}
		}

	}

}