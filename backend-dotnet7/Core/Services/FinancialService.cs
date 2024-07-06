using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Report;
using Microsoft.EntityFrameworkCore;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly ApplicationDbContext _context;

        public FinancialService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FinancialDataDTO> GetFinancialDataAsync(DateTime startDate, DateTime endDate)
        {
            var expenses = await _context.UserExpenses
                .Where(e => e.CreatedDate2 >= startDate && e.CreatedDate2 <= endDate)
                .ToListAsync();

            var incomes = await _context.UserIncomes
                .Where(i => i.CreatedDate2 >= startDate && i.CreatedDate2 <= endDate)
                .ToListAsync();

            var financialData = new FinancialDataDTO
            {
                TotalExpenses = expenses.Sum(e => e.Amount),
                TotalIncomes = incomes.Sum(i => i.Amount),
                TotalBalance = incomes.Sum(i => i.Amount) - expenses.Sum(e => e.Amount),
                Expenses = expenses.Select(e => new ExpenseDTO
                {
                    Amount = e.Amount,
                    Description = e.Description,
                    Category = e.Category,
                    CreatedDate2 = e.CreatedDate2
                }).ToList(),
                Incomes = incomes.Select(i => new IncomeDTO
                {
                    Amount = i.Amount,
                    Description = i.Description,
                    Source = i.Source,
                    Category = i.Category,
                    CreatedDate2 = i.CreatedDate2
                }).ToList()
            };

            return financialData;
        }
    }
}
