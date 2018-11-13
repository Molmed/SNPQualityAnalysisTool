using System;
using Molmed.SQAT.ResultWebService;

namespace Molmed.SQAT.ServiceObjects
{
    public class GdprLogger
    {
        public void LogEvent(string resultPlateName, string action, string user)
        {
            var webService = new ResultWebService.ResultWebService();
            webService.PreAuthenticate = true;
            webService.UseDefaultCredentials = true;
            webService.LogSqatEvent(resultPlateName, action, user, DateTime.Now);
        }
    }
}
