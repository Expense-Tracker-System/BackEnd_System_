using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Organization;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class CreateOrganizationService : ICreateOrganizationService
    {
        public readonly ApplicationDbContext _context;

        public CreateOrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOrganizationServiceAsync(CreateOrganizationDto createOrganizationDto, string userName)
        {
            // Create and add organization
            var organization = new Organization
            {
                Name = createOrganizationDto.title,
                MembersCount = createOrganizationDto.users.Count,
                TotalTakeAmount = 0,
                TotalGetAmount = 0,
                LeaderUsername = userName
            };

            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();

            // Add entries to UserOrganization table
            foreach (var user in createOrganizationDto.users)
            {
                var userOrganization = new UserOrganization
                {
                    OrganizationId = organization.Id,
                    UserId = user.Id
                };
                await _context.UserOrganizations.AddAsync(userOrganization);
            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
    
}
