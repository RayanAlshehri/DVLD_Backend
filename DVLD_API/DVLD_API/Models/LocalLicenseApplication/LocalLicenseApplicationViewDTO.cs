namespace DVLD_API.Models.LocalLicenseApplication
{
    public class LocalLicenseApplicationViewDTO
    {
        public int ApplicationID { get; set; }
        public string ClassName { get; set; }
        public string NationalNumber { get; set; }
        public string FullName { get; set; }
        public string ApplicationDate { get; set; }
        public string Status { get; set; }
    }
}
