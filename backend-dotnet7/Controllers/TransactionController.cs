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

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userName = User.Identity.Name;
                //transaction.getLogDto.UserName = userName;

                var result = await _transactionService.AddTransaction(transaction, userName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult GetTransactions()
        {
            try
            {
                var userName = User.Identity.Name;
                var transactions = _transactionService.GetTransactions(userName);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
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

        [HttpPut("{id}")]
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
