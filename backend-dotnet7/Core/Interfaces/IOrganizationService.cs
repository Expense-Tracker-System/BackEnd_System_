using backend_dotnet7.Core.Dtos.Organization;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IOrganizationService
    {
       // Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync();
        Task<List<OrganizationDto>> GetAllOrganizationsAsync();
        public Task<List<Organization>> GetAllOrganizationsByUserIdAsync(string userId);
        //total balance
        Task<OrganizationStatisticsDto> GetOrganizationStatisticsAsync(long organizationId, DataRangeDto dateRange);
        //divide total into shaire
        Task<OrganizationBalanceDto> GetOrganizationBalanceWithSharesAsync(long organizationId);



    }
}
