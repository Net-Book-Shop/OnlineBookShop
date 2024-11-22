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

    }
}
