using Microsoft.AspNetCore.Mvc;

namespace DepositWithdrawService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepositWithdrawController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<DepositWithdrawController> _logger;

        public DepositWithdrawController(ILogger<DepositWithdrawController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "Deposit")]
        public IEnumerable<WeatherForecast> Deposit()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
