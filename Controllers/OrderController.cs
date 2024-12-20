﻿using Microsoft.AspNetCore.Authorization;
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
       
                if (request == null || request.OrderDetails == null || request.OrderDetails.Count == 0)
                {
                    return BadRequest(new ResponseMessage
                    {
                        StatusCode = 400,
                        Message = "Invalid order request. Please provide valid order and order details."
                    });
                }
                return await _orderService.SaveOrder(request);

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
