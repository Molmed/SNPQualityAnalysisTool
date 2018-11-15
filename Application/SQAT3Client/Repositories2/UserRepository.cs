using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.Repositories2
{
    public class UserRepository
    {
        private string _currentUsername;
        private DataServer _dataServer;

        public UserRepository(DataServer dataServer)
        {
            _dataServer = dataServer;
        }

        public string GetCurrentUserName()
        {
            if (string.IsNullOrEmpty(_currentUsername))
            {
                _currentUsername = _dataServer.GetCurrentUserName();
            }

            return _currentUsername;
        }
    }
}
