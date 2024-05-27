using DVLD_Business;

namespace DVLD_API.Models.TestAppointment
{
    public class TestAppointmentDTO
    {
        public int TestTypeID { get; set; }
        public int PersonID { get; set; }
        public int LocalLicenseApplicationID { get; set; }
        public decimal PaidFee { get; set; }
        public DateTime TestDate { get; set; }
        public int CreatedByUserID { get; set; }
        public int? RetakeTestApplicationID { get; set; }
    }
}
