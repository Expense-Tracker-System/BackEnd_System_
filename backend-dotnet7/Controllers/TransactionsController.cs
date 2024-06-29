using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace backend_dotnet7.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionReposatory _transactionService;

        public TransactionsController(ITransactionReposatory reposatory)
        {
            _transactionService = reposatory;
        }
        [HttpGet]
        public IActionResult GetTransactions() 
        {
            var trans = _transactionService.AllTransaction();
            
           
            return Ok(trans);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransaction(int id)
        {
            var transaction = _transactionService.GetTransaction(id);
            if (transaction == null) { return NotFound(); }
            return Ok(transaction);
        }
        //Get Transactions
       
       


    }
}
