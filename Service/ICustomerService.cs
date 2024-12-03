using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;

namespace OnlineBookShop.Service
{
    public interface ICustomerService
    {
        Task<ResponseMessage> GetAllCustomer();
    }
}
