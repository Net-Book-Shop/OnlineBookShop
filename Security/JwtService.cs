using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineBookShop.Data;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineBookShop.Security
{
    public class JwtService
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly RoleRepository _roleRepository;
        private readonly PrivilegeDetailsRepository _privilegeDetailsRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public JwtService(
            UserRepository userRepository,
            IConfiguration configuration,
            RoleRepository roleRepository,
            PrivilegeDetailsRepository privilegeDetailsRepository,
            PasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _privilegeDetailsRepository = privilegeDetailsRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDTO?> Authenticate(LoginRegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.password))
                return null;

            var userAccount = await _userRepository.FindByUserName(request.username);

            if (userAccount == null) return null;

            // Validate the password
            var verificationResult = _passwordHasher.VerifyHashedPassword(userAccount, userAccount.Password, request.password);
            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            // Retrieve JWT configuration values
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");

            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("JWT secret key is not configured.");

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, request.username),
                    new Claim("UserId", userAccount.Id.ToString()),
                    new Claim(ClaimTypes.Role, userAccount.Role)
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            // Retrieve role and privileges
            var roles = await _roleRepository.FindByRoleName(userAccount.Role);
            var privileges = new List<string>();

            var rolePrivileges = await _privilegeDetailsRepository.FindAllRoleWisePrivilageDetails(roles.Id);
            if (rolePrivileges != null)
            {
                privileges.AddRange(rolePrivileges.Select(p => p.Privilege?.PrivilegeName));
            }

            // Return the login response DTO
            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                UserName = userAccount.UserName,
                Role = userAccount.Role,
                UserCode = userAccount.UserCode,
                Privilages = privileges.Distinct().ToList(),
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
