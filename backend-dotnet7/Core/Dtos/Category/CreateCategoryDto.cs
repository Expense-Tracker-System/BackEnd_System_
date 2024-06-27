namespace backend_dotnet7.Core.Dtos.Category
{
    public class CreateCategoryDto
    {
        public string Title { get; set; }


        public string Icon { get; set; } = "";


        public string Type { get; set; } = "Expense";

        public string? TitleWithIcon
        {
            get
            {
                return this.Icon + " " + this.Title;
            }
        }
    }
}
