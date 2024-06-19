using backend_dotnet7.Core.Dtos.BExpense;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BExpensesController : ControllerBase
    {
        private readonly IBExpenseService _bexpenseService;
        public BExpensesController(IBExpenseService bexpenseService)
        {
            _bexpenseService = bexpenseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BExpense>>> GetAllBExpenses()
        {
            var result = await _bexpenseService.GetAllBExpenses();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<BExpense>>> AddBExpense(BExpenseDto budget)
        {
            var result = await _bexpenseService.AddBExpense(budget);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<BExpense>>> UpdateBExpense(int id, BExpenseDto request)
        {
            var result = await _bexpenseService.UpdateBExpense(id, request);
            if (result is null)
                return NotFound("Expense not found.");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<BExpense>>> DeleteBExpense(int id)
        {
            var result = await _bexpenseService.DeleteBExpense(id);
            if (result is null)
                return NotFound("Expense not found.");
            return Ok(result);
        }


    }
}
