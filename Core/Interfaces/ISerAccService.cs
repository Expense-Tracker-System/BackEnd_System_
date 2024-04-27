using backend_dotnet7.Core.Entities.CreateServing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IServingService
    {
        Task<ServingEntity> AddServing(ServingEntity serving);
        IEnumerable<ServingEntity> GetServings();
        Task<ServingEntity> GetServingById(int id);
        bool DeleteServing(int id);
    }
}
