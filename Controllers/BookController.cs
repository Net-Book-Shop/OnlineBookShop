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
        private readonly IFileService _fileService;

        public BookController(IBookService bookService, IFileService fileService)
        {
            _bookService = bookService;
            _fileService = fileService;
        }


        [Authorize]
        [HttpPost("Add")]
        public async Task<ActionResult<ResponseMessage>> Add([FromForm] AddBookRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return (new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Invalid input data."
                });
            }

            if (model.ImageFile != null)
            {
                // Save the image and set the image path
                var fileResult = _fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    model.ProductImage = fileResult.Item2; // Set image path in the model
                }
                else
                {
                    return (new ResponseMessage
                    {
                        StatusCode = 400,
                        Message = fileResult.Item2 // Return error from file service
                    });
                }
            }

            // Call the service method and await the result
            var result = await _bookService.AddBook(model);

            return (result); // Ensure only ResponseMessage is returned
        }
        [AllowAnonymous]
        [HttpPost("GetAllBookCategoryWise")]
        public async Task<ActionResult<ResponseMessage>> GetAllBookCategoryWise(BookSearchRequestDTO requestDTO)
        {
            var result = await _bookService.GetAllBokkCategoryWise(requestDTO);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("AddBookReviewAndRating")]
        public async Task<ActionResult<ResponseMessage>> AddBookReviewAndRating(ReviewRequestDTO requestDTO)
        {
            var result = await _bookService.AddBookReviewAndRating(requestDTO);
            return result;
        }

        [Authorize]
        [HttpGet("GetAllBookWiseReviewCount")]
        public async Task<ActionResult<ResponseMessage>> GetAllBookWiseReviewCount()
        {
            var result = await _bookService.GetAllBookWiseReviewCount();
            return result;
        }

        [Authorize]
        [HttpPost("UpdateBookDetail")]
        public async Task<ActionResult<ResponseMessage>> UpdateBookDetail(UpdateBookRequeatDTO requestDTO)
        {
            var result = await _bookService.UpdateBookDetail(requestDTO);
            return result;
        }

        [Authorize]
        [HttpPost("GetAllBookDateAndCodeWise")]
        public async Task<ActionResult<ResponseMessage>> GetAllBookDateAndCodeWise(BookSearchRequestDTO requestDTO)
        {
            var result = await _bookService.GetAllBookDateAndCodeWise(requestDTO);
            return result;
        }
    }
}
