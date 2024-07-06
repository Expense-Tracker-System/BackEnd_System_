using backend_dotnet7.Core.Dtos.Auth;

namespace backend_dotnet7.Core.Dtos.Organization
{
    public class CreateOrganizationDto
    {
        public string? title { get; set; }
        public List<UserInfoResult>? users { get; set; }
    }
}
