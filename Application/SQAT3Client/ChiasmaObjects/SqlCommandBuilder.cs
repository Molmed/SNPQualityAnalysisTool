using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class SqlCommandBuilder
    {
        private Boolean MyHasParameters;
        private StringBuilder MyCommand;

        public SqlCommandBuilder(String storedProcedure)
        {
            MyCommand = new StringBuilder("EXECUTE " + storedProcedure);
            MyHasParameters = false;
        }

        private void AddParameterName(String parameterName)
        {
            if (MyHasParameters)
            {
                MyCommand.Append(",");
            }
            MyCommand.Append(" @");
            MyCommand.Append(parameterName);
            MyCommand.Append(" = ");
            MyHasParameters = true;
        }

        public void AddParameter(String parameterName, Boolean parameterValue)
        {
            AddParameterName(parameterName);
            if (parameterValue)
            {
                MyCommand.Append("1");
            }
            else
            {
                MyCommand.Append("0");
            }
        }

        public void AddParameter(String parameterName, Double parameterValue)
        {
            AddParameterName(parameterName);
            MyCommand.Append(parameterValue.ToString().Replace(",", "."));
        }

        public void AddParameter(String parameterName, Byte parameterValue)
        {
            AddParameterName(parameterName);
            MyCommand.Append(parameterValue.ToString());
        }

        public void AddParameter(String parameterName, Int16 parameterValue)
        {
            AddParameterName(parameterName);
            MyCommand.Append(parameterValue.ToString());
        }

        public void AddParameter(String parameterName, Int32 parameterValue)
        {
            AddParameterName(parameterName);
            MyCommand.Append(parameterValue.ToString());
        }

        public void AddParameter(String parameterName, String parameterValue)
        {
            if (parameterValue != null)
            {
                AddParameterName(parameterName);
                MyCommand.Append("'");
                MyCommand.Append(parameterValue);
                MyCommand.Append("'");
            }
        }

        public String GetCommand()
        {
            return MyCommand.ToString();
        }
    }
}
