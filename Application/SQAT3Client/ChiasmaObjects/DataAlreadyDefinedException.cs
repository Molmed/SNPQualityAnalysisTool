using System;
using System.Collections.Specialized;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class DataAlreadyDefinedException : DataException
    {
        private StringCollection MyIdentifiers;

        public DataAlreadyDefinedException(String identifier)
            : base()
        {
            MyIdentifiers = new StringCollection();
            MyIdentifiers.Add(identifier);
        }

        public DataAlreadyDefinedException(String[] identifiers)
            : base()
        {
            MyIdentifiers = new StringCollection();
            MyIdentifiers.AddRange(identifiers);
        }

        public DataAlreadyDefinedException(StringCollection identifiers)
            : base()
        {
            MyIdentifiers = new StringCollection();
            foreach (String identifier in identifiers)
            {
                MyIdentifiers.Add(identifier);
            }
        }

        public override string Message
        {
            get
            {
                Type dataType;

                dataType = GetDataType();
                if (dataType == null)
                {
                    if (MyIdentifiers.Count == 1)
                    {
                        return "Data '" + GetIdentifier() + "' already exists in database.";
                    }
                    else
                    {
                        return "Data already exists in database.";
                    }
                }
                else
                {
                    if (MyIdentifiers.Count == 1)
                    {
                        return dataType.Name + " '" + GetIdentifier() + "' already exists in database.";
                    }
                    else
                    {
                        return dataType.Name + " already exists in database.";
                    }
                }
            }
        }

        public virtual Type GetDataType()
        {
            return null;
        }

        public String GetIdentifier()
        {
            return MyIdentifiers[0];
        }

        public Int32 GetIdentifierCount()
        {
            return MyIdentifiers.Count;
        }

        public StringCollection GetIdentifiers()
        {
            return MyIdentifiers;
        }
    }
}
