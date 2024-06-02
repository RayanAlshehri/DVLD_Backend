using CommonClasses;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDriver
    {
        enum enMode { Add, Update}

        private enMode _Mode;
        
        public int ID { get; set; }

        public clsPerson Person { get; set; }
        public string Password { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreationDate { get;}

        public clsDriver() 
        {
            _Mode = enMode.Add;  
            Person = new clsPerson();

            CreatedByUserID = -1;
            CreationDate = DateTime.Now;
        }

        public clsDriver(int DriverID, int PersonID, string Password, int CreatedByUserID, DateTime CreationDate)
        { 
            _Mode = enMode.Update;
            Person = clsPerson.Find(PersonID);

            this.ID = DriverID;
            this.Password = Password;
            this.CreatedByUserID = CreatedByUserID;
            this.CreationDate = CreationDate;
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreationDate = DateTime.Now;
            string Password = null;

            if (clsDriverData.GetDriverByDriverID(DriverID, ref PersonID, ref Password, ref CreatedByUserID, ref CreationDate))
            {             
                return new clsDriver(DriverID, PersonID, Password, CreatedByUserID, CreationDate);
            }

            return null;
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreationDate = DateTime.Now;
            string Password = null;

            if (clsDriverData.GetDriverByPersonID(PersonID, ref DriverID, ref Password, ref CreatedByUserID, ref CreationDate))
            {             
                return new clsDriver(DriverID, PersonID, Password, CreatedByUserID, CreationDate);
            }

            return null;
        }

        public static bool DoesDriverExistByDriverID(int DriverID)
        {
            return clsDriverData.DoesDriverExist(DriverID);
        }

        public static bool DoesDriverExistByPersonID(int PersonID)
        {
            return clsDriverData.DoesDriverExistByPersonID(PersonID);
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }

        public bool _AddNewDriver()
        {
            this.ID = clsDriverData.AddNewDriver(Person.ID, CreatedByUserID, DateTime.Now);

            return this.ID != -1;
        }

        public bool _UpdateDriver()
        {
            return clsDriverData.UpdateDriver(ID, Person.ID, CreatedByUserID, CreationDate);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewDriver())
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
                    return _UpdateDriver();
            }

            return false;
        }

        public DataTable GetAllLicenses()
        {
            DataTable DT = clsLicenseData.GetAllDriverLicenses(ID);

            if (DT.Columns.Count == 0)
                return DT;

            DataTable CustomDT = new DataTable();

            CustomDT.Columns.Add("License ID", typeof(int));
            CustomDT.Columns.Add("Class", typeof(string));
            CustomDT.Columns.Add("Issue Reason", typeof(string));
            CustomDT.Columns.Add("Issue Date", typeof(string));
            CustomDT.Columns.Add("Expiry Date", typeof(string));
            CustomDT.Columns.Add("Constraints", typeof(string));
            CustomDT.Columns.Add("Status", typeof(string));

            foreach (DataRow Row in DT.Rows)
            {
                CustomDT.Rows.Add(Row["LicenseID"], Row["ClassName"], Row["IssueReason"], Row["IssueDate"],
                    Row["ExpiryDate"], clsLicense.GetConstraintsString((int)Row["Constraints"]), Row["IsActive"]);
            }

            return CustomDT;
        }

        public DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllDriverLicenses(ID);
        }

        public bool HasLicenseForClass(clsLicenseClass.enLicenseClasses LicenseClass)
        {
            return clsLicenseData.DoesLicenseExistByDriverIDandClass(ID, (int)LicenseClass);
        }

        public bool UpdatePassword(string NewPassword)
        {
            try
            {
                NewPassword = clsUtility.HashData(NewPassword);
            }
            catch (Exception EX)
            {
                clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedExceptionSourceName"], EX);
                return false;
            }

            Password = NewPassword;
            return clsDriverData.UpdatePassword(ID, Password);
        }

        public int[] GetActiveLocalLicensesIDs()
        {
            return clsLicenseData.GetAllActiveLocalDriverLicensesIDs(ID);
        }
       
        public int[] GetDriverDetentionsIDsWithUnpaidFines()
        {
            return clsDetentionData.GetDriverDetentionsIDsWithUnpaidFines(ID);
        }

        public int[] GetUpcomingAppointmentsIDsForDriver()
        {
            return clsTestAppointmentData.GetUpcomingAppointmentsIDsForPerson(Person.ID);
        }

        public int GetActiveInternationalLicenseID()
        {
            return clsInternationalLicenseData.GetActiveLicenseID(ID);
        }

        public DataTable GetAllVehicles()
        {
            return clsRegisteredVehicleData.GetAllVehiclesForDriver(ID);
        }
    }
}
