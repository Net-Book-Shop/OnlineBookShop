using OnlineBookShop.Data;

namespace OnlineBookShop.Repository
{
    public class BookRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public BookRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
