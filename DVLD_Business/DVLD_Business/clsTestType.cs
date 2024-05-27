using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    /*
     * TestTypes database table:
     *   Fixid list according to business rules. 
     *   Addition will be done in the database if needed 
     * 
     * clsTestType:
     *   Initialize mode instead of add
    */
    public class clsTestType
    {
        private enum enMode { Initialize, Update} 
        public enum enTestTypes
        {
            Vision = 1, Theory, Practical
        }

        private enMode _Mode;
        public enTestTypes Type {  get; set; }
        public string TestName { get; set; }
        public decimal Fee { get; set; }

        public clsTestType()
        {
            _Mode = enMode.Initialize;

            Type = 0;
            TestName = "";
            Fee = - 1;
        }

        public clsTestType(enTestTypes TestType, string TestName, decimal Fee)
        {
            _Mode = enMode.Update;

            this.Type = TestType;
            this.TestName = TestName;
            this.Fee = Fee;
        }

        public static clsTestType GetTest(enTestTypes TestType)
        {
            string TestName = "";
            decimal Fee = 0;

            if (clsTestTypeData.GetTest((int)TestType,ref TestName, ref Fee))
            {
                return new clsTestType((enTestTypes)TestType, TestName, Fee);
            }

            return null;
        }

        public static DataTable GetAllTests()
        {
            return clsTestTypeData.GetAllTests();
        }

        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)Type, TestName, Fee);
        }

        public bool Save()
        {
            if (_Mode == enMode.Update)     
                return _UpdateTestType();

            return false;
        }
    }
}
