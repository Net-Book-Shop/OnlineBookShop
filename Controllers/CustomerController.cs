using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Service;
using OnlineBookShop.Service.Impl;

namespace OnlineBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        [HttpPost("GetAllCustomer")]
        public async Task<ActionResult<ResponseMessage>> GetAllCustomer()
        {
            var result = await _customerService.GetAllCustomer();
            return result;
        }
    }
}
