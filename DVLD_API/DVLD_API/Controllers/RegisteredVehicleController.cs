using DVLD_API.Models.Vehicles;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;

namespace DVLD_API.Controllers
{
    [Route("api/registered-vehicles")]
    [ApiController]
    public class RegisteredVehicleController : ControllerBase
    {
        [HttpGet("by-registered-vehicle-id/{RegisteredVehicleID}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsRegisteredVehicle> GetVehicleByRegisteredVehicleID(int RegisteredVehicleID)
        {
            clsRegisteredVehicle Vehicle = clsRegisteredVehicle.FindByRegisteredVehicleID(RegisteredVehicleID);

            if (Vehicle == null)
            {
                return NotFound($"Vehicle with ID {RegisteredVehicleID} was not found");
            }

            return Ok(Vehicle);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddNewVehicle([FromBody] AddVehicleDTO NewVehicle)
        {
            clsRegisteredVehicle Vehicle = new clsRegisteredVehicle();

            Vehicle.Driver.ID = NewVehicle.DriverID;
            Vehicle.Make = NewVehicle.Make;
            Vehicle.Model = NewVehicle.Model;
            Vehicle.Year = NewVehicle.Year;
            Vehicle.CreatedByUserID = NewVehicle.CreatedByUserID;

            if (!Vehicle.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(Vehicle.ID);
        }
    }
}
