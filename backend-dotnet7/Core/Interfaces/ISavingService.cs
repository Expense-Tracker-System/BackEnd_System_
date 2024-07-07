using backend_dotnet7.Core.Dtos;
using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ISavingService
    {
        Task<SavingViewEntities> PostSarvingDetails(SavingViewEntities model,string userName);
        public Task<List<savingViewDTO>> GetSavingDetails(string userName, savingViewrequestDTO request);

    }
}
