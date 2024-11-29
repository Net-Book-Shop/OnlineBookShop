using OnlineBookShop.Dto;
using OnlineBookShop.Repository;

namespace OnlineBookShop.Service.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _repository;

        public CustomerService(CustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseMessage> GetAllCustomer()
        {
            try
            {
                var customer = await _repository.GetAllCustomers();
                var customerDtos = customer.Select(customer => new CustomerResponseDTO
                {
                    Id = customer.Id.ToString(),
                    Name = customer.Name,
                    Address = customer.Address,
                    City = customer.City,
                    Email = customer.Email,
                    MobileNumber = customer.MobileNumber,
                    OrderCount = customer.OrderCount,
                    IsActive = customer.IsActive,
                }).ToList();
                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Books retrieved successfully.",
                    Data = customerDtos
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get Customer data: {ex.Message}", ex);
            }
        }
    }
}
