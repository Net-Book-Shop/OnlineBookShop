using OnlineBookShop.Dto;
using OnlineBookShop.Model;

namespace OnlineBookShop.Service
{
    public interface IOrderService
    {
        Task<ResponseMessage> GetStatusWiseOrderList(OrderUpdateRequestDTO request);
        Task<Orders> SaveOrder(OrderRequestDTO orderRequest);
        Task<ResponseMessage>UpdateOrderStatus(OrderUpdateRequestDTO request);
    }
}
