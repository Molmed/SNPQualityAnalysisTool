using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using Molmed.SQAT.DBObjects;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class ChiasmaData : ChiasmaBase
    {
        public const Double NO_CONCENTRATION = -1;
        public const Int32 NO_COUNT = -1;
        public const Double NO_DILUTE_FACTOR = -1;
        public const Int32 NO_ID = -1;
        public const Byte NO_ID_TINY = 0;
        public const Int32 NO_POSITION = -1;
        public const Double NO_VOLUME = -1;
        public const Int32 NO_SIZE_X = -1;
        public const Int32 NO_SIZE_Y = -1;
        public const Int32 NO_SIZE_Z = -1;

        private static String MyConnectionString;
        private static DataServerChiasmaDB MyDatabase = null;

        private static Hashtable MyColumnLenghtTable = null;

        public static DataServerChiasmaDB Database
        {
            get { return MyDatabase; }
            set
            {
                MyDatabase = value;
            }
        }

        public static void LoginChiasmaDataBase()
        {
            LoginChiasmaDataBase(MyConnectionString);
        }

        private static void LoginChiasmaDataBase(String connectionString)
        {
            // Set newLoginInfo to "null" for integrated security, or to a user name and password for manual login.

            // LoginChiasmaDatabase should be called within a try-catch environment
            // Try to connect to the database.
            ChiasmaData.Database = new DataServerChiasmaDB(connectionString);
            if (!ChiasmaData.Database.Connect())
            {
                throw new ConnectDatabaseException();
            }

        }

        public static void SetConnectionString(String connectionString)
        {
            MyConnectionString = connectionString;
        }

        public static void HandleError(String message, Exception exception)
        {
            ShowErrorDialog errorDialog;

            errorDialog = new ShowErrorDialog(message, exception);
            errorDialog.ShowDialog();
        }

        public static void LogoutChiasmaDatabase()
        {
            // Disconnect the data server.
            if (IsNotNull(ChiasmaData.Database))
            {
                ChiasmaData.Database.Disconnect();
                ChiasmaData.Database = null;
            }
        }

        public static Boolean AreEqual(IDataIdentifier object1, IDataIdentifier object2)
        {
            // Check referenses.
            if (IsNull(object1) && IsNull(object2))
            {
                return true;
            }
            if (IsNull(object1) || IsNull(object2))
            {
                return false;
            }

            // Check type.
            if (object1.GetDataType() != object2.GetDataType())
            {
                return false;
            }

            // Check identifier.
            return AreEqual(object1.GetIdentifier(), object2.GetIdentifier());
        }

        public static Boolean AreEqual(IDataIdentity object1, IDataIdentity object2)
        {
            // Check referenses.
            if (IsNull(object1) && IsNull(object2))
            {
                return true;
            }
            if (IsNull(object1) || IsNull(object2))
            {
                return false;
            }

            // Check type.
            if (object1.GetDataType() != object2.GetDataType())
            {
                return false;
            }

            // Check id.
            return object1.GetId() == object2.GetId();
        }

        public static Boolean AreNotEqual(IDataIdentifier object1, IDataIdentifier object2)
        {
            return !AreEqual(object1, object2);
        }

        public static Boolean AreNotEqual(IDataIdentity object1, IDataIdentity object2)
        {
            return !AreEqual(object1, object2);
        }

        protected static void CheckLength(String value,
                                       String argumentName,
                                       Int32 maxLength)
        {
            if (IsToLong(value, maxLength))
            {
                throw new DataArgumentLengthException(argumentName, maxLength);
            }
        }

        protected static void CheckMember(DataIdentityList dataIdentities,
                                                                                 IDataIdentity dataIdentity,
                                                                                 String listName)
        {
            if (IsNull(dataIdentities.GetById(dataIdentity.GetId())))
            {
                throw new DataArgumentException(dataIdentity.GetDataType().ToString() +
                                                                                        " " + dataIdentity.GetIdentifier() +
                                                                                        " is not a member of " + listName);
            }
        }

        protected static void CheckNotEmpty(String value, String argumentName)
        {
            if (IsEmpty(value))
            {
                throw new DataArgumentEmptyException(argumentName);
            }
        }

        protected static void CheckNotMember(DataIdentityList dataIdentities,
                                                                                       IDataIdentity dataIdentity,
                                                                                       String listName)
        {
            if (IsNotNull(dataIdentities.GetById(dataIdentity.GetId())))
            {
                throw new DataArgumentException(dataIdentity.GetDataType().ToString() +
                                                                                        " " + dataIdentity.GetIdentifier() +
                                                                                        " is already a member of " + listName);
            }
        }

        protected static void CheckNotNull(Object value, String argumentName)
        {
            if (IsNull(value))
            {
                throw new DataArgumentNullException(argumentName);
            }
        }

        protected static void CheckNull(Object value, String argumentName)
        {
            if (IsNotNull(value))
            {
                throw new DataArgumentNullException(argumentName);
            }
        }

        protected static void CheckValidColumnIndex(DataTable table,
                                                Int32 columnIndex,
                                                String argumentName)
        {
            if (IsNull(table) ||
                 IsEmpty(table.Columns) ||
                 (columnIndex < 0) ||
                 (columnIndex >= table.Columns.Count))
            {
                throw new DataArgumentException("Invalid column index", argumentName);
            }
        }

        protected static void CheckValidConcentration(Double concentration,
                                                  String argumentName)
        {
            if (!IsValidConcentration(concentration))
            {
                throw new DataArgumentException("Concentration value is not valid.",
                                             argumentName);
            }
        }

        protected static void CheckValidDiluteFactor(Double diluteFactor,
                                                String argumentName)
        {
            if (!IsValidDiluteFactor(diluteFactor))
            {
                throw new DataArgumentException("Dilute factor value is not valid.",
                                             argumentName);
            }
        }

        protected static void CheckValidIndex(Object[] array,
                                          Int32 index,
                                          String argumentName)
        {
            if (IsNull(array) ||
                (array.Length < 1) ||
                (index < 0) ||
                (index >= array.Length))
            {
                throw new DataArgumentException("Invalid array index", argumentName);
            }
        }

        protected static void CheckValidIndex(Object[,] array,
                                          Int32 index1,
                                          Int32 index2,
                                          String argument1Name,
                                          String argument2Name)
        {
            if (IsNull(array) ||
                (index1 < 0) ||
                (index1 > array.GetUpperBound(0)))
            {
                throw new DataArgumentException("Invalid array index", argument1Name);
            }

            if (IsNull(array) ||
                (index2 < 0) ||
                (index2 > array.GetUpperBound(1)))
            {
                throw new DataArgumentException("Invalid array index", argument2Name);
            }
        }

        protected static void CheckValidVolume(Double volume, String argumentName)
        {
            if (!IsValidVolume(volume))
            {
                throw new DataArgumentException("Volume value is not valid.",
                                                                                        argumentName);
            }
        }

        protected static void CloseDataReader(DataReader dataReader)
        {
            if (dataReader != null)
            {
                dataReader.Close();
            }
        }

        private static Int32 LoopWithCharFirst(String string1, String string2)
        {
            // Belongs to CompareStringWithNumbers
            int i = 0, j = 0, startInd1, startInd2;
            String subString1, subString2, numberStr;
            int output;
            Int64 number1, number2;
            while (i < string1.Length && j < string2.Length)
            {
                startInd1 = i;
                startInd2 = j;
                //separate the string part
                while (++i < string1.Length && !char.IsNumber(string1[i]))
                { }
                while (++j < string2.Length && !char.IsNumber(string2[j]))
                { }
                subString1 = string1.Substring(startInd1, i - startInd1);
                subString2 = string2.Substring(startInd2, j - startInd2);
                output = subString1.CompareTo(subString2);
                if (output != 0)
                {
                    return output;
                }
                if (i == string1.Length || j == string2.Length)
                {
                    break;
                }
                startInd1 = i;
                startInd2 = j;
                //handle the number part
                while (++i < string1.Length && char.IsNumber(string1[i]))
                { }
                while (++j < string2.Length && char.IsNumber(string2[j]))
                { }
                numberStr = string1.Substring(startInd1, i - startInd1);
                number1 = Convert.ToInt64(numberStr);
                numberStr = string2.Substring(startInd2, j - startInd2);
                number2 = Convert.ToInt64(numberStr);
                if (number1 > number2)
                {
                    return 1;
                }
                else if (number1 < number2)
                {
                    return -1;
                }
            }
            return 0;
        }

        private static Int32 LoopWithNumberFirst(String string1, String string2)
        {
            // Belongs to CompareStringWithNumbers
            int i = 0, j = 0, startInd1, startInd2;
            String subString1, subString2, numberStr;
            int output;
            Int64 number1, number2;
            while (i < string1.Length && j < string2.Length)
            {
                startInd1 = i;
                startInd2 = j;
                //handle the number part
                while (++i < string1.Length && char.IsNumber(string1[i]))
                { }
                while (++j < string2.Length && char.IsNumber(string2[j]))
                { }
                numberStr = string1.Substring(startInd1, i - startInd1);
                number1 = Convert.ToInt64(numberStr);
                numberStr = string2.Substring(startInd2, j - startInd2);
                number2 = Convert.ToInt64(numberStr);
                if (number1 > number2)
                {
                    return 1;
                }
                else if (number1 < number2)
                {
                    return -1;
                }
                if (i == string1.Length || j == string2.Length)
                {
                    break;
                }
                startInd1 = i;
                startInd2 = j;
                //separate the string part
                while (++i < string1.Length && !char.IsNumber(string1[i]))
                { }
                while (++j < string2.Length && !char.IsNumber(string2[j]))
                { }
                subString1 = string1.Substring(startInd1, i - startInd1);
                subString2 = string2.Substring(startInd2, j - startInd2);
                output = subString1.CompareTo(subString2);
                if (output != 0)
                {
                    return output;
                }
            }
            return 0;
        }


        public static Int32 CompareStringWithNumbers(String string1, String string2)
        {
            // Handles a string with numbers in it, and handles string and number separately
            // e.g. string10 comes after string9, and string10string20 comes after string10string3
            int output;
            if (string1.Length > 0 && char.IsNumber(string1[0]) &&
                string2.Length > 0 && char.IsNumber(string2[0]))
            {
                output = LoopWithNumberFirst(string1, string2);
                if (output != 0)
                {
                    return output;
                }
            }
            else if (string1.Length > 0 && !char.IsNumber(string1[0]) &&
                string2.Length > 0 && !char.IsNumber(string2[0]))
            {
                output = LoopWithCharFirst(string1, string2);
                if (output != 0)
                {
                    return output;
                }
            }
            return string1.CompareTo(string2);
        }

        protected static Int32 GetColumnLength(String tableName, String columnName)
        {
            Int32 columnLength = -1;
            String hashKey;

            if (IsNull(MyColumnLenghtTable))
            {
                // Create column length table.
                MyColumnLenghtTable = new Hashtable();
            }

            hashKey = "Table:" + tableName + "Column:" + columnName;
            if (MyColumnLenghtTable.Contains(hashKey))
            {
                // Get cached value.
                columnLength = (Int32)(MyColumnLenghtTable[hashKey]);
            }
            else
            {
                // Get value from database.
                columnLength = Database.GetColumnLength(tableName, columnName);
                MyColumnLenghtTable.Add(hashKey, columnLength);
            }
            return columnLength;
        }

        protected static Int32 GetId(IDataIdentity dataIdentity)
        {
            if (IsNull(dataIdentity))
            {
                return NO_ID;
            }
            else
            {
                return dataIdentity.GetId();
            }
        }

        public static Boolean IsNoConcentration(Double concentration)
        {
            return concentration == NO_CONCENTRATION;
        }

        public static Boolean IsNoCount(Int32 count)
        {
            return count == NO_COUNT;
        }

        public static Boolean IsNoVolume(Double volume)
        {
            return volume == NO_VOLUME;
        }

        public static Boolean IsValidConcentration(Double concentration)
        {
            return (concentration != NO_CONCENTRATION) && (concentration >= 0);
        }

        public static Boolean IsValidDiluteFactor(Double diluteFactor)
        {
            return (diluteFactor != NO_DILUTE_FACTOR) && (diluteFactor > 0);
        }

        public static Boolean IsValidId(Int32 id)
        {
            return id != NO_ID;
        }

        public static Boolean IsValidPosition(Int32 position)
        {
            return (position != NO_POSITION) && (position >= 0);
        }

        public static Boolean IsValidVolume(Double volume)
        {
            return (volume != NO_VOLUME) && (volume >= 0);
        }
    }


    public abstract class UpdateField
    {
        String MyName;

        public UpdateField(String name)
        {
            MyName = name;
        }

        public String GetName()
        {
            return MyName;
        }

        public abstract void SetSize(Int32 size);
        public abstract Int32 GetSize();
    }

    public class StringUpdateField : UpdateField
    {
        String[] values;

        public StringUpdateField(String name)
            : base(name)
        {
        }

        public String GetValue(Int32 index)
        {
            return values[index];
        }

        public void SetValue(Int32 index, String value)
        {
            values[index] = value;
        }

        public String[] GetUniqueValues()
        {
            Dictionary<String, String> uniqueDict;
            String[] uniqueArray;

            uniqueDict = new Dictionary<string, string>();

            foreach (String tempString in values)
            {
                if (tempString.Trim() != "")
                {
                    if (!uniqueDict.ContainsKey(tempString))
                    {
                        uniqueDict.Add(tempString, "");
                    }
                }
            }

            uniqueArray = new String[uniqueDict.Keys.Count];
            uniqueDict.Keys.CopyTo(uniqueArray, 0);

            return uniqueArray;
        }

        public Boolean HasEmptyValues()
        {
            foreach (String tempString in values)
            {
                if (tempString.Trim() == "")
                {
                    return true;
                }
            }

            return false;
        }

        public override void SetSize(Int32 size)
        {
            values = new String[size];
        }

        public override Int32 GetSize()
        {
            return values.GetLength(0);
        }
    }
}
