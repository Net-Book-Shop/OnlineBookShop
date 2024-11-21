using Microsoft.AspNetCore.Identity;
using OnlineBookShop.Data;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Repository;
using OnlineBookShop.Security;

namespace OnlineBookShop.Service.Impl
{

    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(UserRepository userRepository, PasswordHasher<User> passwordHasher  )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseMessage> GetRoleWiseUserlist(UserRegistorRequestDTO requestDTO)
        { 
            if (string.IsNullOrEmpty(requestDTO.Role))
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Role is required."
                };
            }

            var users = await _userRepository.GetRoleWiseAllUsers(requestDTO.Role);
        
            if (users == null || !users.Any())
            {
                return new ResponseMessage
                {
                    StatusCode = 404,
                    Message = "No users found for the specified role."
                };
            }
            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "Users retrieved successfully.",
                Data = users
            };
        }

        public async Task<ResponseMessage> UserRegistor(UserRegistorRequestDTO userRegister)
        {
            var existingUser = await _userRepository.FindByUserName(userRegister.UserName);
            if (existingUser != null)
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "User already exists."
                };
            }

            var newUser = new User
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                Role = userRegister.Role
            };

            // Hash the password
            newUser.Password = _passwordHasher.HashPassword(newUser, userRegister.Password);

            await _userRepository.SaveUser(newUser);

            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "User registered successfully."
            };

        }
    }
}
