using System;
using System.Collections.Generic;
using System.Text;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class User : DataBarCode
    {
        public enum UserType
        {
            Administrator,
            Developer,
            DNAManager,
            User
        }

        private Boolean MyIsAccountActive;
        private String MyName;
        private UserType MyUserType;

        public User(DataReader dataReader)
            : base(dataReader)
        {
            String userTypeString;

            MyName = dataReader.GetString(UserData.NAME);
            userTypeString = dataReader.GetString(UserData.USER_TYPE);
            MyUserType = (UserType)(Enum.Parse(typeof(UserType), userTypeString));
            MyIsAccountActive = dataReader.GetBoolean(UserData.ACCOUNT_STATUS);
        }

        public override int CompareTo(object obj)
        {
            if (obj is User)
            {
                return MyName.CompareTo(((User)obj).GetName());
            }
            else
            {
                return base.CompareTo(obj);
            }
        }

        public static Int32 GetCommentMaxLength()
        {
            return GetColumnLength(UserData.TABLE, UserData.COMMENT);
        }

        public override DataType GetDataType()
        {
            return DataType.User;
        }

        public static Int32 GetIdentifierMaxLength()
        {
            return GetColumnLength(UserData.TABLE, UserData.IDENTIFIER);
        }

        public String GetName()
        {
            return MyName;
        }

        public static Int32 GetNameMaxLength()
        {
            return GetColumnLength(UserData.TABLE, UserData.NAME);
        }

        public UserType GetUserType()
        {
            return MyUserType;
        }

        public Boolean HasAdministratorRights()
        {
            return (MyUserType == UserType.Administrator) ||
                  (MyUserType == UserType.Developer);
        }

        public Boolean HasDnaManagerRights()
        {
            return (MyUserType == UserType.Administrator) ||
                  (MyUserType == UserType.Developer) ||
                  (MyUserType == UserType.DNAManager);
        }

        public Boolean IsAccountActive()
        {
            return MyIsAccountActive;
        }

        public Boolean IsDeveloper()
        {
            return (MyUserType == UserType.Developer);
        }

        public override void SetComment(String comment)
        {
        }
    }

    public class UserList : DataIdentityList
    {
        public new User GetById(Int32 id)
        {
            return (User)(base.GetById(id));
        }

        public new User this[Int32 index]
        {
            get
            {
                return (User)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        public new User this[String identifier]
        {
            get
            {
                return (User)(base[identifier]);
            }
        }
    }
}
