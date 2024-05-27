using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTakenTest
    {
        public enum enMode { Add, Update}

        public enMode Mode;

        public int TakenTestID { get; set; }
        public int AppointmentID { get; set; }
        public bool Result { get; set; }
        public string Notes { get; set; }
        public int? CreatedByUserID { get; set; }

        public clsTakenTest()
        { 
            Mode = enMode.Add;

            TakenTestID = -1;
            AppointmentID = -1;
            Result = false;
            Notes = "";
            CreatedByUserID = null;
        }

        public clsTakenTest(int TakenTestID, int AppointmentID, bool Result, 
            string Notes, int? CreatedByUserID)
        {
            Mode = enMode.Update;

            this.TakenTestID = TakenTestID;
            this.AppointmentID = AppointmentID;
            this.Result = Result;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
        }

        public static clsTakenTest Find(int TakenTestID)
        {
            int AppointmentID = -1;
            int ?CreatedByUserID = null;
            bool Result = false;
            string Notes = "";

            if (clsTakenTestData.GetTest(TakenTestID, ref AppointmentID, ref Result,
                ref Notes, ref CreatedByUserID))
            {
                return new clsTakenTest(TakenTestID, AppointmentID, Result, Notes, CreatedByUserID);
            }

            return null;
        }

        public static clsTakenTest FindByAppointmentID(int AppointmentID)
        {
            int TakenTestID = -1;
            int? CreatedByUserID = null;
            bool Result = false;
            string Notes = "";

            if (clsTakenTestData.GetTestByAppointmentID(AppointmentID, ref TakenTestID, ref Result,
                ref Notes, ref CreatedByUserID))
            {
                return new clsTakenTest(TakenTestID, TakenTestID, Result, Notes, CreatedByUserID);
            }

            return null;
        }

        private bool _AddNewTest()
        {
            this.TakenTestID = clsTakenTestData.AddNewTest(AppointmentID, Result, Notes, CreatedByUserID);

            return this.TakenTestID != -1;
        }

        private bool _UpdateTest()
        {
            return clsTakenTestData.UpdateTest(TakenTestID, AppointmentID, Result, Notes, CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTest();
            }
            

            return false;
        }
    }
}
