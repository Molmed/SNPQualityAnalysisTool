using System;
using System.Text;
using System.Data;
using System.Reflection;
using System.Xml;
using Molmed.SQAT.GUI;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ChiasmaObjects;

namespace Molmed.SQAT.ServiceObjects
{

    public class ReportGenerator
    {
        private DataServer MyDataServer;

        private const string SECTION_HEADER_VERSION = "Version information";
        private const string SECTION_HEADER_PLATES = "Included plates";
        private const string SECTION_HEADER_GROUPS = "Included working sets";
        private const string SECTION_HEADER_DETAILS = "Detailed filter";
        private const string SECTION_HEADER_CTRL_ITEM_STAT = "Control item statistics";
        private const string SECTION_HEADER_EXP_STAT = "Experiment statistics";
        private const string SECTION_HEADER_ITEM_STAT = "Item statistics";
        private const string SECTION_HEADER_ANNOTATIONS = "Marker annotations";
        private const string SECTION_HEADER_RESULTS = "Genotype results";

        public ReportGenerator(DataServer dServer)
        {
            MyDataServer = dServer;
        }

        public void WriteFromServer(ReportSettings repSettings, SessionSettings sesSettings)
        {
            this.WriteTextReportStreaming(repSettings, sesSettings);
        }

        public void WriteFromClient(ReportSettings repSettings, SessionSettings sesSettings, AdvancedListView2 markerList, AdvancedListView2 itemList, AdvancedListView2 controlItemList)
        {
            switch (repSettings.Get(ReportSettings.Strings.ReportType))
            {
                case ReportSettings.ReportTypeText:
                    this.WriteTextReport(repSettings, sesSettings, markerList, itemList, controlItemList);
                    break;
                case ReportSettings.ReportTypeXML:
                    this.WriteXMLReport(repSettings, sesSettings, markerList, itemList, controlItemList);
                    break;
                default:
                    throw new Exception("Unknown report type encountered.");
            }

        }

        private void WriteTextReportStreaming(ReportSettings repSettings, SessionSettings sesSettings)
        {
            FileServer fs;
            Identifiable[] selectedAnnotationTypes;

            fs = new FileServer();

            try
            {
                //Program name and version.
                if (repSettings.Get(ReportSettings.Bools.IncludeSourceInfo))
                {
                    PrepareNextSection(fs, SECTION_HEADER_VERSION, repSettings);
                    fs.AppendStringToFile(this.GetApplicationVersionDescription());
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Plates section.
                if (repSettings.Get(ReportSettings.Bools.IncludeSelectedPlates))
                {
                    PrepareNextSection(fs, SECTION_HEADER_PLATES, repSettings);
                    MyDataServer.GetPlatesInSelection(fs);
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Groups section.
                if (repSettings.Get(ReportSettings.Bools.IncludeSelectedGroups))
                {
                    PrepareNextSection(fs, SECTION_HEADER_GROUPS, repSettings);
                    MyDataServer.GetGroupsInSelection(fs);
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Detailed filter section.
                if (repSettings.Get(ReportSettings.Bools.IncludeSelectedDetailedFilter))
                {
                    PrepareNextSection(fs, SECTION_HEADER_DETAILS, repSettings);
                    MyDataServer.GetDetailedFilter(fs);
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Control item statistics section.
                if (repSettings.Get(ReportSettings.Bools.IncludeControlItemStat))
                {
                    PrepareNextSection(fs, SECTION_HEADER_CTRL_ITEM_STAT, repSettings);
                    MyDataServer.StreamControlItemStatToFile(fs);
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Experiment statistics section.
                if (repSettings.Get(ReportSettings.Bools.IncludeExperimentStat))
                {
                    PrepareNextSection(fs, SECTION_HEADER_EXP_STAT, repSettings);
                    MyDataServer.StreamExperimentStatToFile(fs, repSettings.Get(ReportSettings.BoolArrays.ExperimentStatFlags));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Marker annotations section.
                if (repSettings.Get(ReportSettings.Bools.IncludeAnnotations))
                {
                    PrepareNextSection(fs, SECTION_HEADER_ANNOTATIONS, repSettings);
                    selectedAnnotationTypes = repSettings.Get(ReportSettings.Identifiables.AnnotationTypes);
                    MyDataServer.GetMarkerAnnotations(selectedAnnotationTypes, sesSettings, fs);
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Item statistics section.
                if (repSettings.Get(ReportSettings.Bools.IncludeItemStat))
                {
                    PrepareNextSection(fs, SECTION_HEADER_ITEM_STAT, repSettings);
                    MyDataServer.StreamItemStatToFile(fs, repSettings.Get(ReportSettings.BoolArrays.ItemStatFlags));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Result section.
                if (repSettings.Get(ReportSettings.Bools.IncludeResults))
                {
                    PrepareNextSection(fs, SECTION_HEADER_RESULTS, repSettings);
                    if (repSettings.Get(ReportSettings.Strings.ResultTableType) == ReportSettings.ResultTableTypePairAll)
                    {
                        MyDataServer.GetApprovedResultFormattedOuter(true, repSettings.Get(ReportSettings.Strings.EmptyResultString), fs);
                    }
                    else if (repSettings.Get(ReportSettings.Strings.ResultTableType) == ReportSettings.ResultTableTypePairApp)
                    {
                        MyDataServer.GetApprovedResultFormatted(fs);
                    }
                    fs.AppendStringToFile(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (fs != null)
                {
                    fs.CloseFile();
                }
            }
        }

        private void WriteTextReport(ReportSettings repSettings, SessionSettings sesSettings, AdvancedListView2 markerList, AdvancedListView2 itemList, AdvancedListView2 controlItemList)
        {
            bool[] markerColumnFlags, itemColumnFlags;
            Identifiable[] selectedAnnotationTypes;
            ResultTable resultMatrix;
            DataTable pairwiseTable, annotationTable, plateTable, groupTable, singleTable;
            DataColumn idColumn;
            AdvancedListView2 annotationList, plateList, groupList, singleList;
            FileServer fs;

            fs = new FileServer();
            try
            {
                //Program name and version.
                if (repSettings.Get(ReportSettings.Bools.IncludeSourceInfo))
                {
                    PrepareNextSection(fs, SECTION_HEADER_VERSION, repSettings);
                    fs.AppendStringToFile(this.GetApplicationVersionDescription());
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Plates section.
                if (repSettings.Get(ReportSettings.Bools.IncludeSelectedPlates))
                {
                    plateTable = MyDataServer.GetPlatesInSelection();
                    plateList = new AdvancedListView2();
                    plateList.ShowFormattedTable(plateTable);
                    PrepareNextSection(fs, SECTION_HEADER_PLATES, repSettings);
                    fs.AppendStringToFile(plateList.GetListAsString(true, true));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Groups section.
                if (repSettings.Get(ReportSettings.Bools.IncludeSelectedGroups))
                {
                    groupTable = MyDataServer.GetGroupsInSelection();
                    groupList = new AdvancedListView2();
                    groupList.ShowFormattedTable(groupTable);
                    PrepareNextSection(fs, SECTION_HEADER_GROUPS, repSettings);
                    fs.AppendStringToFile(groupList.GetListAsString(true, true));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Detailed filter section.
                if (repSettings.Get(ReportSettings.Bools.IncludeSelectedDetailedFilter))
                {
                    singleTable = MyDataServer.GetDetailedFilter();
                    singleList = new AdvancedListView2();
                    singleList.ShowFormattedTable(singleTable);
                    PrepareNextSection(fs, SECTION_HEADER_DETAILS, repSettings);
                    fs.AppendStringToFile(singleList.GetListAsString(true, true));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Control item statistics section.
                if (repSettings.Get(ReportSettings.Bools.IncludeControlItemStat))
                {
                    PrepareNextSection(fs, SECTION_HEADER_CTRL_ITEM_STAT, repSettings);
                    fs.AppendStringToFile(controlItemList.GetListAsString(true, true));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Experiment statistics section.
                if (repSettings.Get(ReportSettings.Bools.IncludeExperimentStat))
                {
                    markerColumnFlags = repSettings.Get(ReportSettings.BoolArrays.ExperimentStatFlags);
                    PrepareNextSection(fs, SECTION_HEADER_EXP_STAT, repSettings);
                    fs.AppendStringToFile(markerList.GetListAsString(true, true, markerColumnFlags));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Marker annotations section.
                if (repSettings.Get(ReportSettings.Bools.IncludeAnnotations))
                {
                    selectedAnnotationTypes = repSettings.Get(ReportSettings.Identifiables.AnnotationTypes);
                    annotationTable = MyDataServer.GetMarkerAnnotations(selectedAnnotationTypes, sesSettings);
                    annotationList = new AdvancedListView2();
                    annotationList.ShowFormattedTable(annotationTable);
                    PrepareNextSection(fs, SECTION_HEADER_ANNOTATIONS, repSettings);
                    fs.AppendStringToFile(annotationList.GetListAsString(true, true));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Item statistics section.
                if (repSettings.Get(ReportSettings.Bools.IncludeItemStat))
                {
                    itemColumnFlags = repSettings.Get(ReportSettings.BoolArrays.ItemStatFlags);
                    PrepareNextSection(fs, SECTION_HEADER_ITEM_STAT, repSettings);
                    fs.AppendStringToFile(itemList.GetListAsString(true, true, itemColumnFlags));
                    fs.AppendStringToFile(Environment.NewLine);
                }

                //Result section.
                if (repSettings.Get(ReportSettings.Bools.IncludeResults))
                {
                    PrepareNextSection(fs, SECTION_HEADER_RESULTS, repSettings);
                    if (repSettings.Get(ReportSettings.Strings.ResultTableType) == ReportSettings.ResultTableTypeExpmRows)
                    {
                        resultMatrix = new ResultTable(MyDataServer, ResultTable.Mode.ExperimentByItem, repSettings.Get(ReportSettings.Strings.EmptyResultString));
                        fs.AppendStringToFile(resultMatrix.GetTabDelimited());
                    }
                    else if (repSettings.Get(ReportSettings.Strings.ResultTableType) == ReportSettings.ResultTableTypeExpmCol)
                    {
                        resultMatrix = new ResultTable(MyDataServer, ResultTable.Mode.ItemByExperiment, repSettings.Get(ReportSettings.Strings.EmptyResultString));
                        fs.AppendStringToFile(resultMatrix.GetTabDelimited());
                    }
                    else if (repSettings.Get(ReportSettings.Strings.ResultTableType) == ReportSettings.ResultTableTypePairAll)
                    {
                        pairwiseTable = MyDataServer.GetApprovedResultFormattedOuter(true, repSettings.Get(ReportSettings.Strings.EmptyResultString));
                        //Remove the ID column.
                        idColumn = pairwiseTable.Columns[0];
                        pairwiseTable.Columns.Remove(idColumn);

                        fs.AppendStringToFile(TabDelimiter.GetString(pairwiseTable, true));
                    }
                    else if (repSettings.Get(ReportSettings.Strings.ResultTableType) == ReportSettings.ResultTableTypePairApp)
                    {
                        pairwiseTable = MyDataServer.GetApprovedResultFormatted();
                        //Remove the ID column.
                        idColumn = pairwiseTable.Columns[0];
                        pairwiseTable.Columns.Remove(idColumn);
                        fs.AppendStringToFile(TabDelimiter.GetString(pairwiseTable, true));
                    }
                    fs.AppendStringToFile(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                throw (ex);

            }
            finally
            {
                if (fs != null)
                {
                    fs.CloseFile();
                }
            }

        }

        private bool IsEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        private void PrepareItemTable(DataTable itemTable)
        {
            foreach (DataRow row in itemTable.Rows)
            {
                foreach (DataColumn column in itemTable.Columns)
                {
                    if (column.ColumnName == "Failedinhexpm" &&
                        IsEmpty(row[column].ToString()))
                    { 
                        row[column] = "0";
                    }
                    if (column.ColumnName == "FailedHWexpm" &&
                        IsEmpty(row[column].ToString()))
                    { 
                        row[column] = "0";
                    }
                }
            }
        }

        private void WriteXMLReport(ReportSettings repSettings, SessionSettings sesSettings, AdvancedListView2 markerList, AdvancedListView2 itemList, AdvancedListView2 controlItemList)
        {
            string appVersion, appName, userName, itemSetting, experimentSetting, noCallIndicator;
            int userId;
            DataTable metaDataTable = null, pairwiseTable = null;
            DataTable markerTable = null, annotationTable = null, annotationTableCopy = null, itemTable = null;
            DataTable plateTable = null, groupTable = null, singleTable = null, controlItemTable = null;
            object[] metaDataValues;
            DataColumn idColumn;
            bool[] expmFlags, itemFlags;
            Identifiable[] annotationTypes;
            bool includeAnnoType;
            XmlTextWriter xw = null;

            if (repSettings.Get(ReportSettings.Bools.IncludeSourceInfo))
            {
                appVersion = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                    Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                    Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                    Assembly.GetExecutingAssembly().GetName().Version.Revision;
                appName = Assembly.GetExecutingAssembly().GetName().Name;
                userName = MyDataServer.GetCurrentUserName();
                userId = MyDataServer.GetCurrentUserId();
                itemSetting = sesSettings.Get(SessionSettings.Strings.ViewMode);
                experimentSetting = sesSettings.Get(SessionSettings.Strings.ExperimentMode);
                noCallIndicator = repSettings.Get(ReportSettings.Strings.EmptyResultString);

                metaDataValues = new Object[13];
                metaDataValues[0] = appName;
                metaDataValues[1] = appVersion;
                metaDataValues[2] = userName;
                metaDataValues[3] = userId;
                metaDataValues[4] = itemSetting;
                metaDataValues[5] = experimentSetting;
                metaDataValues[6] = noCallIndicator;

                metaDataValues[7] = Convert.ToString(sesSettings.Get(SessionSettings.Bools.DemandDuplicates));
                metaDataValues[8] = Convert.ToString(sesSettings.Get(SessionSettings.Bools.DoHWTest));
                metaDataValues[9] = Convert.ToString(sesSettings.Get(SessionSettings.Bools.DoInheritanceTest));
                metaDataValues[10] = Convert.ToString(sesSettings.Get(SessionSettings.Bools.HumanXChr));
                metaDataValues[11] = Convert.ToString(sesSettings.Get(SessionSettings.Bools.HWSkipChildren));

                metaDataValues[12] = sesSettings.Get(SessionSettings.Doubles.HWLimit);

                metaDataTable = new DataTable("MetaData");
                metaDataTable.Columns.Add("ProgramName", Type.GetType("System.String"));
                metaDataTable.Columns.Add("ProgramVersion", Type.GetType("System.String"));
                metaDataTable.Columns.Add("AuthorityName", Type.GetType("System.String"));
                metaDataTable.Columns.Add("authority_id", Type.GetType("System.Int32"));
                metaDataTable.Columns.Add("ItemSetting", Type.GetType("System.String"));
                metaDataTable.Columns.Add("ExperimentSetting", Type.GetType("System.String"));
                metaDataTable.Columns.Add("NoCallIndicator", Type.GetType("System.String"));
                metaDataTable.Columns.Add("DemandDuplicates", Type.GetType("System.String"));
                metaDataTable.Columns.Add("DoHWTest", Type.GetType("System.String"));
                metaDataTable.Columns.Add("DoInheritanceTest", Type.GetType("System.String"));
                metaDataTable.Columns.Add("HumanXChr", Type.GetType("System.String"));
                metaDataTable.Columns.Add("HWSkipChildren", Type.GetType("System.String"));
                metaDataTable.Columns.Add("HWLimit", Type.GetType("System.Double"));

                metaDataTable.Rows.Add(metaDataValues);
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeSelectedPlates))
            {
                plateTable = MyDataServer.GetPlatesInSelection();
                plateTable.TableName = "PlateRow";
                foreach (DataColumn tempColumn in plateTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                plateTable.DataSet.Tables.Remove(plateTable);
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeSelectedGroups))
            {
                groupTable = MyDataServer.GetGroupsInSelection();
                groupTable.TableName = "GroupRow";
                foreach (DataColumn tempColumn in groupTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                groupTable.DataSet.Tables.Remove(groupTable);
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeSelectedDetailedFilter))
            {
                singleTable = MyDataServer.GetDetailedFilter();
                singleTable.TableName = "FilterRow";
                foreach (DataColumn tempColumn in singleTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                singleTable.DataSet.Tables.Remove(singleTable);
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeControlItemStat))
            {
                controlItemTable = controlItemList.Table.Copy();
                controlItemTable.TableName = "CtrlItemStatRow";
                expmFlags = repSettings.Get(ReportSettings.BoolArrays.ExperimentStatFlags);
                //Remove spaces in column names.
                foreach (DataColumn tempColumn in controlItemTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeItemStat))
            {
                itemTable = itemList.Table.Copy();
                itemTable.TableName = "ItemStatRow";
                itemFlags = repSettings.Get(ReportSettings.BoolArrays.ItemStatFlags);
                //Remove deselected columns (the flags and columns are off by 1 since the list contains
                //the ID column as well).
                for (int i = 1; i < itemList.Table.Columns.Count; i++)
                {
                    if (!itemFlags[i - 1])
                    {
                        itemTable.Columns.Remove(itemList.Table.Columns[i].ColumnName);
                    }
                }
                //Remove spaces in column names. 
                foreach (DataColumn tempColumn in itemTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
                PrepareItemTable(itemTable);
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeExperimentStat))
            {
                markerTable = markerList.Table.Copy();
                markerTable.TableName = "ExpmStatRow";
                expmFlags = repSettings.Get(ReportSettings.BoolArrays.ExperimentStatFlags);
                //Remove deselected columns (the flags and columns are off by 1 since the list contains
                //the ID column as well).
                for (int i = 1; i < markerList.Table.Columns.Count; i++ )
                {
                    if (!expmFlags[i - 1])
                    {
                        markerTable.Columns.Remove(markerList.Table.Columns[i].ColumnName);
                    }
                }
                //Remove spaces in column names.
                foreach (DataColumn tempColumn in markerTable.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeAnnotations))
            {
                annotationTable = MyDataServer.GetMarkerAnnotations(repSettings.Get(ReportSettings.Identifiables.AnnotationTypes), sesSettings);
                annotationTable.TableName = "MrkAnnotRow";
                annotationTableCopy = annotationTable.Copy();
                annotationTypes = repSettings.Get(ReportSettings.Identifiables.AnnotationTypes);
                //Remove deselected columns from the copy (the flags and columns are off by 2 since the columns contain
                //the marker ID and marker name as well).
                for (int i = 2; i < annotationTable.Columns.Count; i++)
                {
                    includeAnnoType = false;
                    for (int j = 0; j < annotationTypes.GetLength(0); j++)
                    {
                        if (annotationTable.Columns[i].ColumnName.ToUpper() == annotationTypes[j].Name.ToUpper())
                        {
                            includeAnnoType = true;
                        }
                    }
                    if (!includeAnnoType)
                    {
                        annotationTableCopy.Columns.Remove(annotationTable.Columns[i].ColumnName);
                    }
                }
                //Remove spaces in column names. 
                foreach (DataColumn tempColumn in annotationTableCopy.Columns)
                {
                    tempColumn.ColumnName = tempColumn.ColumnName.Replace(" ", "");
                }
            }

            if (repSettings.Get(ReportSettings.Bools.IncludeResults))
            {
                pairwiseTable = MyDataServer.GetApprovedResultFormattedOuterWithId(true, repSettings.Get(ReportSettings.Strings.EmptyResultString));
                //Remove the ID column.
                idColumn = pairwiseTable.Columns[0];
                pairwiseTable.Columns.Remove(idColumn);
                pairwiseTable.DataSet.Tables.Remove(pairwiseTable);
                pairwiseTable.TableName = "Result";
            }

            try
            {
                xw = new XmlTextWriter(repSettings.Get(ReportSettings.Strings.FilePath) + "\\" + repSettings.Get(ReportSettings.Strings.FileName), null);

                xw.Formatting = Formatting.Indented;

                xw.WriteStartDocument(true);
                xw.WriteStartElement("GenotypingReport");
                WriteDataTableToXml(metaDataTable, xw, "");
                WriteDataTableToXml(plateTable, xw, "Plates");
                WriteDataTableToXml(groupTable, xw, "Groups");
                WriteDataTableToXml(singleTable, xw, "Filters");
                WriteDataTableToXml(controlItemTable, xw, "CtrlItems");
                WriteDataTableToXml(itemTable, xw, "Items");
                WriteDataTableToXml(markerTable, xw, "Experiments");
                WriteDataTableToXml(annotationTableCopy, xw, "Annotations");
                WriteDataTableToXml(pairwiseTable, xw, "Results");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }

        }

        private void WriteDataTableToXml(DataTable table, XmlTextWriter xw, String complexTypeName)
        {
            int newValue;

            if (complexTypeName != "")
            {
                xw.WriteStartElement(complexTypeName);
            }

            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    xw.WriteStartElement(table.TableName);
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.DataType == typeof(Single))
                        {
                            newValue = (int)Math.Round(100000 * Convert.ToSingle(row[column]));
                            xw.WriteElementString(column.ColumnName + "_X100000", newValue.ToString());
                        }
                        else if (column.DataType == typeof(Double))
                        {
                            newValue = (int)Math.Round(100000 * Convert.ToDouble(row[column]));
                            xw.WriteElementString(column.ColumnName + "_X100000", newValue.ToString());
                        }
                        else
                        {
                            xw.WriteElementString(column.ColumnName, row[column].ToString());
                        }
                    }
                    xw.WriteEndElement();
                }
            }

            if (complexTypeName != "")
            {
                xw.WriteEndElement();
            }
        }

        private string GetApplicationVersionDescription()
        {
            string appVersion, appName;

            appVersion = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Revision;

            appName = Assembly.GetExecutingAssembly().GetName().Name;

            return "Generated using " + appName + " " + appVersion + Environment.NewLine;
            
        }

        private void PrepareNextSection(FileServer fs, string sectionHeader, ReportSettings settings)
        {
            //When using one file for the whole report, this function writes the
            //section header in the file. When using split files, this function
            //creates a new file with the section header as a part of the file name.
            if (settings.Get(ReportSettings.Bools.SplitFile))
            {
                fs.CloseFile();
                fs.OpenFileForWriting(settings.Get(ReportSettings.Strings.FilePath) + "\\" 
                    + settings.Get(ReportSettings.Strings.FileName) + "_" + sectionHeader.Replace(" ", "") + ".txt");

            }
            else
            {
                if (!fs.FileIsOpen)
                {
                    fs.OpenFileForWriting(settings.Get(ReportSettings.Strings.FilePath) + "\\"
                    + settings.Get(ReportSettings.Strings.FileName));
                }
                fs.AppendStringToFile(sectionHeader + Environment.NewLine);
            }
        }

    }

}
