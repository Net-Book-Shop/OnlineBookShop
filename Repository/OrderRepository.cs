using OnlineBookShop.Data;

namespace OnlineBookShop.Repository
{
    public class OrderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
