using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Dto;
using OnlineBookShop.Repository;
using OnlineBookShop.Service;
using OnlineBookShop.Service.Impl;

namespace OnlineBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [AllowAnonymous]
        [HttpPost("AddBook")]
        public async Task<ActionResult<ResponseMessage>> AddBook(BookDTO requestDTO)
        {
            var result = await _bookService.AddBook(requestDTO);
            return result;
        }
    }
}
