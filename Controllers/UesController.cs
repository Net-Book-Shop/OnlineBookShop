using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Service;

namespace OnlineBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUserService _userService;

        public ValuesController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<ResponseMessage>> Registor(UserRegistorRequestDTO userRegister)
        {
            var result = await _userService.UserRegistor(userRegister);
            return result;
        }

        [Authorize]
        [HttpPost("GetRoleWiseUserlist")]
        public async Task<ActionResult<ResponseMessage>> GetRoleWiseUserlist(UserRegistorRequestDTO requestDTO)
        {
            var result = await _userService.GetRoleWiseUserlist(requestDTO);
            return result;
        }
    }
}
