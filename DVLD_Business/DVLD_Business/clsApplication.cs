using DVLD_DataAccess;
using System;
using System.Data;


namespace DVLD_Business
{
    public class clsApplication
    {
        public enum enMode { Add, Update}

        public enMode Mode;
        public int ApplicationID { get; set; }
        public clsPerson ApplicantPerson { get; set; }
        public clsService.enServices Service { get; set; }
        public enApplicationStatus Status { get; set; }
        public decimal PaidFee { get; set; }
        public DateTime ApplicationDate { get;}
        public DateTime LastStatusChangeDate { get;}
        public int CreatedByUserID { get; set; }

        public enum enApplicationStatus
        {
            New = 1, Complete, Cancelled
        }

        public clsApplication() 
        {
            Mode = enMode.Add;

            ApplicationID = -1;
            ApplicantPerson = new clsPerson();
            Service = 0;
            Status = enApplicationStatus.New;
            PaidFee = 0;
            ApplicationDate = DateTime.Now;
            LastStatusChangeDate = DateTime.Now;
            CreatedByUserID = -1;
        }

        public clsApplication(int ApplicationID, int ApplicantPersonID, clsService.enServices Service, enApplicationStatus ApplicationStatus,
            decimal PaidFee, DateTime ApplicationDate, DateTime LastStatusChangeDate, int CreatedByUserID)
        {
            Mode = enMode.Update;

            this.ApplicationID = ApplicationID;
            ApplicantPerson = clsPerson.Find(ApplicantPersonID);
            this.Service = Service;
            this.Status = ApplicationStatus;
            this.PaidFee = PaidFee;
            this.ApplicationDate = ApplicationDate;
            this.LastStatusChangeDate = LastStatusChangeDate;
            this.CreatedByUserID = CreatedByUserID;
        }
     
        public static clsApplication Find(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            int ServiceNumber = 0;
            decimal PaidFee = -1;
            byte ApplicationStatus = 0;
            DateTime ApplicationDate = DateTime.Now;
            DateTime LastStatusChangeDate = DateTime.Now;
            int CreatedByUserID = -1;

            if (clsApplicationData.GetApplication(ApplicationID, ref ApplicantPersonID, ref ServiceNumber, ref ApplicationStatus,
                ref PaidFee, ref ApplicationDate, ref LastStatusChangeDate, ref CreatedByUserID))
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, (clsService.enServices)ServiceNumber, (enApplicationStatus)ApplicationStatus,
                    PaidFee, ApplicationDate, LastStatusChangeDate, CreatedByUserID);
            }

            return null;
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            return clsApplicationData.DeleteApplication(ApplicationID);
        }

        private bool _AddNewApplication()
        {
            DateTime CurrentDateTime = DateTime.Now;

            ApplicationID =  clsApplicationData.AddNewApplication(ApplicantPerson.ID, (int)Service,
                (byte)Status, PaidFee, CurrentDateTime, CurrentDateTime, CreatedByUserID);

            return ApplicationID != -1;
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(ApplicationID, ApplicantPerson.ID, (int)Service, (byte)Status, PaidFee,
                ApplicationDate, LastStatusChangeDate, CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewApplication())
                    {
                        ApplicantPerson = clsPerson.Find(ApplicantPerson.ID);
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;
        }

        public bool Complete()
        {
            return clsApplicationData.ChangeApplicationStatus(ApplicationID, (byte)enApplicationStatus.Complete);
        }

        public bool Cancel()
        {
            return clsApplicationData.ChangeApplicationStatus(ApplicationID, (byte)enApplicationStatus.Cancelled);
        }
    }
}
