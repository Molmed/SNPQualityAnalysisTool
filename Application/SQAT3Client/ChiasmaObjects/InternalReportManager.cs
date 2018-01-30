using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.GUI;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class InternalReportManager : ChiasmaData
    {
        public enum InternalReportResultPlateTable
        {
            InternalReportId = 0,
            ResultPlateId = 1
        }

        public enum InternalReportIndvStatTable
        {
            InternalReportId = 0,
            ItemId = 1,
            SuccessRate = 2,
            DuplFail = 3,
            DuplTest = 4,
            InhFail = 5,
            InhTest = 6
        }

        public enum InternalReportMarkerStatTable
        {
            InternalReportId = 0,
            MarkerId = 1,
            XX = 2,
            YY = 3,
            XY = 4,
            NA = 5,
            DuplFail = 6,
            DuplTest = 7,
            InhFail = 8,
            InhTest = 9,
            CtrlInhFail = 10,
            CtrlInhTest = 11
        }

        public enum InternalReportResultTable
        {
            InternalReportId = 0,
            ItemId = 1,
            MarkerId = 2,
            AlleleResultId = 3
        }

        private struct ItemStatTableName
        {
            public const string InternalReportIndvStat = "internal_report_indv_stat";
            public const string InternalReportSampleStat = "internal_report_sample_stat";
        }

        private struct ResultsTableName
        {
            public const string InternalReportResult = "internal_report_result";
            public const string InternalReportResultSampleRef = "internal_report_result_sample_ref";
        }

        private InternalReportManager()
            : base()
        {
        }

        public static void AddInternalReportMarkerStatistics(DataTable markerStatTable)
        {
            Database.AddInternalReportMarkerStatistics(markerStatTable);
        }

        public static void AddInternalReportResultPlates(DataTable resultPlatesTable)
        {
            Database.AddInternalReportResultPlates(resultPlatesTable);
        }

        public static void AddInternalReportResults(DataTable resultsTable)
        {
            Database.AddInternalReportResults(resultsTable);
        }

        private static void CheckInternalReportNotDefined(String[] identifiers)
        {
            StringCollection existingInternalReportIdentifiers;

            existingInternalReportIdentifiers = new StringCollection();
            foreach (String identifier in identifiers)
            {
                if (IsNotNull(GetInternalReport(identifier)))
                {
                    existingInternalReportIdentifiers.Add(identifier);
                }
            }

            if (IsNotEmpty(existingInternalReportIdentifiers))
            {
                throw new InternalReportAlreadyDefinedException(existingInternalReportIdentifiers);
            }
        }

        public static Dictionary<String, Int32> GetAlleleResults()
        {
            DataReader dataReader = null;
            Dictionary<String, Int32> results;
            String tempName;
            Int32 tempId;

            try
            {
                results = new Dictionary<string, int>();
                dataReader = Database.GetAlleleResults();
                while (dataReader.Read())
                {
                    tempId = (Int32)dataReader.GetTinyint(AlleleResultData.ID);
                    tempName = dataReader.GetString(AlleleResultData.NAME);
                    results.Add(tempName, tempId);
                }
            }
            finally
            {
                CloseDataReader(dataReader);
            }
            return results;
        }

        public static InternalReport CreateInternalReport(String identifier, Int32 userId, Int32 projectId, String source,
            Boolean isBulk, String comment, String settingsXml)
        {
            DataReader dataReader = null;
            InternalReport report;

            CheckInternalReportNotDefined(new String[] { identifier });

            // this function should be called within a try - catch environment higher up
            try
            {
                dataReader = Database.CreateInternalReport(identifier, userId, projectId,
                    source, isBulk, comment, settingsXml);
                if (dataReader.Read())
                {
                    report = new InternalReport(dataReader);
                }
                else
                {
                    throw new System.Exception("Unable to create internal report.");
                }

                return report;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        private static String GetDBString(Object dbObject)
        {
            String dbString;

            if ((dbObject == DBNull.Value) || IsEmpty((String)(dbObject)))
            {
                dbString = null;
            }
            else
            {
                dbString = (String)(dbObject);
            }
            return dbString;
        }

        public static InternalReport GetInternalReport(String identifier)
        {
            DataReader dataReader = null;
            InternalReport internalReport = null;

            try
            {
                dataReader = Database.GetInternalReportByIdentifier(identifier);
                if (dataReader.Read())
                {
                    internalReport = new InternalReport(dataReader);
                }
            }
            finally
            {
                CloseDataReader(dataReader);
            }
            return internalReport;
        }

        public static DataTable GetInternalReportItemStatTable()
        {
            DataColumn column;
            DataTable internalReportIndvStatTable;

            // not matching Chiasma db table, must be renamed later
            internalReportIndvStatTable = new DataTable("internal_report_item_stat"); 

            // Define columns.
            column = new DataColumn(InternalReportIndvStatTable.InternalReportId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportIndvStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportIndvStatTable.ItemId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportIndvStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportIndvStatTable.SuccessRate.ToString(), typeof(Double));
            column.AllowDBNull = true;
            column.Unique = false;
            internalReportIndvStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportIndvStatTable.DuplFail.ToString(), typeof(Int32));
            column.AllowDBNull = true;
            column.Unique = false;
            internalReportIndvStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportIndvStatTable.DuplTest.ToString(), typeof(Int32));
            column.AllowDBNull = true;
            column.Unique = false;
            internalReportIndvStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportIndvStatTable.InhFail.ToString(), typeof(Int32));
            column.AllowDBNull = true;
            column.Unique = false;
            internalReportIndvStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportIndvStatTable.InhTest.ToString(), typeof(Int32));
            column.AllowDBNull = true;
            column.Unique = false;
            internalReportIndvStatTable.Columns.Add(column);

            return internalReportIndvStatTable;
        }

        public static DataTable GetInternalReportMarkerStatTable()
        {
            DataColumn column;
            DataTable internalReportMarkerStatTable;

            internalReportMarkerStatTable = new DataTable("internal_report_marker_stat");

            // Define columns.
            column = new DataColumn(InternalReportMarkerStatTable.InternalReportId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.MarkerId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.XX.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.YY.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.XY.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.NA.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.DuplFail.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.DuplTest.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.InhFail.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.InhTest.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.CtrlInhFail.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            column = new DataColumn(InternalReportMarkerStatTable.CtrlInhTest.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportMarkerStatTable.Columns.Add(column);

            return internalReportMarkerStatTable;
        }

        public static DataTable GetInternalReportResultTable()
        {
            DataColumn column;
            DataTable internalReportResultTable;

            internalReportResultTable = new DataTable("internal_report_result");

            // Define columns.
            column = new DataColumn(InternalReportResultTable.InternalReportId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportResultTable.Columns.Add(column);

            column = new DataColumn(InternalReportResultTable.ItemId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportResultTable.Columns.Add(column);

            column = new DataColumn(InternalReportResultTable.MarkerId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportResultTable.Columns.Add(column);

            column = new DataColumn(InternalReportResultTable.AlleleResultId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportResultTable.Columns.Add(column);

            return internalReportResultTable;
        }

        public static DataTable GetInternalReportResultPlateTable()
        {
            DataColumn column;
            DataTable internalReportResultPlateTable;

            internalReportResultPlateTable = new DataTable("internal_report_result_plate");

            // Define columns.
            column = new DataColumn(InternalReportResultPlateTable.InternalReportId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportResultPlateTable.Columns.Add(column);

            column = new DataColumn(InternalReportResultPlateTable.ResultPlateId.ToString(), typeof(Int32));
            column.AllowDBNull = false;
            column.Unique = false;
            internalReportResultPlateTable.Columns.Add(column);

            return internalReportResultPlateTable;
        }

        public class BackgroundUploadInternalReport
        {
            private BackgroundWorker MyBackgroundWorker;
            private Project MyProject;
            private SessionSettings MySessionSettings;
            private const String EMPTY_RESULT_STRING = "*/*";
            private DataServer MySQATDataServer;
            private AdvancedListView2 MyIndividualList, MyControlIndividualList, MyMarkerList;
            private String MyInternalReportName;

            public BackgroundUploadInternalReport(Project currentProject, String internalReportName,
                SessionSettings sesSettings, BackgroundWorker worker, DataServer sqatDataserver,
                AdvancedListView2 markerList, AdvancedListView2 individualList, AdvancedListView2 controlIndividualList)
            {
                MyProject = currentProject;
                MySessionSettings = sesSettings;
                MyBackgroundWorker = worker;
                MySQATDataServer = sqatDataserver;
                MyIndividualList = individualList;
                MyControlIndividualList = controlIndividualList;
                MyMarkerList = markerList;
                MyInternalReportName = internalReportName;
                MyBackgroundWorker.DoWork += new DoWorkEventHandler(UploadInternalReport);
            }
            private void UploadInternalReport(object sender, EventArgs e)
            {
                String appName, appVersion, settingString;
                InternalReport report;
                DataTable resultPlatesTable, itemStatTable, markerStatTable, genotypeResultsTable;
                try
                {
                    MyBackgroundWorker.ReportProgress(0, "Connecting to Chiasma database...");
                    ChiasmaData.LoginChiasmaDataBase();
                    ChiasmaData.Database.BeginTransaction();
                    appName = Assembly.GetExecutingAssembly().GetName().Name;
                    appVersion = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                        Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                        Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                        Assembly.GetExecutingAssembly().GetName().Version.Revision;
                    settingString = GetSettingString();
                    report = InternalReportManager.CreateInternalReport(MyInternalReportName, UserManager.GetCurrentUser().GetId(),
                        MyProject.ID, appName + " " + appVersion, false, "", settingString);

                    //Read and store included result plates.
                    MyBackgroundWorker.ReportProgress(0, "Uploading plate information...");
                    if (MyBackgroundWorker.CancellationPending)
                    {
                        throw new UserCanceledException();
                    }
                    resultPlatesTable = GetResultPlatesTable(report.GetId());
                    if (resultPlatesTable.Rows.Count < 1)
                    {
                        throw new System.Exception("No result plates found.");
                    }
                    ChiasmaData.Database.AddInternalReportResultPlates(resultPlatesTable);

                    //Read and store item statistics.
                    MyBackgroundWorker.ReportProgress(0, "Uploading item statistics...");
                    if (MyBackgroundWorker.CancellationPending)
                    {
                        throw new UserCanceledException();
                    }
                    itemStatTable = GetItemStatsTable(report.GetId());
                    if (itemStatTable.Rows.Count < 1)
                    {
                        throw new System.Exception("No item statistics found.");
                    }
                    SetNameToItemTable(ref itemStatTable);
                    
                    ChiasmaData.Database.AddInternalReportItemStatistics(itemStatTable);

                    //Read and store marker statistics.
                    MyBackgroundWorker.ReportProgress(0, "Uploading marker statistics...");
                    if (MyBackgroundWorker.CancellationPending)
                    {
                        throw new UserCanceledException();
                    }
                    markerStatTable = GetMarkerStatsTable(report.GetId());
                    if (markerStatTable.Rows.Count < 1)
                    {
                        throw new System.Exception("No marker statistics found.");
                    }
                    ChiasmaData.Database.AddInternalReportMarkerStatistics(markerStatTable);

                    //Read and store genotype results.
                    MyBackgroundWorker.ReportProgress(0, "Uploading genotype results...");
                    if (MyBackgroundWorker.CancellationPending)
                    {
                        throw new UserCanceledException();
                    }
                    genotypeResultsTable = GetGenotypeResultsTable(report.GetId());
                    if (genotypeResultsTable.Rows.Count < 1)
                    {
                        throw new System.Exception("No genotype results found.");
                    }
                    SetNameToResultsTable(ref genotypeResultsTable);
                    ChiasmaData.Database.AddInternalReportResults(genotypeResultsTable);
                    ChiasmaData.Database.CommitTransaction();
                    MessageBox.Show("The internal report was successfully updated!", "Upload internal report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (ConnectDatabaseException ex)
                {
                    ChiasmaData.HandleError(ex.GetMessage(), ex);
                }
                catch (InternalReportAlreadyDefinedException ex)
                {
                    ChiasmaData.Database.RollbackTransaction();
                    ChiasmaData.HandleError("Error when uploading report! ", ex);
                }
                catch (UserCanceledException ex)
                {
                    ChiasmaData.Database.RollbackTransaction();
                    MessageBox.Show(ex.GetUploadCanceledMessage(), "Upload internal report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    ChiasmaData.Database.RollbackTransaction();
                    ChiasmaData.HandleError("Error when uploading internal report", ex);
                }
                finally
                {
                    ChiasmaData.LogoutChiasmaDatabase();
                }
            }

            private void SetNameToResultsTable(ref DataTable resultsTable)
            {
                string itemSettings;
                itemSettings = MySessionSettings.Get(SessionSettings.Strings.ViewMode);
                switch (itemSettings)
                {
                    case "SAMPLE":
                        resultsTable.TableName = ResultsTableName.InternalReportResultSampleRef;
                        break;
                    case "SUBJECT":
                        resultsTable.TableName = ResultsTableName.InternalReportResult;
                        break;
                    default:
                        throw new DataException("Unknown item setting");
                }
            }

            private void SetNameToItemTable(ref DataTable itemStatTable)
            {
                string itemSettings;
                itemSettings = MySessionSettings.Get(SessionSettings.Strings.ViewMode);
                switch (itemSettings)
                { 
                    case "SAMPLE":
                        itemStatTable.TableName = ItemStatTableName.InternalReportSampleStat;
                        break;
                    case "SUBJECT":
                        itemStatTable.TableName = ItemStatTableName.InternalReportIndvStat;
                        break;
                    default:
                        throw new DataException("Unknown item setting");
                }
            }

            private static Dictionary<String, Int32> GetAlleleResults()
            {
                DataReader dataReader = null;
                Dictionary<String, Int32> results;
                String tempName;
                Int32 tempId;

                try
                {
                    results = new Dictionary<string, int>();
                    dataReader =  ChiasmaData.Database.GetAlleleResults();
                    while (dataReader.Read())
                    {
                        tempId = (Int32)dataReader.GetTinyint(AlleleResultData.ID);
                        tempName = dataReader.GetString(AlleleResultData.NAME);
                        results.Add(tempName, tempId);
                    }
                }
                finally
                {
                    CloseDataReader(dataReader);
                }
                return results;
            }

            private DataTable GetGenotypeResultsTable(int internalReportId)
            {
                DataTable genotypeResultsTable, pairwiseTable;
                DataRow row;
                DataColumn idColumn;
                int itemId, markerId, alleleResultId;
                String alleles;
                Dictionary<String, Int32> alleleResultDict;
                alleleResultDict = GetAlleleResults();
                itemId = ChiasmaData.NO_ID; markerId = ChiasmaData.NO_ID; alleleResultId = ChiasmaData.NO_ID;
                genotypeResultsTable = GetInternalReportResultTable();
                pairwiseTable = MySQATDataServer.GetApprovedResultFormattedOuterWithId(true, EMPTY_RESULT_STRING);
                //Remove the ID column.
                idColumn = pairwiseTable.Columns[0];
                pairwiseTable.Columns.Remove(idColumn);
                pairwiseTable.DataSet.Tables.Remove(pairwiseTable);
                foreach (DataRow dataRow in pairwiseTable.Rows)
                {
                    foreach (DataColumn dataColumn in pairwiseTable.Columns)
                    {
                        if (dataColumn.ColumnName == "item_id")
                        {
                            itemId = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "experiment_id")
                        {
                            markerId = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Alleles")
                        {
                            alleles = dataRow[dataColumn].ToString();
                            if (alleles == EMPTY_RESULT_STRING)
                            {
                                alleles = "N/A";                                
                            }
                            alleleResultId = alleleResultDict[alleles];
                        }
                    }
                    row = genotypeResultsTable.NewRow();
                    row[(int)InternalReportResultTable.InternalReportId] = internalReportId;
                    row[(int)InternalReportResultTable.ItemId] = itemId;
                    row[(int)InternalReportResultTable.MarkerId] = markerId;
                    row[(int)InternalReportResultTable.AlleleResultId] = alleleResultId;

                    genotypeResultsTable.Rows.Add(row);
                }


                return genotypeResultsTable;
            }

            private DataTable GetMarkerStatsTable(int internalReportId)
            {
                DataTable markerStatsTable = null, inputMarkerStasTable;
                DataRow row;
                int markerId, valXX, valYY, valXY, valNA, duplFail, duplTest, inhFail, inhTest, ctrlInhTest, ctrlInhFail;
                markerId = ChiasmaData.NO_ID;
                valXX = ChiasmaData.NO_COUNT; valYY = ChiasmaData.NO_COUNT; valXY = ChiasmaData.NO_COUNT; valNA = ChiasmaData.NO_COUNT;
                duplFail = ChiasmaData.NO_COUNT; duplTest = ChiasmaData.NO_COUNT; inhTest = ChiasmaData.NO_COUNT; inhFail = ChiasmaData.NO_COUNT;
                ctrlInhTest = ChiasmaData.NO_COUNT; ctrlInhFail = ChiasmaData.NO_COUNT;
                markerStatsTable = GetInternalReportMarkerStatTable();
                inputMarkerStasTable = MyMarkerList.Table.Copy();
                //Remove spaces in column names.
                foreach (DataColumn tempColumn in inputMarkerStasTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                foreach (DataRow dataRow in inputMarkerStasTable.Rows)
                {
                    foreach (DataColumn dataColumn in inputMarkerStasTable.Columns)
                    {
                        if (dataColumn.ColumnName == "experiment_id")
                        {
                             markerId = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "XX")
                        {
                            valXX = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "YY")
                        {
                            valYY = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "XY")
                        {
                            valXY = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Notapproved")
                        {
                            valNA = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Duplfaileditem")
                        {
                            duplFail = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Dupltesteditem")
                        {
                            duplTest = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Inhfaileditem")
                        {
                            inhFail = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Inhtesteditem")
                        {
                            inhTest = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Inhctrlfaileditem")
                        {
                            ctrlInhFail = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Inhctrltesteditem")
                        {
                            ctrlInhTest = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                    }
                    row = markerStatsTable.NewRow();
                    row[(int)InternalReportMarkerStatTable.InternalReportId] = internalReportId;
                    row[(int)InternalReportMarkerStatTable.MarkerId] = markerId;
                    row[(int)InternalReportMarkerStatTable.XX] = valXX;
                    row[(int)InternalReportMarkerStatTable.YY] = valYY;
                    row[(int)InternalReportMarkerStatTable.XY] = valXY;
                    row[(int)InternalReportMarkerStatTable.NA] = valNA;
                    row[(int)InternalReportMarkerStatTable.DuplFail] = duplFail;
                    row[(int)InternalReportMarkerStatTable.DuplTest] = duplTest;
                    row[(int)InternalReportMarkerStatTable.InhFail] = inhFail;
                    row[(int)InternalReportMarkerStatTable.InhTest] = inhTest;
                    row[(int)InternalReportMarkerStatTable.CtrlInhFail] = ctrlInhFail;
                    row[(int)InternalReportMarkerStatTable.CtrlInhTest] = ctrlInhTest;
                    markerStatsTable.Rows.Add(row);
                }

                return markerStatsTable;
            }

            private DataTable GetItemStatsTable(int internalReportId)
            {
                DataTable controlItemTable = null, itemStatTable, itemTable = null;
                DataRow row;
                Individual indv;
                int itemId = ChiasmaData.NO_ID, failedExpm = ChiasmaData.NO_COUNT, testedExpm = ChiasmaData.NO_COUNT;
                int failedDupExpm = ChiasmaData.NO_COUNT, failedInhExpm = ChiasmaData.NO_COUNT;
                double successExpm = -1.0;
                itemStatTable = GetInternalReportItemStatTable();
                // Treat control items
                controlItemTable = MyControlIndividualList.Table.Copy();
                controlItemTable.TableName = "CtrlItemStatRow";
                //Remove spaces in column names.
                foreach (DataColumn tempColumn in controlItemTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                foreach (DataRow dataRow in controlItemTable.Rows)
                {
                    row = itemStatTable.NewRow();
                    foreach (DataColumn dataColumn in controlItemTable.Columns)
                    {
                        if (dataColumn.ColumnName == "item_id")
                        { 
                            itemId = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Failedexpm")
                        {
                            failedExpm = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Testedexpm")
                        {
                            testedExpm = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                    }
                    if (itemId == ChiasmaData.NO_ID)
                    {
                        throw new DataException("Item id not found!");
                    }
                    indv = GetIndividual(itemId, MySessionSettings);
                    if (indv.GetIndividualUsage() == IndividualUsage.InheritanceControl)
                    {
                        row[(int)InternalReportIndvStatTable.InternalReportId] = internalReportId;
                        row[(int)InternalReportIndvStatTable.SuccessRate] = DBNull.Value;
                        row[(int)InternalReportIndvStatTable.DuplFail] = DBNull.Value;
                        row[(int)InternalReportIndvStatTable.DuplTest] = DBNull.Value;
                        row[(int)InternalReportIndvStatTable.ItemId] = itemId;
                        row[(int)InternalReportIndvStatTable.InhFail] = failedExpm;
                        row[(int)InternalReportIndvStatTable.InhTest] = testedExpm;
                        itemStatTable.Rows.Add(row);
                    }
                }

                // Treat normal items(indiviudals)
                itemTable = MyIndividualList.Table.Copy();
                foreach (DataColumn tempColumn in itemTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                foreach (DataRow dataRow in itemTable.Rows)
                {
                    row = itemStatTable.NewRow();
                    foreach (DataColumn dataColumn in itemTable.Columns)
                    {
                        if (dataColumn.ColumnName == "item_id")
                        {
                            itemId = Convert.ToInt32(dataRow[dataColumn].ToString());
                        }
                        else if (dataColumn.ColumnName == "Faileddupexpm")
                        {
                            if (!int.TryParse(dataRow[dataColumn].ToString(), out failedDupExpm))
                            {
                                failedDupExpm = 0;
                            }
                        }
                        else if (dataColumn.ColumnName == "Failedinhexpm")
                        {
                            if (!int.TryParse(dataRow[dataColumn].ToString(), out failedInhExpm))
                            {
                                failedInhExpm = 0;
                            }
                        }
                        else if (dataColumn.ColumnName == "Successexpm")
                        {
                            if (!double.TryParse(dataRow[dataColumn].ToString(), out successExpm))
                            {
                                successExpm = 0;
                            }
                        }                    
                    }
                    row[(int)InternalReportIndvStatTable.InternalReportId] = internalReportId;
                    row[(int)InternalReportIndvStatTable.ItemId] = itemId;
                    row[(int)InternalReportIndvStatTable.SuccessRate] = successExpm;
                    row[(int)InternalReportIndvStatTable.DuplFail] = failedDupExpm;
                    row[(int)InternalReportIndvStatTable.DuplTest] = DBNull.Value;
                    row[(int)InternalReportIndvStatTable.InhFail] = failedInhExpm;
                    row[(int)InternalReportIndvStatTable.InhTest] = DBNull.Value;
                    itemStatTable.Rows.Add(row);
                }

                return itemStatTable;
            }

            private static Individual GetIndividual(Int32 itemId, SessionSettings sessionSettings)
            {
                string itemSetting;

                itemSetting = sessionSettings.Get(SessionSettings.Strings.ViewMode);
                switch (itemSetting)
                {
                    case "SAMPLE":
                        return GetIndividualForSample(itemId);
                    case "SUBJECT":
                        return GetIndividual(itemId);
                    default:
                        throw new DataException("Unknown item setting");
                }
            }

            private static Individual GetIndividualForSample(int sampleId)
            {
                DataReader dataReader = null;
                Sample sample = null;
                try
                {
                    dataReader = Database.GetSample(sampleId);

                    if (dataReader.Read())
                    {
                        sample = new Sample(dataReader);
                    }
                }
                finally
                {
                    CloseDataReader(dataReader);
                }
                if (IsNotNull(sample))
                {
                    return sample.GetIndividual();
                }
                else
                {
                    return null;
                }
            }

            private static Individual GetIndividual(int individualId)
            {
                DataReader dataReader = null;
                Individual individual = null;
                try
                {
                    dataReader = Database.GetIndividual(individualId);
                    if (dataReader.Read())
                    {
                        individual = new Individual(dataReader);
                    }
                }
                finally
                {
                    CloseDataReader(dataReader);
                }
                return individual;            
            }

            private DataTable GetResultPlatesTable(int internalReportId)
            {
                DataTable resultPlatesTable, plateTable = null;
                DataRow row;
                int resultPlateId;
                resultPlatesTable = GetInternalReportResultPlateTable();

                plateTable = MySQATDataServer.GetPlatesInSelection();
                plateTable.TableName = "PlateRow";
                foreach (DataColumn tempColumn in plateTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                plateTable.DataSet.Tables.Remove(plateTable);

                foreach (DataRow plateRow in plateTable.Rows)
                {
                    foreach (DataColumn plateColumn in plateTable.Columns)
                    {
                        if (plateColumn.ColumnName == "plate_id")
                        {
                            resultPlateId = Convert.ToInt32(plateRow[plateColumn].ToString());
                            row = resultPlatesTable.NewRow();
                            row[(int)InternalReportResultPlateTable.InternalReportId] = internalReportId;
                            row[(int)InternalReportResultPlateTable.ResultPlateId] = resultPlateId;
                            resultPlatesTable.Rows.Add(row);
                        }
                    }
                }
                return resultPlatesTable;
            }

            private String GetSettingString()
            {
                String itemSetting, experimentSetting, noCallIndicator, settingString;
                settingString = "<Settings>";
                itemSetting = MySessionSettings.Get(SessionSettings.Strings.ViewMode);
                experimentSetting = MySessionSettings.Get(SessionSettings.Strings.ExperimentMode);
                noCallIndicator = EMPTY_RESULT_STRING;
                settingString += "<ItemSetting>" + itemSetting + "</ItemSetting>";
                settingString += "<ExperimentSetting>" + experimentSetting + "</ExperimentSetting>";
                settingString += "<NoCallIndicator>" + noCallIndicator + "</NoCallIndicator>";
                settingString += "<DemandDuplicates>";
                settingString = settingString + Convert.ToString(MySessionSettings.Get(SessionSettings.Bools.DemandDuplicates));
                settingString += "</DemandDuplicates>";
                settingString += "<DoHWTest>";
                settingString = settingString + Convert.ToString(MySessionSettings.Get(SessionSettings.Bools.DoHWTest));
                settingString += "</DoHWTest>";
                settingString += "<DoInheritanceTest>";
                settingString = settingString + Convert.ToString(MySessionSettings.Get(SessionSettings.Bools.DoInheritanceTest));
                settingString += "</DoInheritanceTest>";
                settingString += "<HumanXChr>";
                settingString = settingString + Convert.ToString(MySessionSettings.Get(SessionSettings.Bools.HumanXChr));
                settingString += "</HumanXChr>";
                settingString += "<HWSkipChildren>";
                settingString = settingString + Convert.ToString(MySessionSettings.Get(SessionSettings.Bools.HWSkipChildren));
                settingString += "</HWSkipChildren>";
                settingString += "<HWLimit>";
                settingString += MySessionSettings.Get(SessionSettings.Doubles.HWLimit);
                settingString += "</HWLimit>";
                settingString += "</Settings>";
                return settingString;

            }

            public static void HandleError(String message, Exception exception)
            {
                ShowErrorDialog errorDialog;

                errorDialog = new ShowErrorDialog(message, exception);
                errorDialog.ShowDialog();
            }

            private static Boolean IsNull(Object testObject)
            {
                return (testObject == null);
            }

            private static Boolean IsNotNull(Object testObject)
            {
                return !IsNull(testObject);
            }
        }
    }
}
