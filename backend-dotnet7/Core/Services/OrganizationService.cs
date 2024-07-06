using AutoMapper;
using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Organization;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{

    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrganizationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OrganizationDto>> GetAllOrganizationsAsync()
        {
            var organizations = await _context.Organizations.ToListAsync();
            return _mapper.Map<List<OrganizationDto>>(organizations);
        }

        public async Task<List<Organization>> GetAllOrganizationsByUserIdAsync(string userId)
        {

    //        var organizations = await _context.ApplicationUsers
    //.Where(u => u.Id == userId)
    //.SelectMany(o => o.userOrganizations)
    //.ToListAsync();

            //var test = await _context.UserOrganizations.Where(uo => uo.Equals(userId)).Select(oraganizationList => new { id = Id, name = Name }).ToList();
            var test = await _context.UserOrganizations.Include(uo => uo.organization).Where(uo => uo.UserId==userId).Select(ou=>ou.organization).ToListAsync();

            //        var test = await _context.UserOrganizations.Where(o => o.UserId == userId).ToList();

            //        var organizations = await _context.UserOrganizations
            //.Where(uo => uo.UserId == userId)
            //.Select(uo => uo.)
            //.ToListAsync();

            return test;
        }

        //total balance service
        public async Task<OrganizationStatisticsDto> GetOrganizationStatisticsAsync(long organizationId, DataRangeDto dateRange)
        {
            var totalIncome = await _context.OrganizationIncomes
                .Where(i => i.OrganizationId == organizationId &&
                            i.CreatedAt >= dateRange.StartDate &&
                            i.CreatedAt <= dateRange.EndDate)
                .SumAsync(i => i.Amount);

            var totalExpenses = await _context.OrganizationExpenses
                .Where(e => e.OrganizationId == organizationId &&
                            e.CreatedAt >= dateRange.StartDate &&
                            e.CreatedAt <= dateRange.EndDate)
                .SumAsync(e => e.Amount);

            var totalBalance = totalIncome - totalExpenses;

            return new OrganizationStatisticsDto
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                TotalBalance = totalBalance
            };
        }
        // divide  into 
        public async Task<OrganizationBalanceDto> GetOrganizationBalanceWithSharesAsync(long organizationId)
        {
            var totalBalance = await GetOrganizationTotalBalanceAsync(organizationId);

            var userShares = await _context.UserOrganizations
                .Where(uo => uo.OrganizationId == organizationId)
                .Include(uo => uo.applicationUser)
                .Select(uo => new UserShareDto
                {
                    UserId = uo.UserId,
                    UserName = uo.applicationUser.UserName,
                    Position = uo.Position,
                    Shares = uo.shares,
                    Amount = 0 
                })
                .ToListAsync();

            var totalShares = userShares.Sum(us => us.Shares);

            if (totalShares > 0)
            {
                foreach (var userShare in userShares)
                {
                    userShare.Amount = (totalBalance * userShare.Shares) / totalShares;
                }
            }

            return new OrganizationBalanceDto
            {
                TotalBalance = totalBalance,
                UserShares = userShares
            };
        }

        private async Task<double> GetOrganizationTotalBalanceAsync(long organizationId)
        {
            var totalIncome = await _context.OrganizationIncomes
                .Where(i => i.OrganizationId == organizationId)
                .SumAsync(i => i.Amount);

            var totalExpenses = await _context.OrganizationExpenses
                .Where(e => e.OrganizationId == organizationId)
                .SumAsync(e => e.Amount);

            return totalIncome - totalExpenses;
        }
    }
}
