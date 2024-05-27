using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    /*
     * LicenseClasses database table:
     *   Fixid list according to business rules. 
     *   Addition will be done in the database if needed 
     *   
     * clsLicense:
     *   Initialize mode instead of add
    */

    public class clsLicenseClass
    {
        private enum enMode { Initialize, Update}
        public enum enLicenseClasses
        {
            SmallMotorcycles = 1, LargeMotorcycles, Normal, Commercial
               , Agricultral, SmallandMediumBuses, TrucksandHeavyVehicles
        }

        private enMode _Mode;
        public enLicenseClasses Class { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte MinimumAge { get; set; }
        public byte ValidityLength { get; set; }
        public decimal Fee { get; set; }

        public clsLicenseClass()
        {
            _Mode = enMode.Initialize;

            Class = 0;
            Name = string.Empty;
            Description = string.Empty;
            MinimumAge = 0;
            ValidityLength = 0;
            Fee = -1;
        }
     
        public clsLicenseClass(enLicenseClasses Class, string Name, string Description, byte MinimumAge, 
            byte ValidityLength, decimal Fee)
        {
            _Mode = enMode.Update;

            this.Class = Class;
            this.Name = Name;
            this.Description = Description;
            this.MinimumAge = MinimumAge;
            this.ValidityLength = ValidityLength;
            this.Fee = Fee;
        }

        public static clsLicenseClass GetClass(enLicenseClasses Class)
        {
            string Name = "";
            string Description = "";
            byte MinimumAge = 0;
            byte ValidityLength = 0;
            decimal Fee = -1;

            if (clsLicenseClassData.GetClass((int)Class, ref Name, ref Description, 
                ref MinimumAge, ref ValidityLength, ref Fee))
            {
                return new clsLicenseClass(Class, Name, Description, MinimumAge, ValidityLength, Fee);
            }

            return null;
        }

        public static DataTable GetAllClasses()
        {
            return clsLicenseClassData.GetAllClasses();
        }    

        private bool _UpdateClass()
        {
            return clsLicenseClassData.UpdateClass((int)Class, Name, Description, MinimumAge, ValidityLength, Fee);
        }

        public bool Save()
        {     
           if (_Mode == enMode.Update)
                return _UpdateClass();

            return false;
        }

        public bool IsAgeValid(byte Age)
        {
            return Age >= MinimumAge;
        }
    }
}
