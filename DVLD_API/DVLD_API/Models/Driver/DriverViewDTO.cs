namespace DVLD_API.Models.Drivers
{
    public class DriverViewDTO
    {
        public int DriverID { get; set; }
        public string NationalNumber { get; set; }
        public string FullName { get; set; }
        public int NumberOfActiveLicenses { get; set; }
        public int NumberOfRegisteredVehicles { get; set; }
    }
}
