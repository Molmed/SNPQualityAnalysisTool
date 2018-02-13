using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public enum IndividualUsage
    {
        Genotyping,
        BlankControl,
        HomozygoteControl,
        InheritanceControl,
        SequencingControl
    }

    public class Individual : DataIdentity
    {
        private Boolean MyIsCommentLoaded;
        private Int32 MyFatherId;
        private Int32 MyMotherId;
        private Int32 MySampleCount;
        private Individual MyFather;
        private Individual MyMother;
        private IndividualUsage MyIndividualUsage;
        private String MyComment;
        private String MyExternalName;

        public Individual(DataReader dataReader)
            : base(dataReader)
        {
            LoadData(dataReader);
        }

        public override DataType GetDataType()
        {
            return DataType.Individual;
        }

        public String GetExternalName()
        {
            return MyExternalName;
        }

        public static Int32 GetExternalNameMaxLength()
        {
            return GetColumnLength(IndividualData.TABLE, IndividualData.EXTERNAL_NAME);
        }

        public static Int32 GetIdentifierMaxLength()
        {
            return GetColumnLength(IndividualData.TABLE, IndividualData.IDENTIFIER);
        }

        public IndividualUsage GetIndividualUsage()
        {
            return MyIndividualUsage;
        }

        public Boolean HasComment()
        {
            LoadComment();
            return IsNotEmpty(MyComment);
        }

        public Boolean HasFather()
        {
            return IsValidId(MyFatherId);
        }

        public Boolean HasMother()
        {
            return IsValidId(MyMotherId);
        }

        public Boolean IsBlankControl()
        {
            return MyIndividualUsage == IndividualUsage.BlankControl;
        }

        public Boolean IsHomozygotControl()
        {
            return MyIndividualUsage == IndividualUsage.HomozygoteControl;
        }

        public Boolean IsInheritanceControl()
        {
            return MyIndividualUsage == IndividualUsage.InheritanceControl;
        }


        private void LoadComment()
        {
            if (!MyIsCommentLoaded)
            {
                DataReader dataReader = null;

                try
                {
                    dataReader = Database.GetIndividual(GetId());
                    if (dataReader.Read())
                    {
                        LoadData(dataReader);
                    }
                    else
                    {
                        throw new DataException("Failed to load individual comment");
                    }
                    MyIsCommentLoaded = true;
                }
                finally
                {
                    CloseDataReader(dataReader);
                }
            }
        }

        private void LoadData(DataReader dataReader)
        {
            String columnNamePrefix, individualUsageString;

            columnNamePrefix = dataReader.GetColumnNamePrefix();

            //Get external name.
            MyExternalName = dataReader.GetString(IndividualData.EXTERNAL_NAME);

            // Get individual usage.
            individualUsageString = dataReader.GetString(IndividualData.INDIVIDUAL_USAGE);
            MyIndividualUsage = (IndividualUsage)(Enum.Parse(typeof(IndividualUsage), individualUsageString));
            // Get comment.
            MyIsCommentLoaded = dataReader.HasColumn(IndividualData.COMMENT);
            if (MyIsCommentLoaded)
            {
                MyComment = dataReader.GetString(IndividualData.COMMENT);
            }

            // Get father.
            MyFather = null;
            MyFatherId = dataReader.GetInt32(IndividualData.FATHER_ID, NO_ID);
            dataReader.SetColumnNamePrefix(columnNamePrefix + IndividualData.FATHER_PREFIX);
            if (IsValidId(MyFatherId) && dataReader.HasColumn(IndividualData.IDENTIFIER))
            {
                MyFather = new Individual(dataReader);
            }
            dataReader.SetColumnNamePrefix(columnNamePrefix);

            // Get mother.
            MyMother = null;
            MyMotherId = dataReader.GetInt32(IndividualData.MOTHER_ID, NO_ID);
            dataReader.SetColumnNamePrefix(columnNamePrefix + IndividualData.MOTHER_PREFIX);
            if (IsValidId(MyMotherId) && dataReader.HasColumn(IndividualData.IDENTIFIER))
            {
                MyMother = new Individual(dataReader);
            }
            dataReader.SetColumnNamePrefix(columnNamePrefix);

            // Get sample count.
            MySampleCount = NO_COUNT;
            if (dataReader.HasColumn(IndividualData.SAMPLE_COUNT))
            {
                MySampleCount = dataReader.GetInt32(IndividualData.SAMPLE_COUNT);
            }
        }
    }

    public class IndividualList : DataIdentityList
    {
        public new Individual GetById(Int32 id)
        {
            return (Individual)(base.GetById(id));
        }

        public new Individual this[Int32 index]
        {
            get
            {
                return (Individual)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        public new Individual this[String identifier]
        {
            get
            {
                return (Individual)(base[identifier]);
            }
        }
    }

    public class IndividualUpdateInfo : DataIdentifierUpdateInfo
    {
        public struct FieldNames
        {
            public const String NEW_FATHER = "New father";
            public const String NEW_MOTHER = "New mother";
            public const String NEW_SEX = "New sex";
            public const String NEW_EXTERNAL_NAME = "New external name";
            public const String NEW_SPECIES = "New species";
        }

        public IndividualUpdateInfo()
        {
            this.Add(FieldNames.NEW_FATHER, new StringUpdateField(FieldNames.NEW_FATHER));
            this.Add(FieldNames.NEW_MOTHER, new StringUpdateField(FieldNames.NEW_MOTHER));
            this.Add(FieldNames.NEW_SEX, new StringUpdateField(FieldNames.NEW_SEX));
            this.Add(FieldNames.NEW_EXTERNAL_NAME, new StringUpdateField(FieldNames.NEW_EXTERNAL_NAME));
            this.Add(FieldNames.NEW_SPECIES, new StringUpdateField(FieldNames.NEW_SPECIES));
        }

    }
}
