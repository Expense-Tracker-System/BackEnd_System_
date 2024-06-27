using AutoMapper;
using backend_dotnet7.Core.Dtos.Transaction;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Services.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {

            CreateMap<Transaction, TransactionDto>();
            CreateMap<CreateTransactionDto, Transaction>();
        }
    }
}
