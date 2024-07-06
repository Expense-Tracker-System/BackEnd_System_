using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialReportController : ControllerBase
    {
        private readonly IFinancialService _financialService;

        public FinancialReportController(IFinancialService financialService)
        {
            _financialService = financialService;
        }

        [HttpGet("GetFinancialData")]
        public async Task<IActionResult> GetFinancialData([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var financialData = await _financialService.GetFinancialDataAsync(startDate, endDate);
                return Ok(financialData);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching the financial data: {ex.Message}");
            }
        }
    }
}
