using Microsoft.AspNetCore.Mvc;
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

        public async Task<ResponseMessage> AddBook(AddBookRequestDTO requestDTO)
        {
            try
            {
                if (requestDTO == null)
                {
            
                    throw new Exception("Request data is null.");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(requestDTO.BookName) ||
                    requestDTO.Qty == null ||
                    requestDTO.CostPrice == null ||
                    requestDTO.SellingPrice == null)
                {
                    throw new Exception("Missing required fields. Ensure BookName, Qty, CostPrice, and SellingPrice are provided.");
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
                    Status = "Available",
                    Supplier = requestDTO.Supplier ?? string.Empty,
                    rating = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    IsActive = requestDTO.IsActive ?? 1,
                    ProductImage = requestDTO.ProductImage ?? string.Empty
                };

                // Save the book into the repository
                await _repository.SaveBookAsync(book);

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Book successfully added.",
                    Data = book.BookCode // Optional: Return generated BookCode
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the book. Details: {ex.Message}", ex);
            }
        }


        public async Task<ResponseMessage> GetAllBokkCategoryWise(BookSearchRequestDTO requestDTO)
        {
            try
            {
                List<Books> books;

                if (string.IsNullOrEmpty(requestDTO.CategoryName) && string.IsNullOrEmpty(requestDTO.SubCategoryName))
                {
                    // Get all books
                    books = await _repository.GetBooksCategoryWiseAsync(null, null);
                }
                else if (!string.IsNullOrEmpty(requestDTO.CategoryName) && string.IsNullOrEmpty(requestDTO.SubCategoryName))
                {
                    // Get books by category name
                    books = await _repository.GetBooksCategoryWiseAsync(requestDTO.CategoryName, null);
                }
                else if (string.IsNullOrEmpty(requestDTO.CategoryName) && !string.IsNullOrEmpty(requestDTO.SubCategoryName))
                {
                    // Get books by subcategory name
                    books = await _repository.GetBooksCategoryWiseAsync(null, requestDTO.SubCategoryName);
                }
                else
                {
                    // Get books by both category and subcategory
                    books = await _repository.GetBooksCategoryWiseAsync(requestDTO.CategoryName, requestDTO.SubCategoryName);
                }


                var bookDTOs = new List<BookDTO>();

                // Map and populate reviews
                foreach (var book in books)
                {
                    var review = await _repository.GetReviewsByBookCode(book.BookCode);
                    List<ReviewRequestDTO> reviewDtoList = null;
                    if (review.Count >0)
                    {
                        reviewDtoList = review.Select(review => new ReviewRequestDTO
                        {
                            BookCode = review.BookCode,
                            CustomerName = review.CustomerName,
                            MobileNumber = review.MobileNumber,
                            Rating = review.Rating,
                            Review = review.Review
                        }).ToList();
                    }
                   

                    bookDTOs.Add(new BookDTO
                    {
                        Id = book.Id.ToString(),
                        BookCode = book.BookCode,
                        CategoryName = book.CategoryName,
                        SubCategoryName = book.SubCategoryName,
                        CategoryCode = book.CategoryCode,
                        Qty = book.Qty,
                        CostPrice = book.CostPrice,
                        SellingPrice = book.SellingPrice,
                        BookName = book.BookName,
                        Description = book.Description,
                        Status = book.Status,
                        Supplier = book.Supplier,
                        rating = book.rating,
                        CreateDate = book.CreateDate.ToString("yyyy-MM-dd"),
                        UpdateDate = book.UpdateDate.ToString("yyyy-MM-dd"),
                        IsActive = book.IsActive,
                        ProductImage = book.ProductImage,
                        reviews = reviewDtoList
                    });
                }

                    return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Books retrieved successfully.",
                    Data = bookDTOs
                };
            }
            catch (Exception ex)
            {
              
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<ResponseMessage> AddBookReviewAndRating(ReviewRequestDTO requestDTO)
        {
            try {
                if (string.IsNullOrEmpty(requestDTO.BookCode))
                {
                    throw new Exception("Book code is null or empty.");
                }
                var existBok = await _repository.GetBookByCode(requestDTO.BookCode);
                if(existBok == null)
                {
                    throw new Exception("Cant find a Book");
                }
                var reviewBoj = new Reviews
                {
                    BookCode = existBok.BookCode,
                    CustomerName = requestDTO.CustomerName,
                    MobileNumber = requestDTO.MobileNumber,
                    Rating = requestDTO.Rating??0,
                    Review = requestDTO.Review
                };
                await _repository.SaveReview(reviewBoj);
                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Added Book Review"
                };

            } catch (Exception ex) {
                throw new Exception($"Failed Book Review Add : {ex.Message}", ex);
            }
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

        public async Task<ResponseMessage> GetAllBookWiseReviewCount()
        {
            try {
                var reviewCounts = await _repository.GetReviewCountsByBookCode();
                List<ReviewBookCountDTO> reviewDtoList = null;
                foreach (var (count, code) in reviewCounts)
                {
                    reviewDtoList = reviewCounts.Select(review => new ReviewBookCountDTO
                    {
                        BookCode = review.Code,
                        Count=review.Count,
                    }).ToList();
                    Console.WriteLine($"BookCode: {code}, Count: {count}");
                }
                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "succsess",
                    Data= reviewDtoList
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed Book Review Add : {ex.Message}", ex);
            }
        }

        public async Task<ResponseMessage> UpdateBookDetail(UpdateBookRequeatDTO requestDTO)
        {
            try
            {
                if (requestDTO == null)
                {
                    throw new Exception("Request data is null.");
                }

                // Fetch the existing book by its code
                var existBook = await _repository.GetBookByCode(requestDTO.BookCode);
                if (existBook == null)
                {
                    throw new Exception("Book details not found.");
                }

                if (!string.IsNullOrEmpty(requestDTO.Description))
                {
                    existBook.Description = requestDTO.Description;
                }

                if (!string.IsNullOrEmpty(requestDTO.Supplier))
                {
                    existBook.Supplier = requestDTO.Supplier;
                }

                if (requestDTO.CostPrice.HasValue && requestDTO.CostPrice > 0)
                {
                    existBook.CostPrice = requestDTO.CostPrice.Value;
                }

                if (requestDTO.SellingPrice.HasValue && requestDTO.SellingPrice > 0)
                {
                    existBook.SellingPrice = requestDTO.SellingPrice.Value;
                }

                if (requestDTO.Qty.HasValue)
                {
                    // Add or subtract quantity
                    existBook.Qty += requestDTO.Qty.Value;

                    // Update the status based on the quantity
                    existBook.Status = existBook.Qty <= 0 ? "Sold Out" : "Available";
                }

                await _repository.UpdateBook(existBook);

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Book details updated successfully."
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update book: {ex.Message}", ex);
            }
        }

        public async Task<ResponseMessage> GetAllBookDateAndCodeWise(BookSearchRequestDTO requestDTO)
        {
            try
            {
                List<Books> books;

                if (string.IsNullOrEmpty(requestDTO.FromDate) && string.IsNullOrEmpty(requestDTO.ToDate) && string.IsNullOrEmpty(requestDTO.BookCode))
                {
                    books = await _repository.GetAllBookDateAndCodeWise(null, null, null);
                }
                else if (!string.IsNullOrEmpty(requestDTO.FromDate) && string.IsNullOrEmpty(requestDTO.ToDate) && string.IsNullOrEmpty(requestDTO.BookCode))
                {
                    books = await _repository.GetAllBookDateAndCodeWise(requestDTO.FromDate, null, null);
                }
                else if (string.IsNullOrEmpty(requestDTO.FromDate) && !string.IsNullOrEmpty(requestDTO.ToDate) && string.IsNullOrEmpty(requestDTO.BookCode))
                {
                    books = await _repository.GetAllBookDateAndCodeWise(null, requestDTO.ToDate, null);
                }
                else if (string.IsNullOrEmpty(requestDTO.FromDate) && string.IsNullOrEmpty(requestDTO.ToDate) && !string.IsNullOrEmpty(requestDTO.BookCode))
                {
                    books = await _repository.GetAllBookDateAndCodeWise(null, null, requestDTO.BookCode);
                }
                else 
                {
                    books = await _repository.GetAllBookDateAndCodeWise(requestDTO.FromDate, requestDTO.ToDate, requestDTO.BookCode);
                }


                var bookDTOs = new List<BookDTO>();

                // Map and populate reviews
                foreach (var book in books)
                {
                    var review = await _repository.GetReviewsByBookCode(book.BookCode);
                    List<ReviewRequestDTO> reviewDtoList = null;
                    if (review.Count > 0)
                    {
                        reviewDtoList = review.Select(review => new ReviewRequestDTO
                        {
                            BookCode = review.BookCode,
                            CustomerName = review.CustomerName,
                            MobileNumber = review.MobileNumber,
                            Rating = review.Rating,
                            Review = review.Review
                        }).ToList();
                    }


                    bookDTOs.Add(new BookDTO
                    {
                        Id = book.Id.ToString(),
                        BookCode = book.BookCode,
                        CategoryName = book.CategoryName,
                        SubCategoryName = book.SubCategoryName,
                        CategoryCode = book.CategoryCode,
                        Qty = book.Qty,
                        CostPrice = book.CostPrice,
                        SellingPrice = book.SellingPrice,
                        BookName = book.BookName,
                        Description = book.Description,
                        Status = book.Status,
                        Supplier = book.Supplier,
                        rating = book.rating,
                        CreateDate = book.CreateDate.ToString("yyyy-MM-dd"),
                        UpdateDate = book.UpdateDate.ToString("yyyy-MM-dd"),
                        IsActive = book.IsActive,
                        ProductImage = book.ProductImage,
                        reviews = reviewDtoList
                    });
                }

                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "Books retrieved successfully.",
                    Data = bookDTOs
                };
            }
            catch (Exception ex)
            {

                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<ResponseMessage> GetAllBookWiseReviews()
        {
            try {
              var bookList = await _repository.GetAllBookWiseReviews();
                if(bookList == null)
                {
                    throw new Exception("Book review is empty");
                }
                return new ResponseMessage
                {
                    StatusCode = 200,
                    Message = "successfully.",
                    Data = bookList
                };
            }
            catch (Exception ex)
            {

                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
    }
}
