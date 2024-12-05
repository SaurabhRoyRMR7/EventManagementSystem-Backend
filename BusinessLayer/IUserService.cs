using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.UserService
{
    public interface IUserService
    {
        public Task<UserDTO> GetUserByIdAsync(int id);
        public Task<User> RegisterUserAsync(UserRegistrationDTO registrationDTO);
        public Task<AuthenticationResponseDTO> AuthenticateUserAsync(LoginDTO loginDTO);
        public Task<List<UserDTO>> GetUsersAsync();
        public Task<User> UpdateUserRoleAsync(int userId, UpdateUserRoleDTO updateUserRoleDTO);
        public Task<bool> DeleteUserAsync(int userId);
        public Task<int?> GetOrganizerIdByUserIdAsync(int userId);


    }
}
