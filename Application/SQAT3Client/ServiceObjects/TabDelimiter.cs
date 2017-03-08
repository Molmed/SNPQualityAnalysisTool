using System;
using System.Data;
using System.Text;

namespace Molmed.SQAT.ServiceObjects
{

	public class TabDelimiter
	{
		
		public static string GetString(DataTable table, bool includeHeaders)
		{
			//Inlcudes all columns.
			bool [] includeColumnIndex;

			includeColumnIndex = new Boolean[table.Columns.Count];
			for (int i = 0 ; i < table.Columns.Count ; i++)
			{
				includeColumnIndex[i] = true;
			}

			return GetString(table, includeHeaders, includeColumnIndex);
		}

		public static string GetString(DataTable table, bool includeHeaders, bool [] includeColumnIndex)
		{
			StringBuilder str;
			string tempStr;
			
			str = new StringBuilder(10 * table.Rows.Count * table.Columns.Count);

			if (includeHeaders)
			{
				tempStr = "";
				for (int j = 0 ; j < table.Columns.Count ; j++)
				{
					if (includeColumnIndex[j])
					{
						tempStr += table.Columns[j].ColumnName + "\t";
					}
				}
				str.Append(tempStr.TrimEnd(null) + Environment.NewLine);
			}

			for (int i = 0 ; i < table.Rows.Count ; i++)
			{
				tempStr = "";
				for (int j = 0 ; j < table.Columns.Count ; j++)
				{
					if (includeColumnIndex[j])
					{
                        if (j > 0)
                        {
                            tempStr += "\t";  //Insert tab, but not if first column.
                        }
                        tempStr += table.Rows[i][j].ToString();
					}
				}
                str.Append(tempStr + Environment.NewLine);
            }

			return str.ToString();			
		}


	}

}