using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;

namespace OnlineBookShop.Service
{
    public interface IBookService
    {
        Task<ResponseMessage> AddBook(BookDTO requestDTO);
    }
}
