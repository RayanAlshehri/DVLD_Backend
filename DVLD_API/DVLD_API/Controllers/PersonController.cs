using Microsoft.AspNetCore.Mvc;
using DVLD_Business;
using System.Data;
using DVLD_API.Models.Person;

namespace DVLD_API.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<PersonViewDTO>> GetAllPersons()
        {
            DataTable dt = clsPerson.GetAllPersons();

            if (dt.Columns.Count == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var PersonsView = dt.AsEnumerable().Select(p => new PersonViewDTO
            {
                NationalNumber = p.Field<string>("NationalNumber") ?? "",
                FirstName = p.Field<string>("FirstName") ?? "",
                SecondName = p.Field<string>("SecondName") ?? "",
                LastName = p.Field<string>("LastName") ?? "",
                Gender = p.Field<string>("Gender") ?? "",
                DateOfBirth = p.Field<string>("DateOfBirth") ?? "",
                Phone = p.Field<string>("Phone") ?? "",
                Email = p.Field<string>("Email") ?? "",
                Address = p.Field<string>("Address") ?? "",
            }).ToList();

            return Ok(PersonsView);
        }

        [HttpGet("bypersonid/{PersonID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsPerson> GetPerson(int PersonID)
        {
            clsPerson Person = clsPerson.Find(PersonID); 

            if (Person == null)
            {
                return NotFound("Person not found");
            }

            return Ok(Person);
        }

        [HttpGet("bynationalnumber/{NationalNumber}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsPerson> GetPerson(string NationalNumber)
        {
            clsPerson Person = clsPerson.Find(NationalNumber);

            if (Person == null)
                return NotFound("Person not found");
            
            return Ok(Person);
        }

        [HttpPost("addperson")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddPerson([FromBody]PersonDTO NewPerson)
        {
            clsPerson Person = new clsPerson();

            Person.FirstName = NewPerson.FirstName;
            Person.SecondName = NewPerson.SecondName;
            Person.ThirdName = NewPerson.ThirdName;
            Person.LastName = NewPerson.LastName;
            Person.Gender = NewPerson.Gender;
            Person.DateOfBirth = NewPerson.DateOfBirth;
            Person.NationalNumber = NewPerson.NationalNumber;
            Person.Country.ID = NewPerson.CountryID;
            Person.Phone = NewPerson.Phone;
            Person.Email = NewPerson.Email;
            Person.Address = NewPerson.Address;
            Person.ImagePath = NewPerson.ImagePath;

            if (Person.Save())
                return Ok(Person.ID);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{PersonID:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdatePerson(int PersonID, [FromBody]PersonDTO UpdatedPerson)
        {
            clsPerson Person = clsPerson.Find(PersonID);

            if (Person == null)
                return BadRequest($"Person with ID {PersonID} was not found");

            Person.FirstName = UpdatedPerson.FirstName;
            Person.SecondName = UpdatedPerson.SecondName;
            Person.ThirdName = UpdatedPerson.ThirdName;
            Person.LastName = UpdatedPerson.LastName;
            Person.Gender = UpdatedPerson.Gender;
            Person.DateOfBirth = UpdatedPerson.DateOfBirth;
            Person.NationalNumber = UpdatedPerson.NationalNumber;
            Person.Country.ID = UpdatedPerson.CountryID;
            Person.Phone = UpdatedPerson.Phone;
            Person.Email = UpdatedPerson.Email;
            Person.Address = UpdatedPerson.Address;
            Person.ImagePath = UpdatedPerson.ImagePath;

            if (Person.Save())
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{PersonID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeletePerson(int PersonID)
        {
            if (!clsPerson.DoesPersonExist(PersonID))
                return NotFound($"Person with ID {PersonID} was not found");

            if (clsPerson.DeletePerson(PersonID))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{NationalNumber}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeletePerson(string NationalNumber)
        {
            if (!clsPerson.DoesPersonExist($"Person with national number {NationalNumber} was not found"))
                return NotFound();

            if (clsPerson.DeletePerson(NationalNumber))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
