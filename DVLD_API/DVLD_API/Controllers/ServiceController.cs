using DVLD_API.Models.Service;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DVLD_API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<clsService>> GetAllServices()
        {
            DataTable dt = clsService.GetAllServices();

            if (dt.Columns.Count == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in fetching data");

            var Services = dt.AsEnumerable().Select(s => new clsService
            {
                Service = (clsService.enServices)s.Field<int>("ServiceID"),
                ServiceName = s.Field<string>("ServiceName"),
                Fee = s.Field<decimal>("Fee")
            }) ;

            return Ok(Services);
        }

        [HttpGet("{ServiceID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsService> GetService(int ServiceID)
        {
            clsService Service = clsService.GetService((clsService.enServices)ServiceID);

            if (Service == null)
                return NotFound($"Service with ID {ServiceID} was not found");

            return Ok(Service);
        }

        [HttpPut("{ServiceID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdateService(int ServiceID, [FromBody] ServiceDTO UpdatedService)
        {
            clsService Service = clsService.GetService((clsService.enServices)ServiceID);

            if (Service == null)
                return NotFound($"Service with ID {ServiceID} was not found");

            Service.ServiceName = UpdatedService.ServiceName;
            Service.Fee = UpdatedService.Fee;

            if (!Service.Save())
                return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");

            return Ok();
        }
    }
}
