using ConstrunctionApp.Model;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Other;
using OnlineBookShop.Repository;
using System.Globalization;

namespace OnlineBookShop.Service.Impl
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly BookRepository _bookRepository;
        private readonly EmailService _emailService;

        public OrderService(OrderRepository orderRepository, CustomerRepository customerRepository, BookRepository bookRepository, EmailService emailService)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
            _emailService = emailService;
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
                    CreateUser = orderRequest.CreateUser ?? "N/A",
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

                string toEmail = "noreply.admin@gmail.com"; 
                string emailBody = GenerateOrderEmailBody(orderRequest, savedOrder, orderDetails);

                // Send the email
                await _emailService.SendEmailAsync(toEmail, savedOrder.OrderCode, emailBody, "no-reply@yourdomain.com");

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


                if (!string.IsNullOrEmpty(existOrder.CustomerEmail))
                {
                    if (request.Status == "Picked up" || request.Status == "Delivered" || request.Status == "Cancel")
                    {
                        var emailBody = GenerateOrderUpdateEmailBody(request, existOrder);
                        await _emailService.SendEmailAsync(
                            existOrder.CustomerEmail, // To: Customer's email
                            $"Order Status Update: {existOrder.OrderCode}", // Subject
                            emailBody, // Body
                            "no-reply@yourdomain.com" // From
                        );
                    }
                }
                

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
            try
            {
                // Parse date inputs
                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (!string.IsNullOrEmpty(request.FromDate))
                {
                    fromDate = DateTime.ParseExact(request.FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(request.ToDate))
                {
                    toDate = DateTime.ParseExact(request.ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                // Fetch orders matching the criteria
                var orderList = await _orderRepository.GetOrdersByCriteriaAsync(
                    request.Status,
                    request.OrderCode,
                    fromDate,
                    toDate,
                    request.CreateUser

                );

                if (!orderList.Any())
                {
                    return new ResponseMessage
                    {
                        StatusCode = 404,
                        Message = "No orders found matching the criteria.",
                        Data = null
                    };
                }

                var orderDtoList = new List<OrderRequestDTO>();

                foreach (var order in orderList)
                {
                    // Fetch order details for the current order
                    var orderDetails = await _orderRepository.GetOrderDetailsByStatus(order.OrderCode);

                    // Map order details to DTO
                    var orderDetailsDto = orderDetails.Select(detail => new OrderDetailRequestDTO
                    {
                        BookCode = detail.BookCode,
                        Qty = detail.Qty,
                        UnitCostPrice = detail.UnitCostPrice,
                        UnitSellingPrice = detail.UnitSellingPrice,
                        Total = detail.Total,
                    }).ToList();

                    // Add the order with its details to the DTO list
                    orderDtoList.Add(new OrderRequestDTO
                    {
                        OrderCode = order.OrderCode,
                        Status = order.Status,
                        CreateDate = order.CreateDate.ToString("yyyy-MM-dd"),
                        OrderAmount = order.OrderAmount,
                        CustomerName = order.CustomerName,
                        Address = order.Address,
                        MobileNumber = order.MobileNumber,
                        TotalQty = order.TotalQty,
                        CustomerEmail = order.CustomerEmail,
                        DeliveryFee = order.DeliveryFee,
                        PaymentMethod = order.PaymentMethod,
                        BankTransactionId = order.BankTransactionId,
                        Discount = order.Discount,
                        TotalCostPrice = order.TotalCostPrice,
                        OrderDetails = orderDetailsDto?? new List<OrderDetailRequestDTO>() // Add order details here
                    });
                }

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Orders retrieved successfully.",
                    Data = orderDtoList
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve orders: {ex.Message}", ex);
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
        private string GenerateOrderEmailBody(OrderRequestDTO orderRequest, Orders savedOrder, List<OrderDetails> orderDetails)
        {
            var emailBody = $@"
        <h2>New Order Received</h2>
        <p><strong>Customer Name:</strong> {orderRequest.Customer.Name}</p>
        <p><strong>Address:</strong> {orderRequest.Customer.Address}</p>
        <p><strong>City:</strong> {orderRequest.Customer.City}</p>
        <p><strong>Mobile:</strong> {orderRequest.Customer.MobileNumber}</p>
        <h3>Order Details:</h3>
        <table border='1' cellpadding='5' cellspacing='0'>
            <thead>
                <tr>
                    <th>Book Name</th>
                    <th>Quantity</th>
                    <th>Total Amount</th>
                </tr>
            </thead>
            <tbody>";

            foreach (var detail in orderDetails)
            {
                emailBody += $@"
            <tr>
                <td>{detail.BookCode}</td> <!-- You can replace with actual book name -->
                <td>{detail.Qty}</td>
                <td>{detail.Total:C}</td>
            </tr>";
            }

            emailBody += $@"
            </tbody>
        </table>
        <p><strong>Total Order Amount:</strong> {savedOrder.OrderAmount:C}</p>
        <p><strong>Order Code:</strong> {savedOrder.OrderCode}</p>
        <p><strong>Status:</strong> {savedOrder.Status}</p>
        <p>Thank you for your order!</p>";

            return emailBody;
        }

        private string GenerateOrderUpdateEmailBody(OrderUpdateRequestDTO request, Orders existOrder)
        {
            return $@"
        <h1>Order Status Update</h1>
        <p>Dear {existOrder.CustomerName},</p>
        <p>Your order <strong>{existOrder.OrderCode}</strong> has been updated to <strong>{request.Status}</strong>.</p>
        <p>Order Details:</p>
        <ul>
            <li><strong>Order Code:</strong> {existOrder.OrderCode}</li>
            <li><strong>Status:</strong> {request.Status}</li>
            <li><strong>Updated Date:</strong> {existOrder.UpdateDate:yyyy-MM-dd HH:mm:ss}</li>
            <li><strong>Total Amount:</strong> ${existOrder.OrderAmount:F2}</li>
        </ul>
        <p>If you have any questions, feel free to contact our support team.</p>
        <p>Thank you,<br>Your Book Shop Team</p>
    ";
        }

    }
}
