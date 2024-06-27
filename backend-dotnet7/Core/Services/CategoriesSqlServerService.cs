using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class CategoriesSqlServerService : ICategoryReposatory
    {
        private readonly ApplicationDbContext _context;

        public CategoriesSqlServerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Find(id);
        }

        public Category AddCategory(Category category)
        {

            _context.Categories.Add(category);
            _context.SaveChanges();

            return _context.Categories.Find(category.CategoryId);
        }
    }
}
