using backend_dotnet7.Core.Dtos.Budget;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet]
        public async Task<ActionResult<List<getbudgetDto>>> GetAllBudgets(string username)
        {
            var result = await _budgetService.GetAllBudgets(username);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetSingleBudget(int id)
        {
            var result = await _budgetService.GetSingleBudget(id);
            if (result is null)
                return NotFound("Budget not found.");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<Budget>>> AddBudget(BudgetDto budget)
        {
            var result = await _budgetService.AddBudget(budget, User);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Budget>>> UpdateBudget(int id, BudgetDto request)
        {
            var result = await _budgetService.UpdateBudget(id, request);
            if (result is null)
                return NotFound("Budget not found.");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Budget>>> DeleteBudget(int id)
        {
            var result = await _budgetService.DeleteBudget(id);
            if (result is null)
                return NotFound("Budget not found.");
            return Ok(result);
        }
    }
}
