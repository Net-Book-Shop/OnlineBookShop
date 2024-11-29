using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Service;
using OnlineBookShop.Service.Impl;
using System.Security.Cryptography.X509Certificates;

namespace OnlineBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpPost("AddCategory")]
        public async Task<ActionResult<ResponseMessage>> AddCategory(CategoryRequestDTO requestDTO)
        {
            var result = await _categoryService.AddCategory(requestDTO);
            return result;
        }
        [AllowAnonymous]
        [HttpPost("AddSubCategory")]
        public async Task<ActionResult<ResponseMessage>> AddSubCategory(CategoryRequestDTO requestDTO)
        {
            var result = await _categoryService.AddSubCategory(requestDTO);
            return result;
        }

        [AllowAnonymous]
        [HttpGet("GetAllCategory")]
        public async Task<ActionResult<ResponseMessage>> GetAllCategory()
        {
            var result = await _categoryService.GetAllCategory();
            return result;
        }
        
        [AllowAnonymous]
        [HttpGet("GetAllSubCategory")]
        public async Task<ActionResult<ResponseMessage>> GetAllSubCategory()
        {
            var result = await _categoryService.GetAllSubCategory();
            return result;
        }
    }
}
