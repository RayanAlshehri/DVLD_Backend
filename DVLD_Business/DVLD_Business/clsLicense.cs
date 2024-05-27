using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

/*
 * clsLicense:
 *   Renew, Replace, and IssueInternationalLicense methods call stored procedures that handle creating application and adding thr new license
*/

namespace DVLD_Business
{
    public class clsLicense
    {
        private enum enMode { Add, Update}

        public enum enLicenseIssueReasons
        {
            FirstTime = 1, Renew, ReplaceLost, ReplaceDamaged
        }

        public enum enConstraints 
        {
            AutoCars = 1, ProsthericLimb = 2, VisionLenses = 4, 
            DayHoursOnly = 8, HandicapedCars = 16
        }

        private enMode _Mode;
        public int ID { get; set; }

        public clsDriver Driver { get; set; }
        public clsLicenseClass ClassInfo { get; set; }
        public int ApplicationID { get; set; }
        public enLicenseIssueReasons IssueReason { get; set; }
        public string IssueReasonString { get { return _GetIssueReasonString(); } }
        public decimal PaidFee { get; set; }
        public DateTime IssueDate { get;}
        public DateTime ExpiryDate { get; set; }
        public int Constraints { get; set; }
        public bool IsActive { get; set; }
        public string LicenseStatusString { get { return IsActive ? "Active" : "Inactive"; } }
        public int CreatedByUserID { get; set; }

        public clsLicense() 
        {
            _Mode = enMode.Add;

            Driver = new clsDriver();
            ClassInfo = new clsLicenseClass();

            ID = -1;
            ApplicationID = -1;
            IssueReason = 0;
            PaidFee = -1;
            IssueDate = DateTime.Now;
            ExpiryDate = DateTime.Now;
            Constraints = -1; ;
            IsActive = false;
            CreatedByUserID = -1; 
        }

        public clsLicense(int LicenseID, int ApplicationID, int DriverID, clsLicenseClass.enLicenseClasses LicenseClass, enLicenseIssueReasons IssueReason, 
            decimal PaidFee, DateTime IssueDate, DateTime ExpiryDate,
            int Constraints, bool IsActive, int CreatedByUserID)
        {
            _Mode = enMode.Update;
            Driver = clsDriver.FindByDriverID(DriverID);

            this.ID = LicenseID;
            this.ClassInfo = clsLicenseClass.GetClass(LicenseClass);
            this.ApplicationID = ApplicationID;
            this.IssueReason = IssueReason;
            this.PaidFee = PaidFee;
            this.IssueDate = IssueDate;
            this.ExpiryDate = ExpiryDate;
            this.Constraints = Constraints;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;
        }

        private static bool _CheckConstraint(int Constraints, enConstraints Constraint)
        {
            return (Constraints & (int)Constraint) == (int)Constraint;
        }

        public static bool DoesLicenseExist(int LicenseID)
        {
            return clsLicenseData.DoesLicenseExist(LicenseID);
        }    

        public static clsLicense FindByLicenseID(int LicenseID)
        {
            byte IssueReason = 0;
            int ApplicationID = -1, DriverID = -1, ClassID = -1,
                Constraints = -1, CreatedByUserID = -1;
            decimal PaidFee = -1;
 
            bool IsActive = false;
            DateTime IssueDate = DateTime.Now, ExpiryDate = DateTime.Now;

            if (clsLicenseData.GetLicenseByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref ClassID,
                ref IssueReason, ref PaidFee, ref IssueDate, ref ExpiryDate,
                ref Constraints, ref IsActive, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, (clsLicenseClass.enLicenseClasses)ClassID, 
                    (enLicenseIssueReasons)IssueReason, PaidFee,IssueDate, ExpiryDate, Constraints, IsActive, CreatedByUserID);
            }

            return null;
        }

        public static clsLicense FindByApplicationID(int ApplicationID)
        {
            int LicenseID = -1;
            int DriverID = -1;
            int ClassID = -1;
            byte IssueReason = 0;
            int Constraints = -1;
            int CreatedByUserID = -1;
            decimal PaidFee = -1;
            bool IsActive = false;
            DateTime IssueDate = DateTime.Now, ExpiryDate = DateTime.Now;

            if (clsLicenseData.GetLicenseByApplicationID(ApplicationID, ref LicenseID, ref DriverID, ref ClassID,
                ref IssueReason, ref PaidFee, ref IssueDate, ref ExpiryDate, ref Constraints, 
                ref IsActive, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, (clsLicenseClass.enLicenseClasses)ClassID, 
                    (enLicenseIssueReasons)IssueReason, PaidFee, IssueDate, ExpiryDate, Constraints, 
                    IsActive, CreatedByUserID);
            }
            else
                return null;
        }
        
        public static string GetConstraintsString(int Constraints)
        {
            if (Constraints == 0)
                return "None";

            string ConstraintsStr = "";

            if (_CheckConstraint(Constraints, enConstraints.AutoCars))
                ConstraintsStr += "Auto Cars";

            if (_CheckConstraint(Constraints, enConstraints.ProsthericLimb))
                ConstraintsStr += ", Prosthetic Limb";

            if (_CheckConstraint(Constraints, enConstraints.VisionLenses))
                ConstraintsStr += ", Vision Lenses";

            if (_CheckConstraint(Constraints, enConstraints.DayHoursOnly))
                ConstraintsStr += ", Day Hours";

            if (_CheckConstraint(Constraints, enConstraints.HandicapedCars))
                ConstraintsStr += ", Handicaped Cars";


            if (ConstraintsStr[0] == ',')
                ConstraintsStr = ConstraintsStr.Remove(0, 2);

            return ConstraintsStr;
        }

        private bool _AddNewLicense()
        {
            this.ID = clsLicenseData.AddNewLicense(ApplicationID, Driver.ID, (int)ClassInfo.Class, (byte)IssueReason, PaidFee, 
                ExpiryDate, Constraints, IsActive, CreatedByUserID);

            return this.ID != -1;
        }

        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(ID, ApplicationID, Driver.ID, (int)ClassInfo.Class, (byte)IssueReason, PaidFee, IssueDate, 
                ExpiryDate, Constraints, IsActive, CreatedByUserID);
        }

        private string _GetIssueReasonString()
        {
            switch (IssueReason)
            {
                case enLicenseIssueReasons.FirstTime:
                    return "First Time";

                case enLicenseIssueReasons.Renew:
                    return "Renew";

                case enLicenseIssueReasons.ReplaceLost:
                    return "Replacement of Lost";

                case enLicenseIssueReasons.ReplaceDamaged:
                    return "Replacement of Damaged";

                default:
                    return "";
            }
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewLicense())
                    {
                        _Mode = enMode.Update;
                        Driver = clsDriver.FindByDriverID(Driver.ID);
                        ClassInfo = clsLicenseClass.GetClass(ClassInfo.Class);
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

        public bool IsDetained()
        {
            return clsDetention.IsLicenseDetained(ID);
        }

        public bool HasInternationalLicense()
        {
            return clsInternationalLicense.DoesLicenseExistByIssuedUsingLocalLicenseID(ID);
        }
    
        public clsLicense Renew(int CreatedByUserID)
        {
            return FindByLicenseID(clsLicenseData.RenewLicense(ID, Driver.Person.ID, Driver.ID, (int)ClassInfo.Class, Constraints, CreatedByUserID));
        }
      
        public clsLicense Replace(enLicenseIssueReasons IssueReason, int CreatedByUserID)
        {
            return FindByLicenseID(clsLicenseData.ReplaceLicense(ID, Driver.Person.ID, Driver.ID, (int)ClassInfo.Class, (byte)IssueReason, Constraints, CreatedByUserID));
        }

        public int IssueInternationalLicense(int CreatedByUserID)
        {
            return clsInternationalLicenseData.IssueLicense(ID, Driver.Person.ID, Driver.ID, CreatedByUserID);
        }      

        public int Detain(decimal FineFees, int CreatedByUserID)
        {
            clsDetention Detention = new clsDetention();

            Detention.LicenseID = ID;
            Detention.Fine = FineFees;
            Detention.CreatedByUserID = CreatedByUserID;
            Detention.IsFinePaid = false;
            Detention.IsReleased = false;

            if (Detention.Save())
                return Detention.ID;

            return -1;
        }
      
        public bool Release(int ReleasedByUserID)
        {
            return clsDetention.ReleaseLicense(ID, ReleasedByUserID);
        }
    }
}
