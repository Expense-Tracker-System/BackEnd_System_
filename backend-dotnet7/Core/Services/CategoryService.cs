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
       
        public List<Category> GetAllCategory() {
           
            
                return _context.categories.ToList();

        }

        public List<Category> GetAllCategory(string title, string search) {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(search)) 
            {
                return GetAllCategory();
            }
            var categoryCollection = _context.categories as IQueryable<Category>;

            if (!string.IsNullOrWhiteSpace(title)) {
                title = title.Trim();
                categoryCollection = categoryCollection.Where(c => c.Title == title);

            }
            if (string.IsNullOrWhiteSpace(search)) { 
                search = search.Trim();
                categoryCollection = categoryCollection.Where(c => c.Icon.Contains(search) || c.Title.Contains(search)) ;
            }
            

            return categoryCollection.ToList();
        }
        public Category GetCategory(int id)
        {
            return _context.categories.Find(id);
        }

        public Category AddCategory(Category category) {
           
            
                _context.categories.Add(category);
                _context.SaveChanges();

                return _context.categories.Find(category.Id);
            
        }
    }
}
