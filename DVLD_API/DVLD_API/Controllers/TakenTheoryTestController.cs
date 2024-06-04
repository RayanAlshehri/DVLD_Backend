using DVLD_API.Models.TakenTheoryTest;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;

namespace DVLD_API.Controllers
{
    [Route("api/taken-theory-tests")]
    [ApiController]
    public class TakenTheoryTestController :ControllerBase
    {
        [HttpGet("by-taken-theory-test-id/{TakenTheoryTestID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsTakenTheoryTest> GetTest(int TakenTheoryTestID)
        {
            clsTakenTheoryTest TakenTheoryTest = clsTakenTheoryTest.FindByTheoryTestID(TakenTheoryTestID);

            if (TakenTheoryTest == null)
                return NotFound($"Taken theory test with ID {TakenTheoryTestID} was not found");

            return Ok(TakenTheoryTest);
        }

        [HttpGet("by-taken-test-id/{TakenTestID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsTakenTheoryTest> GetTestByTakenTestID(int TakenTestID)
        {
            clsTakenTheoryTest TakenTheoryTest = clsTakenTheoryTest.FindByTakenTestID(TakenTestID);

            if (TakenTheoryTest == null)
                return NotFound($"Taken theory test with taken test ID {TakenTestID} was not found");

            return Ok(TakenTheoryTest);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddNewTakenTheoryTest([FromBody] TakenTheoryTestDTO NewTakenTheoryTest)
        {
            clsTakenTheoryTest TakenTheoryTest = new clsTakenTheoryTest
            {
                AppointmentID = NewTakenTheoryTest.AppointmentID,
                Result = NewTakenTheoryTest.Result,
                Notes = NewTakenTheoryTest.Notes,
                CreatedByUserID = NewTakenTheoryTest.CreatedByUserID,
                Grade = NewTakenTheoryTest.Grade,
            };

            if (!TakenTheoryTest.Save())
                return StatusCode(StatusCodes.Status500InternalServerError, "Add test failed");

            return Ok(TakenTheoryTest.TakenTheoryTestID);
        }

        [HttpGet("is-grade-pass/{Grade:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> IsGradePass(int Grade)
        {
            int NumberOfQuestions = clsTheoryTestQuestion.GetNumberOfQuestions();

            if (Grade < 0 || Grade > NumberOfQuestions)
                return BadRequest("Invalid grade");

            return Ok(clsTakenTheoryTest.IsGradePass(Grade, NumberOfQuestions));
        }
    }
}
