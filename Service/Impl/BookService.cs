using OnlineBookShop.Dto;
using OnlineBookShop.Model;
using OnlineBookShop.Repository;

namespace OnlineBookShop.Service.Impl
{
    public class BookService : IBookService
    {
        private readonly BookRepository _repository;

        public BookService(BookRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseMessage> AddBook(BookDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Request data is null."
                };
            }

            // Validate required fields (add as needed)
            if (string.IsNullOrWhiteSpace(requestDTO.BookName) || requestDTO.Qty == null || requestDTO.CostPrice == null || requestDTO.SellingPrice == null)
            {
                return new ResponseMessage
                {
                    StatusCode = 400,
                    Message = "Missing required fields. Ensure BookName, Qty, CostPrice, and SellingPrice are provided."
                };
            }

            string bookCode = await GenerateBookCodeAsync();

            // Create a new Book entity
            var book = new Books
            {
                BookCode = bookCode, 
                CategoryName = requestDTO.CategoryName ?? string.Empty,
                SubCategoryName = requestDTO.SubCategoryName ?? string.Empty,
                CategoryCode = requestDTO.CategoryCode ?? string.Empty,
                Qty = requestDTO.Qty ?? 0,
                CostPrice = requestDTO.CostPrice ?? 0.0,
                SellingPrice = requestDTO.SellingPrice ?? 0.0,
                BookName = requestDTO.BookName ?? string.Empty,
                Description = requestDTO.Description ?? string.Empty,
                Status = requestDTO.Status ?? "Available",
                Supplier = requestDTO.Supplier ?? string.Empty,
                rating = requestDTO.rating ?? 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                IsActive = requestDTO.IsActive ?? 1
            };

            // Save the book into the repository
            await _repository.SaveBookAsync(book);

            return new ResponseMessage
            {
                StatusCode = 200,
                Message = "Book successfully added."
            };
        }


        private async Task<string> GenerateBookCodeAsync()
        {
            var lastBook = await _repository.GetLastBookAsync();

            if (lastBook != null && !string.IsNullOrWhiteSpace(lastBook.BookCode))
            {
                string lastCode = lastBook.BookCode;
                if (lastCode.StartsWith("B") && int.TryParse(lastCode.Substring(1), out int numericPart))
                {
                    return $"B{(numericPart + 1).ToString("D4")}";
                }
            }

            return "B0001";
        }


    }
}
