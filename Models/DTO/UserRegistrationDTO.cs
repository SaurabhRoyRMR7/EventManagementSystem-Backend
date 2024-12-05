// DTO/RegistrationDTO.cs

namespace EventManagementAPI.DTO
{
    public class UserRegistrationDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? GoogleId { get; set; }
        public int UserRoleId { get; set; } // This will be associated with a role (e.g., "participant", "organizer", etc.)

        public string? OrganizationName { get; set; } // Only applicable if UserRoleId is 8
        public string? ContactInfo { get; set; } // Only applicable if UserRoleId is 8
    }
    
}
