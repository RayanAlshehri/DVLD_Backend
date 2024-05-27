using CommonClasses;
using DVLD_DataAccess;
using System;
using System.Data;
using System.Configuration;
using System.CodeDom;

namespace DVLD_Business
{
    public class clsUser 
    {
        enum enMode { Add, Update}

        private enMode _Mode;

        public int ID {  get; set; }

        public clsPerson Person;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int PermissionsOnUsers { get; set; }
        public enum enPermissionsOnUsers { ViewList = 1, Add = 2, Update = 4, Delete = 8, FullPermissions = -1 }

        public clsUser()
        {
            _Mode = enMode.Add;

            Person = new clsPerson();
            ID = -1;
            Username = "";
            Password = "";
            IsActive = true;
            PermissionsOnUsers = -1;
        }

        public clsUser(int UserID, int PersonID, string Username, string Password, bool IsActive, int PermissionsOnUsers)
        {
            _Mode = enMode.Update;

            Person = clsPerson.Find(PersonID);
            this.ID = UserID;
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PermissionsOnUsers = PermissionsOnUsers;
        }

        private bool _AddNewUser()
        {
            try
            {
                Password = clsUtility.HashData(Password);
            }
            catch (Exception EX)
            {
                clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedExceptionSourceName"], EX);
                return false;
            }

            this.ID = clsUserData.AddNewUser(Person.ID, Username, Password, IsActive, PermissionsOnUsers);

            return this.ID != -1;
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(ID, Username, Password, IsActive, PermissionsOnUsers);
        }

        public static clsUser Find(int UserID)
        {
            int PersonID = -1;
            bool IsActive = false;
            string Username = "";  
            string Password = "";
            int PermissionsOnUsers = -1;

            if (clsUserData.GetUser(UserID, ref PersonID, ref Username, ref Password, ref IsActive, ref PermissionsOnUsers))
            {
                clsPerson Person = clsPerson.Find(PersonID);
            
                return new clsUser(UserID,PersonID, Username, Password, IsActive, PermissionsOnUsers);
            }

            return null;          
        }

        public static clsUser Find(string Username, string Password)
        {
            int ID = -1;
            int PersonID = -1;
            bool IsActive = false;
            int PermissionsOnUsers = -1;
     
            if (clsUserData.GetUser(Username, Password, ref ID, ref PersonID, ref IsActive, ref PermissionsOnUsers))
            {
                clsPerson Person = clsPerson.Find(PersonID);

                return new clsUser(ID, PersonID, Username, Password, IsActive, PermissionsOnUsers);
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static bool DoesUserExist(int UserID)
        {
            return clsUserData.DoesUserExist(UserID);
        }

        public static bool DoesUserExistByPersonID(int PersonID)
        {
            return clsUserData.DoesUserExistByPersonID(PersonID);
        }

        public static bool DoesUserExistByUsername(string Username)
        {
            return clsUserData.DoesUserExist(Username);
        }

        public static bool HasPermission(int PermissionsOnUsers,enPermissionsOnUsers Permission)
        {
            if (PermissionsOnUsers == -1)
                return true;

            return ((int)Permission & PermissionsOnUsers) == (int)Permission;
        }
      
        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.Update;
                        Person = clsPerson.Find(Person.ID);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateUser();
            }

            return false;
        }

        public bool UpdatePassword(string NewPassword)
        {
            try
            {
                Password = clsUtility.HashData(NewPassword);               
            }
            catch (Exception EX) 
            {
                clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedExceptionSourceName"], EX);
                return false;
            }

            return clsUserData.UpdatePassword(ID, Password);
        }

        public bool HasPermission(enPermissionsOnUsers Permission)
        {
            return HasPermission(PermissionsOnUsers, Permission);
        }
    }
}
