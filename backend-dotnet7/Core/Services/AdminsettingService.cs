using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend_dotnet7.Core.Services
{
    public class AdminsettingService : IAdminsettingService
    {
        protected readonly ApplicationDbContext dbContext;
        public AdminsettingService (ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<RegisterDto>> GetAll(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var users = await dbContext.Users
                .Where(q => q.Id == userId)
                .Select(q => new RegisterDto()
                {
                    FirstName= q.FirstName,
                    LastName = q.LastName,
                    Username = q.UserName,
                    Email = q.Email,
                    PhoneNumber = q.PhoneNumber,
                })
                .ToListAsync();

            return users;
        }
        public async Task<bool> UpdateUser(RegisterDto request , ClaimsPrincipal users)
        {
            var userId = users.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await dbContext.Users.FirstOrDefaultAsync(q => q.Id == userId);

            if (user is null)  throw new Exception("User not found");

            if (user.UserName == null) { user.UserName = request.Username; }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
