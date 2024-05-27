using DVLD_DataAccess;
using System;

namespace DVLD_Business
{
    public class clsTestAppoinment
    {          
        enum enMode { Add, Update}

        private enMode _Mode;
        public int ID { get; set; }
        public clsTestType TestTypeInfo { get; set; }
        public int PersonID { get; set; }
        public clsLocalLicenseApplication LocalLicenseApplication { get; set; }
        public decimal PaidFee { get; set; }
        public DateTime BookingDate { get;}
        public DateTime TestDate { get; set; }
        public bool IsTaken { get; }
        public int CreatedByUserID { get; set; }
        public int? RetakeTestApplicationID { get; set; }

        public clsTestAppoinment() 
        {
            _Mode = enMode.Add;
            
            ID = -1;

            PersonID = -1;
            LocalLicenseApplication = new clsLocalLicenseApplication();
            TestTypeInfo = new clsTestType();
            PaidFee = -1;
            BookingDate = DateTime.Now;
            TestDate = DateTime.Now;
            IsTaken = false;
            CreatedByUserID = -1; 
            RetakeTestApplicationID = null;
        }

        public clsTestAppoinment(int AppointmentID, clsTestType.enTestTypes TestType, int PersonID, int LocalLicenseApplicationID, decimal PaidFee,  
            DateTime BookingDate, DateTime TestDate, bool IsTaken, int CreatedByUserID, int? RetakeTestApplicationID)
        {
            _Mode = enMode.Update;

            this.ID = AppointmentID;
            this.TestTypeInfo = clsTestType.GetTest(TestType);
            this.PersonID = PersonID;
            this.LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(LocalLicenseApplicationID);
            this.PaidFee = PaidFee;
            this.BookingDate = BookingDate;
            this.TestDate = TestDate;
            this.IsTaken = IsTaken;
            this.CreatedByUserID = CreatedByUserID;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
        }

        public static clsTestAppoinment Find(int AppointmentID)
        {
            int TestType = -1;
            int LocalLicenseApplicationID = -1;
            int PersonID = -1;
            int CreatedByUserID = -1;
            int? RetakeTestApplicationID = null;
            decimal PaidFee = -1;   
            bool IsTaken = false;
            DateTime BookingDate = DateTime.Now;
            DateTime TestDate = DateTime.Now;

            if (clsTestAppointmentData.GetAppointment(AppointmentID, ref TestType, ref PersonID, ref LocalLicenseApplicationID,
                ref PaidFee,ref BookingDate, ref TestDate, ref IsTaken, ref CreatedByUserID, ref RetakeTestApplicationID))
            {
                return new clsTestAppoinment(AppointmentID, (clsTestType.enTestTypes)TestType, PersonID, LocalLicenseApplicationID, PaidFee,
                    BookingDate, TestDate, IsTaken, CreatedByUserID, RetakeTestApplicationID);
            }

            return null;
        }

        public static clsTestAppoinment FindLastAppointment(int LocalLicenseApplicationID, clsTestType.enTestTypes TestType)
        {
            int AppointmentID = -1;
            int PersonID = -1;
            int CreatedByUserID = -1;
            int? RetakeTestApplicationID = null;
            decimal PaidFee = -1;
            bool IsTaken = false;
            DateTime BookingDate = DateTime.Now;
            DateTime TestDate = DateTime.Now;

            if (clsTestAppointmentData.GetLastAppointment(LocalLicenseApplicationID, (int)TestType, ref AppointmentID, ref PersonID, 
                ref PaidFee,ref BookingDate, ref TestDate, ref IsTaken, ref CreatedByUserID, ref RetakeTestApplicationID))
            {
                return new clsTestAppoinment(AppointmentID, TestType, PersonID, LocalLicenseApplicationID, PaidFee, 
                    BookingDate, TestDate, IsTaken, CreatedByUserID, RetakeTestApplicationID);
            }

            return null;
        }
     
        private bool _AddNewAppointment()
        {
            this.ID = clsTestAppointmentData.AddNewAppointment((int)TestTypeInfo.Type, PersonID, LocalLicenseApplication.LocalLicenseApplicationID, PaidFee,
                DateTime.Now, TestDate, false, CreatedByUserID, RetakeTestApplicationID);

            return this.ID != -1;
        } 

        private bool _UpdateAppointment()
        {
            return clsTestAppointmentData.UpdateAppointment(ID, (int)TestTypeInfo.Type, PersonID, LocalLicenseApplication.LocalLicenseApplicationID, PaidFee,
                BookingDate, TestDate, IsTaken, CreatedByUserID, RetakeTestApplicationID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewAppointment())
                    {
                        _Mode = enMode.Update;
                        LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(
                            LocalLicenseApplication.LocalLicenseApplicationID);
                        TestTypeInfo = clsTestType.GetTest(TestTypeInfo.Type);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateAppointment();
            }

            return false;
        }

        public bool ChangeTestDate(DateTime NewDate)
        {
            return clsTestAppointmentData.UpdateAppointmentTestDate(ID, NewDate);
        }
    }
}
