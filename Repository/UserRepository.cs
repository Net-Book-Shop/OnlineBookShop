using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Dto;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class UserRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public UserRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> FindByUserName(string userName)
        {
            return await _dbContext.users.FirstOrDefaultAsync(x => x.UserName == userName);

        }

        public async Task SaveUser(User user)
        {
            _dbContext.users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetRoleWiseAllUsers(string role)
        {
            return await _dbContext.users
         .Where(u => u.Role == role)
         .ToListAsync();
        }
        public async Task<User?> GetLastUserAsync()
        {
            return await _dbContext.users.OrderByDescending(u => u.Id).FirstOrDefaultAsync();
        }


    }
}
