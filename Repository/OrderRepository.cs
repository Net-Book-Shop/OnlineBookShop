using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class OrderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Orders SaveOrder(Orders order)
        {
            _dbContext.Orders.Add(order); // Add the main order
            _dbContext.SaveChanges();    // Save the order to generate its ID

            return order;
        }

        public void SaveOrderDetails(IEnumerable<OrderDetails> orderDetails)
        {
            _dbContext.OrderDetails.AddRange(orderDetails); // Add order details
            _dbContext.SaveChanges();                      // Commit to the database
        }

        public async Task<Orders?> GetLastOrderAsync()
        {
            return await _dbContext.Orders
                .OrderByDescending(b => b.CreateDate)
                .FirstOrDefaultAsync();
        }

        public async Task<Orders> GetOrderByCode(string orderCode)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderCode == orderCode);
        }

        public async Task UpdateOrder(Orders order)
        {
            _dbContext.Orders.Update(order); 
            _dbContext.SaveChanges();   

           
        }

        public async Task<List<Orders>> GetOrdersByStatus(string status)
        {
            return await _dbContext.Orders.Where(x => x.Status == status).ToListAsync();
        }
    }
}
