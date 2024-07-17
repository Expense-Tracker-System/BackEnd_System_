using backend_dotnet7.Core.Dtos.General;

namespace backend_dotnet7.Core.Dtos.Message
{
    public class GetStartedDateAnsEndDateMessageDto : GeneralServiceResponseDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
