using backend_dotnet7.Core.Dtos.Organization;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    public class OrganizationController : ControllerBase
    {
        public readonly ICreateOrganizationService _createOrganizationService;
        private readonly IOrganizationService _organizationService;
        public OrganizationController(ICreateOrganizationService createOrganizationService, IOrganizationService organizationService)
        {
            _createOrganizationService = createOrganizationService;
            _organizationService = organizationService;
        }

        [HttpPost]
        [Route("CreateOrganization")]
        [Authorize]
        public async Task<IActionResult> CreateOrganizationAsync([FromBody] CreateOrganizationDto createOrganizationDto)
        {
            var result = await _createOrganizationService.CreateOrganizationServiceAsync(createOrganizationDto, User.Identity.Name);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetOrganizations")]
        public async Task<ActionResult<List<OrganizationDto>>> GetOrganizations()
        {
            var organizations = await _organizationService.GetAllOrganizationsAsync();
            return Ok(organizations);
        }

        [HttpGet]
        [Route("GetUserOrganizations")]
        public async Task<ActionResult<List<UserOrganization>>> GetUserOrganizations(string userId)
        {
            var organizations = await _organizationService.GetAllOrganizationsByUserIdAsync(userId);
            return Ok(organizations);
        }

        //get total balance
        [HttpGet("{organizationId}/statistics")]
        public async Task<ActionResult<OrganizationStatisticsDto>> GetOrganizationStatistics(
      long organizationId,
      [FromQuery] DateTime startDate,
      [FromQuery] DateTime endDate)
        {
            var dateRange = new DataRangeDto
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var statistics = await _organizationService.GetOrganizationStatisticsAsync(organizationId, dateRange);
            return Ok(statistics);
        }

        //get shair 

        [HttpGet("{organizationId}/balance-with-shares")]
        public async Task<ActionResult<OrganizationBalanceDto>> GetOrganizationBalanceWithShares(long organizationId)
        {
            var result = await _organizationService.GetOrganizationBalanceWithSharesAsync(organizationId);
            return Ok(result);
        }
    }
}
