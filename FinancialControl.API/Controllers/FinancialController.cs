using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly ILogger<FinancialController> _logger;

        public FinancialController(ILogger<FinancialController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            return Ok("Teste");
        }
    }
}
