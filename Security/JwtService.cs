using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineBookShop.Data;
using OnlineBookShop.Dto;
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
        private readonly PrivilegeRepository _privilegeRepository;

        public JwtService(UserRepository userRepository, IConfiguration configuration, RoleRepository roleRepository, PrivilegeDetailsRepository privilegeDetailsRepository, PrivilegeRepository privilegeRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _privilegeDetailsRepository = privilegeDetailsRepository;
            _privilegeRepository = privilegeRepository;
        }

        public async Task<LoginResponseDTO?> Authenticate(LoginRegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.password))
                return null;

            var userAccount = await _userRepository.FindByUserName(request.username);

            if (userAccount is null) return null;

            // Retrieve JWT configuration values
            var issuer = _configuration["JwtConfig:Issure"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");


            //Console.WriteLine($"Issuer: {issuer}");
            //Console.WriteLine($"Audience: {audience}");
            //Console.WriteLine($"Key: {key}");
            //Console.WriteLine($"Token Validity: {tokenValidityMins} minutes");

            // Validate that the key is not null or empty
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(JwtRegisteredClaimNames.Name, request.username) }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);


            var roles = await _roleRepository.FindByRoleName(userAccount.Role);
            var privileges = new List<string>();

                // Retrieve privileges by role
            var rolePrivileges = await _privilegeDetailsRepository.FindAllRoleWisePrivilageDetails(roles.Id);
            if (rolePrivileges != null)
            {
                privileges.AddRange(rolePrivileges.Select(p => p.Privilege?.PrivilegeName));
            }


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
