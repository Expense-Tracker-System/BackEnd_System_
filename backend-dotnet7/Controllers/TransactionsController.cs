using AutoMapper;
using backend_dotnet7.Core.Dtos.Transactions;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace backend_dotnet7.Controllers
{
    [Route("api/categories/{CategoryId}/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionReposatory _transactionService;
       

        public TransactionsController(ITransactionReposatory reposatory)
        {
            _transactionService = reposatory;
           
        }
        [HttpGet]
        public ActionResult<ICollection<TransactionDto>> GetTransactions(int CategoryId) 
        {
            
            var transactions = _transactionService.AllTransaction(CategoryId);
            
            var transactionsDto = new List<TransactionDto>();

            foreach (var transaction in transactions) {
                transactionsDto.Add(new TransactionDto { 
                    Id = transaction.Id,
                    Amount = transaction.Amount,
                    Created = transaction.Created,
                    Note = transaction.Note,
                    CategoryId=transaction.CategoryId,
                   Status=transaction.Status,
                });
            }
           
            return Ok(transactionsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<TransactionDto> GetTransaction(int CategoryId, int id)
        {
            var transaction = _transactionService.GetTransaction(CategoryId,id);
            if (transaction == null) { return NotFound(); }
            var transactionDto = new TransactionDto();
            transactionDto.Id = transaction.Id;
            transactionDto.Amount = transaction.Amount;
            transactionDto.Created = transaction.Created;
            transactionDto.Note = transaction.Note;
            transactionDto.CategoryId = transaction.CategoryId;
            transactionDto.Status = transaction.Status;

            return Ok(transactionDto);
        }
        //Get Transactions
       
       


    }
}
