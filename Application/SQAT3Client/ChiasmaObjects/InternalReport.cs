using System;
using System.Collections.Generic;
using System.Text;
using Molmed.SQAT.ServiceObjects;
using Molmed.SQAT.DBObjects;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class InternalReport : DataIdentity
    {
        private Int32 MyUserId;
        private Int32 MyProjectId;
        private String MySource;
        private String MyComment;
        private Boolean MyIsBulk;
        private String MySettings;

        public InternalReport(DataReader dataReader)
            : base(dataReader)
        {
            MyUserId = dataReader.GetInt32(InternalReportData.USER_ID);
            MyProjectId = dataReader.GetInt32(InternalReportData.PROJECT_ID);
            MySource = dataReader.GetString(InternalReportData.SOURCE);
            MyIsBulk = dataReader.GetBoolean(InternalReportData.IS_BULK);
            MyComment = dataReader.GetString(InternalReportData.COMMENT);
            MySettings = dataReader.GetString(InternalReportData.SETTINGS);
        }

        public String GetComment()
        {
            return MyComment;
        }

        public static Int32 GetCommentMaxLength()
        {
            return GetColumnLength(InternalReportData.TABLE, InternalReportData.COMMENT);
        }

        public override DataType GetDataType()
        {
            return DataType.InternalReport;
        }

        public static Int32 GetIdentifierMaxLength()
        {
            return GetColumnLength(InternalReportData.TABLE, InternalReportData.IDENTIFIER);
        }

        public Boolean GetIsBulk()
        {
            return MyIsBulk;
        }

        public static Int32 GetMarkerStatisticsCommentMaxLength()
        {
            return GetColumnLength(InternalReportMarkerStatData.TABLE, InternalReportMarkerStatData.COMMENT);
        }

        public String GetSettings()
        {
            return MySettings;
        }

        public String GetSource()
        {
            return MySource;
        }

        public static Int32 GetSourceMaxLength()
        {
            return GetColumnLength(InternalReportData.TABLE, InternalReportData.SOURCE);
        }

        public User GetUser()
        {
            return UserManager.GetUser(MyUserId);
        }

        public class InternalReportMarkerStat
        {
            private String MyComment;
            private Int32 MyMarkerId;

            public InternalReportMarkerStat(DataReader dataReader)
            {
                MyMarkerId = dataReader.GetInt32(InternalReportMarkerStatData.MARKER_ID);
                MyComment = dataReader.GetString(InternalReportMarkerStatData.COMMENT);
            }

            public String GetComment()
            {
                return MyComment;
            }

            public Int32 GetMarkerId()
            {
                return MyMarkerId;
            }
        }
    }

    public class InternalReportList : DataIdentityList
    {
        public new InternalReport GetById(Int32 id)
        {
            return (InternalReport)(base.GetById(id));
        }

        public new InternalReport this[Int32 index]
        {
            get
            {
                return (InternalReport)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        public new InternalReport this[String identifier]
        {
            get
            {
                return (InternalReport)(base[identifier]);
            }
        }
    }
}
