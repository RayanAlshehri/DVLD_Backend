namespace DVLD_API.Models.Licenses
{
    public class DriverLicenseViewDTO
    {
        public int LicenseID { get; set; }
        public string ClassName { get; set; }
        public string IssueReason { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Constraints { get; set; }
        public string Status { get; set; }
    }
}
