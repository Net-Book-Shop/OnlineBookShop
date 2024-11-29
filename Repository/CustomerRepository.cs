using ConstrunctionApp.Model;
using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class CustomerRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CustomerRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> FindByCustomerMobile(string mobile)
        {
            return await _dbContext.customers.FirstOrDefaultAsync(x => x.MobileNumber == mobile);

        }
        public async Task SaveCustomer(Customer customer)
        {
            _dbContext.customers.Add(customer);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateCustomer(Customer customer)
        {
            _dbContext.customers.Update(customer);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _dbContext.customers.ToListAsync();
        }
    }
}
