using DVLD_API.Models.Service;
using DVLD_API.Models.TestType;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DVLD_API.Controllers
{
    [Route("api/test-types")]
    [ApiController]
    public class TestTypeController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<clsTestType>> GetAllTestTypes()
        {
            DataTable dt = clsTestType.GetAllTests();

            if (dt.Columns.Count == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in fetching data");

            var TestTypes = dt.AsEnumerable().Select(s => new clsTestType
            {
                Type = (clsTestType.enTestTypes)s.Field<int>("TestTypeID"),
                TestName = s.Field<string>("TestName"),
                Fee = s.Field<decimal>("Fee")
            });

            return Ok(TestTypes);
        }

        [HttpGet("{TestTypeID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsTestType> GetTestType(int TestTypeID)
        {
            clsTestType TestType = clsTestType.GetTest((clsTestType.enTestTypes)TestTypeID);

            if (TestType == null)
                return NotFound($"Test type with ID {TestTypeID} was not found");

            return Ok(TestType);
        }

        [HttpPut("{TestTypeID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdateService(int TestTypeID, [FromBody] TestTypeDTO UpdatedTestType)
        {
            clsTestType TestType = clsTestType.GetTest((clsTestType.enTestTypes)TestTypeID);

            if (TestType == null)
                return NotFound($"Test type with ID {TestTypeID} was not found");

            TestType.TestName = UpdatedTestType.TestName;
            TestType.Fee = UpdatedTestType.Fee;

            if (!TestType.Save())
                return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");

            return Ok();
        }
    }
}
