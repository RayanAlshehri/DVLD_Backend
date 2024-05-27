using DVLD_Business;
using static DVLD_Business.clsApplication;

namespace DVLD_API.Models.Application
{
    public class ApplicationDTO
    {
        public int ApplicantPersonID { get; set; }
        public int ServiceID { get; set; }
        public int Status { get; set; }
        public decimal PaidFee { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
