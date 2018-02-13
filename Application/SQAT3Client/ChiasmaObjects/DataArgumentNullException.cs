using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class DataArgumentNullException : DataArgumentException
    {
        public DataArgumentNullException(String parameterName)
            : base(parameterName)
        {
        }

        public override string Message
        {
            get
            {
                return GetMessageBase() + "can not be null";
            }
        }
    }
}
