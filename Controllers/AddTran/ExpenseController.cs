using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.Entities.AddTrEntity;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenceService _expenseService;

        public ExpenseController(IExpenceService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseEntity expense)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _expenseService.AddExpense(expense);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult GetExpenses()
        {
            try
            {
                var expenses = _expenseService.GetExpenses();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            try
            {
                var expense = await _expenseService.GetExpenseById(id);
                if (expense == null)
                {
                    return NotFound();
                }

                return Ok(expense);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(int id)
        {
            try
            {
                var deleted = _expenseService.DeleteExpense(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
