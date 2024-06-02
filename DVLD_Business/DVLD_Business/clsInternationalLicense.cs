using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsInternationalLicense 
    {
        private enum enMode { Add, Update}

        private enMode _Mode;
        public int ID { get; set; }
        public int ApplicationID { get; set; }

        public clsDriver Driver;
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get;}
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public string LicenseStatus { get { return IsActive ? "Active" : "Inactive"; } }
        public int CreatedByUserID { get; set; }

        public clsInternationalLicense()
        {
            _Mode = enMode.Add;

            ID = -1;
            ApplicationID = -1;
            Driver = new clsDriver();
            IssueDate = DateTime.Now;
            ExpiryDate = DateTime.Now;
            IsActive = false;
            CreatedByUserID = -1;
        }

        public clsInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,  
            DateTime IssueDate,  DateTime ExpiryDate, bool IsActive, int CreatedByUserID)
        {
            _Mode = enMode.Update;


            this.ID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            Driver = clsDriver.FindByDriverID(DriverID);
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpiryDate = ExpiryDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;         
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpiryDate = DateTime.Now;
            bool IsActive = false;
            int CreatedByUserID = -1;

            if (clsInternationalLicenseData.GetLicenseByInternationalLicenseID(InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpiryDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                    IssueDate, ExpiryDate, IsActive, CreatedByUserID);
            }

            return null;
        }

        public static bool DoesLicenseExistByInternationalLicenseID(int InternationalLicenseID)
        {
            return clsInternationalLicenseData.DoesLicenseExistByInternationalLicenseID(InternationalLicenseID);
        }

        public static bool DoesLicenseExistByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
        {
            return clsInternationalLicenseData.DoesLicenseExistByLocalLicenseID(IssuedUsingLocalLicenseID);
        }

        private bool _AddNewLicense()
        {
            this.ID = clsInternationalLicenseData.AddNewLicense(ApplicationID, Driver.ID, IssuedUsingLocalLicenseID,
                DateTime.Now, ExpiryDate, IsActive, CreatedByUserID);

            return this.ID != -1;
        }

        private bool _UpdateLicense()
        {
            return clsInternationalLicenseData.UpdateLicense(ID, ApplicationID, Driver.ID, IssuedUsingLocalLicenseID, 
                IssueDate, ExpiryDate, IsActive, CreatedByUserID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewLicense())
                    {
                        Driver  = clsDriver.FindByDriverID(Driver.ID);
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateLicense();
            }

            return false;
        }

        public bool IsExpired()
        {
            return DateTime.Now > ExpiryDate;
        }
    }
}
