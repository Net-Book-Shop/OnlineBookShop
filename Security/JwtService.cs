using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineBookShop.Data;
using OnlineBookShop.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineBookShop.Security
{
    public class JwtService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _configuration;
        public JwtService(ApplicationDBContext dBContext,IConfiguration configuration) { 
        
            _dbContext = dBContext;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO?> Authenticate(LoginRegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var userAccount = await _dbContext.users.FirstOrDefaultAsync(x => x.UserName == request.UserName);

            if (userAccount is null) return null;

            // Retrieve JWT configuration values
            var issuer = _configuration["JwtConfig:Issure"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");


            // Log the configuration values to ensure they are loaded
            Console.WriteLine($"Issuer: {issuer}");
            Console.WriteLine($"Audience: {audience}");
            Console.WriteLine($"Key: {key}");
            Console.WriteLine($"Token Validity: {tokenValidityMins} minutes");

            // Validate that the key is not null or empty
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(JwtRegisteredClaimNames.Name, request.UserName) }),
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

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                UserName = request.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }

    }
}
