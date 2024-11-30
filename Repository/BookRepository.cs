using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class BookRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public BookRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveBookAsync(Books books)
        {
            _dbContext.Book.Add(books);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Books?> GetLastBookAsync()
        {
            return await _dbContext.Book
                .OrderByDescending(b => b.CreateDate)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Books>> GetBooksCategoryWiseAsync(string? categoryName, string? subCategoryName)
        {
            var query = _dbContext.Book.AsQueryable();

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(book => book.CategoryName == categoryName);
            }

            if (!string.IsNullOrEmpty(subCategoryName))
            {
                query = query.Where(book => book.SubCategoryName == subCategoryName);
            }

            return await query.ToListAsync();
        }

        public async Task<Books> GetBookByCode(string bookCode)
        {
            return await _dbContext.Book.FirstOrDefaultAsync(x => x.BookCode == bookCode);

        }

        public async Task UpdateBook(Books books)
        {
            _dbContext.Book.Update(books);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveReview(Reviews reviews)
        {
            _dbContext.Reviews.Add(reviews);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Reviews>> GetReviewsByBookCode(string bookCode)
        {
            return await _dbContext.Reviews.Where(x => x.BookCode == bookCode).ToListAsync();

        }
        public async Task<List<(int Count, string Code)>> GetReviewCountsByBookCode()
        {
            var reviewCounts = await _dbContext.Reviews
                .GroupBy(x => x.BookCode)
                .Select(g => new
                {
                    Count = g.Count(),
                    Code = g.Key
                })
                .ToListAsync();

            return reviewCounts.Select(r => (r.Count, r.Code)).ToList();
        }


    }
}
