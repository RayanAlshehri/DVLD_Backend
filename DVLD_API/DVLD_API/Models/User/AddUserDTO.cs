namespace DVLD_API.Models.User
{
    public class AddUserDTO
    {
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int PermissionsOnUsers { get; set; }
    }
}
