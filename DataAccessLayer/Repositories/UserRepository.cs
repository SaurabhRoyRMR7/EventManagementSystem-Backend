using EventManagement.UserRepository;
using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.UserRepository
{
    public class UserRepositoryType : IUserRepository
    {
        private readonly EventManagementSystemDataBaseContext _context ;

        public UserRepositoryType(EventManagementSystemDataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> RegisterUserAsync(UserRegistrationDTO registrationDTO)
        {
            var existingUser = await GetUserByEmailAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            var hashedPassword = HashPassword(registrationDTO.Password);

            var newUser = new User
            {
                Name = registrationDTO.Name,
                Email = registrationDTO.Email,
                Password = hashedPassword,
                GoogleId = registrationDTO.GoogleId,
                UserRoleId = registrationDTO.UserRoleId,
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "System"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            if (newUser.UserRoleId == 8) // If the user is an organizer
            {
                var newOrganizer = new Organizer
                {
                    UserId = newUser.UserId,
                    OrganizationName = registrationDTO.OrganizationName,
                    ContactInfo = registrationDTO.ContactInfo,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System"
                };

                _context.Organizers.Add(newOrganizer);
                await _context.SaveChangesAsync();
            }

            return newUser;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashedBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            // Fetch user by ID and select necessary fields
            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<AuthenticationResponseDTO> AuthenticateUserAsync(LoginDTO loginDTO)
        {
            // Check if the user exists
            var user = await GetUserByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return null; // User not found
            }

            // Verify password
            if (!VerifyPasswordHash(loginDTO.Password, user.Password))
            {
                return null; // Invalid password
            }

            // Determine the user role
            string userRole = user.UserRoleId switch
            {
                7 => "Participant",
                8 => "Organizer",
                9 => "Admin",
                _ => "Unknown"
            };

            // Create and return the response DTO with user info
            return new AuthenticationResponseDTO
            {
                UserId = user.UserId,
                UserRoleId = user.UserRoleId,
                UserRole = userRole
            };
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashedBytes = sha256.ComputeHash(bytes);
                var hashedPassword = Convert.ToBase64String(hashedBytes);
                return storedHash == hashedPassword;
            }
        }
        public async Task<int?> GetOrganizerIdByUserIdAsync(int userId)
        {
            // Find the organizer based on UserId
            var organizer = await _context.Organizers
                                          .FirstOrDefaultAsync(o => o.UserId == userId);

            // If no organizer found, return null
            if (organizer == null)
            {
                return null;
            }

            // Return the OrganizerId if found
            return organizer.OrganizerId;
        }
        public async Task<List<UserDTO>> GetUsersAsync()
        {
            // Fetch all users with their associated UserRole
            var users = await _context.Users
                                      .Include(u => u.UserRole)
                                      .Select(u => new UserDTO
                                      {
                                          UserId = u.UserId,
                                          Name = u.Name,
                                          Email = u.Email,
                                          UserRoleId = u.UserRoleId
                                      })
                                      .ToListAsync();

            return users;
        }
        public async Task<User> UpdateUserRoleAsync(int userId, UpdateUserRoleDTO updateUserRoleDTO)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var role = await _context.UserRoles
                                     .FirstOrDefaultAsync(r => r.UserRoleId == updateUserRoleDTO.UserRoleID);

            if (role == null)
            {
                throw new ArgumentException("Role not found.");
            }

            user.UserRoleId = updateUserRoleDTO.UserRoleID;
            user.LastModifiedAt = DateTime.Now;
            user.LastModifiedBy = "Admin"; 

            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users
                                     .Include(u => u.Organizers)
                                     .Include(u => u.UserRole)
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Prevent deletion of Admin user (RoleId == 9)
            if (user.UserRoleId == 9)
            {
                throw new InvalidOperationException("Cannot delete an Admin user.");
            }

            // Remove related organizers
            foreach (var organizer in user.Organizers)
            {
                _context.Organizers.Remove(organizer);
            }

            // Remove the user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }


    }

}
