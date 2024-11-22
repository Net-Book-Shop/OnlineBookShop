using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Security;

namespace OnlineBookShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)=>
            _jwtService = jwtService;

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>>Login(LoginRegisterDTO loginReq)
        {
            var result = await _jwtService.Authenticate(loginReq);
            if(result is null) return Unauthorized();
            return Ok(result);
        }
        
    }
    
}
