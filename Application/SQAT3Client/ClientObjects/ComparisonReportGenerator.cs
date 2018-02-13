using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.ClientObjects
{

    public class ComparisonReportGenerator : InvestigationReportGenerator
    {
        private ComparisonSettings MySettings;
        private InvestigationSourceInfo MySourceInfo1, MySourceInfo2;

        public ComparisonReportGenerator(InvestigationSourceInfo sourceInfo1, InvestigationSourceInfo sourceInfo2, ComparisonSettings settings, string connectionString)
            : 
            base(connectionString)
        {
            MySourceInfo1 = sourceInfo1;
            MySourceInfo2 = sourceInfo2;
            MySettings = settings;
        }

        override public string GenerateReport()
        {
            ComparisonPrescanInfo prescanInfo;
            Comparison cmp;

            //Scan the sources for used items and experiments to avoid loading unnecessary results into memory.
            OnStatusChange("Prescanning");
            prescanInfo = GetPrescanInfo(MySourceInfo1, MySourceInfo2);

            //Get the results and do internal checking using the information found by the pre-scan.
            OnStatusChange("Loading results from source 1");
            MySourceInfo1 = Parse(MySourceInfo1, prescanInfo);
            OnStatusChange("Loading results from source 2");
            MySourceInfo2 = Parse(MySourceInfo2, prescanInfo);

            //Perform the actual comparison.
            OnStatusChange("Performing comparison");
            cmp = new Comparison(MySourceInfo1, MySourceInfo2, MySettings, prescanInfo);
            cmp.StatusChange += new Investigation.InvestigationStatusChangeHandler(Investigation_StatusChanged);
            cmp.Compare();

            OnStatusChange("Preparing report");
            return cmp.ToString();
        }

        private ComparisonPrescanInfo GetPrescanInfo(InvestigationSourceInfo sourceInfo1, InvestigationSourceInfo sourceInfo2)
        {
            //Get information about used items and experiments from both sources.
            CIStringCollection itemsSource1, experimentsSource1, itemsSource2, experimentsSource2;

            //Get used items and experiments in source 1.
            switch (sourceInfo1.SourceType)
            {
                case InvestigationSourceType.Session:
                    ScanSession(sourceInfo1.Session, out itemsSource1, out experimentsSource1);
                    break;
                case InvestigationSourceType.ReferenceSet:
                    ScanReferenceSet(sourceInfo1.SourceName, out itemsSource1, out experimentsSource1);
                    break;
                case InvestigationSourceType.File:
                    ScanFile(sourceInfo1.SourceName, out itemsSource1, out experimentsSource1);
                    break;
                default:
                    throw new Exception("Unknown comparison source type.");
            }

            //Get used items and experiments in source 2.
            switch (sourceInfo2.SourceType)
            {
                case InvestigationSourceType.Session:
                    ScanSession(sourceInfo2.Session, out itemsSource2, out experimentsSource2);
                    break;
                case InvestigationSourceType.ReferenceSet:
                    ScanReferenceSet(sourceInfo2.SourceName, out itemsSource2, out experimentsSource2);
                    break;
                case InvestigationSourceType.File:
                    ScanFile(sourceInfo2.SourceName, out itemsSource2, out experimentsSource2);
                    break;
                default:
                    throw new Exception("Unknown comparison source type.");
            }

            return new ComparisonPrescanInfo(itemsSource1, itemsSource2, experimentsSource1, experimentsSource2);
        }

        private InvestigationSourceInfo Parse(InvestigationSourceInfo sourceInfo, ComparisonPrescanInfo prescanInfo)
        {
            //Read genotype results for the items and experiments present in both sources.
            DataTable results;

            switch (sourceInfo.SourceType)
            {
                case InvestigationSourceType.Session:
                    results = ParseFromSession(sourceInfo.Session,
                                sourceInfo.ItemTypes,
                                sourceInfo.GenotypeStatuses,
                                prescanInfo.ItemsUnion,
                                prescanInfo.ExperimentsUnion);
                    break;
                case InvestigationSourceType.ReferenceSet:
                    results = ParseFromReferenceSet(sourceInfo.SourceName, prescanInfo.ItemsUnion, prescanInfo.ExperimentsUnion);
                    break;
                case InvestigationSourceType.File:
                    results = ParseFromFile(sourceInfo.SourceName,
                                prescanInfo.ItemsUnion,
                                prescanInfo.ExperimentsUnion);
                    break;
                default:
                    throw new Exception("Unknown comparison source type.");
            }
            sourceInfo.Load(results, MySettings.Mode);

            return sourceInfo;
        }

    }
}