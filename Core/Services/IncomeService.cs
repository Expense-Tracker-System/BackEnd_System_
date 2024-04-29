using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities.AddTrEntity;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly ApplicationDbContext _context;

        public IncomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IncomeEntity> AddIncome(IncomeEntity income)
        {
            _context.IncomeEntitys.Add(income);
            await _context.SaveChangesAsync();
            return income;
        }

        public IEnumerable<IncomeEntity> GetIncome()
        {
            return _context.IncomeEntitys.ToList();
        }

        public bool DeleteIncome(int id)
        {
            var income = _context.IncomeEntitys.Find(id);
            if (income != null)
            {
                _context.IncomeEntitys.Remove(income);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<IncomeEntity> GetIncomeById(int id)
        {
            return await _context.IncomeEntitys.FindAsync(id);
        }
    }
}
