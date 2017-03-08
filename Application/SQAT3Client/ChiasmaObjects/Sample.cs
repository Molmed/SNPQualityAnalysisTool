using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;

namespace Molmed.SQAT.ChiasmaObjects
{

    public class Sample : DataIdentity
    {
        private Int32 MyGenericContainerId;
        private int MyTubeId;
        private int MyPlateId;
        private Double MyConcentrationCurrent;
        private Double MyConcentrationCustomer;
        private Int32 MyConcentrationCurrentDeviceId;
        private String MyExternalName;
        private Individual MyIndividual;
        private Int32 MyIndividualId;
        private Int32 MyPositionX;
        private Int32 MyPositionY;
        private Int32 MyPositionZ;
        private Int32 MyStateId;
        private Double MyVolumeCurrent;
        private Double MyVolumeCustomer;
        private Int32 MyFragmentLength;
        private Double MyMolarConcentration;
        private Boolean MyIsHighlighted;
        private Int32 MyFragmentLengthDeviceId;
        private Int32 MyMolarConcentrationDeviceId;
        private int MySeqStateId;
        private int MySeqTypeId;
        private int MySeqApplicationId;
        private int MyTagIndexId;

        public Sample(DataReader dataReader)
            : base(dataReader)
        {
            Int32 sampleSeriesId;

            MyExternalName = dataReader.GetString(SampleData.EXTERNAL_NAME);
            sampleSeriesId = dataReader.GetInt32(SampleData.SAMPLE_SERIES_ID);
            MyStateId = dataReader.GetInt32(SampleData.STATE_ID);
            MyPlateId = dataReader.GetInt32(SampleData.PLATE_ID, NO_ID);
            MyTubeId = dataReader.GetInt32(SampleData.TUBE_ID, NO_ID);
            MyGenericContainerId = Math.Max(MyTubeId, MyPlateId);
            MyPositionX = dataReader.GetInt32(SampleData.POSITION_X, NO_POSITION);
            MyPositionY = dataReader.GetInt32(SampleData.POSITION_Y, NO_POSITION);
            MyPositionZ = dataReader.GetInt32(SampleData.POSITION_Z, NO_POSITION);
            MyConcentrationCurrent = dataReader.GetDouble(SampleData.CONCENTRATION_CURRENT, NO_CONCENTRATION);
            MyConcentrationCustomer = dataReader.GetDouble(SampleData.CONCENTRATION_CUSTOMER, NO_CONCENTRATION);
            MyConcentrationCurrentDeviceId = dataReader.GetInt32(SampleData.CONCENTRATION_CURRENT_DEVICE_ID, NO_ID);
            MyVolumeCurrent = dataReader.GetDouble(SampleData.VOLUME_CURRENT, NO_VOLUME);
            MyVolumeCustomer = dataReader.GetDouble(SampleData.VOLUME_CUSTOMER, NO_VOLUME);
            MyFragmentLength = dataReader.GetInt32(SampleData.FRAGMENT_LENGTH, NO_COUNT);
            MyFragmentLengthDeviceId = dataReader.GetInt32(SampleData.FRAGMENT_LENGTH_DEVICE_ID, NO_ID);
            MyMolarConcentration = dataReader.GetDouble(SampleData.MOLAR_CONCENTRATION, NO_CONCENTRATION);
            MyMolarConcentrationDeviceId = dataReader.GetInt32(SampleData.MOLAR_CONCENTRATION_DEVICE_ID, NO_ID);
            MyIsHighlighted = false;
            if (!dataReader.IsDBNull(SampleData.IS_HIGHLIGHTED))
            {
                MyIsHighlighted = dataReader.GetBoolean(SampleData.IS_HIGHLIGHTED);
            }
            MySeqStateId = dataReader.GetInt32(SampleData.SEQ_STATE_ID, NO_ID);
            MySeqTypeId = dataReader.GetInt32(SampleData.SEQ_TYPE_ID, NO_ID);
            MySeqApplicationId = dataReader.GetInt32(SampleData.SEQ_APPLICATION_ID, NO_ID);
            MyTagIndexId = dataReader.GetInt32(SampleData.TAG_INDEX_ID, NO_ID);
            LoadIndividual(dataReader);
        }

        public override DataType GetDataType()
        {
            return DataType.Sample;
        }

        public Individual GetIndividual()
        {
            DataReader dataReader = null;

            if (IsNull(MyIndividual))
            {
                try
                {
                    dataReader = Database.GetIndividual(MyIndividualId);
                    if (dataReader.Read())
                    {
                        MyIndividual = new Individual(dataReader);
                    }
                }
                finally
                {
                    CloseDataReader(dataReader);
                }
            }
            return MyIndividual;
        }

        public void LoadIndividual(DataReader dataReader)
        {
            // Check that individual belong to this sample.
            if (GetId() != dataReader.GetInt32(SampleData.SAMPLE_ID))
            {
                throw new DataException("Trying to load individual to the wrong sample!");
            }

            // Get individual.
            MyIndividual = null;
            MyIndividualId = dataReader.GetInt32(SampleData.INDIVIDUAL_ID);
            dataReader.SetColumnNamePrefix(SampleData.INDIVIDUAL_PREFIX);
            if (dataReader.HasColumn(IndividualData.IDENTIFIER))
            {
                MyIndividual = new Individual(dataReader);
            }
            dataReader.ResetColumnNamePrefix();
        }
    }

    public class SampleList : DataIdentityList
    {
        public new Sample GetById(Int32 id)
        {
            return (Sample)(base.GetById(id));
        }

        public new Sample this[Int32 Index]
        {
            get
            {
                return (Sample)(base[Index]);
            }
            set
            {
                base[Index] = value;
            }
        }

        public new Sample this[String identifier]
        {
            get
            {
                return (Sample)(base[identifier]);
            }
        }

        public override void Sort()
        {
            base.Sort(new SampleComparer());
        }
        private class SampleComparer : IComparer
        {
            public SampleComparer()
                : base()
            {
            }

            public int Compare(Object object1, Object object2)
            {
                Sample sample1, sample2;

                sample1 = (Sample)object1;
                sample2 = (Sample)object2;

                return ChiasmaData.CompareStringWithNumbers(sample1.GetIdentifier(), sample2.GetIdentifier());
            }
        }


    }
}
