using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class PrivilegeDetailsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PrivilegeDetailsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPrivilegeDetailsAsync(List<PrivilegeDetails> privilegeDetails)
        {
            _dbContext.privilegeDetails.AddRange(privilegeDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePrivilegeDetailsAsync(List<PrivilegeDetails> privilegeDetails)
        {
            foreach (var detail in privilegeDetails)
            {
                _dbContext.privilegeDetails.Update(detail);
            }
            await _dbContext.SaveChangesAsync();
        }


        public async Task<List<PrivilegeDetails>> FindPrivilegeDetailRoleIdAndPrivilageIDs(List<Guid> privilegeIds, Guid roleId)
        {
            return await _dbContext.privilegeDetails
                .Where(x => privilegeIds.Contains(x.PrivilegeId) && x.RoleId == roleId)
                .ToListAsync();
        }
        public async Task<List<PrivilegeDetails>> FindAllRoleWisePrivilageDetails( Guid roleId)
        {
            return await _dbContext.privilegeDetails
       .Include(pd => pd.Privilege) 
       .Where(pd => pd.RoleId == roleId)
       .ToListAsync();
        }


    }
}
