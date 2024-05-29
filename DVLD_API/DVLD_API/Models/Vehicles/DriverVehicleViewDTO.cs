namespace DVLD_API.Models.Vehicles
{
    public class DriverVehicleViewDTO
    {
        public int RegisteredVehicleID { get; set; }
        public string VehicleMake {  get; set; }
        public string VehicleModel { get; set; }
        public int Year { get; set; }   
        public string LicensePlateNumber { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
