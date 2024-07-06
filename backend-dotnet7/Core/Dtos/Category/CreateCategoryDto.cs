using backend_dotnet7.Core.Dtos.Transactions;
using System.ComponentModel.DataAnnotations;


namespace backend_dotnet7.Core.Dtos.Category
{
    public class CreateCategoryDto
    {
      
        public string Title { get; set; }
        public string Icon { get; set; }

        public ICollection<CreateTransactionDto> Transactions { get; set; } = new List<CreateTransactionDto>();
        
    }
}
