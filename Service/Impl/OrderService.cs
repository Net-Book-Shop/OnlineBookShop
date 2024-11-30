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
        private readonly BookRepository _bookRepository;

        public OrderService(OrderRepository orderRepository, CustomerRepository customerRepository, BookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
        }

        public async Task<ResponseMessage> SaveOrder(OrderRequestDTO orderRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(orderRequest.Customer.MobileNumber) ||
                    string.IsNullOrEmpty(orderRequest.Customer.Address) ||
                    string.IsNullOrEmpty(orderRequest.Customer.Name))
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
                    await _customerRepository.SaveCustomer(customerObj);
                }

                var orderCode = await GenerateOrderCodeAsync();

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
                    BankTransactionId = orderRequest.BankTransactionId ?? "N/A",
                    Discount = orderRequest.Discount ?? 0,
                    OrderAmount = orderRequest.OrderAmount ?? 0,
                    TotalCostPrice = orderRequest.TotalCostPrice ?? 0,
                    Status = "Pending",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsActive = 1
                };

                var savedOrder = await _orderRepository.SaveOrderAsync(order);

                var orderDetails = orderRequest.OrderDetails.Select(item => new OrderDetails
                {
                    OrderCode = savedOrder.OrderCode,
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

                await _orderRepository.SaveOrderDetailsAsync(orderDetails);

                foreach (var item in orderRequest.OrderDetails)
                {
                    var existBook = await _bookRepository.GetBookByCode(item.BookCode);

                    if (existBook != null && item.Qty >= 0)
                    {
                        existBook.Qty -= item.Qty ?? 0;

                        if (existBook.Qty == 0)
                        {
                            existBook.Status = "Sold Out";
                        }

                        await _bookRepository.UpdateBook(existBook);
                    }
                }

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Order saved successfully.",
                    Data = savedOrder.Id
                };
            }
            catch (Exception ex)
            {
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

                await _orderRepository.UpdateOrder(existOrder);

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

        public async Task<ResponseMessage> GetIncomeAndLastMonthProfit()
        {
            try
            {
                var sumsDataObj = await _orderRepository.GetDeleveryFeeDiscountOrderAmountTotalCostSum("Delivered");
                var cusotmer = await _customerRepository.ActiveCustomerCount();
               

                var lastMonthProfit = sumsDataObj.OrderAmountSum - (sumsDataObj.TotalCostPriceSum + sumsDataObj.DiscountSum + sumsDataObj.DeliveryFeeSum) ;
                var totalOrderAmount = sumsDataObj.OrderAmountSum;
                var totalDiscount = sumsDataObj.DiscountSum;
                var totalDeliveryFee = sumsDataObj.DeliveryFeeSum;
                var totalCostprice = sumsDataObj.TotalCostPriceSum;

                var dashboarddto = new DashboardResponseDTO
                {
                    LastMonthProfit = lastMonthProfit,
                    TotaDeliveryFee = totalDeliveryFee,
                    TotalCostPrice = totalCostprice,
                    TotaDiscount = totalDiscount,
                    TotaOrderAmunt = totalOrderAmount,
                    TotaCustomer = cusotmer
                };
                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "successfully.",
                    Data = dashboarddto
                };

            }
            catch (Exception ex) {
                throw new Exception($"Failed to generate Income and Moth profit: {ex.Message}", ex);
            }
        }
    }
}
