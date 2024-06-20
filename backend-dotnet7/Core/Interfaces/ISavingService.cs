using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ISavingService
    {
        Task<SavingViewEntities> GetSarvingData(SavingViewEntities model);
    }
}
