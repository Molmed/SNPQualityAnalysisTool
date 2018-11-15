using System;
using SqatData.Repositories;

namespace Molmed.SQAT.ServiceObjects
{
    public class GdprLogger
    {
        private readonly ResultWebService.ResultWebService _webService;

        public GdprLogger()
        {
            _webService = new ResultWebService.ResultWebService();
            _webService.PreAuthenticate = true;
            _webService.UseDefaultCredentials = true;
        }

        public void LogResultPlateOpened(string resultPlateName, string action, string user)
        {
            _webService.LogSqatEvent(resultPlateName, action, user, DateTime.Now);
        }

        public void LogSessionEvent(string sessionName, string action, string user, PlateRepository plateRepo)
        {
            var plateList = plateRepo.GetPlatesBySession(sessionName);
            foreach (var plate in plateList)
            {
                _webService.LogSqatEvent(plate.Identifier, action, user, DateTime.Now);
            }
        }
    }
}
