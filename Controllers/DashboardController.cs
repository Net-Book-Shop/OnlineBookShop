using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Service;

namespace OnlineBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private  readonly IOrderService _orderService;

        public DashboardController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("GetIncomeAndLastMonthProfit")]
        public async Task<ActionResult<ResponseMessage>> GetIncomeAndLastMonthProfit()
        {
            var result = await _orderService.GetIncomeAndLastMonthProfit();
            return result;
        }
    }
}
