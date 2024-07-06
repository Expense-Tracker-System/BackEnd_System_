using backend_dotnet7.Core.Dtos.Organization;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ICreateOrganizationService
    {
        Task<bool> CreateOrganizationServiceAsync(CreateOrganizationDto createOrganizationDto, string userName);
    }
}
