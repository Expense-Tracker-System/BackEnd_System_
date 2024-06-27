using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ICategoryReposatory
    {
        public List<Category> GetAllCategories();

        public Category GetCategory(int id);

        public Category AddCategory(Category category);
    }
}
