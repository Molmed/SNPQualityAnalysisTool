using System;
using Molmed.SQAT.DBObjects;

namespace Molmed.SQAT.ClientObjects
{

    public class ReportSettings
    {
        private bool MyIncludeSourceInfo;
        private bool MyIncludeSelectedPlates;
        private bool MyIncludeSelectedGroups;
        private bool MyIncludeDetailedFilter;
        private bool MyIncludeExperimentStatistics;
        private bool MyIncludeMarkerAnnotations;
        private bool MyIncludeItemStatistics;
        private bool MyIncludeControlItemStatistics;
        private bool MyIncludeResults;
        private bool MySplitFile;
        private bool[] MyExperimentStatFlags;
        private bool[] MyItemStatFlags;
        private string MyEmptyResultString;
        private string MyReportType;
        private string MyResultTableType;
        private string MyFilePath;
        private string MyFileName;
        private Identifiable[] MyAnnotationTypes;

        public const string ReportTypeText = "TEXT";
        public const string ReportTypeXML = "XML";

        public const string ResultTableTypeExpmRows = "TABLE_EXPERIMENT_ROWS";
        public const string ResultTableTypeExpmCol = "TABLE_EXPERIMENT_COLUMNS";
        public const string ResultTableTypePairAll = "PAIRWISE_ALL";
        public const string ResultTableTypePairApp = "PAIRWISE_APPROVED";


        public bool Get(ReportSettings.Bools item)
        {
            switch (item)
            {
                case ReportSettings.Bools.IncludeSourceInfo:
                    return MyIncludeSourceInfo;
                case ReportSettings.Bools.IncludeSelectedPlates:
                    return MyIncludeSelectedPlates;
                case ReportSettings.Bools.IncludeSelectedGroups:
                    return MyIncludeSelectedGroups;
                case ReportSettings.Bools.IncludeSelectedDetailedFilter:
                    return MyIncludeDetailedFilter;
                case ReportSettings.Bools.IncludeExperimentStat:
                    return MyIncludeExperimentStatistics;
                case ReportSettings.Bools.IncludeAnnotations:
                    return MyIncludeMarkerAnnotations ;
                case ReportSettings.Bools.IncludeItemStat:
                    return MyIncludeItemStatistics;
                case ReportSettings.Bools.IncludeControlItemStat:
                    return MyIncludeControlItemStatistics;
                case ReportSettings.Bools.IncludeResults:
                    return MyIncludeResults;
                case ReportSettings.Bools.SplitFile:
                    return MySplitFile;
                default:
                    throw new Exception("Attempting to get unknown report setting.");
            }
        }

        public void Set(ReportSettings.Bools item, bool val)
        {
            switch (item)
            {
                case ReportSettings.Bools.IncludeSourceInfo:
                    MyIncludeSourceInfo = val;
                    break;
                case ReportSettings.Bools.IncludeSelectedPlates:
                    MyIncludeSelectedPlates = val;
                    break;
                case ReportSettings.Bools.IncludeSelectedGroups:
                    MyIncludeSelectedGroups = val;
                    break;
                case ReportSettings.Bools.IncludeSelectedDetailedFilter:
                    MyIncludeDetailedFilter = val;
                    break;
                case ReportSettings.Bools.IncludeExperimentStat:
                    MyIncludeExperimentStatistics = val;
                    break;
                case ReportSettings.Bools.IncludeAnnotations:
                    MyIncludeMarkerAnnotations = val;
                    break;
                case ReportSettings.Bools.IncludeItemStat:
                    MyIncludeItemStatistics = val;
                    break;
                case ReportSettings.Bools.IncludeControlItemStat:
                    MyIncludeControlItemStatistics = val;
                    break;
                case ReportSettings.Bools.IncludeResults:
                    MyIncludeResults = val;
                    break;
                case ReportSettings.Bools.SplitFile:
                    MySplitFile = val;
                    break;
                default:
                    throw new Exception("Attempting to set unknown report setting.");
            }
        }

        public bool [] Get(ReportSettings.BoolArrays item)
        {
            switch (item)
            {
                case ReportSettings.BoolArrays.ExperimentStatFlags:
                    return MyExperimentStatFlags;
                case ReportSettings.BoolArrays.ItemStatFlags:
                    return MyItemStatFlags;
                default:
                    throw new Exception("Attempting to get unknown report setting.");
            }
        }

        public void Set(ReportSettings.BoolArrays item, bool[] val)
        {
            switch (item)
            {
                case ReportSettings.BoolArrays.ExperimentStatFlags:
                    MyExperimentStatFlags = val;
                    break;
                case ReportSettings.BoolArrays.ItemStatFlags:
                    MyItemStatFlags = val;
                    break;
                default:
                    throw new Exception("Attempting to set unknown report setting.");
            }
        }

        public string Get(ReportSettings.Strings item)
        {
            switch (item)
            {
                case ReportSettings.Strings.EmptyResultString:
                    return MyEmptyResultString;
                case ReportSettings.Strings.ReportType:
                    return MyReportType;
                case ReportSettings.Strings.ResultTableType:
                    return MyResultTableType;
                case ReportSettings.Strings.FilePath:
                    return MyFilePath;
                case ReportSettings.Strings.FileName:
                    return MyFileName;
                default:
                    throw new Exception("Attempting to get unknown report setting.");
            }
        }

        public void Set(ReportSettings.Strings item, string val)
        {
            switch (item)
            {
                case ReportSettings.Strings.EmptyResultString:
                    MyEmptyResultString = val;
                    break;
                case ReportSettings.Strings.ReportType:
                    MyReportType = val;
                    break;
                case ReportSettings.Strings.ResultTableType:
                    MyResultTableType = val;
                    break;
                case ReportSettings.Strings.FilePath:
                    MyFilePath = val;
                    break;
                case ReportSettings.Strings.FileName:
                    MyFileName = val;
                    break;
                default:
                    throw new Exception("Attempting to set unknown report setting.");
            }
        }

        public Identifiable[] Get(ReportSettings.Identifiables item)
        {
            switch (item)
            {
                case ReportSettings.Identifiables.AnnotationTypes:
                    return MyAnnotationTypes;
                default:
                    throw new Exception("Attempting to get unknown report setting.");
            }
        }

        public void Set(ReportSettings.Identifiables item, Identifiable[] val)
        {
            switch (item)
            {
                case ReportSettings.Identifiables.AnnotationTypes:
                    MyAnnotationTypes = val;
                    break;
                default:
                    throw new Exception("Attempting to set unknown report setting.");
            }
        }

        public enum Bools
        {
            IncludeSourceInfo = 1,
            IncludeSelectedPlates = 2,
            IncludeSelectedGroups = 3,
            IncludeSelectedDetailedFilter = 4,
            IncludeExperimentStat = 5,
            IncludeItemStat = 6,
            IncludeControlItemStat = 7,
            IncludeAnnotations = 8,
            IncludeResults = 9,
            SplitFile = 10
        }

        public enum BoolArrays
        {
            ExperimentStatFlags = 1,
            ItemStatFlags = 2
        }

        public enum Strings
        {
            EmptyResultString = 1,
            ReportType = 2,
            ResultTableType = 3,
            FilePath = 4,
            FileName = 5
        }

        public enum Identifiables
        {
            AnnotationTypes = 1
        }


    }

}
