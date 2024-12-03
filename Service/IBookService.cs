using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;

namespace OnlineBookShop.Service
{
    public interface IBookService
    {
        Task <ResponseMessage> AddBook(AddBookRequestDTO requestDTO);
        Task<ResponseMessage> AddBookReviewAndRating(ReviewRequestDTO requestDTO);
        Task<ResponseMessage> GetAllBokkCategoryWise(BookSearchRequestDTO requestDTO);
        Task<ResponseMessage> GetAllBookDateAndCodeWise(BookSearchRequestDTO requestDTO);
        Task<ResponseMessage> GetAllBookWiseReviewCount();
        Task<ResponseMessage> GetAllBookWiseReviews();
        Task<ResponseMessage> UpdateBookDetail(UpdateBookRequeatDTO requestDTO);
    }
}
