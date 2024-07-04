namespace backend_dotnet7.Core.Dtos.Organization
{
   
        public class OrganizationBalanceDto
        {
            public double TotalBalance { get; set; }
            public List<UserShareDto> UserShares { get; set; }
        }
    
}
