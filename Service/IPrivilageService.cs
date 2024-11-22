using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;

namespace OnlineBookShop.Service
{
    public interface IPrivilageService
    {
        Task<ResponseMessage> AddPrivilage(PrivilageRequestDTO requestDTO);
        Task<ResponseMessage> AssignPrivileges(PrivilageRequestDTO requestDTO);
        Task<ActionResult<ResponseMessage>> GetAllPrivileges();
        Task<ResponseMessage> GetRoleWisePrivileges(PrivilageRequestDTO requestDTO);
        Task<ResponseMessage> UpdatePrivilages(PrivilageRequestDTO requestDTO);
    }
}
