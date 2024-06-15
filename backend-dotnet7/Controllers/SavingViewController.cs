using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingViewController : ControllerBase
    {
        public readonly IServingService _srvingService;
        public SavingViewController(IServingService srvingService)
        {

            _srvingService = srvingService;

        }

        [HttpPost]

        public async Task<IActionResult> GetSarvingData(SavingViewEntities modle)
        {
            return await _srvingService.GetSarvingData(modle);
        }
    }
}
