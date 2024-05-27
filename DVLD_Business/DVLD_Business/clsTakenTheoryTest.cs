using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTakenTheoryTest : clsTakenTest
    {
        private new enum enMode { Add, Update}

        private enMode _Mode;

        private static float _MinPercentageOfCorrectAnswersToPass = 60;
        public int TakenTheoryTestID { get; set; }
        public int Grade { get; set; }

        public clsTakenTheoryTest() 
        {
            _Mode = enMode.Add;

            TakenTheoryTestID = -1;          
            Grade = -1;
        }

        public clsTakenTheoryTest(int TakenTestID, int AppointmentID, bool Result, string Notes, int? CreatedByUserID,
            int TakenTheoryTestID, int Grade) : base(TakenTestID, AppointmentID, Result, Notes, CreatedByUserID)
        {
            _Mode = enMode.Update;

            this.TakenTheoryTestID = TakenTheoryTestID;
            this.Grade = Grade;
        }

        public static clsTakenTheoryTest FindByTheoryTestID(int TakenTheoryTestID)
        {
            int TakenTestID = -1;
            int Grade = -1;

            if (clsTakenTheoryTestData.GetTestByTheoryTestID(TakenTheoryTestID, ref TakenTestID, ref Grade))
            {
                clsTakenTest TakenTest = clsTakenTest.Find(TakenTestID);

                return new clsTakenTheoryTest(TakenTest.TakenTestID, TakenTest.AppointmentID, TakenTest.Result, 
                    TakenTest.Notes, TakenTest.CreatedByUserID, TakenTheoryTestID, Grade);
            }

            return null;
        }

        public static clsTakenTheoryTest FindByTakenTestID(int TakenTestID)
        {
            int TakenTheoryTestID = -1;
            int Grade = -1;

            if (clsTakenTheoryTestData.GetTestByTakenTestID(TakenTestID, ref TakenTheoryTestID, ref Grade))
            {
                clsTakenTest TakenTest = clsTakenTest.Find(TakenTestID);

                return new clsTakenTheoryTest(TakenTestID, TakenTest.AppointmentID, TakenTest.Result, TakenTest.Notes,
                    TakenTest.CreatedByUserID, TakenTheoryTestID, Grade);
            }

            return null;
        }

        public static bool IsGradePass(int Grade, int NumberOfQuestions)
        {
            return ((float)Grade / NumberOfQuestions * 100) >= _MinPercentageOfCorrectAnswersToPass;
        }

        private bool _AddNewTest()
        {
            this.TakenTheoryTestID = clsTakenTheoryTestData.AddNewTest(TakenTestID, Grade);
            
            return this.TakenTheoryTestID != -1;
        }

        private bool _UpdateTest()
        {
            return clsTakenTheoryTestData.UpdateTest(TakenTheoryTestID, TakenTestID, Grade);
        }

        public new bool Save()
        {
            base.Mode = (clsTakenTest.enMode)_Mode;

            if (!base.Save())
                return false;

            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewTest())
                    {
                        _Mode = enMode.Update;
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
