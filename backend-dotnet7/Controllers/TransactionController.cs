using System;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure endpoint requires authentication
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("AddTransaction")]

        public async Task<IActionResult> AddTransaction([FromBody] GetTransactionDto getTransactionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var transactionDto = new TransactionDto
                {
                    Id = getTransactionDto.Id,
                    Amount = getTransactionDto.Amount,
                    Description = getTransactionDto.Description,
                    userName = User.Identity.Name
                };

                var result = await _transactionService.AddTransaction(transactionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

       
        [HttpGet("GetTransactions")]

        public async Task<IActionResult> GetTransactions()
        {
                var userName = User.Identity.Name;
                var transactions =  _transactionService.GetTransactions(userName);
                return Ok(transactions);
            
        }

       
        [HttpDelete("DeleteTransaction")]

        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var userName = User.Identity.Name;
                var deleted = await _transactionService.DeleteTransaction(id, userName);
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

        
        [HttpPut("UpdateTransaction")]

        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDto transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userName = User.Identity.Name;

                var updatedTransaction = await _transactionService.UpdateTransaction(id, transaction, userName);
                if (updatedTransaction == null)
                {
                    return NotFound();
                }

                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
