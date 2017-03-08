using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public interface IDataBarCode
    {
        String GetBarCode();
        Boolean HasBarCode();
    }

    public abstract class DataBarCode : DataComment, IDataBarCode
    {
        private String MyBarCode;

        public DataBarCode(DataReader dataReader)
            : base(dataReader)
        {
            if (dataReader.HasColumn(BarCodeData.BAR_CODE))
            {
                MyBarCode = dataReader.GetString(BarCodeData.BAR_CODE);
            }
            else
            {
                MyBarCode = null;
            }
        }

        public String GetBarCode()
        {
            if (IsNull(MyBarCode))
            {
                LoadBarCode();
            }
            return MyBarCode;
        }

        public static Int32 GetBarCodeMaxLength()
        {
            return Math.Min(GetColumnLength(BarCodeData.TABLE_EXTERNAL, BarCodeData.BAR_CODE_COLUMN),
                                             GetColumnLength(BarCodeData.TABLE_INTERNAL, BarCodeData.BAR_CODE_COLUMN));
        }

        public Boolean HasBarCode()
        {
            if (IsNull(MyBarCode))
            {
                LoadBarCode();
            }
            return IsNotNull(MyBarCode);
        }

        protected virtual void LoadBarCode()
        {
            // Override this method if loading of bar code
            // is separated from construction of object.
        }
    }
}
