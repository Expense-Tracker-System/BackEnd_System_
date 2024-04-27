namespace backend_dotnet7.Core.Entities.CreateServing
{
    public class ServingEntity
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string ServingType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
