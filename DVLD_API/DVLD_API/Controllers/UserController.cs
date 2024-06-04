using DVLD_API.Models.User;
using DVLD_Business;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace DVLD_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<UserViewDTO>> GetAllUsers()
        {
            DataTable dt = clsUser.GetAllUsers();

            if (dt.Columns.Count == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var UsersView = dt.AsEnumerable().Select(row => new UserViewDTO
            {
                UserID = row.Field<int>("UserID"),
                Username = row.Field<string>("Username") ?? "",
                FullName = row.Field<string>("FullName") ?? "",
                AccountStatus = row.Field<string>("AccountStatus") ?? ""
            }).ToList();

            return Ok(UsersView);
        }

        [HttpGet("{UserID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsUser> GetUser(int UserID)
        {
            clsUser User = clsUser.Find(UserID);

            if (User == null)
                return NotFound($"User with ID {UserID} was not found");
            
            return Ok(User);
        }

        [HttpGet("{Username}/{Password}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<clsUser> GetUser(string Username, string Password)
        {
            clsUser User = clsUser.Find(Username, Password);

            if (User == null)         
                return NotFound();
            
            return Ok(User);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> AddNewUser([FromBody] AddUserDTO NewUser)
        {
            clsUser User = new clsUser();

            User.Person.ID = NewUser.PersonID;
            User.Username = NewUser.Username;
            User.Password = NewUser.Password;
            User.IsActive = NewUser.IsActive;
            User.PermissionsOnUsers = NewUser.PermissionsOnUsers;

            if (User.Save())
                return Ok(User.ID);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{UserID:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdateUser(int UserID, [FromBody]UpdateUserDTO UpdatedUser)
        {
            clsUser User = clsUser.Find(UserID);

            if (User == null)
                return BadRequest($"User with ID {UserID} was not found");

            User.Username = UpdatedUser.Username;
            User.IsActive = UpdatedUser.IsActive;
            User.PermissionsOnUsers = UpdatedUser.PermissionsOnUsers;

            if (User.Save())
                 return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{UserID:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUser(int UserID)
        {
            if (!clsUser.DoesUserExist(UserID))
                return NotFound($"User with ID {UserID} was not found");

            if (clsUser.DeleteUser(UserID))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }      
    }
}
