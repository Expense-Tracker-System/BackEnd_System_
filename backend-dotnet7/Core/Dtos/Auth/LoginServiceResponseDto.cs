using backend_dotnet7.Core.Dtos.General;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class LoginServiceResponseDto : GeneralServiceResponseDto
    {
        public string? NewToken { get; set; }

        //This would be return to the front
        public UserInfoResult? userInfo { get; set; }
        public bool is2FactorRequired { get; set; }
        public string? provider { get; set; }
    }
}
