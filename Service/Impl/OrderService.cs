using ConstrunctionApp.Model;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Repository;

namespace OnlineBookShop.Service.Impl
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;

        public OrderService(OrderRepository orderRepository, CustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Orders> SaveOrder(OrderRequestDTO orderRequest)
        {
            try
            {
                if(string.IsNullOrEmpty(orderRequest.Customer.MobileNumber)|| string.IsNullOrEmpty(orderRequest.Customer.Address)|| string.IsNullOrEmpty(orderRequest.Customer.Name))
                {
                    throw new Exception("Customer data is empty");
                }
                var existCustomer = await _customerRepository.FindByCustomerMobile(orderRequest.Customer.MobileNumber);
                if (existCustomer == null)
                {
                    var customerObj = new Customer
                    {
                        Name = orderRequest.Customer.Name,
                        Email = orderRequest.Customer.Email ?? "N/A",
                        Address = orderRequest.Customer.Address,
                        City = orderRequest.Customer.City,
                        MobileNumber = orderRequest.Customer.MobileNumber,
                        OrderCount = 0,
                        IsActive = true
                    };
                    _customerRepository.SaveCustomer(customerObj);
                }
                
                // Generate a unique order code
                var orderCode = await GenerateOrderCodeAsync();

                // Map the order request DTO to the database model
                var order = new Orders
                {
                    OrderCode = orderCode,
                    TotalQty = orderRequest.TotalQty ?? 0,
                    CustomerName = orderRequest.CustomerName ?? "Unknown",
                    Address = orderRequest.Address ?? "Unknown",
                    MobileNumber = orderRequest.MobileNumber ?? "Unknown",
                    CustomerEmail = orderRequest.CustomerEmail ?? "Unknown",
                    DeliveryFee = orderRequest.DeliveryFee ?? 0,
                    PaymentMethod = orderRequest.PaymentMethod ?? "Not Specified",
                    Discount = orderRequest.Discount ?? 0,
                    OrderAmount = orderRequest.OrderAmount ?? 0,
                    TotalCostPrice = orderRequest.TotalCostPrice ?? 0,
                    Status = "Pending",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsActive = 1
                };

                // Save the order to the database
                var savedOrder = _orderRepository.SaveOrder(order);

                // Prepare and map order details
                var orderDetails = orderRequest.OrderDetails
                    .Select(item => new OrderDetails
                    {
                        OrderCode=savedOrder.OrderCode,
                        BookCode = item.BookCode ?? "Unknown",
                        Qty = item.Qty ?? 0,
                        UnitCostPrice = item.UnitCostPrice ?? 0,
                        UnitSellingPrice = item.UnitSellingPrice ?? 0,
                        Total = item.Total ?? 0,
                        Status = "Pending",
                        IsActive = 1,
                        OrderID = savedOrder.Id,
                        Orders = savedOrder
                    }).ToList();

                // Save the order details to the database
                _orderRepository.SaveOrderDetails(orderDetails);

                return savedOrder;
            }
            catch (Exception ex)
            {
                // Log the exception (you can implement logging here)
                throw new Exception($"Failed to save order: {ex.Message}", ex);
            }
        }

        public async Task<ResponseMessage> UpdateOrderStatus(OrderUpdateRequestDTO request)
        {
            try {
                if (string.IsNullOrEmpty(request.OrderCode) || string.IsNullOrEmpty(request.Status))
                {
                    throw new Exception("Order code or Order status is empty!");
                }
                var existOrder = await _orderRepository.GetOrderByCode(request.OrderCode);
                if (existOrder == null) { throw new Exception("Cant find a Order"); }
                
                existOrder.Status = request.Status;
                existOrder.UpdateDate= DateTime.Now;

                _orderRepository.UpdateOrder(existOrder);

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Order Update successfully."
                };

            }
            catch (Exception ex) 
            { 
                throw new Exception(ex.Message, ex); 
            }
        }

        public async Task<ResponseMessage> GetStatusWiseOrderList(OrderUpdateRequestDTO request)
        {
            try {
                if (!string.IsNullOrEmpty(request.Status))
                {
                    throw new Exception("status null or empty");
                }
              var orderList = await _orderRepository.GetOrdersByStatus(request.Status);
                var orderDtoList = orderList.Select(order => new OrderRequestDTO
                   {
                       OrderCode = order.OrderCode
                   }).ToList();

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Order Update successfully.",
                    Data = orderDtoList
                };
            } catch (Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<string> GenerateOrderCodeAsync()
        {
            try
            {
                var lastOrder = await _orderRepository.GetLastOrderAsync();

                if (lastOrder != null && !string.IsNullOrWhiteSpace(lastOrder.OrderCode))
                {
                    string lastCode = lastOrder.OrderCode;
                    if (lastCode.StartsWith("OR") && int.TryParse(lastCode.Substring(2), out int numericPart))
                    {
                        return $"OR{(numericPart + 1).ToString("D4")}";
                    }
                }

                return "OR0001";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate order code: {ex.Message}", ex);
            }
        }

    }
}
