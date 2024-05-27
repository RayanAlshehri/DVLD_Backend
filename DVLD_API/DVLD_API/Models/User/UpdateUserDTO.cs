namespace DVLD_API.Models.User
{
    public class UpdateUserDTO
    {
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public int PermissionsOnUsers { get; set; }
    }
}
