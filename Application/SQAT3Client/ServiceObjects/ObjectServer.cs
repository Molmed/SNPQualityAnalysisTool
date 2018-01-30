using System;
using System.Data;
using Molmed.SQAT.DBObjects;

namespace Molmed.SQAT.ServiceObjects
{
    public class ObjectServer
    {
        private DataServer MyDataServer;

        public ObjectServer(DataServer dServer)
        {
            MyDataServer = dServer;
        }

        public ProjectCollection GetProjects()
        {
            DataTable projectTable;
            ProjectCollection projectList;

            projectTable = MyDataServer.GetProjects();

            projectList = new ProjectCollection();
            for (int i = 0; i < projectTable.Rows.Count; i++)
            {
                projectList.Add(new Project((int)projectTable.Rows[i]["id"], 
                    projectTable.Rows[i]["identifier"].ToString()));
            }

            return projectList;
        }

        public IdentifiableGroupCollection GetGroups(Identifiable parent)
        {
            DataTable groupTable;
            IdentifiableGroupCollection groupList;
            IdentifiableGroup tempGroup;

            groupList = new IdentifiableGroupCollection();

            //Get any groups under the parent.
            groupTable = MyDataServer.GetGroups(parent.ID);
            for (int i = 0; i < groupTable.Rows.Count; i++)
            {
                switch (groupTable.Rows[i]["type"].ToString())
                {
                    case "MarkerSet":
                        tempGroup = (IdentifiableGroup) new MarkerGroup((int)groupTable.Rows[i]["wset_id"],
                            groupTable.Rows[i]["identifier"].ToString(),
                            groupTable.Rows[i]["description"].ToString());
                        break;
                    case "AssaySet":
                        tempGroup = (IdentifiableGroup) new AssayGroup((int)groupTable.Rows[i]["wset_id"],
                            groupTable.Rows[i]["identifier"].ToString(),
                            groupTable.Rows[i]["description"].ToString());
                        break;
                    case "IndividualSet":
                        tempGroup = (IdentifiableGroup) new IndividualGroup((int)groupTable.Rows[i]["wset_id"],
                            groupTable.Rows[i]["identifier"].ToString(),
                            groupTable.Rows[i]["description"].ToString());
                        break;
                    case "SampleSet":
                        tempGroup = (IdentifiableGroup) new SampleGroup((int)groupTable.Rows[i]["wset_id"],
                            groupTable.Rows[i]["identifier"].ToString(),
                            groupTable.Rows[i]["description"].ToString());
                        break;
                    case "PlateSet":
                        tempGroup = (IdentifiableGroup) new PlateGroup((int)groupTable.Rows[i]["wset_id"],
                            groupTable.Rows[i]["identifier"].ToString(),
                            groupTable.Rows[i]["description"].ToString());
                        break;
                    case "GenotypeSet":
                        tempGroup = (IdentifiableGroup)new GenotypeGroup((int)groupTable.Rows[i]["wset_id"],
                            groupTable.Rows[i]["identifier"].ToString(),
                            groupTable.Rows[i]["description"].ToString());
                        break;
                    default:
                        throw new Exception("Unknown working set " + groupTable.Rows[i]["type"].ToString());
                }
                groupList.Add(tempGroup);
            }
            return groupList;

        }

        public PlateCollection GetPlates(Identifiable parent)
        {
            DataTable plateTable;
            PlateCollection plateList;
            Plate tempPlate = null;

            plateList = new PlateCollection();
            //Get any plates under the parent.
            if (parent is Project)
            {
                plateTable = MyDataServer.GetPlatesInProject(parent.ID);
            }
            else
            {
                plateTable = MyDataServer.GetPlatesInGroup(parent.ID);
            }
            for (int i = 0; i < plateTable.Rows.Count; i++)
            {
                tempPlate = new Plate((int)plateTable.Rows[i]["plate_id"],
                    plateTable.Rows[i]["identifier"].ToString(),
                    plateTable.Rows[i]["description"].ToString());

                plateList.Add(tempPlate);
            }

            return plateList;
        }

        public SessionItemCollection GetSessions(Identifiable parent)
        {
            DataTable sessionTable;
            SessionItemCollection sessionList;
            SessionItem tempSession;

            sessionList = new SessionItemCollection();

            //Get any sessions under the parent.
            sessionTable = MyDataServer.GetSessions(parent.ID);
            for (int i = 0; i < sessionTable.Rows.Count; i++)
            {
                switch (sessionTable.Rows[i]["type"].ToString())
                {
                    case "AnalyzedGenotypeSet":
                        tempSession = new OldSavedSession((int)sessionTable.Rows[i]["wset_id"],
                            sessionTable.Rows[i]["identifier"].ToString(),
                            sessionTable.Rows[i]["description"].ToString());
                        break;
                    case "ResultGenotypeSet":
                        tempSession = new OldApprovedSession((int)sessionTable.Rows[i]["wset_id"],
                            sessionTable.Rows[i]["identifier"].ToString(),
                            sessionTable.Rows[i]["description"].ToString());
                        break;
                    case "SavedSession":
                        tempSession = new SavedSession((int)sessionTable.Rows[i]["wset_id"],
                            sessionTable.Rows[i]["identifier"].ToString(),
                            sessionTable.Rows[i]["description"].ToString());
                        break;
                    case "ApprovedSession":
                        tempSession = new ApprovedSession((int)sessionTable.Rows[i]["wset_id"],
                            sessionTable.Rows[i]["identifier"].ToString(),
                            sessionTable.Rows[i]["description"].ToString());
                        break;
                    default:
                        throw new Exception("Unknown session type " + sessionTable.Rows[i]["type"].ToString());
                }
                sessionList.Add(tempSession);
            }

            return sessionList;
        }

        public void AddSelectedItem(Identifiable item)
        {
            if ((item is GenotypeGroup) || (item is SessionItem)) { MyDataServer.AddSelectedGenotypeGroup(item.ID); }
            else if (item is MarkerGroup) { MyDataServer.AddSelectedMarkerGroup(item.ID); }
            else if (item is AssayGroup) { MyDataServer.AddSelectedAssayGroup(item.ID); }
            else if (item is IndividualGroup) { MyDataServer.AddSelectedIndividualGroup(item.ID); }
            else if (item is SampleGroup) { MyDataServer.AddSelectedSampleGroup(item.ID); }
            else if (item is PlateGroup) { MyDataServer.AddSelectedPlateGroup(item.ID); }
            else if (item is Plate) { MyDataServer.AddSelectedPlate(item.ID); }
        }

 
    }
}