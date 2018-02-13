using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Molmed.SQAT.ChiasmaObjects
{
    public struct DataCommentData
    {
        public const String COMMENT = "comment";
    }

    public struct DataIdentifierData
    {
        public const String IDENTIFIER = "identifier";
    }

    public struct DataIdentityData
    {
        public const String ID = "id";
    }

    public struct AlleleResultData
    {
        public const String ID = "id";
        public const String NAME = "name";
        public const String COMPLEMENT = "complement";
        public const String TABLE = "allele_result";
    }

    public struct BarCodeData
    {
        public const String BAR_CODE = "barcode";
        public const String BAR_CODE_COLUMN = "code";
        public const String CODE_LENGTH = "code_length";
        public const String IDENTIFIABLE_ID = "identifiable_id";
        public const String KIND = "kind";
        public const String TABLE_EXTERNAL = "external_barcode";
        public const String TABLE_INTERNAL = "internal_barcode";
    }

    public struct IndividualData
    {
        public const String COMMENT = "comment";
        public const String EXTERNAL_NAME = "external_name";
        public const String FATHER_ID = "father_id";
        public const String FATHER_IDENTIFIER = "father_identifier";
        public const String FATHER_PREFIX = "father_";
        public const String IDENTIFIER = "identifier";
        public const String IDENTIFIER_FILTER = "identifier_filter";
        public const String INDIVIDUAL_ID = "id";
        public const String INDIVIDUAL_USAGE = "individual_usage";
        public const String MOTHER_ID = "mother_id";
        public const String MOTHER_IDENTIFIER = "mother_identifier";
        public const String MOTHER_PREFIX = "mother_";
        public const String SAMPLE_COUNT = "sample_count";
        public const String SEX_ID = "sex_id";
        public const String SPECIES_ID = "species_id";
        public const String TABLE = "individual";
    }

    public struct InternalReportData
    {
        public const String COMMENT = "comment";
        public const String IDENTIFIER = "identifier";
        public const String IDENTIFIER_FILTER = "identifier_filter";
        public const String INTERNAL_REPORT_ID = "id";
        public const String FULL_INTERNAL_REPORT_ID = "internal_report_id";
        public const String IS_BULK = "is_bulk";
        public const String PROJECT_ID = "project_id";
        public const String SETTINGS = "settings";
        public const String SOURCE = "source";
        public const String TABLE = "internal_report";
        public const String USER_ID = "authority_id";
    }

    public struct InternalReportIndvStatData
    {
        public const String DUPL_FAIL = "dupl_fail";
        public const String DUPL_TEST = "dupl_test";
        public const String INDIVIDUAL = "individual";
        public const String INDIVIDUAL_ID = "individual_id";
        public const String INH_FAIL = "inh_fail";
        public const String INH_TEST = "inh_test";
        public const String INTERNAL_REPORT_ID = "internal_report_id";
        public const String SUCCESS_RATE = "success_rate";
        public const String TABLE = "internal_report_indv_stat";
    }

    public struct InternalReportMarkerStatData
    {
        public const String CTRL_INH_FAIL = "ctrl_inh_fail";
        public const String CTRL_INH_TEST = "ctrl_inh_test";
        public const String DUPL_FAIL = "dupl_fail";
        public const String DUPL_TEST = "dupl_test";
        public const String INH_FAIL = "inh_fail";
        public const String INH_TEST = "inh_test";
        public const String INTERNAL_REPORT_ID = "internal_report_id";
        public const String MARKER = "marker";
        public const String MARKER_ID = "marker_id";
        public const String NA = "NA";
        public const String TABLE = "internal_report_marker_stat";
        public const String XX = "XX";
        public const String XY = "XY";
        public const String YY = "YY";
        public const String COMMENT = "comment";
    }

    public struct InternalReportResultData
    {
        public const String INDIVIDUAL_ID = "individual_id";
        public const String INTERNAL_REPORT_ID = "internal_report_id";
        public const String MARKER_ID = "marker_id";
        public const String TABLE = "internal_report_result";
        public const String TOP_ALLELE_RESULT_ID = "top_allele_result_id";
    }

    public struct SampleData
    {
        public const String COMMENT = "comment";
        public const String CONCENTRATION_CURRENT = "concentration_current";
        public const String CONCENTRATION_CURRENT_DEVICE_ID = "concentration_current_device_id";
        public const String CONCENTRATION_CUSTOMER = "concentration_customer";
        public const String EXTERNAL_NAME = "external_name";
        public const String IDENTIFIER = "identifier";
        public const String IDENTIFIER_FILTER = "identifier_filter";
        public const String INDIVIDUAL_ID = "individual_id";
        public const String INDIVIDUAL_PREFIX = "individual_";
        public const String PLATE_ID = "plate_id";
        public const String POSITION_X = "pos_x";
        public const String POSITION_Y = "pos_y";
        public const String POSITION_Z = "pos_z";
        public const String SAMPLE_ID = "id";
        public const String SAMPLE_SERIES_ID = "sample_series_id";
        public const String STATE_ID = "state_id";
        public const String TABLE = "sample";
        public const String TUBE_ID = "tube_id";
        public const String VOLUME_CURRENT = "volume_current";
        public const String VOLUME_CUSTOMER = "volume_customer";
        public const String SAMPLE_SERIES_IDENTIFIER = "sample_series_identifier";
        public const String FRAGMENT_LENGTH = "fragment_length";
        public const String MOLAR_CONCENTRATION = "molar_concentration";
        public const String IS_HIGHLIGHTED = "is_highlighted";
        public const String FRAGMENT_LENGTH_DEVICE_ID = "fragment_length_device_id";
        public const String MOLAR_CONCENTRATION_DEVICE_ID = "molar_concentration_device_id";
        public const String METHOD = "method";
        public const String SEQ_INFO_PREFIX = "seq_info_";
        public const String SEQ_STATE_ID = "seq_state_category_id";
        public const String SEQ_TYPE_ID = "seq_type_category_id";
        public const String SEQ_APPLICATION_ID = "seq_application_category_id";
        public const String TAG_INDEX_ID = "tag_index_id";
    }

    public struct UserData
    {
        public const String ACCOUNT_STATUS = "account_status";
        public const String COMMENT = "comment";
        public const String IDENTIFIER = "identifier";
        public const String NAME = "name";
        public const String TABLE = "authority";
        public const String USER_ID = "id";
        public const String USER_TYPE = "user_type";
        public const String BAR_CODE_LENGTH = "barcode_length";
    }

    public class DataServerChiasmaDB
    {
        private Int32 MyCommandTimeout;
        private SqlConnection MyConnection;
        private String MyConnectionString;
        private SqlTransaction MyTransaction;

        public DataServerChiasmaDB(String connectionString)
        {
            MyCommandTimeout = 3600;
            MyConnectionString = connectionString;
        }

        public DataServerChiasmaDB(String userName, String password)
            : this(userName, password, "GTDB2_devel")
        {

        }

        public DataServerChiasmaDB(String userName, String password, String database)
        {
            //MyCommandTimeout = Settings.Default.DatabaseCommandTimeout;
            MyCommandTimeout = 3600;
            SetConnectionString(userName, password, database);
        }

        public Boolean AuthenticateApplication(String applicationName,
                                   String applicationVersion)
        {
            //Returns true if the application with name appName and version
            //appVersion is allowed to connect.
            String cmdText;

            cmdText = "SELECT COUNT(*) FROM application_version " +
                      "WHERE identifier = '" + applicationName + "' AND " +
                      "version = '" + applicationVersion + "'";
            return (this.ExecuteScalar(cmdText) > 0);
        }


        public Int32 CommandTimeout
        {
            get
            {
                return MyCommandTimeout;
            }
            set
            {
                MyCommandTimeout = value;
            }
        }

        private void AssertDatabaseConnection()
        {
            //If the database connection is broken, try to reconnect.
            if (MyConnection.State == ConnectionState.Closed || MyConnection.State == ConnectionState.Broken)
            {
                Connect();
            }
        }

        public void BeginTransaction()
        {
            AssertDatabaseConnection();

            if (IsNull(MyTransaction))
            {
                MyTransaction = MyConnection.BeginTransaction();
            }
            else
            {
                throw new Exception("Transaction already active.");
            }
        }

        public void RollbackTransaction()
        {
            if (IsNotNull(MyTransaction))
            {
                try
                {
                    MyTransaction.Rollback();
                }
                catch
                {
                }
                MyTransaction = null;
            }
            else
            {
                throw new Exception("Unable to rollback inactive transaction.");
            }
        }

        public void CommitTransaction()
        {
            if (IsNotNull(MyTransaction))
            {
                MyTransaction.Commit();
                MyTransaction = null;
            }
            else
            {
                throw new Exception("Unable to commit inactive transaction.");
            }
        }


        public Boolean Connect()
        {
            //Opens the database connection.
            string dummy;
            try
            {
                MyConnection = new SqlConnection(MyConnectionString);
                MyConnection.Open();
                // Enable extraction from xml variables within db-procedures (p_CreateInternalReport)
                ExecuteSync("SET ARITHABORT ON");
            }
            catch (Exception ex)
            {
                dummy = ex.GetType().ToString(); 
                throw new ConnectDatabaseException();
            }

            return (MyConnection.State == ConnectionState.Open);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ExecuteSync(string cmdText)
        {
            SqlCommand cmd;

            cmd = new SqlCommand(cmdText, MyConnection);
            cmd.CommandTimeout = MyCommandTimeout;
            cmd.ExecuteNonQuery();
        }


        public void Disconnect()
        {
            //Closes the current database connection.
            if ((MyConnection.State == ConnectionState.Open) ||
                 (MyConnection.State == ConnectionState.Fetching))
            {
                MyConnection.Close();
            }
        }

        private Int32 ExecuteCommand(SqlCommandBuilder commandBuilder)
        {
            return GetCommand(commandBuilder).ExecuteNonQuery();
        }

        private Int32 ExecuteCommand(String sqlQuery)
        {
            return GetCommand(sqlQuery).ExecuteNonQuery();
        }

        public Int32 ExecuteScalar(SqlCommandBuilder commandBuilder)
        {
            return ExecuteScalar(commandBuilder.GetCommand());
        }

        public Int32 ExecuteScalar(String sqlQuery)
        {
            return Convert.ToInt32(GetCommand(sqlQuery).ExecuteScalar());
        }

        public DataReader GetAlleleResults()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetAlleleResults");

            return GetReader(commandBuilder);
        }

        public Int32 GetColumnLength(String tableName, String columnName)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetColumnLength");
            commandBuilder.AddParameter("table", tableName);
            commandBuilder.AddParameter("column", columnName);

            return ExecuteScalar(commandBuilder);
        }

        public SqlCommand GetCommand(SqlCommandBuilder commandBuilder)
        {
            return GetCommand(commandBuilder.GetCommand());
        }

        public SqlCommand GetCommand(String sqlQuery)
        {
            SqlCommand command;

            AssertDatabaseConnection();

            if (IsNull(MyTransaction))
            {
                command = new SqlCommand(sqlQuery, MyConnection);
            }
            else
            {
                command = new SqlCommand(sqlQuery, MyConnection, MyTransaction);
            }
            command.CommandTimeout = CommandTimeout;
            return command;
        }

        public DataReader GetUserCurrent()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetUserCurrent");

            return GetRow(commandBuilder);
        }

        public DataReader GetUsers()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetUsers");

            return GetReader(commandBuilder);
        }

        public Boolean HasPendingTransaction()
        {
            return (MyTransaction != null);
        }

        private static Boolean IsEmpty(ICollection collection)
        {
            return ((collection == null) || (collection.Count == 0));
        }

        private static Boolean IsEmpty(String testString)
        {
            return (testString == null) || (testString.Trim().Length == 0);
        }

        private static Boolean IsNull(Object testObject)
        {
            return (testObject == null);
        }

        private static Boolean IsNotNull(Object testObject)
        { 
            return !IsNull(testObject);
        }

        public void AddInternalReportItemStatistics(DataTable internalReportItemStatTable)
        {
            // Can refer to internal_report_indv_stat OR internal_report_sample_stat
            AddTableData(internalReportItemStatTable);
        }

        public void AddInternalReportMarkerStatistics(DataTable internalReportMarkerStatTable)
        {
            AddTableData(internalReportMarkerStatTable);
        }

        public void AddInternalReportResultPlates(DataTable internalReportResultPlateTable)
        {
            AddTableData(internalReportResultPlateTable);
        }

        public void AddInternalReportResults(DataTable internalReportResultTable)
        {
            AddTableData(internalReportResultTable);
        }

        private void AddTableData(DataTable table)
        {
            SqlBulkCopy bulkCopy;

            AssertDatabaseConnection();

            if (IsNull(MyTransaction))
            {
                bulkCopy = new SqlBulkCopy(MyConnection,
                                        SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints,
                                        null);
            }
            else
            {
                bulkCopy = new SqlBulkCopy(MyConnection,
                                         SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints,
                                         MyTransaction);
            }
            bulkCopy.BulkCopyTimeout = MyCommandTimeout;
            bulkCopy.DestinationTableName = "dbo." + table.TableName;
            bulkCopy.WriteToServer(table);
        }

        public DataReader CreateInternalReport(String identifier, Int32 userId, Int32 projectId, String source,
    Boolean isBulk, String comment, String settings)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_CreateInternalReport");

            commandBuilder.AddParameter(InternalReportData.IDENTIFIER, identifier);
            commandBuilder.AddParameter(InternalReportData.USER_ID, userId);
            commandBuilder.AddParameter(InternalReportData.PROJECT_ID, projectId);
            commandBuilder.AddParameter(InternalReportData.SOURCE, source);
            commandBuilder.AddParameter(InternalReportData.IS_BULK, isBulk);
            commandBuilder.AddParameter(InternalReportData.COMMENT, comment);
            commandBuilder.AddParameter(InternalReportData.SETTINGS, settings);

            return GetRow(commandBuilder);
        }

        public DataReader GetIndividual(Int32 individualId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetIndividualById");
            commandBuilder.AddParameter(IndividualData.INDIVIDUAL_ID, individualId);

            return GetRow(commandBuilder);
        }

        public DataReader GetSample(Int32 sampleId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetSampleById");
            commandBuilder.AddParameter(SampleData.SAMPLE_ID, sampleId);

            return GetRow(commandBuilder);
        }

        public DataReader GetInternalReportById(Int32 internalReportId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetInternalReportById");
            commandBuilder.AddParameter(InternalReportData.INTERNAL_REPORT_ID, internalReportId);

            return GetRow(commandBuilder);
        }

        public DataReader GetInternalReportByIdentifier(String identifier)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetInternalReportByIdentifier");
            commandBuilder.AddParameter(InternalReportData.IDENTIFIER, identifier);

            return GetRow(commandBuilder);
        }

        private DataReader GetReader(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder, CommandBehavior.SingleResult);
        }

        private DataReader GetReader(SqlCommandBuilder commandBuilder,
                                  CommandBehavior commandBehavior)
        {
            SqlCommand dataReaderCommand;
            dataReaderCommand = GetCommand(commandBuilder);

            return new DataReader(dataReaderCommand.ExecuteReader(commandBehavior), dataReaderCommand);
        }

        private DataReader GetRow(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder, CommandBehavior.SingleRow |
                            CommandBehavior.SingleResult);
        }

        private void SetConnectionString(String userName, String password, String database)
        {
            String dataServerAdress;
            dataServerAdress = "mm-wchs001";
            if (IsEmpty(userName) || IsEmpty(password))
            {
                MyConnectionString = "data source=" + dataServerAdress +
                                   ";integrated security=true;" +
                                   "initial catalog=" + database + ";";
            }
            else
            {
                MyConnectionString = "data source=" + dataServerAdress +
                                   ";integrated security=false;" +
                                   "initial catalog=" + database + ";" +
                                   "user id=" + userName + ";" +
                                   "pwd=" + password + ";";
            }
        }
    }
}
