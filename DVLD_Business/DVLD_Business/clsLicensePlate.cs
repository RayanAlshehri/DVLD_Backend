using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    /*
     * clsLicensePlate:
     *   Add license plate is with the register vehicle stored procedure as one unit of work
     *   No need for update 
    */    
    public class clsLicensePlate
    {
        public int ID { get; set; }
        public string Number { get; set; }

        public clsLicensePlate() 
        { 
            ID = -1;
            Number = string.Empty;
        }

        public clsLicensePlate(int LicensePlateID, string LicensePlateNumber)
        {
            this.ID = LicensePlateID;
            this.Number = LicensePlateNumber;
        }

        public static clsLicensePlate Find(int LicensePlateID)
        {
            string LicensePlateNumber = string.Empty;

            if (clsLicensePlateData.GetLicensePlate(LicensePlateID, ref LicensePlateNumber))
            {
                return new clsLicensePlate(LicensePlateID, LicensePlateNumber);
            }

            return null;
        }

        public static bool DoesLicensePlateExist(string LicensePlateNumber)
        {
            return clsLicensePlateData.DoesLicensePlateExist(LicensePlateNumber);
        }
    }
}
