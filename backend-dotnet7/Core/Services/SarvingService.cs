using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class SarvingService : IServingService

    {

        private readonly ApplicationDbContext _context;

        public SarvingService(ApplicationDbContext context)
        {
            _context=context;
        }
        public async Task<SavingViewEntities> GetSarvingData(SavingViewEntities model)
        {
            _context.SavingViewEntitiess.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

    }
}
