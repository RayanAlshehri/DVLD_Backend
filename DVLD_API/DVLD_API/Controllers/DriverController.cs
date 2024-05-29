using DVLD_API.Models.Drivers;
using DVLD_API.Models.Licenses;
using DVLD_API.Models.Vehicles;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DVLD_API.Controllers
{
    [Route("api/drivers")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<DriverViewDTO>> GetAllDrivers()
        {
            DataTable dt = clsDriver.GetAllDrivers();

            if (dt.Columns.Count == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var DriversView = dt.AsEnumerable().Select(d => new DriverViewDTO
            {
                DriverID = d.Field<int>("DriverID"),
                NationalNumber = d.Field<string>("NationalNumber") ?? "",
                FullName = d.Field<string>("FullName") ?? "",
                NumberOfActiveLicenses = d.Field<int>("ActiveLicenses"),
                NumberOfRegisteredVehicles = d.Field<int>("RegisteredVehicles"),

            }).ToList();

            return Ok(DriversView);
        }

        [HttpGet("by-driver-id/{DriverID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsDriver> GetDriverByDriverID(int DriverID)
        {
            clsDriver Driver = clsDriver.FindByDriverID(DriverID);

            if (Driver == null)
            {
                return NotFound("Driver not found");
            }

            return Ok(Driver);
        }

        [HttpGet("by-person-id/{PersonID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsDriver> GetDriverByPersonID(int PersonID)
        {
            clsDriver Driver = clsDriver.FindByPersonID(PersonID);

            if (Driver == null)
            {
                return NotFound("Driver not found");
            }

            return Ok(Driver);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddNewDriver([FromBody]AddDriverDTO NewDriver)
        {
            clsDriver Driver = new clsDriver();

            Driver.Person.ID = NewDriver.PersonID;
            Driver.CreatedByUserID = NewDriver.CreatedByUserID;
            
            if (!Driver.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(Driver.ID);
        }

        [HttpGet("driver-exists-by-person-id/{PersonID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<bool> DoesDriverExistByPersonID(int PersonID)
        {
            return Ok(clsDriver.DoesDriverExistByPersonID(PersonID));
        }

        [HttpGet("{DriverID:int}/licenses")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<DriverLicenseViewDTO>> GetAllLicenses(int DriverID)
        {
            clsDriver Driver = clsDriver.FindByDriverID(DriverID);

            if (Driver == null)
            {
                return NotFound("Driver not found");
            }

            DataTable dt = Driver.GetAllLicenses();

            if (dt.Columns.Count == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var Licenses = dt.AsEnumerable().Select(l => new DriverLicenseViewDTO
            {
                LicenseID = l.Field<int>("License ID"),
                ClassName = l.Field<string>("Class") ?? "",
                IssueReason = l.Field<string>("Issue Reason") ?? "",
                IssueDate = l.Field<string>("Issue Date") ?? "",
                ExpiryDate = l.Field<string>("Expiry Date") ?? "",
                Constraints = l.Field<string>("Constraints") ?? "",
                Status = l.Field<string>("Status") ?? "",
            }).ToList();

            return Ok(Licenses);
        }

        [HttpGet("{DriverID:int}/vehicles")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<DriverViewDTO>> GetAllVehicles(int DriverID)
        {
            clsDriver Driver = clsDriver.FindByDriverID(DriverID);

            if (Driver == null)
            {
                return NotFound("Driver not found");
            }

            DataTable dt = Driver.GetAllVehicles();

            if (dt.Columns.Count == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var Vehicles = dt.AsEnumerable().Select(v => new DriverVehicleViewDTO
            {
                RegisteredVehicleID = v.Field<int>("RegisteredVehicleID"),
                VehicleMake = v.Field<string>("VehicleMake") ?? "",
                VehicleModel = v.Field<string>("VehicleModel") ?? "",
                Year = v.Field<int>("Year"),
                LicensePlateNumber = v.Field<string>("LicensePlateNumber") ?? "",
                RegisterDate = v.Field<DateTime>("RegisterDate"),
            }).ToList();

            return Ok(Vehicles);
        }
    }
}
