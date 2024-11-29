using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;

namespace OnlineBookShop.Service
{
    public interface IUserService
    {
        Task<ResponseMessage> GetRoleWiseUserlist(UserRegistorRequestDTO requestDTO);
        Task<ActionResult<ResponseMessage>> UpdateUser(UserRegistorRequestDTO requestDTO);
        Task<ResponseMessage> UserRegistor(UserRegistorRequestDTO userRegistor);
    }
}
