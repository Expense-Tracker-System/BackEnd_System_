using backend_dotnet7.Core.Entities.CreateServing;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Services
{
    public class ServingService : IServingService
    {
        private readonly ApplicationDbContext _context;

        public ServingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServingEntity> AddServing(ServingEntity serving)
        {
            try
            {
                _context.Servings.Add(serving);
                await _context.SaveChangesAsync();
                return serving;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // For simplicity, rethrow the exception
                throw;
            }
        }

        public IEnumerable<ServingEntity> GetServings()
        {
            return _context.Servings.ToList();
        }

        public bool DeleteServing(int id)
        {
            var serving = _context.Servings.Find(id);
            if (serving != null)
            {
                _context.Servings.Remove(serving);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<ServingEntity> GetServingById(int id)
        {
            return await _context.Servings.FindAsync(id);
        }
    }
}
