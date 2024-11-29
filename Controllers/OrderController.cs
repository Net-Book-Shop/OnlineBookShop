using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Service;

namespace OnlineBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [AllowAnonymous]
        [HttpPost("saveOrder")]
        public async Task<ActionResult<ResponseMessage>> SaveOrder([FromBody] OrderRequestDTO request)
        {
            try
            {
                if (request == null || request.OrderDetails == null || request.OrderDetails.Count == 0)
                {
                    return BadRequest(new ResponseMessage
                    {
                        StatusCode = 400,
                        Message = "Invalid order request. Please provide valid order and order details."
                    });
                }

                // Save the order and details using the service
                var savedOrder = await _orderService.SaveOrder(request);

                return Ok(new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Order saved successfully.",
                    Data = savedOrder.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage
                {
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpPost("UpdateOrderStatus")]
        public async Task<ActionResult<ResponseMessage>> UpdateOrderStatus(OrderUpdateRequestDTO request)
        {
            var savedOrder = await _orderService.UpdateOrderStatus(request);
            return savedOrder;
        }

        [Authorize]
        [HttpPost("GetStatusWiseOrderList")]
        public async Task<ActionResult<ResponseMessage>> GetStatusWiseOrderList(OrderUpdateRequestDTO request)
        {
            return await _orderService.GetStatusWiseOrderList(request);
           
        }
    }
}
