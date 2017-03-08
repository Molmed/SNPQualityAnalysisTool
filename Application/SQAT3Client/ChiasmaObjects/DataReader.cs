using System;
using System.Data;
using System.Data.SqlClient;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class DataReader
    {
        private SqlDataReader MyDataReader;
        private String MyColumnNamePrefix;
        private SqlCommand MyCommand;

        public DataReader(SqlDataReader dataReader, SqlCommand command)
        {
            MyDataReader = dataReader;
            MyCommand = command;
            MyColumnNamePrefix = "";
        }

        public void Close()
        {
            if (MyDataReader != null)
            {
                MyDataReader.Close();
                MyDataReader = null;
            }
        }

        public void Cancel()
        {
            //Attempts to cancel the query and then closes the data reader.
            if (MyCommand != null)
            {
                MyCommand.Cancel();
            }
            Close();
        }

        public Boolean GetBoolean(String columnName)
        {
            return MyDataReader.GetBoolean(GetColumnIndex(columnName));
        }

        private Int32 GetColumnIndex(String columnName)
        {
            columnName = MyColumnNamePrefix + columnName;
            return MyDataReader.GetOrdinal(columnName);
        }

        public String GetColumnNamePrefix()
        {
            return MyColumnNamePrefix;
        }

        public DateTime GetDateTime(String columnName)
        {
            return MyDataReader.GetDateTime(GetColumnIndex(columnName));
        }

        public Decimal GetDecimal(String columnName)
        {
            return MyDataReader.GetDecimal(GetColumnIndex(columnName));
        }

        public Decimal GetDecimal(String columnName, Decimal defaultValue)
        {
            Int32 columnIndex;

            columnIndex = GetColumnIndex(columnName);
            if (MyDataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return MyDataReader.GetDecimal(columnIndex);
            }
        }

        public Double GetDouble(String columnName)
        {
            return MyDataReader.GetDouble(GetColumnIndex(columnName));
        }

        public Double GetDouble(String columnName, Double defaultValue)
        {
            Int32 columnIndex;

            columnIndex = GetColumnIndex(columnName);
            if (MyDataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return MyDataReader.GetDouble(columnIndex);
            }
        }

        public Byte GetTinyint(String columnName)
        {
            return MyDataReader.GetByte(GetColumnIndex(columnName));
        }

        public Int16 GetInt16(String columnName)
        {
            return MyDataReader.GetInt16(GetColumnIndex(columnName));
        }

        public Int16 GetInt16(String columnName, Int16 defaultValue)
        {
            Int32 columnIndex;

            columnIndex = GetColumnIndex(columnName);
            if (MyDataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return MyDataReader.GetInt16(columnIndex);
            }
        }

        public Int32 GetInt32(String columnName)
        {
            return MyDataReader.GetInt32(GetColumnIndex(columnName));
        }

        public Int32 GetInt32(String columnName, Int32 defaultValue)
        {
            Int32 columnIndex;

            columnIndex = GetColumnIndex(columnName);
            if (MyDataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return MyDataReader.GetInt32(columnIndex);
            }
        }

        public String GetString(String columnName)
        {
            Int32 columnIndex;

            columnIndex = GetColumnIndex(columnName);
            if (MyDataReader.IsDBNull(columnIndex))
            {
                return null;
            }
            else
            {
                return MyDataReader.GetString(columnIndex);
            }
        }

        public Boolean HasColumn(String columnName)
        {
            Int32 columnIndex;

            columnName = MyColumnNamePrefix + columnName;
            for (columnIndex = 0; columnIndex < MyDataReader.FieldCount; columnIndex++)
            {
                if (MyDataReader.GetName(columnIndex) == columnName)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsDBNull(String columnName)
        {
            return MyDataReader.IsDBNull(GetColumnIndex(columnName));
        }

        public Boolean IsNotDBNull(String columnName)
        {
            return !MyDataReader.IsDBNull(GetColumnIndex(columnName));
        }

        public Boolean NextResult()
        {
            if (MyDataReader == null)
            {
                return false;
            }
            else
            {
                return MyDataReader.NextResult();
            }
        }

        public Boolean Read()
        {
            if (MyDataReader == null)
            {
                return false;
            }
            else
            {
                return MyDataReader.Read();
            }
        }

        public void ResetColumnNamePrefix()
        {
            MyColumnNamePrefix = "";
        }

        public void SetColumnNamePrefix(String prefix)
        {
            MyColumnNamePrefix = prefix;
        }
    }
}
