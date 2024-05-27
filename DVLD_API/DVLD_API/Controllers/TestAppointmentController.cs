using DVLD_API.Models.TestAppointment;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;

namespace DVLD_API.Controllers
{
    [Route("api/testappointments")]
    [ApiController]
    public class TestAppointmentController : ControllerBase
    {
        [HttpGet("{AppointmentID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsTestAppoinment>GetTestAppointment(int AppointmentID)
        {
            clsTestAppoinment Appointment = clsTestAppoinment.Find(AppointmentID);

            if (Appointment == null)
                return NotFound($"Appointment with ID {AppointmentID} was not found");

            return Ok(Appointment);
        }

        [HttpGet("{LocalLicenseApplicationID:int}/{TestType:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsTestAppoinment> GetLastAppointment(int LocalLicenseApplicationID, int TestType)
        {
            clsTestAppoinment Appointment = clsTestAppoinment.FindLastAppointment(
                LocalLicenseApplicationID, (clsTestType.enTestTypes)TestType);

            if (Appointment == null)
                return NotFound();

            return Ok(Appointment);
        }

        [HttpPost("addappointment")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddNewAppointment([FromBody] TestAppointmentDTO NewAppointment)
        {
            clsTestAppoinment Appointment = new clsTestAppoinment();

            Appointment.TestTypeInfo.Type = (clsTestType.enTestTypes)NewAppointment.TestTypeID;
            Appointment.PersonID = NewAppointment.PersonID;
            Appointment.LocalLicenseApplication.LocalLicenseApplicationID = NewAppointment.LocalLicenseApplicationID;
            Appointment.PaidFee = NewAppointment.PaidFee;
            Appointment.TestDate = NewAppointment.TestDate;
            Appointment.CreatedByUserID = NewAppointment.CreatedByUserID;
            Appointment.RetakeTestApplicationID = NewAppointment.RetakeTestApplicationID;

            if (!Appointment.Save())
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(Appointment.ID);
        }

        [HttpPatch("{AppointmentID:int}/{NewDate:datetime}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdateAppointment(int AppointmentID, DateTime NewDate)
        {
            clsTestAppoinment Appointment = clsTestAppoinment.Find(AppointmentID);

            if (Appointment != null)
                return NotFound();

            if (!Appointment.ChangeTestDate(NewDate))
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
