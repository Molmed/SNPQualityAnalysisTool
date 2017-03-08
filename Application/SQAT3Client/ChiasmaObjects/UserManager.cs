using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class UserManager : ChiasmaData
    {
        private static UserList MyUsers = null;
        private static User MyCurrentUser = null;

        private UserManager()
            : base()
        {
        }

        public static UserList GetActiveUsers()
        {
            return GetActiveUsers(true);
        }

        public static UserList GetActiveUsers(Boolean includeDevelopers)
        {
            UserList activeUsers;

            LoadUsers();
            activeUsers = new UserList();
            foreach (User user in MyUsers)
            {
                if (user.IsAccountActive() &&
                    (includeDevelopers || !user.IsDeveloper()))
                {
                    activeUsers.Add(user);
                }
            }
            return activeUsers;
        }

        public static User GetCurrentUser()
        {
            LoadUsers();
            return MyCurrentUser;
        }

        public static User GetUser(Int32 userId)
        {
            LoadUsers();
            return MyUsers.GetById(userId);
        }

        public static User GetUser(String identifier)
        {
            LoadUsers();
            return MyUsers[identifier];
        }

        public static UserList GetUsers()
        {
            LoadUsers();
            return MyUsers;
        }

        private static void LoadUsers()
        {
            DataReader dataReader = null;

            if (IsNull(MyUsers))
            {
                try
                {
                    // Get information about all users from database.
                    dataReader = Database.GetUsers();
                    MyUsers = new UserList();
                    while (dataReader.Read())
                    {
                        MyUsers.Add(new User(dataReader));
                    }
                    dataReader.Close();

                    // Get current user from database.
                    dataReader = Database.GetUserCurrent();
                    if (dataReader.Read())
                    {
                        MyCurrentUser = new User(dataReader);

                        // Return same user object as in MyUsers.
                        MyCurrentUser = MyUsers.GetById(MyCurrentUser.GetId());
                    }
                    else
                    {
                        throw new DataException("Could not retrieve current user from database!");
                    }
                }
                catch
                {
                    MyUsers = null;
                    MyCurrentUser = null;
                    throw;
                }
                finally
                {
                    CloseDataReader(dataReader);
                }
            }
        }

        public static void Refresh()
        {
            MyUsers = null;
            MyCurrentUser = null;
            LoadUsers();
        }
    }
}
