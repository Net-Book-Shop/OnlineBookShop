using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ActionResult<ResponseMessage>> UpdateUser(UserRegistorRequestDTO requestDTO)
        {
            try {
                if (string.IsNullOrEmpty(requestDTO.UserName))
                {
                    throw new Exception("User name is empty!");
                }
                var existingUser = await _userRepository.FindByUserName(requestDTO.UserName);
                if (existingUser == null)
                {
                    throw new Exception("Cant find a user!");
                }
                existingUser.UserName = requestDTO.UserName;
                existingUser.Password = _passwordHasher.HashPassword(existingUser, requestDTO.Password);
                existingUser.Email = requestDTO.Email;

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "User Update successfully."
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed Update Customer: {ex.Message}", ex);
            }
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
            // Generate a new userCode
            string userCode = await GenerateUserCodeAsync();

            var newUser = new User
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                Role = userRegister.Role,
                UserCode = userCode
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

        private async Task<string> GenerateUserCodeAsync()
        {
            var lastUser = await _userRepository.GetLastUserAsync(); 

            if (lastUser != null && !string.IsNullOrEmpty(lastUser.UserCode))
            {
                string lastUserCode = lastUser.UserCode;

                if (lastUserCode.Length > 1 && lastUserCode.StartsWith("U"))
                {
                    if (int.TryParse(lastUserCode.Substring(1), out int numericPart))
                    {
                        return "U" + (numericPart + 1).ToString("D4");
                    }
                }
            }
      
            return "U0001";
        }

    }
}
