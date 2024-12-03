using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;

namespace OnlineBookShop.Service
{
    public interface ICategoryService
    {
        Task<ResponseMessage> AddCategory(CategoryRequestDTO requestDTO);
        Task<ResponseMessage> AddSubCategory(CategoryRequestDTO requestDTO);
        Task<ResponseMessage> GetAllCategory();
        Task<ResponseMessage> GetAllSubCategory();
    }
}
