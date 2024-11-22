using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Service;

namespace OnlineBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivilegeController : ControllerBase
    {
        private readonly IPrivilageService _privilageService;

        public PrivilegeController(IPrivilageService privilageService)
        {
            _privilageService = privilageService;
        }

        [Authorize]
        [HttpPost("AddPrivilage")]
        public async Task<ActionResult<ResponseMessage>> AddPrivilage(PrivilageRequestDTO requestDTO)
        {
            var result = await _privilageService.AddPrivilage(requestDTO);
            return result;
        }

        [Authorize]
        [HttpPost("GetRoleWisePrivileges")]
        public async Task<ActionResult<ResponseMessage>> GetRoleWisePrivileges(PrivilageRequestDTO requestDTO)
        {
            var result = await _privilageService.GetRoleWisePrivileges(requestDTO);
            return result;
        }
        [Authorize]
        [HttpPost("GetAllPrivileges")]
        public async Task<ActionResult<ResponseMessage>> GetAllPrivileges()
        {
            var result = await _privilageService.GetAllPrivileges();
            return result;
        }

        [Authorize]
        [HttpPost("AssignPrivileges")]
        public async Task<ActionResult<ResponseMessage>> AssignPrivileges(PrivilageRequestDTO requestDTO)
        {
            var result = await _privilageService.AssignPrivileges(requestDTO);
            return result;
        }
        [Authorize]
        [HttpPost("UpdatePrivilages")]
        public async Task<ActionResult<ResponseMessage>> UpdatePrivilages(PrivilageRequestDTO requestDTO)
        {
            var result = await _privilageService.UpdatePrivilages(requestDTO);
            return result;
        }
    }
}
