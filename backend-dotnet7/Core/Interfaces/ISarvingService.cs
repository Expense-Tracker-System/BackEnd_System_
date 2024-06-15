using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IServingService
    {
        Task<SavingViewEntities> GetSarvingData(SavingViewEntities model);
    }
}
