using DVLD_Business;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace DVLD_API.Controllers
{
    [Route("api/licenses")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        [HttpGet("bylicenseid/{LicenseID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsLicense> GetLicenseByLicenseID(int LicenseID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
            {
                return NotFound($"License with ID {LicenseID} was not found");
            }

            return Ok(License);
        }

        [HttpGet("byapplicationid/{ApplicationID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsLicense> GetLicenseByApplicationID(int ApplicationID)
        {
            clsLicense License = clsLicense.FindByApplicationID(ApplicationID);

            if (License == null)
            {
                return NotFound($"License with application ID {ApplicationID} was not found");
            }

            return Ok(License);
        }

        [HttpGet("{LicenseID:int}/isexpired")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<bool> IsLicenseExpired(int LicenseID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
                return NotFound($"License with ID {LicenseID} was not found");

            return Ok(License.IsExpired());
        }

        [HttpGet("{LicenseID:int}/isdetained")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<bool> IsLicenseDetained(int LicenseID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
                return NotFound($"License with ID {LicenseID} was not found");

            return Ok(License.IsDetained());
        }

        [HttpGet("{LicenseID:int}/hasinternationallicense")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<bool> DoesLicenseHaveInternationalLicense(int LicenseID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
                return NotFound($"License with ID {LicenseID} was not found");

            return Ok(License.HasInternationalLicense());
        }

        [HttpPost("renew/{LicenseID:int}/{CreatedByUserID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsLicense> RenewLicense(int LicenseID, int CreatedByUserID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
                return NotFound($"License with ID {LicenseID} was not found");

            if (!License.IsExpired() || !License.IsActive)
                return Conflict("License has to be expired and active");

            clsLicense NewLicense = License.Renew(CreatedByUserID);

            if (NewLicense == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(NewLicense);
        }

        [HttpPost("replace/{LicenseID:int}/{IssueReason:int}/{CreatedByUserID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsLicense> ReplaceLicense(int LicenseID, int IssueReason, int CreatedByUserID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
                return NotFound($"License with ID {LicenseID} was not found");

            if (!License.IsActive)
                return Conflict("License has to be active");

            clsLicense NewLicense = License.Replace((clsLicense.enLicenseIssueReasons)IssueReason, CreatedByUserID);

            if (NewLicense == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(NewLicense);
        }

        [HttpPost("issueinternationallicense/{LocalLicenseID:int}/{CreatedByUserID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> IssueInternationalLicense(int LocalLicenseID, int CreatedByUserID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LocalLicenseID);

            if (License == null)
                return NotFound($"License with ID {LocalLicenseID} was not found");

            if (!License.IsActive)
                return Conflict("License has to be active");

            int InternationalLicenseID = License.IssueInternationalLicense(CreatedByUserID);

            if (InternationalLicenseID == -1)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(InternationalLicenseID);
        }

        [HttpPost("detain/{LicenseID:int}/{Fine:decimal}/{CreatedByUserID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> DetainLicense(int LicenseID, decimal Fine, int CreatedByUserID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
                return NotFound($"License with ID {LicenseID} was not found");

            if (License.IsDetained())
                return Conflict("License is already detained");

            int DetentionID = License.Detain(Fine, CreatedByUserID);

            if (DetentionID == -1)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(DetentionID);
        }

        [HttpPatch("Releaselicense/{LicenseID:int}/{ReleasedByUserID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult ReleaseLicense(int LicenseID, int ReleasedByUserID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
                return NotFound($"License with ID {LicenseID} was not found");

            if (!License.IsDetained())
                return Conflict("License is not detained");

            if (!License.Release(ReleasedByUserID))
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
