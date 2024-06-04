using DVLD_API.Models.TakenTest;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;

namespace DVLD_API.Controllers
{
    [Route("api/taken-tests")]
    [ApiController]
    public class TakenTestController : ControllerBase
    {
        [HttpGet("by-taken-test-id/{TakenTestID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsTakenTest> GetTest(int TakenTestID)
        {
            clsTakenTest TakenTest = clsTakenTest.Find(TakenTestID);

            if (TakenTest == null) 
                return NotFound($"Taken test with ID {TakenTestID} was not found");

            return Ok(TakenTest);
        }

        [HttpGet("by-appointment-id/{AppointmentID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsTakenTest> GetTestByAppointmentID(int AppointmentID)
        {
            clsTakenTest TakenTest = clsTakenTest.FindByAppointmentID(AppointmentID);

            if (TakenTest == null)
                return NotFound($"Taken test with appointment ID {AppointmentID} was not found");

            return Ok(TakenTest);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddNewTakenTest([FromBody] TakenTestDTO NewTakenTest)
        {
            clsTakenTest TakenTest = new clsTakenTest
            {
                AppointmentID = NewTakenTest.AppointmentID,
                Result = NewTakenTest.Result,
                Notes = NewTakenTest.Notes,
                CreatedByUserID = NewTakenTest.CreatedByUserID,
            };

            if (!TakenTest.Save())
                return StatusCode(StatusCodes.Status500InternalServerError, "Add test failed");

            return Ok(TakenTest.TakenTestID);
        }
    }
}

