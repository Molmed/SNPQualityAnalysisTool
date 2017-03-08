using System;
using System.Collections.Specialized;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class DataException : ApplicationException
    {
        private StringCollection MyErrorCollection;

        public DataException()
            : base()
        {
            MyErrorCollection = null;
        }

        public DataException(String message)
            : base(message)
        {
            MyErrorCollection = null;
        }

        public DataException(String message, StringCollection errorCollection)
            : base(message)
        {
            MyErrorCollection = new StringCollection();
            foreach (String errorString in errorCollection)
            {
                MyErrorCollection.Add(errorString);
            }
        }

        public DataException(String message, System.Exception exception)
            : base(message, exception)
        {
            MyErrorCollection = null;
        }

        public StringCollection GetErrorCollection()
        {
            return MyErrorCollection;
        }

        public Boolean HasErrorCollection()
        {
            return MyErrorCollection != null;
        }
    }
}
