using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Repository;

namespace OnlineBookShop.Service.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _repository;

        public CategoryService(CategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseMessage> AddCategory(CategoryRequestDTO requestDTO)
        {
            if (string.IsNullOrWhiteSpace(requestDTO.CategoryName) || string.IsNullOrWhiteSpace(requestDTO.CategoryCode)){
                return new ResponseMessage { StatusCode = 400, Message = "Category name and code requvlid" };
            }

            var exsitCategoryCode = await _repository.FindCategoryByCode(requestDTO.CategoryCode);
            if (exsitCategoryCode != null) {
                return new ResponseMessage { StatusCode = 400, Message = "Category code allray exsict" };

            }
            var obj = new Category
            {
                CategoryName = requestDTO.CategoryName,
                CategoryCode = requestDTO.CategoryCode,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsActive = 1
            };
            await _repository.SaveCategory(obj);
            return new ResponseMessage { StatusCode = 200, Message = "successfully added." };

        }

        public async Task<ResponseMessage> AddSubCategory(CategoryRequestDTO requestDTO)
        {
            if (string.IsNullOrWhiteSpace(requestDTO.CategoryName) || string.IsNullOrWhiteSpace(requestDTO.CategoryCode))
            {
                return new ResponseMessage { StatusCode = 400, Message = "Category name and code requvlid" };
            }

            var exsitCategoryCode = await _repository.FindSubCategoryByCode(requestDTO.CategoryCode);
            if (exsitCategoryCode != null)
            {
                return new ResponseMessage { StatusCode = 400, Message = "Category code allray exsict" };

            }
            var obj = new SubCategory
            {
                SubCategoryName = requestDTO.CategoryName,
                SubCategoryCode = requestDTO.CategoryCode,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsActive = 1
            };
            await _repository.SaveSubCategory(obj);
            return new ResponseMessage { StatusCode = 200, Message = "successfully added." };
        }
    }
}
