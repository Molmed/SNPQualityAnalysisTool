using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class DataArgumentException : DataException
    {
        private String MyParameterName;
        private String MyMessage;

        public DataArgumentException(String message, String parameterName)
            : base()
        {
            MyMessage = message;
            MyParameterName = parameterName;
        }

        public DataArgumentException(String parameterName)
            : base()
        {
            MyMessage = "";
            MyParameterName = parameterName;
        }

        protected String GetMessageBase()
        {
            String className, methodName, stackTrace;
            String[] stackArray;

            if (this.TargetSite.Name.Contains("Check"))
            {
                // Get next to last class and method name from the stack trace.
                stackTrace = this.StackTrace.Substring(this.StackTrace.IndexOf(")") + 3);
                stackTrace = stackTrace.Substring(0, stackTrace.IndexOf("("));
                stackArray = stackTrace.Split(new char[] { '.' });
                className = stackArray[stackArray.Length - 2];
                methodName = stackArray[stackArray.Length - 1];
            }
            else
            {
                className = this.TargetSite.ReflectedType.Name;
                methodName = this.TargetSite.Name;
            }

            return "Error in method " + methodName +
                          " in class " + className +
                          "." + Environment.NewLine +
                          "Parameter " + this.MyParameterName + " ";
        }

        public override string Message
        {
            get
            {
                return MyMessage + Environment.NewLine + GetMessageBase();
            }
        }
    }
}
