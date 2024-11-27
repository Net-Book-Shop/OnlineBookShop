using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Data;
using OnlineBookShop.Model;

namespace OnlineBookShop.Repository
{
    public class CategoryRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CategoryRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveCategory(Category category)
        {
            _dbContext.Category.Add(category);
            await _dbContext.SaveChangesAsync();
        }
        public async Task SaveSubCategory(SubCategory category)
        {
            _dbContext.SubCategory.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Category> FindCategoryByCode(string code)
        {
            return await _dbContext.Category.FirstOrDefaultAsync(x => x.CategoryCode == code);

        }
        public async Task<SubCategory> FindSubCategoryByCode(string code)
        {
            return await _dbContext.SubCategory.FirstOrDefaultAsync(x => x.SubCategoryCode == code);

        }

    }
}
