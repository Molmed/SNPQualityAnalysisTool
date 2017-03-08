using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class UserCanceledException : ApplicationException
    {
        public UserCanceledException()
            : base()
        {
        }

        public String GetUploadCanceledMessage()
        {
            return "Upload canceled!";
        }
    }
}
