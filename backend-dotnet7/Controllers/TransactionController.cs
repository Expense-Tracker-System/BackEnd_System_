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
        [Route("AddTransaction")]

        public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transactionDto)
        {
            var userName = User.Identity.Name;
            var transactions = await _transactionService.AddTransaction(userName, transactionDto);
            return Ok(transactions);
        }

       
        [HttpGet]
        [Route("GetTransactions")]

        public async Task<IActionResult> GetTransactions()
        {
                var userName = User.Identity.Name;
                var transactions =  _transactionService.GetTransactions(userName);
                return Ok(transactions);
            
        }

       
        [HttpDelete]
        [Route("DeleteTransaction/{id}")]

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

        
        [HttpPut]
        [Route("UpdateTransaction")]

        public async Task<IActionResult> UpdateTransaction( [FromBody] TransactionDto transaction)
        {
            

                var userName = User.Identity.Name;

                var updatedTransaction = await _transactionService.UpdateTransaction( transaction, userName);
                

                return Ok(updatedTransaction);
           
        }
        
    }
}
