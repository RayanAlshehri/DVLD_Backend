using DVLD_Business;
using Microsoft.AspNetCore.Mvc;

namespace DVLD_API.Controllers
{
    [Route("api/international-license")]
    [ApiController]
    public class InternationalLicenseController : ControllerBase
    {
        [HttpGet("{InternationalLicenseID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsInternationalLicense> GetInternationalLicense(int InternationalLicenseID)
        {
            clsInternationalLicense License = clsInternationalLicense.Find(InternationalLicenseID);

            if (License == null)
            {
                return NotFound("International license not found");
            }

            return Ok(License);
        }

        [HttpGet("is-expired/{InternationalLicenseID:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<bool> IsLicenseExpired(int InternationalLicenseID)
        {
            clsInternationalLicense License = clsInternationalLicense.Find(InternationalLicenseID);

            if (License == null)
            {
                return BadRequest("International license not found");
            }

            return Ok(License.IsExpired());
        }
    }
}
