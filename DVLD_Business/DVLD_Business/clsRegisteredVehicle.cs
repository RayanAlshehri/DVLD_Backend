using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsRegisteredVehicle
    {
        private enum enMode { Add, Update}

        private enMode _Mode;

        public int ID { get; set; }

        public clsDriver Driver;
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public clsLicensePlate LicensePlate;
        public DateTime RegisterDate { get; }
        public int CreatedByUserID { get; set; }

        public clsRegisteredVehicle()
        {
            _Mode = enMode.Add;

            ID = -1;
            Driver = new clsDriver();
            Make = "";
            Model = "";
            Year = -1;
            LicensePlate = new clsLicensePlate();
            RegisterDate = DateTime.Now;
            CreatedByUserID = -1;
        }

        public clsRegisteredVehicle(int RegisteredVehicleID, int DriverID, string Make, string Model, int Year, 
            int LicensePlateID, DateTime RegisterDate, int CreatedByUserID)
        {
            _Mode = enMode.Update;

            ID = RegisteredVehicleID;
            Driver = clsDriver.FindByDriverID(DriverID);
            this.Make = Make;
            this.Model = Model;
            this.Year = Year;
            LicensePlate = clsLicensePlate.Find(LicensePlateID);
            this.RegisterDate = RegisterDate;
            this.CreatedByUserID = CreatedByUserID;
        }

        public static clsRegisteredVehicle FindByRegisteredVehicleID(int RegisteredVehicleID)
        {
            int DriverID = -1;
            string Make = "";
            string Model = "";
            int Year = -1;
            int LicensePlateID = -1;
            DateTime RegisterDate = DateTime.Now;
            int CreatedByUserID = -1;

            if (clsRegisteredVehicleData.GetRegisteredVehicle(RegisteredVehicleID, ref DriverID, ref Make, ref Model, 
                ref Year, ref LicensePlateID, ref RegisterDate, ref CreatedByUserID))
            {
                return new clsRegisteredVehicle(RegisteredVehicleID, DriverID, Make, Model, Year, 
                    LicensePlateID, RegisterDate, CreatedByUserID);
            }

            return null;
        }

        public static clsRegisteredVehicle FindByLicensePlateID(int LicensePlateID)
        {
            int RegisteredVehicleID = -1;
            int DriverID = -1;
            string Make = "";
            string Model = "";
            int Year = -1;
            DateTime RegisterDate = DateTime.Now;
            int CreatedByUserID = -1;

            if (clsRegisteredVehicleData.GetRegisteredVehicleByLicensePlateID(LicensePlateID, ref RegisteredVehicleID, ref DriverID, ref Make, ref Model,
                ref Year, ref RegisterDate, ref CreatedByUserID))
            {
                return new clsRegisteredVehicle(RegisteredVehicleID, DriverID, Make, Model, Year, 
                    LicensePlateID, RegisterDate, CreatedByUserID);
            }

            return null;
        }

        private bool _AddNewVehicle()
        {
            int LicensePlateID = -1; // LicensePlate is added in the database

            this.ID = clsRegisteredVehicleData.AddNewVehicle(Driver.ID, Make, Model, Year, ref LicensePlateID, DateTime.Now, CreatedByUserID);
            LicensePlate.ID = LicensePlateID;

            return ID != -1;
        }

        private bool _UpdateVehicle()
        {
            return clsRegisteredVehicleData.UpdateVehcile(ID, Driver.ID, Make, Model, Year, LicensePlate.ID, RegisterDate, CreatedByUserID);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    if (_AddNewVehicle())
                    {
                        Driver = clsDriver.FindByDriverID(Driver.ID);
                        LicensePlate = clsLicensePlate.Find(LicensePlate.ID);
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateVehicle();
            }

            return false;
        }
    }
}
