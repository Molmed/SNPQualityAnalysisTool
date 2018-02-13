using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class ConnectDatabaseException : ApplicationException
    {
        private String MyUserName;
        public ConnectDatabaseException()
            : base()
        { 
        
        }

        public ConnectDatabaseException(String userName)
            : base()
        {
            MyUserName = userName;
        }
        public String GetUserName()
        {
            return MyUserName;
        }

        public String GetMessage()
        {
            return "Could not connect user " + MyUserName + " to database";
        }
    }
}
