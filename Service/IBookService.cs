using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;

namespace OnlineBookShop.Service
{
    public interface IBookService
    {
        Task <ResponseMessage> AddBook(AddBookRequestDTO requestDTO);
        Task<ResponseMessage> AddBookReviewAndRating(ReviewRequestDTO requestDTO);
        Task<ResponseMessage> GetAllBokkCategoryWise(BookSearchRequestDTO requestDTO);
        Task<ResponseMessage> GetAllBookWiseReviewCount();
    }
}
