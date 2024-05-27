using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLocalLicenseApplication : clsApplication 
    {
        private new enum enMode { Add, Update}

        private enMode _Mode;
        public int LocalLicenseApplicationID { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }

        public clsLocalLicenseApplication()
        {
            _Mode = enMode.Add;
           
            LocalLicenseApplicationID = -1;      
            LicenseClassInfo = new clsLicenseClass();
        }

        public clsLocalLicenseApplication(int ApplicationID, int ApplicantPersonID , clsService.enServices Service, enApplicationStatus ApplicationStatus, 
            decimal PaidFee, DateTime ApplicationDate, DateTime LastStatusChangeDate, int CreatedByUserID, 
            int LocalLicenseApplicationID, clsLicenseClass.enLicenseClasses LicenseClass) : base(ApplicationID, ApplicantPersonID, Service, 
                ApplicationStatus, PaidFee, ApplicationDate,LastStatusChangeDate, CreatedByUserID)
        {
            _Mode = enMode.Update;

            this.LocalLicenseApplicationID = LocalLicenseApplicationID;
            this.LicenseClassInfo = clsLicenseClass.GetClass(LicenseClass);
        }

        public static clsLocalLicenseApplication FindByLocalLicenseApplicationID(int LocalLicenseApplicationID)
        {
            int ApplicationID = -1;
            int LicenseClassID = -1;          

            if (clsLocalLicenseApplicationData.GetApplication(LocalLicenseApplicationID,
                ref ApplicationID, ref LicenseClassID))
            {
                clsApplication Application = clsApplication.Find(ApplicationID);

                return new clsLocalLicenseApplication(ApplicationID, Application.ApplicantPerson.ID, Application.Service, Application.Status, 
                    Application.PaidFee, Application.ApplicationDate, Application.LastStatusChangeDate, Application.CreatedByUserID,
                    LocalLicenseApplicationID, (clsLicenseClass.enLicenseClasses)LicenseClassID);
            }

            return null;
        }

        public static clsLocalLicenseApplication FindByApplicationID(int ApplicationID)
        {        
            int LocalLicenseApplicationID = -1;
            int LicenseClassID = -1;
       
            if (clsLocalLicenseApplicationData.GetApplicationByApplicationID(ApplicationID, 
                ref LocalLicenseApplicationID, ref LicenseClassID))
            {
                clsApplication Application = clsApplication.Find(ApplicationID);

                return new clsLocalLicenseApplication(ApplicationID, Application.ApplicantPerson.ID, Application.Service, Application.Status,
                    Application.PaidFee, Application.ApplicationDate, Application.LastStatusChangeDate, Application.CreatedByUserID,
                    LocalLicenseApplicationID, (clsLicenseClass.enLicenseClasses)LicenseClassID);
            }

            return null;
        } 

        public static DataTable GetAllApplications()
        {
            return clsLocalLicenseApplicationData.GetAllApplications();
        }

        public static bool DoesPersonHaveNewApplicationForLicenseClass(int PersonID, clsLicenseClass.enLicenseClasses LicenseClass)
        {
            return clsLocalLicenseApplicationData.DoesPersonHaveNewApplicationForLicenseClass(PersonID, (int)LicenseClass);
        }
      
        private bool _AddNewApplication()
        {                         
            this.LocalLicenseApplicationID = clsLocalLicenseApplicationData.AddNewApplication(ApplicationID, (int)LicenseClassInfo.Class);

            return LocalLicenseApplicationID != -1;
        }

        private bool _UpdateApplication()
        {       
            return clsLocalLicenseApplicationData.UpdateApplication(LocalLicenseApplicationID, ApplicationID, (int)LicenseClassInfo.Class);
        }

        public new bool Save()
        {
            base.Mode = (clsApplication.enMode)_Mode;

            if (!base.Save())
                return false;

            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewApplication())
                    {
                        LicenseClassInfo = clsLicenseClass.GetClass(LicenseClassInfo.Class);
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;
        }

        public int IssueLicesne(int Constraints, int CreatedByUserID)
        {
            clsDriver Driver = clsDriver.FindByPersonID(ApplicantPerson.ID);

            if (Driver == null)
            {
                Driver = new clsDriver();

                Driver.Person.ID = ApplicantPerson.ID;
                Driver.CreatedByUserID = CreatedByUserID;

                if (!Driver.Save())
                    return -1;
            }

            clsLicense License = new clsLicense();

            License.Driver.ID = Driver.ID;
            License.ClassInfo.Class = LicenseClassInfo.Class;
            License.ApplicationID = ApplicationID;
            License.IssueReason = clsLicense.enLicenseIssueReasons.FirstTime;
            License.PaidFee = LicenseClassInfo.Fee;
            License.ExpiryDate = DateTime.Now.AddYears(LicenseClassInfo.ValidityLength);
            License.Constraints = Constraints;
            License.IsActive = true;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                Complete();
                return License.ID;
            }

            return -1;
        }     
    }
}
