using DVLD_API.Models.Application;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;


namespace DVLD_API.Controllers
{
    [Route("api/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpGet("{ApplicationID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsApplication> GetApplication(int ApplicationID)
        {
            clsApplication Application = clsApplication.Find(ApplicationID);

            if (Application == null)
            {
                return NotFound("Application not found");
            }

            return Ok(Application);
        }

        [HttpPost("addapplication")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddApplication([FromBody] ApplicationDTO NewApplication)
        {
            clsApplication Application = new clsApplication();

            Application.ApplicantPerson.ID = NewApplication.ApplicantPersonID;
            Application.Service = (clsService.enServices)NewApplication.ServiceID;
            Application.Status = (clsApplication.enApplicationStatus)NewApplication.Status;
            Application.PaidFee = NewApplication.PaidFee;
            Application.CreatedByUserID = NewApplication.CreatedByUserID;

            if (Application.Save())
                return Ok(Application.ApplicationID);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPatch("{ApplicationID:int}/{Status:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateApplicationStatus(int ApplicationID, int Status)
        {
            clsApplication Application = clsApplication.Find(ApplicationID);

            if (Application == null)
                return NotFound($"Application with ID {ApplicationID} was not found");
            
            switch ((clsApplication.enApplicationStatus)Status)
            {
                case clsApplication.enApplicationStatus.Complete:
                    if (Application.Complete())
                        return Ok();
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError);

                case clsApplication.enApplicationStatus.Cancelled:
                    if (Application.Cancel())
                        return Ok();
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError);

                default:
                    return BadRequest("Status value does not match any possible one");
            }
        }
    }
}
