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

        [AllowAnonymous]
        [HttpPost("AddBook")]
        public async Task<ActionResult<ResponseMessage>> AddBook(AddBookRequestDTO requestDTO)
        {
            var result = await _bookService.AddBook(requestDTO);
            return result;
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

    }
}
