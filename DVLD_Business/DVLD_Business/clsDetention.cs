using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDetention
    {
        enum enMode { Add, Update}

        private enMode _Mode;

        public int ID {  get; set; }
        public int LicenseID { get; set; }
        public DateTime DetentionDate { get;}
        public decimal Fine { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsFinePaid { get; set; }
        public bool IsReleased {  get; set; }
        public DateTime? ReleaseDate { get;}
        public int? ReleaseApplicationID { get; set; }
        public int? ReleasedByUserID { get; set; }   

        public clsDetention()
        {
            _Mode = enMode.Add;

            ID = -1;
            LicenseID = -1;
            DetentionDate = DateTime.Now;
            Fine = -1;
            CreatedByUserID = -1;
            IsFinePaid = false;
            IsReleased = false;
            ReleaseDate = null;
            ReleaseApplicationID = null;
            ReleasedByUserID = null;
        }

        public clsDetention (int DetentionID, int LicenseID, DateTime DetentionDate, decimal Fine, int CreatedByUserID, bool IsFinePaid,
            bool IsReleased, DateTime? ReleaseDate, int? ReleaseApplicationID, int? ReleasedByUserID)
        {
            _Mode = enMode.Update;

            this.ID = DetentionID;
            this.LicenseID = LicenseID;
            this.DetentionDate = DetentionDate;
            this.Fine = Fine;
            this.CreatedByUserID = CreatedByUserID;
            this.IsFinePaid= IsFinePaid;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.ReleasedByUserID = ReleasedByUserID;
        }

        public static clsDetention FindByDetentionID(int DetentionID)
        {
            int LicenseID = -1;
            DateTime DetentionDate = DateTime.Now;
            decimal Fine = -1;
            int CreatedByUserID = -1; 
            bool IsFinePaid = false;
            bool IsReleased = false;
            DateTime? ReleaseDate = null;
            int? ReleaseApplicationID = null;
            int? ReleasedByUserID = null;

            if (clsDetentionData.GetDetentionByDetentionID(DetentionID, ref LicenseID, ref DetentionDate, ref Fine, ref CreatedByUserID,
                ref IsFinePaid, ref IsReleased, ref ReleaseDate, ref ReleaseApplicationID, ref ReleasedByUserID))
            { 
                return new clsDetention(DetentionID, LicenseID, DetentionDate, Fine, CreatedByUserID, IsFinePaid, 
                    IsReleased, ReleaseDate, ReleaseApplicationID, ReleasedByUserID);
            }

            return null;
        }

        public static clsDetention FindLastDetentionByLicenseID(int LicenseID)
        {
            int DetentionID = -1;
            DateTime DetentionDate = DateTime.Now;
            decimal Fine = -1;
            int CreatedByUserID = -1;
            bool IsFinePaid = false;
            bool IsReleased = false;
            DateTime? ReleaseDate = null;
            int? ReleaseApplicationID = null;
            int? ReleasedByUserID = null;

            if (clsDetentionData.GetLastDetentionByLicenseID(LicenseID, ref DetentionID, ref DetentionDate, ref Fine, ref CreatedByUserID,
                ref IsFinePaid, ref IsReleased, ref ReleaseDate, ref ReleaseApplicationID, ref ReleasedByUserID))
            {
                return new clsDetention(DetentionID, LicenseID, DetentionDate, Fine, CreatedByUserID, IsFinePaid,
                    IsReleased, ReleaseDate, ReleaseApplicationID, ReleasedByUserID);
            }

            return null;
        }

        public static DataTable GetAllDetention()
        {
            return clsDetentionData.GetAllDetentions();
        }
     
        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetentionData.IsLicenseDetained(LicenseID);
        }

        private bool _AddNewDetention()
        {
            this.ID = clsDetentionData.AddNewDetention(LicenseID, DateTime.Now, Fine, CreatedByUserID, IsFinePaid, IsReleased);

            return this.ID != -1;
        }

        private bool _UpdateDetention()
        {
            return clsDetentionData.UpdateDetention(ID, LicenseID, DetentionDate, Fine, CreatedByUserID,IsFinePaid,
                IsReleased, ReleaseDate, ReleaseApplicationID, ReleasedByUserID);
        }

        public bool Save()
        {
           switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewDetention())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateDetention();
            }

            return false;
        }

        public bool MarkFineAsPaid()
        {
            if (clsDetentionData.MarkFineAsPaid(ID))
            {
                IsFinePaid = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ReleaseLicense(int LicenseID, int ReleasedByUserID)
        {
            return clsDetentionData.ReleaseLicense(LicenseID, ReleasedByUserID);
        }
    }
}
