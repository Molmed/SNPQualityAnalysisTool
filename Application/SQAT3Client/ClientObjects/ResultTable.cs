using System;
using System.Data;
using System.Text;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.ClientObjects
{
    public class ResultTable
    {
        private string[,] MyResult;  //The matrix with the actual results, the dimensions depending on the mode.        

        public enum Mode
        {
            ExperimentByItem = 0,
            ItemByExperiment = 1
        }

        public ResultTable(DataServer dServer, ResultTable.Mode mode, string emptyResultString)
        {
            DataTable experimentTable, itemTable, alleleTable;
            int itemIndex = 1, experimentIndex = 1;
            string currentItem, currentExperiment, previousItem, previousExperiment;

            //Get experiments and items.
            experimentTable = dServer.GetResultExperiments();
            itemTable = dServer.GetResultItems();

            //Set up matrix.
            if (mode == ResultTable.Mode.ItemByExperiment)
            {
                alleleTable = dServer.GetApprovedResultFormattedOuter(true, emptyResultString);

                MyResult = new string[itemTable.Rows.Count + 1, experimentTable.Rows.Count + 1];

                for (int i = 0; i < experimentTable.Rows.Count; i++)
                {
                    MyResult[0, i + 1] = experimentTable.Rows[i][1].ToString();
                }

                for (int i = 0; i < itemTable.Rows.Count; i++)
                {
                    MyResult[i + 1, 0] = itemTable.Rows[i][1].ToString();
                }

                previousItem = alleleTable.Rows[0][1].ToString();
                previousExperiment = alleleTable.Rows[0][2].ToString();

                for (int i = 0; i < alleleTable.Rows.Count; i++)
                {
                    currentItem = alleleTable.Rows[i][1].ToString();
                    currentExperiment = alleleTable.Rows[i][2].ToString();
                    if (currentItem != previousItem)
                    {
                        previousItem = currentItem;
                        itemIndex++;
                        if (experimentTable.Rows.Count < 2)
                        {
                            //Will never change experiment, set to 1.
                            experimentIndex = 1;
                        }
                        else
                        {
                            //Will change experiment also and thus be increased to 1.
                            experimentIndex = 0;
                        }
                    }
                    if (currentExperiment != previousExperiment)
                    {
                        previousExperiment = currentExperiment;
                        experimentIndex++;
                    }
                    MyResult[itemIndex, experimentIndex] = alleleTable.Rows[i][3].ToString();

                }
            }
            else
            {
                alleleTable = dServer.GetApprovedResultFormattedOuter(false, emptyResultString);

                MyResult = new string[experimentTable.Rows.Count + 1, itemTable.Rows.Count + 1];

                for (int i = 0; i < experimentTable.Rows.Count; i++)
                {
                    MyResult[i + 1, 0] = experimentTable.Rows[i][1].ToString();
                }

                for (int i = 0; i < itemTable.Rows.Count; i++)
                {
                    MyResult[0, i + 1] = itemTable.Rows[i][1].ToString();
                }

                previousItem = alleleTable.Rows[0][1].ToString();
                previousExperiment = alleleTable.Rows[0][2].ToString();

                for (int i = 0; i < alleleTable.Rows.Count; i++)
                {
                    currentItem = alleleTable.Rows[i][1].ToString();
                    currentExperiment = alleleTable.Rows[i][2].ToString();
                    if (currentExperiment != previousExperiment)
                    {
                        previousExperiment = currentExperiment;
                        experimentIndex++;
                        if (itemTable.Rows.Count < 2)
                        {
                            //Will never change item, set to 1.
                            itemIndex = 1;
                        }
                        else
                        {
                            //Will change item also and thus be increased to 1.
                            itemIndex = 0;
                        }
                    }
                    if (currentItem != previousItem)
                    {
                        previousItem = currentItem;
                        itemIndex++;
                    }
                    MyResult[experimentIndex, itemIndex] = alleleTable.Rows[i][3].ToString();

                }
            }

        }

        public string GetTabDelimited()
        {
            //Returns a tab delimited string representation of the matrix.
            StringBuilder str;

            str = new StringBuilder(10 * MyResult.GetLength(0) * MyResult.GetLength(1));

            for (int i = 0; i < MyResult.GetLength(0); i++)
            {
                for (int j = 0; j < MyResult.GetLength(1); j++)
                {
                    str.Append(MyResult[i, j]);
                    if (j < MyResult.GetLength(1) - 1)
                    {
                        str.Append("\t");
                    }
                }
                str.Append(Environment.NewLine);
            }

            return str.ToString();
        }

        public string[,] GetResultMatrix()
        {
            return MyResult;
        }



    }

}