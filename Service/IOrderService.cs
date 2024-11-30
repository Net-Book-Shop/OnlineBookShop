using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;

namespace OnlineBookShop.Service
{
    public interface IOrderService
    {
        Task<ResponseMessage> GetIncomeAndLastMonthProfit();
        Task<ResponseMessage> GetStatusWiseOrderList(OrderUpdateRequestDTO request);
        Task<ResponseMessage> SaveOrder(OrderRequestDTO orderRequest);
        Task<ResponseMessage>UpdateOrderStatus(OrderUpdateRequestDTO request);
    }
}
