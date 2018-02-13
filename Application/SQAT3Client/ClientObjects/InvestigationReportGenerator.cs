using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using Molmed.SQAT.GUI;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.ClientObjects
{

    abstract public class InvestigationReportGenerator
    {
        private string MyConnectionString;

        public event Investigation.InvestigationStatusChangeHandler StatusChange;

        public InvestigationReportGenerator(string connectionString)
        {
            MyConnectionString = connectionString;
        }

        abstract public string GenerateReport();

        protected void ScanSession(SessionForm selectedSession, out CIStringCollection usedItems, out CIStringCollection usedExperiments)
        {
            DataTable itemsDataTable, experimentsDataTable;
            CIStringCollection itemsCollection, experimentsCollection;

            //Get all items from the selected session.
            OnStatusChange("Retrieving items from session");
            itemsDataTable = selectedSession.GetAllItems();
            OnStatusChange("Storing session items in memory");
            itemsCollection = new CIStringCollection();
            for (int i = 0; i < itemsDataTable.Rows.Count; i++)
            {
                itemsCollection.Add(itemsDataTable.Rows[i][1].ToString());
                if (i % 1000 == 0)
                {
                    OnStatusChange("Storing session items in memory (" + i.ToString() + ")");
                }
            }

            //Get all experiments from the selected session.
            OnStatusChange("Retrieving experiments from session");
            experimentsDataTable = selectedSession.GetAllExperiments();
            OnStatusChange("Storing session experiments in memory");
            experimentsCollection = new CIStringCollection();
            for (int i = 0; i < experimentsDataTable.Rows.Count; i++)
            {
                experimentsCollection.Add(experimentsDataTable.Rows[i][1].ToString());
                if (i % 1000 == 0)
                {
                    OnStatusChange("Storing session experiments in memory (" + i.ToString() + ")");
                }
            }

            //Set out parameters.
            usedItems = itemsCollection;
            usedExperiments = experimentsCollection;
        }

        protected void ScanReferenceSet(string setName, out CIStringCollection usedItems, out CIStringCollection usedExperiments)
        {
            CIStringCollection itemsCollection, experimentsCollection;

            //Get all items from the selected reference set.
            OnStatusChange("Retrieving items from reference set");
            itemsCollection = new CIStringCollection(DataServer.GetItemsInReferenceSet(setName, MyConnectionString, Convert.ToInt32(Configuration.GetFromFile("synctimeout"))));

            //Get all experiments from the selected session.
            OnStatusChange("Retrieving experiments from reference set");
            experimentsCollection = new CIStringCollection(DataServer.GetExperimentsInReferenceSet(setName, MyConnectionString, Convert.ToInt32(Configuration.GetFromFile("synctimeout"))));

            //Set out parameters.
            usedItems = itemsCollection;
            usedExperiments = experimentsCollection;
        }

        protected void ScanFile(string filePath, out CIStringCollection usedItems, out CIStringCollection usedExperiments)
        {
            CIStringCollection itemsCollection, experimentsCollection;
            FileServer fServer;

            fServer = new FileServer();
            fServer.StatusChange += new FileServer.FileServerStatusChangeHandler(FileServer_StatusChanged);
            OnStatusChange("Retrieving items from file");
            itemsCollection = new CIStringCollection(fServer.GetItemsInComparisonFile(filePath));
            OnStatusChange("Retrieving experiments from file");
            experimentsCollection = new CIStringCollection(fServer.GetExperimentsInComparisonFile(filePath));

            //Set out parameters.
            usedItems = itemsCollection;
            usedExperiments = experimentsCollection;
        }

        protected DataTable ParseFromSession(SessionForm selectedSession, StringCollection itemTypes, StringCollection genotypeStatuses, CIStringCollection items, CIStringCollection experiments)
        {
            DataTable results;

            //Get the genotypes from the selected session.
            OnStatusChange("Retrieving genotypes from session");
            results = selectedSession.GetAllGenotypes(itemTypes, genotypeStatuses, items.StringCollection, experiments.StringCollection);

            return results;
        }

        protected DataTable ParseFromReferenceSet(string setName, CIStringCollection items, CIStringCollection experiments)
        {
            DataTable results;

            //Get the genotypes from the selected reference set.
            OnStatusChange("Retrieving genotypes from reference set");
            results = DataServer.GetResultsInReferenceSet(setName, items.StringCollection, experiments.StringCollection, MyConnectionString, Convert.ToInt32(Configuration.GetFromFile("synctimeout")));

            return results;
        }

        protected DataTable ParseFromFile(string filePath, CIStringCollection items, CIStringCollection experiments)
        {
            DataTable results;
            FileServer fServer;
            string[][] fileContents;

            fServer = new FileServer();
            OnStatusChange("Retrieving genotypes from file");
            fileContents = fServer.ReadMultipleColumns(filePath, 3);

            results = new DataTable("Genotype");
            results.Columns.Add("Item", Type.GetType("System.String"));
            results.Columns.Add("Experiment", Type.GetType("System.String"));
            results.Columns.Add("Alleles", Type.GetType("System.String"));

            for (int i = 0; i < fileContents.GetLength(0); i++)
            {
                if (items.Contains(fileContents[i][0].Trim()) && experiments.Contains(fileContents[i][1].Trim()))
                {
                    results.Rows.Add(new object[] { fileContents[i][0], fileContents[i][1], fileContents[i][2] });
                }
                if (i % 1000 == 0)
                {
                    OnStatusChange("Finished reading line " + i.ToString());
                }
            }

            return results;
        }

        protected void FileServer_StatusChanged(object sender, FileServerStatusEventArgs e)
        {
            OnStatusChange(e.Status);
        }

        protected void Investigation_StatusChanged(InvestigationStatusEventArgs e)
        {
            OnStatusChange(e.Status);
        }

        protected void OnStatusChange(string status)
        {
            if (StatusChange != null)
            {
                StatusChange(new InvestigationStatusEventArgs(status));
            }
        }


    }
}