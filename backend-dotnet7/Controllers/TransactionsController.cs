using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionReposatory _transactionService;

        public TransactionsController(ITransactionReposatory reposatory)
        {
            _transactionService = reposatory;
        }
        [HttpGet("{id?}")]
        public IActionResult GetTransactions(int? id) 
        {
            var trans = _transactionService.AllTransaction();
            if (id == null) { return Ok(trans); }
           var tran = trans.Where(x => x.Id == id);
            return Ok(tran);
        }
        //Get Transactions
       
       


    }
}
