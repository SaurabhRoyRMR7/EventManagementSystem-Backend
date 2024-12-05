using EventManagement.UserRepository;
using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            // Fetch the user by ID using the repository method
            var userDTO = await _userRepository.GetUserByIdAsync(id);

            // If no user found, handle accordingly (could throw exception or return null)
            if (userDTO == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return userDTO;
        }

        public async Task<User> RegisterUserAsync(UserRegistrationDTO registrationDTO)
        {
           
            return await _userRepository.RegisterUserAsync(registrationDTO);
        }

        public async Task<AuthenticationResponseDTO> AuthenticateUserAsync(LoginDTO loginDTO)
        {
            // Authenticate the user by calling the repository
            var authenticationResponse = await _userRepository.AuthenticateUserAsync(loginDTO);

            // If authentication failed (null response), handle it
            if (authenticationResponse == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return authenticationResponse;
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            // Fetch all users via the repository
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> UpdateUserRoleAsync(int userId, UpdateUserRoleDTO updateUserRoleDTO)
        {
            // Update the user role by calling the repository method
            return await _userRepository.UpdateUserRoleAsync(userId, updateUserRoleDTO);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            // Delete the user by calling the repository method
            return await _userRepository.DeleteUserAsync(userId);
        }
        public async Task<int?> GetOrganizerIdByUserIdAsync(int userId)
        {
            // Call the repository to get the OrganizerId by UserId
            return await _userRepository.GetOrganizerIdByUserIdAsync(userId);
        }
    }
}
