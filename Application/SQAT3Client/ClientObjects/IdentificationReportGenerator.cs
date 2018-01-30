using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.ClientObjects
{

    public class IdentificationReportGenerator : InvestigationReportGenerator
    {
        private IdentificationSettings MySettings;
        private InvestigationSourceInfo MySourceInfo;

        public IdentificationReportGenerator(InvestigationSourceInfo sourceInfo, IdentificationSettings settings, string connectionString)
            :
            base(connectionString)
        {
            MySourceInfo = sourceInfo;
            MySettings = settings;
        }

        override public string GenerateReport()
        {
            IdentificationPrescanInfo prescanInfo;
            Identification idf;

            //Scan the sources for used items and experiments to avoid loading unnecessary results into memory.
            OnStatusChange("Prescanning");
            prescanInfo = GetPrescanInfo(MySourceInfo);

            //Get the results and do internal checking using the information found by the pre-scan.
            OnStatusChange("Loading results");
            MySourceInfo = Parse(MySourceInfo, prescanInfo);

            //Perform the actual identification.
            OnStatusChange("Performing identification");
            idf = new Identification(MySourceInfo, MySettings, prescanInfo);
            idf.StatusChange += new Investigation.InvestigationStatusChangeHandler(Investigation_StatusChanged);
            idf.Identify();

            OnStatusChange("Preparing report");
            return idf.ToString();
        }

        private IdentificationPrescanInfo GetPrescanInfo(InvestigationSourceInfo sourceInfo)
        {
            //Get information about used items and experiments from one source.
            CIStringCollection items, experiments;

            //Get used items and experiments in source 1.
            switch (sourceInfo.SourceType)
            {
                case InvestigationSourceType.Session:
                    ScanSession(sourceInfo.Session, out items, out experiments);
                    break;
                case InvestigationSourceType.ReferenceSet:
                    ScanReferenceSet(sourceInfo.SourceName, out items, out experiments);
                    break;
                case InvestigationSourceType.File:
                    ScanFile(sourceInfo.SourceName, out items, out experiments);
                    break;
                default:
                    throw new Exception("Unknown comparison source type.");
            }

            return new IdentificationPrescanInfo(items, experiments);
        }

        private InvestigationSourceInfo Parse(InvestigationSourceInfo sourceInfo, IdentificationPrescanInfo prescanInfo)
        {
            //Read genotype results for the items and experiments present in both sources.
            DataTable results;

            switch (sourceInfo.SourceType)
            {
                case InvestigationSourceType.Session:
                    results = ParseFromSession(sourceInfo.Session,
                                sourceInfo.ItemTypes,
                                sourceInfo.GenotypeStatuses,
                                prescanInfo.Items,
                                prescanInfo.Experiments);
                    break;
                case InvestigationSourceType.ReferenceSet:
                    results = ParseFromReferenceSet(sourceInfo.SourceName, prescanInfo.Items, prescanInfo.Experiments);
                    break;
                case InvestigationSourceType.File:
                    results = ParseFromFile(sourceInfo.SourceName,
                                prescanInfo.Items,
                                prescanInfo.Experiments);
                    break;
                default:
                    throw new Exception("Unknown comparison source type.");
            }
            sourceInfo.Load(results, MySettings.Mode);

            return sourceInfo;
        }

    }
}