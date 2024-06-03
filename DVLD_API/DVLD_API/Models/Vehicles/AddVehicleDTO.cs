using DVLD_Business;

namespace DVLD_API.Models.Vehicles
{
    public class AddVehicleDTO
    {
        public int DriverID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
