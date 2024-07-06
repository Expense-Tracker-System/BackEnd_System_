namespace backend_dotnet7.Core.Dtos.Organization
{
    public class OrganizationDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int MembersCount { get; set; }
        public double TotalTakeAmount { get; set; }
        public double TotalGetAmount { get; set; }
        public string? LeaderUserName { get; set; }
    }
}
