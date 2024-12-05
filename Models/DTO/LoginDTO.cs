// DTO/LoginDTO.cs

namespace EventManagementAPI.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationResponseDTO
    {
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string UserRole { get; set; }
    }

}
