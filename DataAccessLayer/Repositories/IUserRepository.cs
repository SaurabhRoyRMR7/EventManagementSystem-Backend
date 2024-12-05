using EventManagementAPI.Models;
using EventManagementAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.UserRepository
{
   public interface IUserRepository
    {
        Task<User> RegisterUserAsync(UserRegistrationDTO registrationDTO);
        Task<User> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<AuthenticationResponseDTO> AuthenticateUserAsync(LoginDTO loginDTO);
        Task<int?> GetOrganizerIdByUserIdAsync(int userId);
        Task<List<UserDTO>> GetUsersAsync();
        Task<User> UpdateUserRoleAsync(int userId, UpdateUserRoleDTO updateUserRoleDTO);
        Task<bool> DeleteUserAsync(int userId);
    }
}
