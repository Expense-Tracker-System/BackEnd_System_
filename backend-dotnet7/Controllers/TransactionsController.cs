using AutoMapper;
using backend_dotnet7.Core.Dtos.Transaction;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/Category/{CategoryId}/transactions")]
    [ApiController]
  
        public class TransactionsController : ControllerBase
        {
            private readonly ITransactionReposatory _transactionService;
            private readonly IMapper _mapper;

            public TransactionsController(ITransactionReposatory transactionReposatory, IMapper mapper)
            {

                _transactionService = transactionReposatory;
                _mapper = mapper;
            }

            [HttpGet]
            public ActionResult<ICollection<TransactionDto>> GetTransactions(int CategoryId)
            {

                var transactions = _transactionService.AllTransaction(CategoryId);
                var mappedtransactions = _mapper.Map<ICollection<TransactionDto>>(transactions);
                return Ok(mappedtransactions);
            }
            [HttpGet("{id}", Name = "GetTransaction")]
            public ActionResult<TransactionDto> GetTransaction(int CategoryId, int id)
            {
                var transaction = _transactionService.GetTransaction(CategoryId, id);
                if (transaction == null) { return NotFound(); }

                var mappedtransaction = _mapper.Map<TransactionDto>(transaction);
                return Ok(mappedtransaction);
            }
            //get Transaction


            [HttpPost]
            public ActionResult<TransactionDto> CreateTransactions(int CategoryId, CreateTransactionDto transaction)
            {
                var transactionEntity = _mapper.Map<Transaction>(transaction);
                var newTransaction = _transactionService.AddTransaction(CategoryId, transactionEntity);
                var transactionReturn = _mapper.Map<TransactionDto>(newTransaction);


                return CreatedAtRoute("GetTransaction", new { CategoryId = CategoryId, id = transactionReturn.TransactionId },
                    transactionReturn);
            }

            [HttpPut]
            public IActionResult UpdateTransaction()
            {
                return Ok();
            }

            [HttpDelete]
            public IActionResult DeleteTransaction()
            {
                return Ok();
            }
        }
}
