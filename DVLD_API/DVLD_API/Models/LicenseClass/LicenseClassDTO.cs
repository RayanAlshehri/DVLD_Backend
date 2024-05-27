namespace DVLD_API.Models.LicenseClass
{
    public class LicenseClassDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte MinimumAge { get; set; }
        public byte ValidityLength { get; set; }
        public decimal Fee { get; set; }
    }
}
