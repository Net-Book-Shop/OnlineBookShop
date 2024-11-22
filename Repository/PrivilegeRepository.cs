using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class PrivilegeRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PrivilegeRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPrivilage(Privilege privilege)
        {
            _dbContext.privileges.Add(privilege);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Privilege>> FindPrivilegesByIdsAsync(List<Guid> ids)
        {
            return await _dbContext.privileges
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<List<Privilege>> FindAllPrivilages()
        {
            return await _dbContext.privileges.ToListAsync();
        }
      


    }
}
