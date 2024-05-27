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
     * Services database table:
     *   Fixid list according to business rules. 
     *   Addition will be done in the database if needed 
     *   Fee is the only updatable field
     *   
     * clsService:
     *   Initialize mode instead of add
    */

    public class clsService
    {
        private enum enMode { Initialize, Update }
        public enum enServices
        {
            IssueLicenseFirstTime = 1, RepeatCheckup, LicenseRenew, ReplaceLost, ReplaceDamaged,
            LicenseRelease, IssueInternationalLicense
        }

        private enMode _Mode;
        public enServices Service { get; set; }
        public string ServiceName { get; set; }
        public decimal Fee { get; set; }
        
        public clsService() 
        {
            _Mode = enMode.Initialize;

            Service = 0;
            ServiceName = "";
            Fee = -1;
        }
        public clsService(enServices Service, string ServiceName, decimal Fee) 
        {
            _Mode = enMode.Update;

            this.Service = Service;
            this.ServiceName = ServiceName;
            this.Fee = Fee;
        }

        public static DataTable GetAllServices()
        {
            return clsServiceData.GetAllServices();
        }      

        public static clsService GetService(enServices Service)
        {
            string ServiceName = "";
            decimal Fee = -1;

            if (clsServiceData.GetService((int)Service, ref ServiceName, ref Fee))
                return new clsService(Service, ServiceName, Fee);

            return null;
        }

        private bool _UpdateService()
        {
            return clsServiceData.UpdateService((int)Service, ServiceName, Fee);
        }

        public bool Save()
        {
            if (_Mode == enMode.Update)
                return _UpdateService();

            return false;
        }
    }
}
