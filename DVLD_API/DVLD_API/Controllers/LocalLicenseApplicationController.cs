using DVLD_API.Models.Application;
using DVLD_API.Models.LocalLicenseApplication;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DVLD_API.Controllers
{
    [Route("api/locallicenseapplications")]
    [ApiController]
    public class LocalLicenseApplicationController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<LocalLicenseApplicationViewDTO>> GetAllApplications()
        {
            DataTable dt = clsLocalLicenseApplication.GetAllApplications();

            if (dt.Columns.Count == 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            List<LocalLicenseApplicationViewDTO> Applications = dt.AsEnumerable().Select(a => new LocalLicenseApplicationViewDTO
            {
                ApplicationID = a.Field<int>("ApplicationID"),
                ClassName = a.Field<string>("ClassName"),
                NationalNumber = a.Field<string>("NationalNumber"),
                FullName = a.Field<string>("FullName"),
                ApplicationDate = a.Field<string>("ApplicationDate"),
                Status = a.Field<string>("Status")
            }).ToList();   
            
            return Ok(Applications);
        }

        [HttpGet("byLocallicenseapplicationid/{LocalLicenseApplicationID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsLocalLicenseApplication> GetApplicationByLocalLicenseApplicationID(int LocalLicenseApplicationID)
        {
            clsLocalLicenseApplication Application = 
                clsLocalLicenseApplication.FindByLocalLicenseApplicationID(LocalLicenseApplicationID);

            if (Application == null)
                return NotFound("Application not found");

            return Ok(Application);
        }

        [HttpGet("byapplicationid/{ApplicationID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsLocalLicenseApplication> GetApplicationByApplicationID(int ApplicationID)
        {
            clsLocalLicenseApplication Application = 
                clsLocalLicenseApplication.FindByApplicationID(ApplicationID);

            if (Application == null)          
                return NotFound("Application not found");
            
            return Ok(Application);
        }

        [HttpPost("addapplication")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddLocalLicenseApplication([FromBody] LocalLicenseApplicationDTO NewApplication)
        {
            if (clsLocalLicenseApplication.DoesPersonHaveNewApplicationForLicenseClass(
                NewApplication.ApplicantPersonID, (clsLicenseClass.enLicenseClasses)NewApplication.LicenseClassID))
            {
                return Conflict("Person already has a new application for the selected license class.");
            }

            if (clsLicense.DoesPersonHaveLicenseForClass(
                NewApplication.ApplicantPersonID, (clsLicenseClass.enLicenseClasses)NewApplication.LicenseClassID))
            {
                return Conflict("Person already has license with the same selected license class.");
            }

            clsLocalLicenseApplication Application = new clsLocalLicenseApplication();

            Application.ApplicantPerson.ID = NewApplication.ApplicantPersonID;
            Application.Service = (clsService.enServices)NewApplication.ServiceID;
            Application.Status = (clsApplication.enApplicationStatus)NewApplication.Status;
            Application.PaidFee = NewApplication.PaidFee;
            Application.CreatedByUserID = NewApplication.CreatedByUserID;
            Application.LicenseClassInfo.Class = (clsLicenseClass.enLicenseClasses)NewApplication.LicenseClassID;

            if (Application.Save())
                return Ok(Application.LocalLicenseApplicationID);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("issuelicense")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> IssueLicense([FromBody] IssueLicenseDTO IssueLicenseInfo)
        {
            clsLocalLicenseApplication Application = clsLocalLicenseApplication.FindByApplicationID(IssueLicenseInfo.ApplicationID);
            
            if (Application == null)
                return NotFound("Application not found");

            int LicenseID = Application.IssueLicesne(IssueLicenseInfo.Constraints, IssueLicenseInfo.CreatedByUserID);

            if (LicenseID != -1)
                return Ok(LicenseID);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
