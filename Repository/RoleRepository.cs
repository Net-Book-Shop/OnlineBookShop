using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class RoleRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public RoleRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Roles> FindByRoleName(string roleName)
        {
            return await _dbContext.roles.FirstOrDefaultAsync(x => x.Name == roleName);

        }
    }
}
