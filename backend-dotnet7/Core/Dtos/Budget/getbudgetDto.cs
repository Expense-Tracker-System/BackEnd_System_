using backend_dotnet7.Core.Dtos.BExpense;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Dtos.Budget
{
    public class getbudgetDto
    {
        public int Id { get; set; }
        public string budgetName {  get; set; }
        public string budgetDescription{ get; set; }
        public double budgetAmount { get; set; }
        public double spent {  get; set; }
        public double remain {  get; set; }
        public List<BExpenseDto> expenses { get; set; }


    }
}
