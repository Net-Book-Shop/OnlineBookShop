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
        public async Task<Orders> SaveOrderAsync(Orders order)
        {
            _dbContext.Orders.Add(order); // Add the main order
            _dbContext.SaveChanges();    // Save the order to generate its ID

            return order;
        }

        public async Task SaveOrderDetailsAsync(IEnumerable<OrderDetails> orderDetails)
        {
            _dbContext.OrderDetails.AddRange(orderDetails); // Add order details
            await _dbContext.SaveChangesAsync();           // Use async method to commit changes
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

        public async Task<List<OrderDetails>> GetOrderDetailsByStatus(string ordercode)
        {
            return await _dbContext.OrderDetails.Where(x => x.OrderCode == ordercode).ToListAsync();
        }

        public async Task<List<Orders>> GetOrdersByCriteriaAsync(string? status, string? orderCode, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrEmpty(orderCode))
            {
                query = query.Where(x => x.OrderCode.Contains(orderCode));
            }

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.CreateDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.CreateDate <= toDate.Value);
            }

            return await query.ToListAsync();
        }


        public async Task<(double DeliveryFeeSum, double DiscountSum, double OrderAmountSum, double TotalCostPriceSum)> GetDeleveryFeeDiscountOrderAmountTotalCostSum(string status)
        {
            var lastMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1); // Start of last month
            var lastMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);    // End of last month

            var result = await _dbContext.Orders
                .Where(x => x.Status == status && x.CreateDate >= lastMonthStart && x.CreateDate <= lastMonthEnd)
                .GroupBy(x => 1) 
                .Select(g => new
                {
                    DeliveryFeeSum = g.Sum(o => o.DeliveryFee ),
                    DiscountSum = g.Sum(o => o.Discount ),
                    OrderAmountSum = g.Sum(o => o.OrderAmount),
                    TotalCostPriceSum = g.Sum(o => o.TotalCostPrice)
                })
                .FirstOrDefaultAsync();

            return result != null
                ? (result.DeliveryFeeSum, result.DiscountSum, result.OrderAmountSum, result.TotalCostPriceSum)
                : (0, 0, 0, 0); 
        }


    }
}
