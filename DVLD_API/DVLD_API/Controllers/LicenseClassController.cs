
using DVLD_API.Models.LicenseClass;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DVLD_API.Controllers
{
    [Route("api/license-classes")]
    [ApiController]  
    public class LicenseClassController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<clsLicenseClass>> GetAllLicenseClasses()
        {
            DataTable dt = clsLicenseClass.GetAllClasses();

            if (dt.Columns.Count == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in fetching data");

            var LicenseClasses = dt.AsEnumerable().Select(s => new clsLicenseClass
            {
                Class = (clsLicenseClass.enLicenseClasses)s.Field<int>("ClassID"),
                Name = s.Field<string>("ClassName"),
                Fee = s.Field<decimal>("Fee")
            });

            return Ok(LicenseClasses);
        }

        [HttpGet("{LicenseClassID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsLicenseClass> GetLicenseClass(int LicenseClassID)
        {
            clsLicenseClass LicenseClass = clsLicenseClass.GetClass((clsLicenseClass.enLicenseClasses)LicenseClassID);

            if (LicenseClass == null)
                return NotFound($"LicenseClass with ID {LicenseClassID} was not found");

            return Ok(LicenseClass);
        }

        [HttpPut("{LicenseClassID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdateLicenseClass(int LicenseClassID, [FromBody] LicenseClassDTO UpdatedLicenseClass)
        {
            clsLicenseClass LicenseClass = clsLicenseClass.GetClass((clsLicenseClass.enLicenseClasses)LicenseClassID);

            if (LicenseClass == null)
                return NotFound($"LicenseClass with ID {LicenseClassID} was not found");

            LicenseClass.Name = UpdatedLicenseClass.Name;
            LicenseClass.Description = UpdatedLicenseClass.Description;
            LicenseClass.MinimumAge = UpdatedLicenseClass.MinimumAge;
            LicenseClass.ValidityLength = UpdatedLicenseClass.ValidityLength;
            LicenseClass.Fee = UpdatedLicenseClass.Fee;

            if (!LicenseClass.Save())
                return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");

            return Ok();
        }
    }
}
