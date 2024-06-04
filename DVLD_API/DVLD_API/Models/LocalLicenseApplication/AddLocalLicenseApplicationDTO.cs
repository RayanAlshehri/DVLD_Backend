using DVLD_API.Models.Application;

namespace DVLD_API.Models.LocalLicenseApplication
{
    public class AddLocalLicenseApplicationDTO :ApplicationDTO
    {
        public int LicenseClassID { get; set; }
    }
}
