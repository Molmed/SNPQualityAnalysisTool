using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;

namespace Molmed.SQAT.ServiceObjects
{
	public class DataServer
	{
		private SqlConnection MyConnection;
		private SqlCommand MyCommand;
		private DataSet MyDataSet;
		private bool MyDataDeliveryFlag;   //True when has data to deliver, false otherwise.
		private Thread MyThread;
		private int MyAsyncTimeout;
		private int MySyncTimeout;

        public const int EXPERIMENT_STAT_TABLE_INDEX = 0;
        public const int ITEM_STAT_TABLE_INDEX = 1;
        public const int CONTROL_ITEM_STAT_TABLE_INDEX = 2;

		//Declare event which fires when some change has occurred with
		//the pending genotype statuses (either new pending changes or a submission).
		public event EventHandler GenotypeStatusChange;

		//Declare event which fires when data is ready for delivery from
		//an asynchronus method.
		private event EventHandler DataReady;

		//Declare event which fires when an asynchronus method encountered
		//an error.
		private event ThreadExceptionEventHandler DataError;


        private const string GET_PLATE_SELECTION_STRING = "SELECT p.plate_id, p.identifier AS [Plate] FROM #plate pt "
            + "INNER JOIN dbo.plate p ON p.plate_id = pt.plate_id "
            + "ORDER BY p.identifier ";

        private const string GET_SINGLE_SELECTION_STRING = 
            "SELECT id, [Name], [Type] FROM "
            + "( "
            + "SELECT smp.sample_id AS id, smp.identifier AS [Name], 'Sample' AS [Type] FROM #picked_sample ps "
            + "INNER JOIN dbo.sample smp ON smp.sample_id = ps.sample_id "
            + "UNION "
            + "SELECT idv.individual_id AS id, idv.identifier AS [Name], 'Individual' AS [Type] FROM #picked_individual ps "
            + "INNER JOIN dbo.individual idv ON idv.individual_id = ps.individual_id "
            + "UNION "
            + "SELECT a.assay_id AS id, a.identifier AS [Name], 'Assay' AS [Type] FROM #picked_assay pa "
            + "INNER JOIN dbo.assay a ON a.assay_id = pa.assay_id "
            + "UNION "
            + "SELECT m.marker_id AS id, m.identifier AS [Name], 'Marker' AS [Type] FROM #picked_marker pm "
            + "INNER JOIN dbo.marker m ON m.marker_id = pm.marker_id "
            + ") all_objects "
            + "ORDER BY [Type], [Name] ";

        private const string GET_GROUPS_SELECTION_STRING = 
            "SELECT w.wset_id, w.identifier AS [Group], wt.name AS [Type] "
            + "FROM dbo.wset w "
            + "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id "
            + "WHERE w.wset_id IN ( "
            + "SELECT wset_id FROM #genotype_wset "
            + "UNION SELECT wset_id FROM #marker_wset "
            + "UNION SELECT wset_id FROM #assay_wset "
            + "UNION SELECT wset_id FROM #sample_wset "
            + "UNION SELECT wset_id FROM #individual_wset "
            + ") "
            + "ORDER BY w.identifier ";


        private const string GET_APPROVED_RESULT_STRING =
            "SELECT ag.genotype_id, ist.identifier AS [Item], "
            + "es.identifier AS [Experiment], ag.alleles AS [Alleles]"
            + "FROM #approved_genotype ag "
            + "INNER JOIN #item_stat ist ON ist.item_id = ag.item_id "
            + "INNER JOIN #experiment_stat es ON es.experiment_id = ag.experiment_id "
            + "ORDER BY ist.identifier, es.identifier ";

        private const string GET_ITEM_STAT_STRING =
             "SELECT "
            + "ist.item_id, "
            + "ist.identifier AS [Item//CN], "
            + "CASE WHEN ISNULL(ec.run_experiments, 0) > 0 THEN CAST(ist.success_experiments AS REAL)/CAST(ec.run_experiments AS REAL) ELSE 0 END AS [Success expm//PN], "
            + "CASE WHEN ISNULL(ecnf.run_experiments, 0) > 0 THEN CAST(ist.success_experiments AS REAL)/CAST(ecnf.run_experiments AS REAL) ELSE 0 END AS [Success nf expm//PN], "
            + "ist.total_gt AS [Total gt//IN], "
            + "ist.no_result_gt AS [No result gt//IF], "
            + "ist.failed_dup_exp AS [Failed dup expm//IF], "
            + "ist.failed_inh_exp [Failed inh expm//IF], "
            + "ist.failed_HW_exp [Failed HW expm//IF], "
            + "ist.rejected_gt [Rejected gt//IN], "
            + "ist.chrX_warning_exp [Chr X warn expm//IF] "
            + "FROM #item_stat ist "
            + "INNER JOIN #experiment_count ec ON ec.item_id = ist.item_id "
            + "INNER JOIN #experiment_count_nonfailed ecnf ON ecnf.item_id = ist.item_id ";

        private const string GET_CONTROL_ITEM_STAT_STRING =
             "SELECT item_id, "
            + "identifier AS [Item//CN], "
            + "failed_exp AS [Failed expm//IF], "
            + "tested_exp AS [Tested expm//IN] "
            + "FROM #control_item_stat ";

		public DataServer(string connectionString, string isolationLevel, int syncTimeout, int asyncTimeout)
		{
			MyConnection = new SqlConnection(connectionString);
			MyConnection.Open();
			MySyncTimeout = syncTimeout;
			MyAsyncTimeout = asyncTimeout;
			MyDataDeliveryFlag = false;
            
			ExecuteSync("SET TRANSACTION ISOLATION LEVEL " + isolationLevel);
		}

        public void SetDataAsyncEventHandlers(EventHandler dataReadyHandler, ThreadExceptionEventHandler dataErrorHandler)
        {
            DataReady = null;
            DataError = null;
            DataReady += dataReadyHandler;
            DataError += dataErrorHandler;
        }

		public void Close()
		{
			if (MyConnection != null)
			{
				MyConnection.Close();
			}
		}

		public void BeginTransaction()
		{
			string cmdText;

			cmdText = "";
			cmdText += "BEGIN TRANSACTION ";
			ExecuteSync(cmdText);
		}

		public void CommitTransaction()
		{
			string cmdText;

			cmdText = "";
			cmdText += "COMMIT TRANSACTION ";
			ExecuteSync(cmdText);
		}

		public void RollbackTransaction()
		{
			string cmdText;

			cmdText = "";
			cmdText += "ROLLBACK TRANSACTION ";
			ExecuteSync(cmdText);
		}

		public static bool IsCorrectVersion(string connectionString, string appName, string appVersion)
		{
			string cmdText;
			SqlConnection cn = null;
			SqlCommand sqlCmd;
			int rows;

			try
			{
				cmdText = "";
				cmdText += "SELECT COUNT(*) FROM dbo.application_version ";
				cmdText += "WHERE identifier = '" + appName + "' AND ";
				cmdText += "version = '" + appVersion + "'";

				cn = new SqlConnection(connectionString);
				cn.Open();
				sqlCmd = new SqlCommand(cmdText, cn);
				rows = Convert.ToInt32(sqlCmd.ExecuteScalar());
				return (rows > 0);
			}
			catch (Exception e)
			{
				throw(e);
			}
			finally
			{
				if (cn != null)
				{
					cn.Close();
				}
			}
		}

		public static string GetDatabaseName(string connectionString)
		{
			string cmdText;
			SqlConnection cn = null;
			SqlCommand sqlCmd;
			object retValue;
			string retString;

			try
			{
				cmdText = "";
				cmdText += "SELECT DB_NAME() ";

				cn = new SqlConnection(connectionString);
				cn.Open();
				sqlCmd = new SqlCommand(cmdText, cn);
				retValue = sqlCmd.ExecuteScalar();
				retString = (retValue != null ? retValue.ToString() : "");

				return retString;
			}
			catch (Exception e)
			{
				throw(e);
			}
			finally
			{
				if (cn != null)
				{
					cn.Close();
				}
			}					
		}

		public static string GetUserName(string connectionString)
		{
			string cmdText;
			SqlConnection cn = null;
			SqlCommand sqlCmd;
			object retValue;

			try
			{
				cmdText = "";
				cmdText += "SELECT name FROM dbo.authority WHERE identifier = SYSTEM_USER ";

				cn = new SqlConnection(connectionString);
				cn.Open();
				sqlCmd = new SqlCommand(cmdText, cn);
				retValue = sqlCmd.ExecuteScalar();
				if (retValue == null)
				{
					throw new Exception("Your user name is not present in the database.");
				}

				return retValue.ToString();
			}
			catch (Exception e)
			{
				throw(e);
			}
			finally
			{
				if (cn != null)
				{
					cn.Close();
				}
			}
		}

        public string GetCurrentUserName()
        {
            DataTable t;

            t = FillTableSyncNoFormat("SELECT name FROM authority WHERE identifier = SYSTEM_USER");

            if (t.Rows.Count != 1)
            {
                throw new Exception("Unable to retrieve the user name.");
            }

            return t.Rows[0][0].ToString();
        }

        public int GetCurrentUserId()
        {
            try
            {
                return ExecuteIntSync("SELECT authority_id FROM authority WHERE identifier = SYSTEM_USER");
            }
            catch
            {
                throw new Exception("Unable to retrieve the user id.");
            }
        }

        public static StringCollection GetGenotypeStatuses(string connectionString, int timeout)
        {
            return GetSingleColumn("SELECT name FROM dbo.status ", connectionString, timeout);
        }

        public static StringCollection GetIndividualTypes(string connectionString, int timeout)
        {
            return GetSingleColumn("SELECT name FROM individual_type ", connectionString, timeout);
        }

        public static StringCollection GetReferenceSets(string connectionString, int timeout)
        {
            return GetSingleColumn("SELECT identifier FROM reference_set ORDER BY identifier ", connectionString, timeout);
        }

        public static StringCollection GetItemsInReferenceSet(string setName, string connectionString, int timeout)
        {
            string cmdText;

            cmdText = "";
            cmdText += "DECLARE @referenceSetId TINYINT ";
            cmdText += "SELECT @referenceSetId = reference_set_id FROM reference_set WHERE identifier = '" + setName + "' ";
            cmdText += "SELECT DISTINCT item FROM reference_genotype WHERE reference_set_id = @referenceSetId ";

            return GetSingleColumn(cmdText, connectionString, timeout);
        }

        public static StringCollection GetExperimentsInReferenceSet(string setName, string connectionString, int timeout)
        {
            string cmdText;

            cmdText = "";
            cmdText += "DECLARE @referenceSetId TINYINT ";
            cmdText += "SELECT @referenceSetId = reference_set_id FROM reference_set WHERE identifier = '" + setName + "' ";
            cmdText += "SELECT DISTINCT experiment FROM reference_genotype WHERE reference_set_id = @referenceSetId ";

            return GetSingleColumn(cmdText, connectionString, timeout);
        }

        public static DataTable GetResultsInReferenceSet(string setName, StringCollection includedItems, StringCollection includedExperiments, string connectionString, int timeout)
        {

            //Returns all genotypes from selected items and experiments.
            string cmdText;
            SqlConnection cn = null;
            SqlCommand sqlCmd;
            SqlDataAdapter dAdapter;
            DataSet dSet;

            try
            {
                cn = new SqlConnection(connectionString);
                cn.Open();
                sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = timeout;
                sqlCmd.Connection = cn;

                cmdText = "";
                cmdText += "CREATE TABLE #comparison_item(identifier VARCHAR(255) NOT NULL PRIMARY KEY) ";
                cmdText += "CREATE TABLE #comparison_experiment(identifier VARCHAR(255) NOT NULL PRIMARY KEY) ";
                sqlCmd.CommandText = cmdText;
                sqlCmd.ExecuteNonQuery();

                foreach (string tempVal in includedItems)
                {
                    cmdText = "";
                    cmdText += "INSERT INTO #comparison_item (identifier) VALUES ('" + tempVal + "')";
                    sqlCmd.CommandText = cmdText;
                    sqlCmd.ExecuteNonQuery();
                }

                foreach (string tempVal in includedExperiments)
                {
                    cmdText = "";
                    cmdText += "INSERT INTO #comparison_experiment (identifier) VALUES ('" + tempVal + "')";
                    sqlCmd.CommandText = cmdText;
                    sqlCmd.ExecuteNonQuery();
                }

                cmdText = "";
                cmdText += "DECLARE @referenceSetId INTEGER ";
                cmdText += "";
                cmdText += "SELECT @referenceSetId = reference_set_id FROM reference_set WHERE identifier COLLATE latin1_general_ci_as = '" + setName + "' COLLATE latin1_general_ci_as ";
                cmdText += "";
                cmdText += "SELECT item, experiment, alleles ";
                cmdText += "FROM reference_genotype ";
                cmdText += "WHERE item COLLATE latin1_general_ci_as IN (SELECT identifier COLLATE latin1_general_ci_as FROM #comparison_item) AND ";
                cmdText += "experiment COLLATE latin1_general_ci_as IN (SELECT identifier COLLATE latin1_general_ci_as FROM #comparison_experiment) AND ";
                cmdText += "reference_set_id = @referenceSetId ";

                sqlCmd.CommandText = cmdText;
                dAdapter = new SqlDataAdapter();
                dAdapter.SelectCommand = sqlCmd;
                dSet = new DataSet();

                dAdapter.Fill(dSet);

                return dSet.Tables[0];
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                if (cn != null)
                {
                    cn.Close();
                }
            }
        }

        private static StringCollection GetSingleColumn(string cmdText, string connectionString, int timeout)
        {
            SqlConnection cn = null;
            SqlCommand sqlCmd;
            SqlDataReader dr;
            StringCollection tempStrings;

            try
            {
                tempStrings = new System.Collections.Specialized.StringCollection();
                cn = new SqlConnection(connectionString);
                cn.Open();
                sqlCmd = new SqlCommand(cmdText, cn);
                sqlCmd.CommandTimeout = timeout;
                dr = sqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    tempStrings.Add(dr.GetString(0));
                }
                dr.Close();

                return tempStrings;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                if (cn != null)
                {
                    cn.Close();
                }
            }
        }

        private static void ExecuteCommand(string cmdText, string connectionString)
        {
            SqlConnection cn = null;
            SqlCommand sqlCmd;

            try
            {
                cn = new SqlConnection(connectionString);
                cn.Open();
                sqlCmd = new SqlCommand(cmdText, cn);
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                if (cn != null)
                {
                    cn.Close();
                }
            }
        }

		public void UpdateGenotypeStatus(int [] genotypeID, string newStatus)
		{
			string cmdText;
            string statusCode;

			try
			{
                switch (newStatus.ToUpper())
                {
                    case "LOAD":
                        statusCode = "1";
                        break;
                    case "REJECTED":
                        statusCode = "2";
                        break;
                    default:
                        throw new Exception("Unknown status code encountered.");
                }

				BeginTransaction();
				for (int i=0; i < genotypeID.Length; i++)
				{
					//Delete any previous changes for this genotype and insert the new one.
					cmdText = "";
					cmdText = "DELETE FROM #pending_status WHERE genotype_id = " + genotypeID[i].ToString() + " ";
					cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
					cmdText += "VALUES (" + genotypeID[i].ToString() + ", " + statusCode + ") ";

					ExecuteSync(cmdText);
				}
				CommitTransaction();
				OnGenotypeStatusChange();
			}
			catch (Exception e)
			{
				RollbackTransaction();
				throw(e);
			}
		}

		public void SubmitGenotypeStatus()
		{
			string cmdText;

			cmdText = "";
			cmdText += "EXEC pSQAT_SubmitStatus ";

			ExecuteSync(cmdText);
			OnGenotypeStatusChange();
		}

		public void ClearGenotypeStatus()
		{
			string cmdText;

			cmdText = "";
			cmdText += "TRUNCATE TABLE #pending_status ";

			ExecuteSync(cmdText);
			OnGenotypeStatusChange();
		}

		public bool IsPendingGenotypeStatus()
		{
			string cmdText;
			int numberPending;

			cmdText = "";
			cmdText += "SELECT COUNT(*) FROM #pending_status ";
			numberPending = ExecuteIntSync(cmdText);
			return numberPending > 0 ? true : false;
		}

        public void ApplyExperimentCutoff(int cutoffPercent)
        {
            string cmdText;

            BeginTransaction();

            try
            {
                cmdText = "";
                cmdText += "DECLARE @rejectedStatusId TINYINT ";
                cmdText += "DECLARE @rejected_experiment TABLE (experiment_id INTEGER NOT NULL PRIMARY KEY) ";

                cmdText += "SELECT @rejectedStatusId = status_id FROM dbo.status WHERE name = 'REJECTED' ";

                cmdText += "INSERT INTO @rejected_experiment (experiment_id) ";
                cmdText += "SELECT experiment_id FROM #experiment_stat WHERE (100.0 * success_rate) < " + cutoffPercent + " ";

                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ( ";
                cmdText += "SELECT genotype_id FROM #all_genotypes WHERE experiment_id IN (SELECT experiment_id FROM @rejected_experiment) ) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "SELECT genotype_id, @rejectedStatusId FROM #all_genotypes ";
                cmdText += "WHERE experiment_id IN (SELECT experiment_id FROM @rejected_experiment) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();

            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw (ex);
            }
        }

        public void ApplyItemCutoff(int cutoffPercent, bool allExperiments)
        {
            string cmdText;

            BeginTransaction();

            try
            {
                cmdText = "";
                cmdText += "DECLARE @rejectedStatusId TINYINT ";
                cmdText += "DECLARE @rejected_item TABLE (item_id INTEGER NOT NULL PRIMARY KEY) ";

                cmdText += "SELECT @rejectedStatusId = status_id FROM dbo.status WHERE name = 'REJECTED' ";

                if (allExperiments)
                {
                    cmdText += "INSERT INTO @rejected_item (item_id) ";
                    cmdText += "SELECT item_id FROM (" + GET_ITEM_STAT_STRING + ") t WHERE (100.0 * [Success expm//PN]) < " + cutoffPercent + " ";
                }
                else
                {
                    cmdText += "INSERT INTO @rejected_item (item_id) ";
                    cmdText += "SELECT item_id FROM (" + GET_ITEM_STAT_STRING + ") t WHERE (100.0 * [Success nf expm//PN]) < " + cutoffPercent + " ";
                }

                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ( ";
                cmdText += "SELECT genotype_id FROM #all_genotypes WHERE item_id IN (SELECT item_id FROM @rejected_item) ) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "SELECT genotype_id, @rejectedStatusId FROM #all_genotypes ";
                cmdText += "WHERE item_id IN (SELECT item_id FROM @rejected_item) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();

            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw (ex);
            }
        }

		public void ClearRerunExperiments()
		{
			string cmdText;

			cmdText = "";
			cmdText += "TRUNCATE TABLE #rerun_experiment ";

			ExecuteSync(cmdText);
		}

		public void AddRerunExperiment(int experimentId)
		{
			string cmdText;

			cmdText = "";
			cmdText += "INSERT INTO #rerun_experiment (experiment_id) VALUES ( ";
			cmdText += experimentId.ToString() + ") ";

			ExecuteSync(cmdText);
		}

		public DataTable GetProjects()
		{
			string cmdText;

			cmdText = "";
			cmdText += "SELECT p.project_id AS id, p.identifier ";
			cmdText += "FROM dbo.project p ";
			cmdText += "ORDER BY p.identifier ";

			return FillTableSync(cmdText);
		}

        public bool GetCurrentUserPermittedInProject(string projectIdentifier)
        {
            string cmdText;

            //Check if the user has full permissions.
            if (this.GetCurrentUserHasFullPermissions())
            {
                return true;
            }

            //Not full permissions, check if the user has specific permissions.
            cmdText = "";
            cmdText += "SELECT COUNT(*) ";
            cmdText += "FROM dbo.permission p ";
            cmdText += "INNER JOIN dbo.authority a ON a.authority_id = p.authority_id ";
            cmdText += "INNER JOIN dbo.project prj ON prj.project_id = p.project_id "; 
            cmdText += "WHERE a.identifier = SYSTEM_USER ";
            cmdText += "AND prj.identifier = '" + projectIdentifier + "' ";

            return (this.ExecuteIntSync(cmdText) > 0);
        }

        public bool GetCurrentUserHasFullPermissions()
        {
            string cmdText;

            cmdText = "";
            cmdText += "SELECT COUNT(*) ";
            cmdText += "FROM dbo.authority a ";
            cmdText += "WHERE a.identifier = SYSTEM_USER ";
            cmdText += "AND (a.user_type = 'Administrator' OR a.user_type = 'Developer') "; 

            return (this.ExecuteIntSync(cmdText) > 0);
        }

		public DataTable GetGroups(int projectId)
		{
			string cmdText;

			cmdText = "";
			cmdText += "SELECT w.wset_id, w.identifier, w.description, wt.name AS type FROM dbo.wset w ";
			cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
			cmdText += "AND ";
			cmdText += "(wt.name = 'GenotypeSet' ";
			cmdText += "OR wt.name = 'MarkerSet' ";
            cmdText += "OR wt.name = 'AssaySet' ";
            cmdText += "OR wt.name = 'IndividualSet' ";
            cmdText += "OR wt.name = 'SampleSet' ";
			cmdText += "OR wt.name = 'PlateSet') ";
            cmdText += "WHERE w.project_id = " + projectId.ToString() + " ";
            cmdText += "AND w.deleted = 0 ";
			cmdText += "ORDER BY type, w.identifier ";

			return FillTableSync(cmdText);
		}

        public DataTable GetPlatesInGroup(int parentId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "DECLARE @plate_kind_id TINYINT ";
            cmdText += "SELECT @plate_kind_id = kind_id FROM kind WHERE name = 'RESULT_PLATE' ";
            cmdText += "SELECT p.plate_id, p.identifier, p.description FROM dbo.plate p ";
            cmdText += "WHERE p.plate_id IN ( ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member WHERE wset_id = " + parentId + " ";
            cmdText += "AND kind_id = @plate_kind_id) ";
            cmdText += "ORDER BY p.identifier ";

            return FillTableSync(cmdText);
        }

        public DataTable GetPlatesInProject(int projectId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "SELECT p.plate_id, p.identifier, p.description FROM dbo.plate p ";
            cmdText += "WHERE p.project_id = " + projectId.ToString() + " ";

            return FillTableSync(cmdText);
        }

        public string GetSessionProject()
        {
            string cmdText;
            DataTable projectTable;

            //Find the projects to which the currently analyzed plates belong.
            cmdText = "";
            cmdText += "SELECT DISTINCT prj.identifier FROM project prj ";
            cmdText += "INNER JOIN plate p ON p.project_id = prj.project_id ";
            cmdText += "INNER JOIN #plate tp ON tp.plate_id = p.plate_id ";

            projectTable = FillTableSync(cmdText);

            if (projectTable.Rows.Count != 1)
            {
                //More or less than project found, unable to determine the correct project.
                return null;
            }
            else
            {
                //Exactly one project found, return the name.
                return projectTable.Rows[0][0].ToString();
            }
        }

        public DataTable GetSessions(int projectId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "SELECT w.wset_id, w.identifier, w.description, wt.name AS type FROM dbo.wset w ";
            cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
            cmdText += "AND ";
            cmdText += "(wt.name = 'AnalyzedGenotypeSet' ";
            cmdText += "OR wt.name = 'ResultGenotypeSet' ";
            cmdText += "OR wt.name = 'SavedSession' ";
            cmdText += "OR wt.name = 'ApprovedSession') ";
            cmdText += "WHERE w.project_id = " + projectId.ToString() + " ";
            cmdText += "AND w.deleted = 0 ";
            cmdText += "ORDER BY type, w.identifier ";

            return FillTableSync(cmdText);
        }

		public DataTable GetSessions(string parentProjectIdentifier)
		{
			string cmdText;

			cmdText = "";
			cmdText += "DECLARE @projectId INTEGER; ";
			cmdText += "SELECT @projectId = project_id FROM dbo.project ";
			cmdText += "WHERE identifier = '" + parentProjectIdentifier + "'; ";
			cmdText += "SELECT w.wset_id, w.identifier AS [Name], w.description AS [Description] FROM dbo.wset w ";
			cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
			cmdText += "AND ";
			cmdText += "(wt.name = 'AnalyzedGenotypeSet' ";
            cmdText += "OR wt.name = 'ResultGenotypeSet' ";
            cmdText += "OR wt.name = 'SavedSession' ";
            cmdText += "OR wt.name = 'ApprovedSession') ";
            cmdText += "WHERE w.project_id = @projectId ";
			cmdText += "AND w.deleted = 0 ";
			cmdText += "ORDER BY w.identifier ";

			return FillTableSync(cmdText);			
		}

		public void DeleteSession(int wsetId)
		{
			string cmdText;

			cmdText = "";
			cmdText += "UPDATE dbo.wset SET deleted = 1 WHERE wset_id = " + wsetId.ToString() + " ";

			ExecuteSync(cmdText);
		}

        public void UnlockApprovedSession(int wsetId)
        {
            string cmdText;

            this.BeginTransaction();

            try
            {
                //Remove all genotype locks from the session.
                cmdText = "";
                cmdText += "UPDATE dbo.denorm_genotype SET locked_wset_id = NULL WHERE locked_wset_id = " + wsetId.ToString() + " ";

                ExecuteSync(cmdText);

                //Change the type of the session.
                cmdText = "";
                cmdText += "DECLARE @wsetTypeId INTEGER ";
                cmdText += "SELECT @wsetTypeId = wset_type_id FROM dbo.wset_type ";
                cmdText += "WHERE name = 'SavedSession' ";
                cmdText += "UPDATE dbo.wset SET wset_type_id = @wsetTypeId WHERE wset_id = " + wsetId + " ";

                ExecuteSync(cmdText);

                this.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.RollbackTransaction();
                throw (ex);
            }

        }

		public void SaveApprovedSession(SessionSettings settings, string project, string name, string description)
		{
			string cmdText;
			int wsetId;

			try
			{
				BeginTransaction();
				wsetId = SaveSession(settings, project, name, description);
				cmdText = "";
				cmdText += "EXEC pSQAT_Approve " + wsetId.ToString() + " ";
				ExecuteSync(cmdText);
				CommitTransaction();
			}
			catch (Exception e)
			{
				RollbackTransaction();
				throw (e);
			}
		}

	    public void LogResultPlatesInSession(string sessionName, string action)
	    {
	        var cmd = $"exec p_LogSessionByWset '{sessionName}', '{action}'";
            ExecuteSync(cmd);
	    }


		public void SaveNormalSession(SessionSettings settings, string project, string name, string description)
		{
			try
			{
				BeginTransaction();
				SaveSession(settings, project, name, description);
				CommitTransaction();
			}
			catch (Exception e)
			{
				RollbackTransaction();
				throw (e);
			}
		}

		private int SaveSession(SessionSettings settings, string project, string name, string description)
		{
			//Saves a session and returns the id of the new workingset.
			string cmdText;
			int wsetId;

			//Create wset.
			cmdText = "";
			cmdText += "DECLARE @wset_type_id INTEGER ";
			cmdText += "DECLARE @wset_id INTEGER ";
			cmdText += "DECLARE @authority_id INTEGER ";
            cmdText += "DECLARE @project_id INTEGER ";
			cmdText += "SELECT @wset_type_id = wset_type_id FROM dbo.wset_type WHERE name = 'SavedSession' ";
			cmdText += "SELECT @authority_id = authority_id FROM dbo.authority WHERE identifier = SYSTEM_USER ";
			cmdText += "SELECT @wset_id = MAX(wset_id) + 1 FROM dbo.wset WITH (UPDLOCK) ";
            cmdText += "SELECT @project_id = project_id FROM project WHERE identifier = '" + project + "' ";
			cmdText += "INSERT INTO dbo.wset (wset_id, identifier, authority_id, wset_type_id, project_id, description) ";
			cmdText += "VALUES ";
			cmdText += "(@wset_id, '" + name + "', @authority_id, @wset_type_id, @project_id, '" + description + "') ";
			cmdText += "SELECT @wset_id ";
			wsetId = ExecuteIntSync(cmdText);

            //Store selected plates.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'RESULT_PLATE' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.plate_id, @kind_id ";
            cmdText += "FROM #plate t ";
            ExecuteSync(cmdText);

            //Store selected genotype sets (for backward compatibility).
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.wset_id, @kind_id ";
            cmdText += "FROM #genotype_wset t ";
            ExecuteSync(cmdText);

            //Store selected marker sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.wset_id, @kind_id ";
            cmdText += "FROM #marker_wset t ";
            ExecuteSync(cmdText);

            //Store selected assay sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.wset_id, @kind_id ";
            cmdText += "FROM #assay_wset t ";
            ExecuteSync(cmdText);

            //Store selected sample sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.wset_id, @kind_id ";
            cmdText += "FROM #sample_wset t ";
            ExecuteSync(cmdText);

            //Store selected individual sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.wset_id, @kind_id ";
            cmdText += "FROM #individual_wset t ";
            ExecuteSync(cmdText);

            //Store selected samples.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'SAMPLE' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.sample_id, @kind_id ";
            cmdText += "FROM #picked_sample t ";
            ExecuteSync(cmdText);

            //Store selected assays.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'ASSAY' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.assay_id, @kind_id ";
            cmdText += "FROM #picked_assay t ";
            ExecuteSync(cmdText);

            //Store selected individuals.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'INDIVIDUAL' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.individual_id, @kind_id ";
            cmdText += "FROM #picked_individual t ";
            ExecuteSync(cmdText);

            //Store selected markers.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'MARKER' ";
            cmdText += "INSERT INTO dbo.wset_member (wset_id, identifiable_id, kind_id) ";
            cmdText += "SELECT " + wsetId + ", t.marker_id, @kind_id ";
            cmdText += "FROM #picked_marker t ";
            ExecuteSync(cmdText);

            //Store session settings.
			StoreSessionSetting(name, "ViewSetting", settings.GetString(SessionSettings.Strings.ViewMode).ToLower());
            StoreSessionSetting(name, "ExperimentSetting", settings.GetString(SessionSettings.Strings.ExperimentMode).ToLower());
            StoreSessionSetting(name, "DupViewSetting", settings.Get(SessionSettings.Bools.DemandDuplicates) ? "DupOnly" : "AllSamples");
			StoreSessionSetting(name, "InhSettingDoTest", settings.Get(SessionSettings.Bools.DoInheritanceTest));
			StoreSessionSetting(name, "DetectX", settings.Get(SessionSettings.Bools.HumanXChr));
			StoreSessionSetting(name, "HWSettingDoFail", settings.Get(SessionSettings.Bools.DoHWTest));
			StoreSessionSetting(name, "HWCutoff", settings.Get(SessionSettings.Doubles.HWLimit));
			StoreSessionSetting(name, "HWSettingRemoveChildren", settings.Get(SessionSettings.Bools.HWSkipChildren));

			return wsetId;
		}

		
		public SessionSettings LoadSessionSettings(string sessionName, out bool notProperlyLoaded)
		{
			//Prepares data in the temporary tables and returns the saved session settings.
			//notProperlyLoaded is set to true if at least one of the settings could not be loaded. 
			SessionSettings settings;
			string tempString;
			bool tempBool;
			double tempDouble;
			bool loadError, anyError;

			settings = new SessionSettings();
			anyError = false;

			tempString = GetSessionSettingString(sessionName, "ViewSetting", out loadError).ToUpper();
			if (!loadError) { settings.Set(SessionSettings.Strings.ViewMode, tempString); } else { anyError = true; }

            tempString = GetSessionSettingString(sessionName, "ExperimentSetting", out loadError).ToUpper();
            if (!loadError) { settings.Set(SessionSettings.Strings.ExperimentMode, tempString); } else { anyError = true; }			

			tempString = GetSessionSettingString(sessionName, "DupViewSetting", out loadError).ToUpper();
			if (!loadError) { settings.Set(SessionSettings.Bools.DemandDuplicates, tempString == "DUPONLY"); } else { anyError = true; }

			tempBool = GetSessionSettingBool(sessionName, "InhSettingDoTest", out loadError);
			if (!loadError) { settings.Set(SessionSettings.Bools.DoInheritanceTest, tempBool); } else { anyError = true; }

			tempBool = GetSessionSettingBool(sessionName, "DetectX", out loadError);
			if (!loadError) { settings.Set(SessionSettings.Bools.HumanXChr, tempBool); } else { anyError = true; }

			tempBool = GetSessionSettingBool(sessionName, "HWSettingDoFail", out loadError);
			if (!loadError) { settings.Set(SessionSettings.Bools.DoHWTest, tempBool); } else { anyError = true; }

			tempDouble = GetSessionSettingDouble(sessionName, "HWCutoff", out loadError);
			if (!loadError) { settings.Set(SessionSettings.Doubles.HWLimit, tempDouble); } else { anyError = true; }

			tempBool = GetSessionSettingBool(sessionName, "HWSettingRemoveChildren", out loadError);
			if (!loadError) { settings.Set(SessionSettings.Bools.HWSkipChildren, tempBool); } else { anyError = true; }

			notProperlyLoaded = anyError;

			return settings;
		}


		public DataTable GetUsedWsets(int wset_id)
		{
			//Returns a table with the used workingsets in a saved session.
			string cmdText;

			cmdText = "";
            cmdText += "DECLARE @plateKindId INTEGER ";
            cmdText += "DECLARE @wsetKindId INTEGER ";
            cmdText += "SELECT @plateKindId = kind_id FROM dbo.kind WHERE name = 'RESULT_PLATE' ";
            cmdText += "SELECT @wsetKindId = kind_id FROM dbo.kind WHERE name = 'WSET' ";
			cmdText += "SELECT p.plate_id AS [Id], p.identifier AS [Name], ";
			cmdText += "p.description AS [Description], 'Plate' AS [Type] ";
			cmdText += "FROM dbo.plate p ";
            cmdText += "INNER JOIN dbo.wset_member wm ON (wm.identifiable_id = p.plate_id AND wm.wset_id = " + wset_id.ToString() + " AND wm.kind_id = @plateKindId) ";
            cmdText += "UNION ";
            cmdText += "SELECT w.wset_id AS [Id], w.identifier AS [Name], ";
            cmdText += "w.description AS [Description], 'Working set' AS [Type] ";
            cmdText += "FROM dbo.wset w ";
            cmdText += "INNER JOIN dbo.wset_member wm ON (wm.identifiable_id = w.wset_id AND wm.wset_id = " + wset_id.ToString() + " AND wm.kind_id = @wsetKindId) ";

			return FillTableSync(cmdText);
		}

		public bool GetWsetExists(string wsetName)
		{
			return (ExecuteIntSync("SELECT COUNT(*) FROM dbo.wset WHERE identifier = '" + wsetName + "'") > 0);
		}

		public DataTable GetDuplicateTest(int [] experimentID, TestSuccess success)
		{
			return GetExperimentGenotypes("#duplicate_test", experimentID, TestSuccessToString(success), "NOT alleles = 'N/A'");   
		}

		public DataTable GetInheritanceTest(int [] experimentID, TestSuccess success)
		{
            return GetExperimentGenotypes("#inheritance_test", experimentID, TestSuccessToString(success), "NOT alleles = 'N/A'");   
		}

		public DataTable GetBlankTest(int [] experimentID, TestSuccess success)
		{
            return GetExperimentGenotypes("#blank_test", experimentID, TestSuccessToString(success), null);   
		}

		public DataTable GetHomozygoteTest(int [] experimentID, TestSuccess success)
		{
            return GetExperimentGenotypes("#homozygote_test", experimentID, TestSuccessToString(success), "NOT alleles = 'N/A'");   
		}

		public DataTable GetAllExperimentGenotypes(int [] experimentID, string where)
		{
			return GetExperimentGenotypes("#all_genotypes", experimentID, null, where);   
		}

        public DataTable GetAllItemGenotypes(int[] itemID, string where)
        {
            return GetItemGenotypes("#all_genotypes", itemID, null, where);
        }

        public DataTable GetPendingGenotypes()
        {
            return GetExperimentGenotypes("#all_genotypes", null, null, "NOT new_status_id IS NULL");
        }

        public DataTable GetAllItems()
        {
            string cmdText;

            cmdText = "";
            cmdText += "SELECT item_id, Item FROM ( ";
            cmdText += "SELECT item_id, identifier AS Item FROM #item_stat ";
            cmdText += "UNION SELECT item_id, identifier AS Item FROM #control_item_stat ";
            cmdText += ") all_items ";
            cmdText += "ORDER BY Item ";

            return FillTableSync(cmdText);
        }

        public DataTable GetExperimentStatColumns()
        {
            //Returns a table with the columns from experiment statistics, but no rows.
            string cmdText;

            cmdText = CreateExperimentStatQuery(0, null);

            return FillTableSyncNoFormat(cmdText);
        }

        public DataTable GetItemStatColumns()
        {
            //Returns a table with the columns from item statistics, but no rows.
            string cmdText;

            cmdText = "";
            cmdText += "SELECT TOP 0 * FROM (" + GET_ITEM_STAT_STRING + ") es ";

            return FillTableSync(cmdText);
        }

		public DataTable GetItemStat(int experimentID)
		{
			string cmdText;

			cmdText = "";
            cmdText += "EXEC pSQAT_GetItemStatistics " + experimentID.ToString() + " ";

			return FillTableSync(cmdText);
		}

		public DataTable GetExperimentStat(int itemID)
		{
			string cmdText;

			cmdText = "";
			cmdText += "EXEC pSQAT_GetExperimentStatistics " + itemID.ToString() + " ";

			return FillTableSync(cmdText);
		}

		public DataTable GetControlStat(int controlItemID)
		{
			string cmdText;

			cmdText = "";
			cmdText += "EXEC pSQAT_GetControlStatistics " + controlItemID.ToString() + " ";

			return FillTableSync(cmdText);
		}

		public DataTable GetResultExperiments()
		{
			//Returns all experiments ordered by identifier.
			string cmdText;

			cmdText = "";
			cmdText += "SELECT experiment_id, identifier FROM #experiment_stat ";
			cmdText += "ORDER BY identifier ";

			return FillTableSync(cmdText);
		}

		public DataTable GetResultItems()
		{
			//Returns all items ordered by identifier.
			string cmdText;

			cmdText = "";
			cmdText += "SELECT item_id, identifier FROM #item_stat ";
			cmdText += "ORDER BY identifier ";

			return FillTableSync(cmdText);				
		}

        public DataTable GetAllResults(SessionSettings settings, StringCollection includedItemTypes, StringCollection includedGenotypeStatuses, StringCollection includedItems, StringCollection includedExperiments)
        {
            //Returns all genotypes from selected item types, statuses, items and experiments.
            string cmdText;

            cmdText = "";
            cmdText += "TRUNCATE TABLE #comparison_item ";
            cmdText += "TRUNCATE TABLE #comparison_experiment ";
            ExecuteSync(cmdText);

            foreach (string tempVal in includedItems)
            {
                AddComparisonItem(tempVal);
            }

            foreach (string tempVal in includedExperiments)
            {
                AddComparisonExperiment(tempVal);
            }

            cmdText = "";
            cmdText += "SELECT i.identifier AS Item, e.identifier AS Experiment, dg.alleles AS Alleles ";
            cmdText += "FROM #all_genotypes ag ";
            cmdText += "INNER JOIN dbo.denorm_genotype dg ON dg.genotype_id = ag.genotype_id ";
            cmdText += "INNER JOIN dbo.status s ON (s.status_id = dg.status_id ";
            cmdText += "AND s.name IN " + this.CreateInClause(includedGenotypeStatuses) + ") ";
            if (settings.GetString(SessionSettings.Strings.ViewMode) == "SAMPLE")
            {
                cmdText += "INNER JOIN dbo.sample i ON i.sample_id = dg.sample_id ";
                cmdText += "INNER JOIN dbo.individual idv ON idv.individual_id = i.individual_id ";
                cmdText += "INNER JOIN dbo.individual_type idt ON (idt.individual_type_id = idv.individual_type_id ";
                cmdText += "AND idt.name IN " + this.CreateInClause(includedItemTypes) + ") ";
            }
            else
            {
                cmdText += "INNER JOIN dbo.individual i ON i.individual_id = dg.individual_id ";
                cmdText += "INNER JOIN dbo.individual_type idt ON (idt.individual_type_id = i.individual_type_id ";
                cmdText += "AND idt.name IN " + this.CreateInClause(includedItemTypes) + ") ";
            }
            if (settings.GetString(SessionSettings.Strings.ExperimentMode) == "ASSAY")
            {
                cmdText += "INNER JOIN dbo.assay e ON e.assay_id = dg.assay_id ";
            }
            else
            {
                cmdText += "INNER JOIN dbo.marker e ON e.marker_id = dg.marker_id ";
            }
            cmdText += "WHERE i.identifier COLLATE latin1_general_ci_as IN (SELECT identifier COLLATE latin1_general_ci_as FROM #comparison_item) ";
            cmdText += "AND e.identifier COLLATE latin1_general_ci_as IN (SELECT identifier COLLATE latin1_general_ci_as FROM #comparison_experiment) ";
            return FillTableSync(cmdText);
        }

		public DataTable GetApprovedResult()
		{
			//Returns all approved genotypes along with their corresponding
			//item and experiment.
			string cmdText;

			cmdText = "";
		    cmdText += "SELECT ag.genotype_id, ag.item_id, ";
			cmdText += "ist.identifier AS item, ag.experiment_id, es.identifier AS experiment, ag.alleles ";
			cmdText += "FROM #approved_genotype ag ";
            cmdText += "INNER JOIN #item_stat ist ON ist.item_id = ag.item_id ";
            cmdText += "INNER JOIN #experiment_stat es ON es.experiment_id = ag.experiment_id ";
			cmdText += "ORDER BY ist.identifier, es.identifier ";

			return FillTableSync(cmdText);
		}

		public DataTable GetApprovedResultFormatted()
		{
			//Returns all approved genotypes along with their corresponding
			//item and experiment formatted for showing.
			return FillTableSync(GET_APPROVED_RESULT_STRING);
		}

        public void GetApprovedResultFormatted(FileServer fs)
        {
            StreamQueryResultToFile(GET_APPROVED_RESULT_STRING, null, fs);
        }


        public DataTable GetApprovedResultFormattedOuter(bool sortItemFirst, string emptyResultString)
        {
            //Returns all pairwise combinations of items and experiments
            //to which the approved results are outer joined (a no result is
            //replaced by the specified string).
            return FillTableSync(CreateApprovedResultOuterString(sortItemFirst, emptyResultString));
        }

        public DataTable GetApprovedResultFormattedOuterWithId(bool sortItemFirst, string emptyResultString)
        {
            //Returns all pairwise combinations of items and experiments
            //to which the approved results are outer joined (a no result is
            //replaced by the specified string). The id numbers of the items and
            //experiments are included.
            return FillTableSync(CreateApprovedResultOuterStringWithId(sortItemFirst, emptyResultString));
        }

        public void GetApprovedResultFormattedOuter(bool sortItemFirst, string emptyResultString, FileServer fs)
        {
            StreamQueryResultToFile(CreateApprovedResultOuterString(sortItemFirst, emptyResultString), null, fs);
        }

        private string CreateApprovedResultOuterString(bool sortItemFirst, string emptyResultString)
        {
            string cmdText;

            cmdText = "";
            cmdText += "DECLARE @pair TABLE( ";
            cmdText += "item_id INTEGER, ";
            cmdText += "item_name VARCHAR(255), ";
            cmdText += "experiment_id INTEGER, ";
            cmdText += "experiment_name VARCHAR(255), ";
            cmdText += "PRIMARY KEY (item_id, experiment_id) ";
            cmdText += ") ";
            cmdText += "";
            cmdText += "INSERT INTO @pair (item_id, item_name, experiment_id, experiment_name) ";
            cmdText += "SELECT ist.item_id, ist.identifier, es.experiment_id, es.identifier ";
            cmdText += "FROM #item_stat ist, #experiment_stat es ";
            cmdText += "";
            cmdText += "SELECT 0, p.item_name AS [Item], ";
            cmdText += "p.experiment_name AS [Experiment], ISNULL(ag.alleles, '" + emptyResultString + "') AS [Alleles] ";
            cmdText += "FROM @pair p ";
            cmdText += "LEFT OUTER JOIN #approved_genotype ag ON (ag.item_id = p.item_id AND ag.experiment_id = p.experiment_id) ";

            if (sortItemFirst)
            {
                cmdText += "ORDER BY p.item_name, p.experiment_name ";
            }
            else
            {
                cmdText += "ORDER BY p.experiment_name, p.item_name ";
            }

            return cmdText;
        }

        private string CreateApprovedResultOuterStringWithId(bool sortItemFirst, string emptyResultString)
        {
            string cmdText;

            cmdText = "";
            cmdText += "DECLARE @pair TABLE( ";
            cmdText += "item_id INTEGER, ";
            cmdText += "item_name VARCHAR(255), ";
            cmdText += "experiment_id INTEGER, ";
            cmdText += "experiment_name VARCHAR(255), ";
            cmdText += "PRIMARY KEY (item_id, experiment_id) ";
            cmdText += ") ";
            cmdText += "";
            cmdText += "INSERT INTO @pair (item_id, item_name, experiment_id, experiment_name) ";
            cmdText += "SELECT ist.item_id, ist.identifier, es.experiment_id, es.identifier ";
            cmdText += "FROM #item_stat ist, #experiment_stat es ";
            cmdText += "";
            cmdText += "SELECT 0,p.item_id, p.item_name AS [Item], ";
            cmdText += "p.experiment_id, p.experiment_name AS [Experiment], ISNULL(ag.alleles, '" + emptyResultString + "') AS [Alleles] ";
            cmdText += "FROM @pair p ";
            cmdText += "LEFT OUTER JOIN #approved_genotype ag ON (ag.item_id = p.item_id AND ag.experiment_id = p.experiment_id) ";

            if (sortItemFirst)
            {
                cmdText += "ORDER BY p.item_id, p.experiment_id ";
            }
            else
            {
                cmdText += "ORDER BY p.experiment_id, p.item_id ";
            }

            return cmdText;
        }

		public DataTable GetGenotypeHistory(int genotypeId)
		{
			string cmdText;

			cmdText = "";
			cmdText += "EXEC pSQAT_GetGenotypeHistory " + genotypeId.ToString() + " ";

			return FillTableSync(cmdText);
		}

		public void CreateCommonTempTables()
		{
			//Create temporary tables where the contents of the wsets are stored,
			//and where any specially selected samples, individuals, markers or assays are stored.
			string cmdText;

			cmdText = "";
            cmdText += "CREATE TABLE #plate(plate_id INTEGER PRIMARY KEY) ";
			cmdText += "CREATE TABLE #genotype_wset(wset_id INTEGER PRIMARY KEY) ";
			cmdText += "CREATE TABLE #marker_wset(wset_id INTEGER PRIMARY KEY) ";
			cmdText += "CREATE TABLE #assay_wset(wset_id INTEGER PRIMARY KEY) ";
			cmdText += "CREATE TABLE #sample_wset(wset_id INTEGER PRIMARY KEY) ";
            cmdText += "CREATE TABLE #individual_wset(wset_id INTEGER PRIMARY KEY) ";
            cmdText += "CREATE TABLE #picked_sample(sample_id INTEGER PRIMARY KEY) ";
			cmdText += "CREATE TABLE #picked_assay(assay_id INTEGER PRIMARY KEY) ";
            cmdText += "CREATE TABLE #picked_individual(individual_id INTEGER PRIMARY KEY) ";
            cmdText += "CREATE TABLE #picked_marker(marker_id INTEGER PRIMARY KEY) ";
            cmdText += "CREATE TABLE #comparison_item(identifier VARCHAR(255) NOT NULL PRIMARY KEY) ";
            cmdText += "CREATE TABLE #comparison_experiment(identifier VARCHAR(255) NOT NULL PRIMARY KEY) ";

            cmdText += "CREATE TABLE #all_genotypes( ";
            cmdText += "genotype_id INTEGER PRIMARY KEY, ";
            cmdText += "item_id INTEGER NOT NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL ";
            cmdText += ") ";

            cmdText += "CREATE TABLE #duplicate_test( ";
            cmdText += "item_id INTEGER NOT NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL, ";
            cmdText += "success BIT NOT NULL, ";
            cmdText += "PRIMARY KEY (item_id, experiment_id)) ";

            cmdText += "CREATE TABLE #inheritance_test( ";
            cmdText += "item_id INTEGER NOT NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL, ";
            cmdText += "success BIT NOT NULL, ";
            cmdText += "PRIMARY KEY (item_id, experiment_id)) ";

            cmdText += "CREATE TABLE #inheritance_trio( ";
            cmdText += "father_id INTEGER NULL, ";
            cmdText += "mother_id INTEGER NULL, ";
            cmdText += "child_id INTEGER NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL, ";
            cmdText += "success BIT NOT NULL ";
            cmdText += ")";

            cmdText += "CREATE TABLE #het_X_male_test( ";
            cmdText += "item_id INTEGER NOT NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL, ";
            cmdText += "success BIT NOT NULL, ";
            cmdText += "PRIMARY KEY(item_id, experiment_id)) ";

            cmdText += "CREATE TABLE #blank_test( ";
            cmdText += "item_id INTEGER NOT NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL, ";
            cmdText += "success BIT NOT NULL) ";  //No primary key since uses all available genotypes, not only consensus. 

            cmdText += "CREATE TABLE #homozygote_test( ";
            cmdText += "item_id INTEGER NOT NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL, ";
            cmdText += "success BIT NOT NULL) ";  //No primary key since uses all available genotypes, not only consensus. 

            cmdText += "CREATE TABLE #approved_genotype( ";
            cmdText += "genotype_id INTEGER NOT NULL PRIMARY KEY, ";
            cmdText += "item_id INTEGER NOT NULL, ";
            cmdText += "experiment_id INTEGER NOT NULL, ";
            cmdText += "alleles VARCHAR(3) NOT NULL, ";
            cmdText += "UNIQUE (item_id, experiment_id)) ";

            cmdText += "CREATE TABLE #experiment_stat( ";
			cmdText += "experiment_id INTEGER PRIMARY KEY, ";
			cmdText += "identifier VARCHAR(255) NOT NULL UNIQUE, ";
			cmdText += "fq_XX INTEGER NOT NULL, ";
			cmdText += "fq_YY INTEGER NOT NULL, ";
			cmdText += "fq_XY INTEGER NOT NULL, ";
			cmdText += "not_approved INTEGER NOT NULL, ";
			cmdText += "success_rate REAL NOT NULL, ";
			cmdText += "fq_XX_exp INTEGER NOT NULL, ";
			cmdText += "fq_YY_exp INTEGER NOT NULL, ";
			cmdText += "fq_XY_exp INTEGER NOT NULL, ";
			cmdText += "HW_chi2 REAL NOT NULL, ";
            cmdText += "chrom_name VARCHAR(20) NULL, ";
			cmdText += "fq_X REAL NOT NULL, ";
			cmdText += "fq_Y REAL NOT NULL, ";
			cmdText += "dupl_failed_item INTEGER NULL, ";
			cmdText += "dupl_tested_item INTEGER NULL, ";
			cmdText += "item_validation REAL NULL, ";
			cmdText += "item_mismatch REAL NULL, ";
			cmdText += "inh_failed_item INTEGER NULL, ";
			cmdText += "inh_tested_item INTEGER NULL, ";
			cmdText += "HW_failure VARCHAR(20) NOT NULL, ";
			cmdText += "rejected_gt INTEGER NOT NULL, ";
			cmdText += "inh_ctrl_failed_item INTEGER NULL, ";
			cmdText += "inh_ctrl_tested_item INTEGER NULL, ";
			cmdText += "blank_ctrl_failed_item INTEGER NOT NULL, ";
			cmdText += "blank_ctrl_tested_item INTEGER NOT NULL, ";
			cmdText += "homozyg_ctrl_failed_item INTEGER NOT NULL, ";
			cmdText += "homozyg_ctrl_tested_item INTEGER NOT NULL ";
            cmdText += ") ";

            cmdText += "CREATE TABLE #item_stat( ";
			cmdText += "item_id INTEGER PRIMARY KEY, ";
			cmdText += "identifier VARCHAR(255) NOT NULL, ";
			cmdText += "success_experiments INTEGER NOT NULL, ";
			cmdText += "total_gt INTEGER NOT NULL, ";
			cmdText += "no_result_gt INTEGER NOT NULL, ";
			cmdText += "failed_dup_exp INTEGER NOT NULL, ";
			cmdText += "failed_inh_exp INTEGER NULL, ";
			cmdText += "failed_HW_exp INTEGER NULL, ";
			cmdText += "rejected_gt INTEGER NOT NULL, ";
			cmdText += "chrX_warning_exp INTEGER NULL ";
            cmdText += ") ";

            cmdText += "CREATE TABLE #control_item_stat( ";
			cmdText += "item_id INTEGER PRIMARY KEY, ";
			cmdText += "identifier VARCHAR(255) NOT NULL, ";
			cmdText += "failed_exp INTEGER NULL, ";
			cmdText += "tested_exp INTEGER NULL ";
            cmdText += ") ";

            cmdText += "CREATE TABLE #experiment_count( ";
            cmdText += "item_id INTEGER NOT NULL PRIMARY KEY, ";
            cmdText += "run_experiments INTEGER NOT NULL ";
            cmdText += ") ";

            cmdText += "CREATE TABLE #experiment_count_nonfailed( ";
            cmdText += "item_id INTEGER NOT NULL PRIMARY KEY, ";
            cmdText += "run_experiments INTEGER NOT NULL ";
            cmdText += ") ";

            cmdText += "CREATE TABLE #pending_status(genotype_id INTEGER PRIMARY KEY, new_status_id TINYINT NOT NULL) ";

            cmdText += "CREATE TABLE #rerun_experiment(experiment_id INTEGER PRIMARY KEY) ";

            cmdText += "CREATE TABLE #control_item(item_id INTEGER NOT NULL PRIMARY KEY) ";

            cmdText += "CREATE TABLE #plate_well(pos_x INTEGER NOT NULL, pos_y INTEGER NOT NULL, PRIMARY KEY(pos_x, pos_y) ) ";			
            cmdText += "CREATE TABLE #plate_experiment(experiment_id INTEGER PRIMARY KEY) ";
            cmdText += "CREATE TABLE #plate_item(item_id INTEGER PRIMARY KEY) ";

			ExecuteSync(cmdText);
		}

        public void AddSelectedAssayGroup(int wsetId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #assay_wset (wset_id) VALUES ";
            cmdText += "(" + wsetId.ToString() + ") ";

            ExecuteSync(cmdText);
        }

        public void AddSelectedPlate(int wsetId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #plate (plate_id) VALUES ";
            cmdText += "(" + wsetId.ToString() + ") ";

            ExecuteSync(cmdText);
        }

        public void AddSelectedGenotypeGroup(int wsetId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #genotype_wset (wset_id) VALUES ";
            cmdText += "(" + wsetId.ToString() + ") ";

            ExecuteSync(cmdText);
        }

        public void AddSelectedMarkerGroup(int wsetId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #marker_wset (wset_id) VALUES ";
            cmdText += "(" + wsetId.ToString() + ") ";

            ExecuteSync(cmdText);
        }

        public void AddSelectedSampleGroup(int wsetId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #sample_wset (wset_id) VALUES ";
            cmdText += "(" + wsetId.ToString() + ") ";

            ExecuteSync(cmdText);
        }

        public void AddSelectedIndividualGroup(int wsetId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #individual_wset (wset_id) VALUES ";
            cmdText += "(" + wsetId.ToString() + ") ";

            ExecuteSync(cmdText);
        }

        public void AddSelectedPlateGroup(int wsetId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "DECLARE @plate_kind_id INTEGER ";
            cmdText += "SELECT @plate_kind_id = kind_id FROM kind WHERE name = 'RESULT_PLATE' ";
            cmdText += "INSERT INTO #plate (plate_id) ";
            cmdText += "(SELECT identifiable_id FROM dbo.wset_member WHERE wset_id = " + wsetId.ToString() + " ";
            cmdText += "AND kind_id = @plate_kind_id) ";

            ExecuteSync(cmdText);
        }

		public void ClearSelectedGroups()
		{
			string cmdText;

			cmdText = "";
            cmdText += "TRUNCATE TABLE #plate ";
            cmdText += "TRUNCATE TABLE #assay_wset ";
            cmdText += "TRUNCATE TABLE #genotype_wset ";
            cmdText += "TRUNCATE TABLE #marker_wset ";
            cmdText += "TRUNCATE TABLE #sample_wset ";
            cmdText += "TRUNCATE TABLE #individual_wset ";
		
			ExecuteSync(cmdText);
		}

        public void AddSelectedSample(int sampleId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #picked_sample (sample_id) VALUES (" + sampleId.ToString() + ")";
            ExecuteSync(cmdText);
        }

        public void AddSelectedAssay(int assayId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #picked_assay (assay_id) VALUES (" + assayId.ToString() + ")";
            ExecuteSync(cmdText);
        }

        public void AddSelectedMarker(int markerId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #picked_marker (marker_id) VALUES (" + markerId.ToString() + ")";
            ExecuteSync(cmdText);
        }

        public void AddSelectedIndividual(int individualId)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #picked_individual (individual_id) VALUES (" + individualId.ToString() + ")";
            ExecuteSync(cmdText);
        }

        public void AddComparisonItem(string identifier)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #comparison_item (identifier) VALUES ('" + identifier + "')";
            ExecuteSync(cmdText);
        }

        public void AddComparisonExperiment(string identifier)
        {
            string cmdText;

            cmdText = "";
            cmdText += "INSERT INTO #comparison_experiment (identifier) VALUES ('" + identifier + "')";
            ExecuteSync(cmdText);
        }

		public void ClearSelectedItems()
		{
			string cmdText;

			cmdText = "";
            cmdText += "TRUNCATE TABLE #picked_sample ";
            cmdText += "TRUNCATE TABLE #picked_individual ";
            cmdText += "TRUNCATE TABLE #picked_assay ";
            cmdText += "TRUNCATE TABLE #picked_marker ";
		
			ExecuteSync(cmdText);			
		}

		public void ClearPlateSelection()
		{
			string cmdText;

			cmdText = "";
            cmdText += "TRUNCATE TABLE #plate_well ";
            cmdText += "TRUNCATE TABLE #plate_experiment ";
            cmdText += "TRUNCATE TABLE #plate_item ";			
		
			ExecuteSync(cmdText);
		}

		public void AddPlateWell(int posX, int posY)
		{
			string cmdText;

			cmdText = "";
			cmdText += "INSERT INTO #plate_well (pos_x, pos_y) VALUES ";
			cmdText += "(" + posX.ToString() + ", " + posY.ToString() + ") ";
		
			ExecuteSync(cmdText);
		}

		public void AddPlateItem(int itemId)
		{
			string cmdText;

			cmdText = "";
			cmdText += "INSERT INTO #plate_item (item_id) VALUES ";
			cmdText += "(" + itemId.ToString() + ") ";
		
			ExecuteSync(cmdText);
		}

		public void AddPlateExperiment(int experimentId)
		{
			string cmdText;

			cmdText = "";
			cmdText += "INSERT INTO #plate_experiment (experiment_id) VALUES ";
			cmdText += "(" + experimentId.ToString() + ") ";
		
			ExecuteSync(cmdText);
		}

        public void StoreSelectionFromSession(int sessionWsetId)
        {
            string cmdText;

            //Plates.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'RESULT_PLATE' ";
            cmdText += "INSERT INTO #plate (plate_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "WHERE kind_id = @kind_id ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Genotype sets (for backward compability).
            cmdText = "";
            cmdText += "INSERT INTO #genotype_wset (wset_id) ";
            cmdText += "SELECT w.wset_id FROM dbo.wset w ";
            cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
            cmdText += "WHERE (wt.name = 'AnalyzedGenotypeSet' OR wt.name = 'ResultGenotypeSet') ";
            cmdText += "AND w.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Marker sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO #marker_wset (wset_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "INNER JOIN dbo.wset w ON w.wset_id = wm.identifiable_id ";
            cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
            cmdText += "WHERE wt.name = 'MarkerSet' ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Assay sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO #assay_wset (wset_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "INNER JOIN dbo.wset w ON w.wset_id = wm.identifiable_id ";
            cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
            cmdText += "WHERE wt.name = 'AssaySet' ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Sample sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO #sample_wset (wset_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "INNER JOIN dbo.wset w ON w.wset_id = wm.identifiable_id ";
            cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
            cmdText += "WHERE wt.name = 'SampleSet' ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Individual sets.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'WSET' ";
            cmdText += "INSERT INTO #individual_wset (wset_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "INNER JOIN dbo.wset w ON w.wset_id = wm.identifiable_id ";
            cmdText += "INNER JOIN dbo.wset_type wt ON wt.wset_type_id = w.wset_type_id ";
            cmdText += "WHERE wt.name = 'IndividualSet' ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Samples.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'SAMPLE' ";
            cmdText += "INSERT INTO #picked_sample (sample_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "WHERE kind_id = @kind_id ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Assays.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'ASSAY' ";
            cmdText += "INSERT INTO #picked_assay (assay_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "WHERE kind_id = @kind_id ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Individuals.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'INDIVIDUAL' ";
            cmdText += "INSERT INTO #picked_individual (individual_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "WHERE kind_id = @kind_id ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

            //Markers.
            cmdText = "";
            cmdText += "DECLARE @kind_id INTEGER ";
            cmdText += "SELECT @kind_id = kind_id FROM dbo.kind WHERE name = 'MARKER' ";
            cmdText += "INSERT INTO #picked_marker (marker_id) ";
            cmdText += "SELECT identifiable_id FROM dbo.wset_member wm ";
            cmdText += "WHERE kind_id = @kind_id ";
            cmdText += "AND wm.wset_id = " + sessionWsetId.ToString() + " ";
            ExecuteSync(cmdText);

        }

        public FilterIndicator GetUsedFilters()
        {
            FilterIndicator usedFilters;

            usedFilters = new FilterIndicator();

            usedFilters.Plate = (ExecuteIntSync("SELECT COUNT(*) FROM #plate ") > 0);
            usedFilters.GenotypeGroup = (ExecuteIntSync("SELECT COUNT(*) FROM #genotype_wset ") > 0);
            usedFilters.MarkerGroup = (ExecuteIntSync("SELECT COUNT(*) FROM #marker_wset ") > 0);
            usedFilters.AssayGroup = (ExecuteIntSync("SELECT COUNT(*) FROM #assay_wset ") > 0);
            usedFilters.SampleGroup = (ExecuteIntSync("SELECT COUNT(*) FROM #sample_wset ") > 0);
            usedFilters.IndividualGroup = (ExecuteIntSync("SELECT COUNT(*) FROM #individual_wset ") > 0);
            usedFilters.Sample = (ExecuteIntSync("SELECT COUNT(*) FROM #picked_sample ") > 0);
            usedFilters.Assay = (ExecuteIntSync("SELECT COUNT(*) FROM #picked_assay ") > 0);
            usedFilters.Individual = (ExecuteIntSync("SELECT COUNT(*) FROM #picked_individual ") > 0);
            usedFilters.Marker = (ExecuteIntSync("SELECT COUNT(*) FROM #picked_marker ") > 0);

            return usedFilters;
        }

        public void StreamExperimentStatToFile(FileServer fs, bool [] includedColumns)
        {
            StreamQueryResultToFile(CreateExperimentStatQuery(int.MaxValue, null), includedColumns, fs);
        }

        public void StreamItemStatToFile(FileServer fs, bool [] includedColumns)
        {
            StreamQueryResultToFile(GET_ITEM_STAT_STRING, includedColumns, fs);
        }

        public void StreamControlItemStatToFile(FileServer fs)
        {
            StreamQueryResultToFile(GET_CONTROL_ITEM_STAT_STRING, null, fs);
        }

        private void StreamQueryResultToFile(string sqlQuery, bool [] includedColumns, FileServer fs)
        {
            //Streams the result of a query to the current FileServer target file.
            //The first column (which is normally the ID column) in the table is skipped.
            //The first column should not be included in the includedColumns array.
            //If includedColumns is null, all columns will be included.
            SqlDataReader dr;
            SqlCommand cmd;
            DataTable schemaTable;
            string tempDisplayName, tempName;
            int tempPrefixIndex;

            cmd = new SqlCommand(sqlQuery, MyConnection);
            cmd.CommandTimeout = MySyncTimeout;
            dr = cmd.ExecuteReader();


            //Get the schema definition and exclude column formatting to be able to write the column names.
            schemaTable = dr.GetSchemaTable();

            for (int i = 1; i < schemaTable.Rows.Count - 1; i++)
            {
                tempName = schemaTable.Rows[i]["ColumnName"].ToString();
                tempPrefixIndex = tempName.IndexOf("//");
                if (tempName.Length >= 4 && tempPrefixIndex > 0)
                {
                    tempDisplayName = tempName.Substring(0, tempName.Length - 4);
                }
                else
                {
                    tempDisplayName = tempName;
                }

                if (includedColumns != null)
                {
                    if (includedColumns[i - 1])
                    {
                        fs.AppendStringToFile(tempDisplayName + "\t");
                    }
                }
                else
                {
                    fs.AppendStringToFile(tempDisplayName + "\t");
                }
            }
            //The last column, ended with new line.
            tempName = schemaTable.Rows[schemaTable.Rows.Count - 1]["ColumnName"].ToString();
            tempPrefixIndex = tempName.IndexOf("//");
            if (tempName.Length >= 4 && tempPrefixIndex > 0)
            {
                tempDisplayName = tempName.Substring(0, tempName.Length - 4);
            }
            else
            {
                tempDisplayName = tempName;
            }

            if (includedColumns != null)
            {
                if (includedColumns[schemaTable.Rows.Count - 2])
                {
                    fs.AppendStringToFile(tempDisplayName + Environment.NewLine);
                }
                else
                {
                    fs.AppendStringToFile(Environment.NewLine);
                }
            }
            else
            {
                fs.AppendStringToFile(tempDisplayName + Environment.NewLine);
            }

            //Write the actual data.
            while (dr.Read())
            {
                for (int i = 1; i < dr.FieldCount - 1; i++)
                {
                    if (includedColumns != null)
                    {
                        if (includedColumns[i - 1])
                        {
                            fs.AppendStringToFile(Convert.ToString(dr.GetValue(i)) + "\t");
                        }
                    }
                    else
                    {
                        fs.AppendStringToFile(Convert.ToString(dr.GetValue(i)) + "\t");
                    }
                }
                //The last column, ended with new line.
                if (includedColumns != null)
                {
                    if (includedColumns[dr.FieldCount - 2])
                    {
                        fs.AppendStringToFile(Convert.ToString(dr.GetValue(dr.FieldCount - 1)) + Environment.NewLine);
                    }
                    else
                    {
                        fs.AppendStringToFile(Environment.NewLine);
                    }
                }
                else
                {
                    fs.AppendStringToFile(Convert.ToString(dr.GetValue(dr.FieldCount - 1)) + Environment.NewLine);
                }
            }

            dr.Close();
            
        }


        public void RejectPlateExperiments(int plateId, SessionSettings settings)
        {
            string cmdText, statusCode, idName;

            try
            {
                statusCode = "2";
                idName = (settings.GetString(SessionSettings.Strings.ExperimentMode) == "MARKER") ? "marker_id" : "assay_id";

                BeginTransaction();

                //Delete any previous changes for these experiments and insert the new ones.
                cmdText = "";
                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ";
                cmdText += "(SELECT genotype_id FROM dbo.denorm_genotype WHERE plate_id = " + plateId.ToString() + " ";
                cmdText += "AND " + idName + " IN (SELECT experiment_id FROM #plate_experiment) ";
                cmdText += "AND genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "(SELECT genotype_id, " + statusCode + " FROM dbo.denorm_genotype WHERE plate_id = " + plateId.ToString() + " ";
                cmdText += "AND " + idName + " IN (SELECT experiment_id FROM #plate_experiment) ";
                cmdText += "AND genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw (e);
            }
        }


        public void RejectPlateItems(int plateId, SessionSettings settings)
        {
            string cmdText, statusCode, idName;

            try
            {
                statusCode = "2";
                idName = (settings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT") ? "individual_id" : "sample_id";

                BeginTransaction();

                //Delete any previous changes for these items and insert the new ones.
                cmdText = "";
                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ";
                cmdText += "(SELECT genotype_id FROM dbo.denorm_genotype WHERE plate_id = " + plateId.ToString() + " ";
                cmdText += "AND " + idName + " IN (SELECT item_id FROM #plate_item) ";
                cmdText += "AND genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "(SELECT genotype_id, " + statusCode + " FROM dbo.denorm_genotype WHERE plate_id = " + plateId.ToString() + " ";
                cmdText += "AND " + idName + " IN (SELECT item_id FROM #plate_item) ";
                cmdText += "AND genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw (e);
            }
        }


        public void RejectPlateWells(int plateId)
        {
            string cmdText, statusCode;

            try
            {
                statusCode = "2";

                BeginTransaction();

                //Delete any previous changes for these wells and insert the new ones.
                cmdText = "";
                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ";
                cmdText += "(SELECT genotype_id FROM dbo.denorm_genotype dg ";
                cmdText += "INNER JOIN #plate_well pw ON (pw.pos_x = dg.pos_x AND pw.pos_y = dg.pos_y) ";
                cmdText += "WHERE dg.plate_id = " + plateId.ToString() + " ";
                cmdText += "AND dg.genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "(SELECT genotype_id, " + statusCode + " FROM dbo.denorm_genotype dg ";
                cmdText += "INNER JOIN #plate_well pw ON (pw.pos_x = dg.pos_x AND pw.pos_y = dg.pos_y) ";
                cmdText += "WHERE dg.plate_id = " + plateId.ToString() + " ";
                cmdText += "AND dg.genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw (e);
            }
        }

        public void UpdateItemStatus(int [] itemId, string newStatus)
        {
            string cmdText, statusCode;

            try
            {
                if (newStatus.ToUpper() == "LOAD")
                {
                    statusCode = "1";
                }
                else if (newStatus.ToUpper() == "REJECTED")
                {
                    statusCode = "2";
                }
                else
                {
                    throw new Exception("Unrecognized status code " + newStatus + ".");
                }

                BeginTransaction();

                //Delete any previous changes for these items and insert the new ones.
                cmdText = "";
                cmdText += "DECLARE @item TABLE(item_id INTEGER NOT NULL PRIMARY KEY) ";
                for (int i = 0; i < itemId.GetLength(0); i++)
                {
                    cmdText += "INSERT INTO @item (item_id) VALUES (" + itemId[i].ToString() + ") ";
                }

                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ";
                cmdText += "(SELECT genotype_id FROM #all_genotypes ag ";
                cmdText += "WHERE ag.item_id IN (SELECT item_id FROM @item)) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "(SELECT genotype_id, " + statusCode + " FROM #all_genotypes ag ";
                cmdText += "WHERE ag.item_id IN (SELECT item_id FROM @item)) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw (e);
            }
        }

        public void UpdateExperimentStatus(int[] experimentId, string newStatus)
        {
            string cmdText, statusCode;

            try
            {
                if (newStatus.ToUpper() == "LOAD")
                {
                    statusCode = "1";
                }
                else if (newStatus.ToUpper() == "REJECTED")
                {
                    statusCode = "2";
                }
                else
                {
                    throw new Exception("Unrecognized status code " + newStatus + ".");
                }

                BeginTransaction();

                //Delete any previous changes for these items and insert the new ones.
                cmdText = "";
                cmdText += "DECLARE @experiment TABLE(experiment_id INTEGER NOT NULL PRIMARY KEY) ";
                for (int i = 0; i < experimentId.GetLength(0); i++)
                {
                    cmdText += "INSERT INTO @experiment (experiment_id) VALUES (" + experimentId[i].ToString() + ") ";
                }

                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ";
                cmdText += "(SELECT genotype_id FROM #all_genotypes ag ";
                cmdText += "WHERE ag.experiment_id IN (SELECT experiment_id FROM @experiment)) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "(SELECT genotype_id, " + statusCode + " FROM #all_genotypes ag ";
                cmdText += "WHERE ag.experiment_id IN (SELECT experiment_id FROM @experiment)) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw (e);
            }
        }

        public void UpdateAllStatus(string newStatus)
        {
            string cmdText, statusCode;

            try
            {
                if (newStatus.ToUpper() == "LOAD")
                {
                    statusCode = "1";
                }
                else if (newStatus.ToUpper() == "REJECTED")
                {
                    statusCode = "2";
                }
                else
                {
                    throw new Exception("Unrecognized status code " + newStatus + ".");
                }

                BeginTransaction();

                //Delete any previous changes and insert the new ones.
                cmdText = "";

                cmdText += "DELETE FROM #pending_status ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "SELECT genotype_id, " + statusCode + " FROM #all_genotypes ag ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw (e);
            }
        }

        public void UpdatePlateSelectionStatus(int plateId, SessionSettings settings, string newStatus)
        {
            string cmdText, statusCode, itemIdName, experimentIdName;

            try
            {
                if (newStatus.ToUpper() == "LOAD")
                {
                    statusCode = "1";
                }
                else if (newStatus.ToUpper() == "REJECTED")
                {
                    statusCode = "2";
                }
                else
                {
                    throw new Exception("Unrecognized status code " + newStatus + ".");
                }

                itemIdName = (settings.GetString(SessionSettings.Strings.ViewMode) == "SUBJECT") ? "individual_id" : "sample_id";
                experimentIdName = (settings.GetString(SessionSettings.Strings.ExperimentMode) == "MARKER") ? "marker_id" : "assay_id";

                BeginTransaction();

                //Delete any previous changes for these items and insert the new ones.
                cmdText = "";
                cmdText += "DELETE FROM #pending_status WHERE genotype_id IN ";
                cmdText += "(SELECT genotype_id FROM dbo.denorm_genotype dg ";
                cmdText += "INNER JOIN #plate_well pw ON (pw.pos_x = dg.pos_x AND pw.pos_y = dg.pos_y) ";
                cmdText += "WHERE dg.plate_id = " + plateId.ToString() + " ";
                cmdText += "AND dg." + itemIdName + " IN (SELECT item_id FROM #plate_item) ";
                cmdText += "AND dg." + experimentIdName + " IN (SELECT experiment_id FROM #plate_experiment) ";
                cmdText += "AND genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                cmdText += "INSERT INTO #pending_status (genotype_id, new_status_id) ";
                cmdText += "(SELECT genotype_id, " + statusCode + " FROM dbo.denorm_genotype dg ";
                cmdText += "INNER JOIN #plate_well pw ON (pw.pos_x = dg.pos_x AND pw.pos_y = dg.pos_y) ";
                cmdText += "WHERE dg.plate_id = " + plateId.ToString() + " ";
                cmdText += "AND dg." + itemIdName + " IN (SELECT item_id FROM #plate_item) ";
                cmdText += "AND dg." + experimentIdName + " IN (SELECT experiment_id FROM #plate_experiment) ";
                cmdText += "AND genotype_id IN (SELECT genotype_id FROM #all_genotypes)) ";

                ExecuteSync(cmdText);

                CommitTransaction();
                OnGenotypeStatusChange();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw (e);
            }
        }


		public DataTable GetSelectedItems(DataServer.IdentifiableType type, FilterIndicator usedFilters, DataServer.Mode mode)
		{
			string cmdText;

			cmdText = "";
            cmdText += "DECLARE @genotype_kind_id INTEGER ";
            cmdText += "DECLARE @sample_kind_id INTEGER ";
            cmdText += "DECLARE @individual_kind_id INTEGER ";
            cmdText += "DECLARE @marker_kind_id INTEGER ";
            cmdText += "DECLARE @assay_kind_id INTEGER ";
            cmdText += "SELECT @genotype_kind_id = kind_id FROM kind WHERE name = 'GENOTYPE' ";
            cmdText += "SELECT @sample_kind_id = kind_id FROM kind WHERE name = 'SAMPLE' ";
            cmdText += "SELECT @individual_kind_id = kind_id FROM kind WHERE name = 'INDIVIDUAL' ";
            cmdText += "SELECT @marker_kind_id = kind_id FROM kind WHERE name = 'MARKER' ";
            cmdText += "SELECT @assay_kind_id = kind_id FROM kind WHERE name = 'ASSAY' ";
			switch (type)
			{
				case IdentifiableType.Sample:
					cmdText += "SELECT DISTINCT dg.sample_id AS identifiable_id, s.identifier AS name ";
                    cmdText += "FROM dbo.denorm_genotype dg ";
                    cmdText += "INNER JOIN dbo.sample s ON s.sample_id = dg.sample_id ";
					break;
				case IdentifiableType.Assay:
					cmdText += "SELECT DISTINCT dg.assay_id AS identifiable_id, a.identifier AS name ";
                    cmdText += "FROM dbo.denorm_genotype dg ";
                    cmdText += "INNER JOIN dbo.assay a ON a.assay_id = dg.assay_id ";
                    break;
                case IdentifiableType.Marker:
                    cmdText += "SELECT DISTINCT dg.marker_id AS identifiable_id, m.identifier AS name ";
                    cmdText += "FROM dbo.denorm_genotype dg ";
                    cmdText += "INNER JOIN dbo.marker m ON m.marker_id = dg.marker_id ";
                    break;
                case IdentifiableType.Individual:
                    cmdText += "SELECT DISTINCT dg.individual_id AS identifiable_id, s.identifier AS name ";
                    cmdText += "FROM dbo.denorm_genotype dg ";
                    cmdText += "INNER JOIN dbo.individual s ON s.individual_id = dg.individual_id ";
                    break;
                default:
					throw new Exception("Unknown IdentifiableType encountered.");
			}

            cmdText += "WHERE 1 = 1 ";
            if (usedFilters.Plate)
            {
                cmdText += "AND dg.plate_id IN (SELECT plate_id FROM #plate) ";
            }
            if (usedFilters.GenotypeGroup)
            {
                cmdText += "AND dg.genotype_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #genotype_wset gw ON (gw.wset_id = wm.wset_id AND wm.kind_id = @genotype_kind_id)) ";
            }
            if (usedFilters.SampleGroup)
            {
                cmdText += "AND dg.sample_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #sample_wset sw ON (sw.wset_id = wm.wset_id AND wm.kind_id = @sample_kind_id)) ";
            }
            if (usedFilters.IndividualGroup)
            {
                cmdText += "AND dg.individual_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #individual_wset idvw ON (idvw.wset_id = wm.wset_id AND wm.kind_id = @individual_kind_id)) ";
            }
            if (usedFilters.MarkerGroup)
            {
                cmdText += "AND dg.marker_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #marker_wset mw ON (mw.wset_id = wm.wset_id AND wm.kind_id = @marker_kind_id)) ";
            }
            if (usedFilters.AssayGroup)
            {
                cmdText += "AND dg.assay_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #assay_wset aw ON (aw.wset_id = wm.wset_id AND wm.kind_id = @assay_kind_id)) ";
            }
            if (usedFilters.Sample)
            {
                cmdText += "AND dg.sample_id IN (SELECT sample_id FROM #picked_sample) ";
            }
            if (usedFilters.Assay)
            {
                cmdText += "AND dg.assay_id IN (SELECT assay_id FROM #picked_assay) ";
            }
            if (usedFilters.Individual)
            {
                cmdText += "AND dg.individual_id IN (SELECT individual_id FROM #picked_individual) ";
            }
            if (usedFilters.Marker)
            {
                cmdText += "AND dg.marker_id IN (SELECT marker_id FROM #picked_marker) ";
            }
            cmdText += "ORDER BY name ";

            if (mode == Mode.Sync)
            {
                return FillTableSync(cmdText);
            }
            else
            {
                FillDataSetAsync(cmdText);
                return null;
            }
		}

		public void RejectMinority(int [] experimentId, int majorityPercent)
		{
            //The markerId array can be null to reject minorities from all markers.
			string cmdText;

			try
			{
				BeginTransaction();
                if (experimentId != null)
                {
                    for (int i = 0; i < experimentId.Length; i++)
                    {
                        cmdText = "";
                        cmdText += "EXEC pSQAT_RejectMinority " + experimentId[i].ToString() + ", ";
                        cmdText += majorityPercent.ToString() + " ";

                        ExecuteSync(cmdText);
                    }
                }
                else
                {
                    cmdText = "";
                    cmdText += "EXEC pSQAT_RejectMinority NULL, " + majorityPercent.ToString() + " ";

                    ExecuteSync(cmdText);
                }
				CommitTransaction();
				OnGenotypeStatusChange();
			}
			catch (Exception e)
			{
				RollbackTransaction();
				throw (e);
			}
		}

		public DataTable GetPlatesInSelection()
		{
            //Returns a table containing the selected plates.
			return FillTableSync(GET_PLATE_SELECTION_STRING);
		}

        public void GetPlatesInSelection(FileServer fs)
        {
            StreamQueryResultToFile(GET_PLATE_SELECTION_STRING, null, fs);  
        }

        public DataTable GetGroupsInSelection()
        {
            //Returns a table containing the selected groups.
            return FillTableSync(GET_GROUPS_SELECTION_STRING);
        }

        public void GetGroupsInSelection(FileServer fs)
        {
            StreamQueryResultToFile(GET_GROUPS_SELECTION_STRING, null, fs);
        }

        public DataTable GetDetailedFilter()
        {
            //Returns a table containing the individually picked items and experiments.
            return FillTableSync(GET_SINGLE_SELECTION_STRING);
        }

        public void GetDetailedFilter(FileServer fs)
        {
            StreamQueryResultToFile(GET_SINGLE_SELECTION_STRING, null,  fs);
        }

		public DataSet GetPlateContents(int plateId)
		{
			string cmdText;

			cmdText = "";
			cmdText += "EXEC pSQAT_GetPlateContents " + plateId.ToString();

			return FillDataSetSync(cmdText);
		}

		public DataTable GetMarkerAnnotations(Identifiable [] annotationTypes, SessionSettings settings)
		{
			return FillTableSync(CreateMarkerAnnotationsString(annotationTypes, settings));
		}

        public void GetMarkerAnnotations(Identifiable[] annotationTypes, SessionSettings settings, FileServer fs)
        {
            StreamQueryResultToFile(CreateMarkerAnnotationsString(annotationTypes, settings), null, fs);
        }

        private string CreateMarkerAnnotationsString(Identifiable[] annotationTypes, SessionSettings settings)
        {
            string cmdText;

            cmdText = "";
            cmdText += "SELECT m.marker_id, m.identifier AS [Marker]";
            for (int i = 0; i < annotationTypes.GetLength(0); i++)
            {
                cmdText += ", dbo.fGetAnnotationConcat (m.marker_id, ";
                cmdText += annotationTypes[i].ID.ToString() + ", ';') AS [" + annotationTypes[i].Name + "] ";
            }
            cmdText += "FROM dbo.marker m ";
            if (settings.GetString(SessionSettings.Strings.ExperimentMode).ToUpper() == "MARKER")
            {
                cmdText += "INNER JOIN #experiment_stat es ON es.experiment_id = m.marker_id ";
            }
            else
            {
                cmdText += "INNER JOIN dbo.assay a ON a.marker_id = m.marker_id ";
                cmdText += "INNER JOIN #experiment_stat es ON es.experiment_id = a.assay_id ";
            }

            return cmdText;
        }

		public DataTable GetAnnotationTypes()
		{
			string cmdText;

			cmdText = "";
			cmdText += "SELECT annotation_type_id, identifier AS [Annotation type] FROM dbo.annotation_type ";

			return FillTableSync(cmdText);
		}

		public DataTable GetRerunItems(SessionSettings settings, RerunSettings pickSettings)
		{
			string cmdText;

			cmdText = "";
			cmdText += "EXEC pSQAT_GetRerunItems ";
			cmdText += settings.GetString(SessionSettings.Strings.ViewMode) + ", ";			
			cmdText += pickSettings.GetMissingGenotypeString() + ", ";
			cmdText += pickSettings.MissingGenotypePercent.ToString() + ", ";
			cmdText += pickSettings.GetLowAlleleFrqString() + ", ";
			cmdText += pickSettings.LowAlleleFrqPercent.ToString() + ", ";
			cmdText += pickSettings.GetDuplicateErrorsString() + " ";

			return FillTableSync(cmdText);
		}

        public DataTable GetFailedInheritanceTrios(int[] experimentID)
        {
            //If experimentID is null, all experiments will be shown, otherwise only those in experimentID.

            string cmdText = "";

            cmdText = "";
            if (experimentID != null)
            {
                cmdText += "DECLARE @selected_experiment TABLE(experiment_id INTEGER PRIMARY KEY) ";
                for (int i = 0; i < experimentID.Length; i++)
                {
                    cmdText += "INSERT INTO @selected_experiment (experiment_id) VALUES (" + experimentID[i].ToString() + ") ";
                }
            }

            cmdText += "SELECT ic.individual_id AS child_id, ift.identifier AS Father, imt.identifier AS Mother, ic.identifier AS Child, COUNT(*) AS [Failed experiments] ";
            cmdText += "FROM #inheritance_trio iht ";
            cmdText += "LEFT OUTER JOIN dbo.individual ift ON ift.individual_id = iht.father_id ";
            cmdText += "LEFT OUTER JOIN dbo.individual imt ON imt.individual_id = iht.mother_id ";
            cmdText += "LEFT OUTER JOIN dbo.individual ic ON ic.individual_id = iht.child_id ";
            cmdText += "WHERE iht.success = 0 ";
            if (experimentID != null)
            {
                cmdText += "AND iht.experiment_id IN (SELECT experiment_id FROM @selected_experiment) ";
            }
            cmdText += "GROUP BY ic.individual_id, ift.identifier, imt.identifier, ic.identifier ";

            return FillTableSync(cmdText);

        }

		public DataSet PerformTest(SessionSettings settings, DataServer.Mode mode)
		{
			string cmdText;

			cmdText = "";
			cmdText += "EXEC pSQAT_TestMarkers "; 
			cmdText += settings.GetString(SessionSettings.Strings.ViewMode) + ", ";
			cmdText += settings.GetString(SessionSettings.Bools.DoInheritanceTest) + ", ";
			cmdText += settings.GetString(SessionSettings.Bools.HumanXChr) + ", ";
			cmdText += settings.GetString(SessionSettings.Bools.DemandDuplicates) + ", ";
			cmdText += settings.GetString(SessionSettings.Bools.DoHWTest) + ", ";
			cmdText += settings.GetString(SessionSettings.Bools.HWSkipChildren) + ", ";
			cmdText += settings.GetString(SessionSettings.Doubles.HWLimit);

			if (mode == Mode.Sync)
			{
				return FillDataSetSync(cmdText);
			}
			else
			{
				FillDataSetAsync(cmdText);
				return null;
			}
		}


        public DataSet TestExperimentBatch(SessionSettings settings, FilterIndicator usedFilters, int [] experimentIDs, bool initiate, DataServer.Mode mode)
        {
            string cmdText;

            //Generate SQL query depending on which filters that have been used.
            cmdText = "";
            cmdText += "DECLARE @selcmd VARCHAR(3000) ";
            cmdText += "";
            cmdText += "SET @selcmd = ' ";
            cmdText += "SELECT dg.genotype_id AS genotype_id, ";
            cmdText += (settings.GetString(SessionSettings.Strings.ViewMode) == "SAMPLE") ? "dg.sample_id AS item_id, " : "dg.individual_id AS item_id, ";
            cmdText += (settings.GetString(SessionSettings.Strings.ExperimentMode) == "ASSAY") ? "dg.assay_id AS experiment_id, " : "dg.marker_id AS experiment_id, ";
            cmdText += "dg.alleles AS alleles, ";
            cmdText += "ISNULL(ps.new_status_id, dg.status_id) AS status_id ";
            cmdText += "FROM dbo.denorm_genotype dg ";
            cmdText += "LEFT JOIN #pending_status ps ON ps.genotype_id = dg.genotype_id ";  //Obs, behövs bara vid interaktiv!
            cmdText += "WHERE 1 = 1 ";

            if (usedFilters.Plate)
            {
                cmdText += "AND dg.plate_id IN (SELECT plate_id FROM #plate) ";
            }
            if (usedFilters.GenotypeGroup)
            {
                cmdText += "AND dg.genotype_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #genotype_wset gw ON (gw.wset_id = wm.wset_id AND wm.kind_id IN (SELECT kind_id FROM kind WHERE name = ''GENOTYPE''))) ";
            }
            if (usedFilters.SampleGroup)
            {
                cmdText += "AND dg.sample_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #sample_wset sw ON (sw.wset_id = wm.wset_id AND wm.kind_id IN (SELECT kind_id FROM kind WHERE name = ''SAMPLE''))) ";
            }
            if (usedFilters.IndividualGroup)
            {
                cmdText += "AND dg.individual_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #individual_wset idvw ON (idvw.wset_id = wm.wset_id AND wm.kind_id IN (SELECT kind_id FROM kind WHERE name = ''INDIVIDUAL''))) ";
            }
            if (usedFilters.MarkerGroup)
            {
                cmdText += "AND dg.marker_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #marker_wset mw ON (mw.wset_id = wm.wset_id AND wm.kind_id IN (SELECT kind_id FROM kind WHERE name = ''MARKER''))) ";
            }
            if (usedFilters.AssayGroup)
            {
                cmdText += "AND dg.assay_id IN (SELECT wm.identifiable_id FROM dbo.wset_member wm INNER JOIN #assay_wset aw ON (aw.wset_id = wm.wset_id AND wm.kind_id IN (SELECT kind_id FROM kind WHERE name = ''ASSAY''))) ";
            }
            if (usedFilters.Sample)
            {
                cmdText += "AND dg.sample_id IN (SELECT sample_id FROM #picked_sample) ";
            }
            if (usedFilters.Assay)
            {
                cmdText += "AND dg.assay_id IN (SELECT assay_id FROM #picked_assay) ";
            }
            if (usedFilters.Individual)
            {
                cmdText += "AND dg.individual_id IN (SELECT individual_id FROM #picked_individual) ";
            }
            if (usedFilters.Marker)
            {
                cmdText += "AND dg.marker_id IN (SELECT marker_id FROM #picked_marker) ";
            }

            //Add the statement for only analyzing markers in the current batch.
            cmdText += "AND dg.marker_id IN ( ";
            for (int i = 0; i < experimentIDs.GetLength(0) - 1; i++)
            {
                if (experimentIDs[i] > 0)
                {
                    cmdText += experimentIDs[i].ToString() + ", ";
                }
            }
            if (experimentIDs[experimentIDs.GetLength(0)-1] > 0)
            {
                cmdText += experimentIDs[experimentIDs.GetLength(0)-1].ToString() + " ";
            }
            cmdText += ")' ";
            cmdText += "";
            cmdText += "EXEC pSQAT_TestMarkers ";
            cmdText += initiate ? "1, " : "0, ";
            cmdText += "@selcmd, '";
            cmdText += settings.GetString(SessionSettings.Strings.ViewMode) + "', '";
            cmdText += settings.GetString(SessionSettings.Strings.ExperimentMode) + "', ";
            cmdText += settings.GetString(SessionSettings.Bools.DoInheritanceTest) + ", ";
            cmdText += settings.GetString(SessionSettings.Bools.HumanXChr) + ", ";
            cmdText += settings.GetString(SessionSettings.Bools.DemandDuplicates) + ", ";
            cmdText += settings.GetString(SessionSettings.Bools.DoHWTest) + ", ";
            cmdText += settings.GetString(SessionSettings.Bools.HWSkipChildren) + ", ";
            cmdText += settings.GetString(SessionSettings.Doubles.HWLimit);


            //Exectute query.
            if (mode == Mode.Sync)
            {
                return FillDataSetSync(cmdText);
            }
            else
            {
                FillDataSetAsync(cmdText);
                return null;
            }

        }

        public DataSet GetTestResults(DataServer.Mode mode, int experimentMaxRows, string experimentOrderClause)
        {
            string cmdText;

            cmdText = CreateExperimentStatQuery(experimentMaxRows, experimentOrderClause);
            cmdText += GET_ITEM_STAT_STRING + GET_CONTROL_ITEM_STAT_STRING;

            //Exectute query.
            if (mode == Mode.Sync)
            {
                return FillDataSetSync(cmdText);
            }
            else
            {
                FillDataSetAsync(cmdText);
                return null;
            }
        }

        public DataSet PopDeliveredDataSet()
		{
			DataSet returnDataSet;

			if (MyDataDeliveryFlag)
			{
				returnDataSet = MyDataSet;
				MyDataDeliveryFlag = false;
				MyDataSet = null;
				return Format(returnDataSet);
			}
			else
			{
				return null;
			}
		}

		public Thread ExecutionThread
		{
			get
			{
				return MyThread;
			}
		}

		public void Abort()
		{
			//Attempts to abort asynchronus operations and returns 
			//only when the database command thread has stopped.
			if (MyThread != null)
			{
				if (MyCommand != null)
				{
					MyCommand.Cancel();
				}
				MyThread.Join();
			}
		}

		private void StoreSessionSetting(string sessionName, string settingName, string val)
		{
			string cmdText;
			string keyName;

			keyName = sessionName + "_" + settingName;
			cmdText = "";
			cmdText += "INSERT INTO session_setting (key_name, subkey, value_char) ";
			cmdText += "VALUES ";
			cmdText += "('" + keyName + "', '" + keyName + "', '" + val + "') ";
			ExecuteSync(cmdText);
		}

		private void StoreSessionSetting(string sessionName, string settingName, bool val)
		{
			string cmdText;
			string keyName;

			keyName = sessionName + "_" + settingName;
			cmdText = "";
			cmdText += "INSERT INTO session_setting (key_name, subkey, value_int) ";
			cmdText += "VALUES ";
			cmdText += "('" + keyName + "', '" + keyName + "', " + (val ? "1" : "0") + ") ";
			ExecuteSync(cmdText);
		}

		private void StoreSessionSetting(string sessionName, string settingName, double val)
		{
			string cmdText;
			string keyName;

			keyName = sessionName + "_" + settingName;
			cmdText = "";
			cmdText += "INSERT INTO session_setting (key_name, subkey, value_dec) ";
			cmdText += "VALUES ";
			cmdText += "('" + keyName + "', '" + keyName + "', " + val.ToString().Replace(",", ".") + ") ";
			ExecuteSync(cmdText);
		}

		private bool GetSessionSettingBool(string sessionName, string settingName, out bool loadError)
		{
			string cmdText;
			DataTable tempTable;

			cmdText = "";
			cmdText += "SELECT value_int FROM dbo.session_setting WHERE key_name = '" + sessionName + "_" + settingName + "' ";
			tempTable = FillTableSync(cmdText);
			if (tempTable.Rows.Count > 0)
			{
				loadError = false;
				return (Convert.ToInt32(tempTable.Rows[0][0]) == 1 ? true : false);
			}
			else
			{
				loadError = true;
				return false;
			}
		}

		private string GetSessionSettingString(string sessionName, string settingName, out bool loadError)
		{
			string cmdText;
			DataTable tempTable;

			cmdText = "";
			cmdText += "SELECT value_char FROM dbo.session_setting WHERE key_name = '" + sessionName + "_" + settingName + "' ";
			tempTable = FillTableSync(cmdText);
			if (tempTable.Rows.Count > 0)
			{
				loadError = false;
				return tempTable.Rows[0][0].ToString();
			}
			else
			{
				loadError = true;
				return "";
			}
		}

		private double GetSessionSettingDouble(string sessionName, string settingName, out bool loadError)
		{
			string cmdText;
			DataTable tempTable;

			cmdText = "";
			cmdText += "SELECT value_dec FROM dbo.session_setting WHERE key_name = '" + sessionName + "_" + settingName + "' ";
			tempTable = FillTableSync(cmdText);
			if (tempTable.Rows.Count > 0)
			{
				loadError = false;
				return Convert.ToDouble(tempTable.Rows[0][0]);
			}
			else
			{
				loadError = true;
				return 0;
			}
		}

        private string CreateExperimentStatQuery(int maxRows, string orderClause)
        {
            string cmdText;

            cmdText = "SELECT TOP " + maxRows.ToString() + " experiment_id, "
            + "identifier AS [Experiment//CN], "
            + "fq_XX AS [XX//IN], "
            + "fq_YY AS [YY//IN], "
            + "fq_XY AS [XY//IN], "
            + "not_approved AS [Not approved//IF], "
            + "success_rate AS [Success rate//PN], "
            + "fq_XX_exp AS [Expct XX//IN], "
            + "fq_YY_exp AS [Expct YY//IN], "
            + "fq_XY_exp AS [Expct XY//IN], "
            + "HW_chi2 AS [HW chi2//RN], "
            + "chrom_name AS [Chrom//CN], "
            + "fq_X AS [X fq//PN], "
            + "fq_Y AS [Y fq//PN], "
            + "dupl_failed_item AS [Dupl failed item//IF], "
            + "dupl_tested_item AS [Dupl tested item//IN], "
            + "item_validation AS [Item validtn//PN], "
            + "item_mismatch AS [Item mismatch//PF], "
            + "inh_failed_item AS [Inh failed item//IF], "
            + "inh_tested_item AS [Inh tested item//IN], "
            + "HW_failure AS [HW failure//CF], "
            + "rejected_gt AS [Rejected gt//IN], "
            + "inh_ctrl_failed_item AS [Inh ctrl failed item//IF], "
            + "inh_ctrl_tested_item AS [Inh ctrl tested item//IN], "
            + "blank_ctrl_failed_item AS [Blank ctrl failed item//IF], "
            + "blank_ctrl_tested_item AS [Blank ctrl tested item//IN], "
            + "homozyg_ctrl_failed_item AS [Homozyg ctrl failed item//IF], "
            + "homozyg_ctrl_tested_item AS [Homozyg ctrl tested item//IN] "
            + "FROM #experiment_stat ";
            if (orderClause != null)
            {
                if (orderClause.Trim() != "")
                {
                    cmdText += "ORDER BY " + orderClause + " ";
                }
            }

            return cmdText;
        }

        private string CreateInClause(StringCollection items)
        {
            //Returns the expression for an SQL IN-clause from a string collection,
            //e.g. "('Item1', 'Item2')" if the collection contains the strings "Item1" and "Item2".
            //The collection must contain at least one item.
            string s;

            if (items.Count < 1)
            {
                throw new Exception("Unable to create IN-clause from a collection with no items.");
            }

            s = "";
            s += "(";
            for (int i = 0 ; i < items.Count - 1; i++)
            {
                s += "'" + items[i] + "', ";
            }
            s += "'" + items[items.Count - 1] + "')";

            return s;
        }

		private DataSet Format(DataSet dSet)
		{
			//Modifies the data set by moving the column format code
			//to the extended properties of the columns in all tables.
			foreach (DataTable table in dSet.Tables)
			{
				this.Format(table);
			}
			return dSet;
		}

		private DataTable Format(DataTable table)
		{
			//Modifies the data table by moving the column format code
			//to the extended properties of the columns.
			string tempCode;
			string tempDisplayName;
			int tempPrefixIndex;

			foreach(DataColumn column in table.Columns)
			{
				tempPrefixIndex = column.ColumnName.IndexOf("//");
				if (column.ColumnName.Length >= 4 && tempPrefixIndex > 0)
				{
					//The format code is the two characters after "//". Move the format characters
					//from the column name to extended properties.
					tempCode = column.ColumnName.Substring(column.ColumnName.Length - 2);
					tempDisplayName = column.ColumnName.Substring(0, column.ColumnName.Length - 4); 
					column.ExtendedProperties.Add("FormatCode", tempCode);
					column.ColumnName = tempDisplayName;
				}
			}
			return table;
		}

		private void OnGenotypeStatusChange()
		{
			//Raise event if we have any subscribers.
			if (GenotypeStatusChange != null)
			{
				GenotypeStatusChange(this, EventArgs.Empty);
			}			
		}

		private void OnDataReady()
		{
			//Raise event if we have any subscribers.
			if (DataReady != null)
			{
				MyDataDeliveryFlag = true;
				DataReady(this, EventArgs.Empty);
			}			
		}

		private void OnDataError(Exception e)
		{
			ThreadExceptionEventArgs a;
			//Raise event if we have any subscribers.
			if (DataError != null)
			{
				MyDataDeliveryFlag = false;
				a = new ThreadExceptionEventArgs(e);
				DataError(this, a);
			}			
		}

		private DataTable GetExperimentGenotypes(string tableName, int [] experimentID, string success, string whereClause)
		{
			//If experimentID is null, all experiments will be shown, otherwise only those in experimentID.
			//Set success to null in order to show all tested genotypes or when the table does not have
			//a success column.
			//The whereClause string should not contain the WHERE command (e.g. it should only be "alleles = 'N/A'").
			//and can be null.
			//The returned table writes the genotype status within paranthesis if it is a pending status change.

			string cmdText = "";

			cmdText = "";
			if (experimentID != null)
			{
				cmdText += "DECLARE @selected_experiment TABLE(experiment_id INTEGER PRIMARY KEY) ";
				for (int i = 0; i < experimentID.Length; i++)
				{
					cmdText += "INSERT INTO @selected_experiment (experiment_id) VALUES (" + experimentID[i].ToString() + ") ";
				}
			}

			cmdText += "SELECT DISTINCT dg.genotype_id, p.identifier AS [Plate], ";
            cmdText += "dbo.fTranslateCoord(dg.pos_x, dg.pos_y) AS [Position], ";
            cmdText += "idv.identifier AS [Individual], ";
			cmdText += "smp.identifier AS [Sample], m.identifier AS [Marker], a.identifier AS [Assay], ";
			cmdText += "dg.alleles AS [Result], ";
			cmdText += "CASE WHEN ps.new_status_id IS NULL THEN sn.name ELSE '(' + psn.name + ')' END AS [Status] ";
			cmdText += "FROM " + tableName + " t ";
            cmdText += "INNER JOIN #all_genotypes ag ON (ag.item_id = t.item_id AND ag.experiment_id = t.experiment_id) ";
			cmdText += "INNER JOIN dbo.denorm_genotype dg ON dg.genotype_id = ag.genotype_id ";
            cmdText += "INNER JOIN dbo.individual idv ON idv.individual_id = dg.individual_id ";
            cmdText += "INNER JOIN dbo.sample smp ON smp.sample_id = dg.sample_id ";
            cmdText += "INNER JOIN dbo.assay a ON a.assay_id = dg.assay_id ";
            cmdText += "INNER JOIN dbo.marker m ON m.marker_id = dg.marker_id ";
            cmdText += "INNER JOIN dbo.plate p ON p.plate_id = dg.plate_id ";
            cmdText += "INNER JOIN dbo.status sn ON sn.status_id = dg.status_id ";
			cmdText += "LEFT OUTER JOIN #pending_status ps ON ps.genotype_id = dg.genotype_id ";
            cmdText += "LEFT OUTER JOIN dbo.status psn ON psn.status_id = ps.new_status_id ";
			cmdText += "WHERE 1 = 1 "; //To make all where clauses be appended with AND.
			if (experimentID != null)
			{
				cmdText += "AND ag.experiment_id IN (SELECT experiment_id FROM @selected_experiment) ";
			}
			if (success != null)
			{
				cmdText += "AND t.success = " + success + " ";			
			}
			if (whereClause != null)
			{
				cmdText += "AND " + whereClause + " ";
			}

			return FillTableSync(cmdText);
		}


        private DataTable GetItemGenotypes(string tableName, int[] itemID, string success, string whereClause)
        {
            //If itemID is null, all items will be shown, otherwise only those in itemID.
            //Set success to null in order to show all tested genotypes or when the table does not have
            //a success column.
            //The whereClause string should not contain the WHERE command (e.g. it should only be "alleles = 'N/A'").
            //and can be null.
            //The returned table writes the genotype status within paranthesis if it is a pending status change.

            string cmdText = "";

            cmdText = "";
            if (itemID != null)
            {
                cmdText += "DECLARE @selected_item TABLE(item_id INTEGER PRIMARY KEY) ";
                for (int i = 0; i < itemID.Length; i++)
                {
                    cmdText += "INSERT INTO @selected_item (item_id) VALUES (" + itemID[i].ToString() + ") ";
                }
            }

            cmdText += "SELECT DISTINCT dg.genotype_id, p.identifier AS [Plate], ";
            cmdText += "dbo.fTranslateCoord(dg.pos_x, dg.pos_y) AS [Position], ";
            cmdText += "idv.identifier AS [Individual], ";
            cmdText += "smp.identifier AS [Sample], m.identifier AS [Marker], a.identifier AS [Assay], ";
            cmdText += "dg.alleles AS [Result], ";
            cmdText += "CASE WHEN ps.new_status_id IS NULL THEN sn.name ELSE '(' + psn.name + ')' END AS [Status] ";
            cmdText += "FROM " + tableName + " t ";
            cmdText += "INNER JOIN #all_genotypes ag ON (ag.item_id = t.item_id AND ag.experiment_id = t.experiment_id) ";
            cmdText += "INNER JOIN dbo.denorm_genotype dg ON dg.genotype_id = ag.genotype_id ";
            cmdText += "INNER JOIN dbo.individual idv ON idv.individual_id = dg.individual_id ";
            cmdText += "INNER JOIN dbo.sample smp ON smp.sample_id = dg.sample_id ";
            cmdText += "INNER JOIN dbo.assay a ON a.assay_id = dg.assay_id ";
            cmdText += "INNER JOIN dbo.marker m ON m.marker_id = dg.marker_id ";
            cmdText += "INNER JOIN dbo.plate p ON p.plate_id = dg.plate_id ";
            cmdText += "INNER JOIN dbo.status sn ON sn.status_id = dg.status_id ";
            cmdText += "LEFT OUTER JOIN #pending_status ps ON ps.genotype_id = dg.genotype_id ";
            cmdText += "LEFT OUTER JOIN dbo.status psn ON psn.status_id = ps.new_status_id ";
            cmdText += "WHERE 1 = 1 "; //To make all where clauses be appended with AND.
            if (itemID != null)
            {
                cmdText += "AND ag.item_id IN (SELECT item_id FROM @selected_item) ";
            }
            if (success != null)
            {
                cmdText += "AND t.success = " + success + " ";
            }
            if (whereClause != null)
            {
                cmdText += "AND " + whereClause + " ";
            }

            return FillTableSync(cmdText);
        }

        private DataTable FillTableSyncNoFormat(string cmdText)
        {
            return FillDataSetSyncNoFormat(cmdText).Tables[0];
        }

		private DataTable FillTableSync(string cmdText)
		{
			return FillDataSetSync(cmdText).Tables[0];
		}

        [MethodImpl(MethodImplOptions.Synchronized)]
        private DataSet FillDataSetSyncNoFormat(string cmdText)
        {
            DataSet dSet;
            SqlCommand cmd;
            SqlDataAdapter dAdapter;

            dSet = new DataSet();
            cmd = new SqlCommand(cmdText, MyConnection);
            cmd.CommandTimeout = MySyncTimeout;
            dAdapter = new SqlDataAdapter();
            dAdapter.SelectCommand = cmd;

            dAdapter.Fill(dSet);
            return dSet;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
		private DataSet FillDataSetSync(string cmdText)
		{
			DataSet dSet;
			SqlCommand cmd;
			SqlDataAdapter dAdapter;

			dSet = new DataSet();
			cmd = new SqlCommand(cmdText, MyConnection);
			cmd.CommandTimeout = MySyncTimeout;
			dAdapter = new SqlDataAdapter();
			dAdapter.SelectCommand = cmd;

			dAdapter.Fill(dSet);
			return Format(dSet);
		}

        [MethodImpl(MethodImplOptions.Synchronized)]
		private void ExecuteSync(string cmdText)
		{
			SqlCommand cmd;

			cmd = new SqlCommand(cmdText, MyConnection);
			cmd.CommandTimeout = MySyncTimeout;
			cmd.ExecuteNonQuery();
		}

        [MethodImpl(MethodImplOptions.Synchronized)]
		private int ExecuteIntSync(string cmdText)
		{
			SqlCommand cmd;

			cmd = new SqlCommand(cmdText, MyConnection);
			cmd.CommandTimeout = MySyncTimeout;
			return (int)cmd.ExecuteScalar();
		}

        [MethodImpl(MethodImplOptions.Synchronized)]
		private void FillDataSetAsync(string cmdText)
		{
			MyDataDeliveryFlag = false;
			MyCommand = new SqlCommand(cmdText, MyConnection);
			MyCommand.CommandTimeout = MyAsyncTimeout;

			MyThread = new Thread(new ThreadStart(ExecuteSelect));
			MyThread.Start();
		}

        [MethodImpl(MethodImplOptions.Synchronized)]
		private void ExecuteSelect()
		{
			SqlDataAdapter dAdapter;

			try
			{
				dAdapter = new SqlDataAdapter();
				MyDataSet = new DataSet();
				dAdapter.SelectCommand = MyCommand;
				dAdapter.Fill(MyDataSet);
                MyCommand = null;
				OnDataReady();
			}
			catch (Exception e)
			{
				//If the exception was thrown because the user cancelled, do nothing.
				//For other exceptions raise error event.
				if (e.Message.IndexOf("cancelled") < 0)
				{
					OnDataError(e);
				}
			}
		}

		private string TestSuccessToString(TestSuccess val)
		{
			if (val == TestSuccess.Failure)
			{ return "0"; }
			else if (val == TestSuccess.Success)
			{ return "1"; }
			else
			{ return null; }
		}

		public enum TestSuccess
		{
			Success = 0,
			Failure = 1,
			All = 2
		}


		public enum IdentifiableType
		{
			Sample = 0,
			Marker = 1,
			Assay = 2,
            Individual = 3
		}

		public enum Mode
		{
			Sync = 0,
			Async = 1
		}


	}
}