using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class CategoryService : ICategoryReposatory
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Category> GetAllCategory()
        {
            return _context.categories.ToList();

        }
        public Category GetCategory(int id)
        {
            return _context.categories.Find(id);
        }
    }
}
