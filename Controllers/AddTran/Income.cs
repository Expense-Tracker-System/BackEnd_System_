using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.Entities.AddTrEntity; // Assuming there's an IncomeEntity class in this namespace
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddIncome([FromBody] IncomeEntity incomeEntity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _incomeService.AddIncome(incomeEntity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult GetIncomes()
        {
            try
            {
                var incomes = _incomeService.GetIncomes();
                return Ok(incomes);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeById(int id)
        {
            try
            {
                var income = await _incomeService.GetIncomeById(id);
                if (income == null)
                {
                    return NotFound();
                }

                return Ok(income);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteIncome(int id)
        {
            try
            {
                var deleted = _incomeService.DeleteIncome(id);
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
