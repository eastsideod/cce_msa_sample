using Microsoft.AspNetCore.Mvc;
using Core.Settings;
using Core.Connector;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly RabbitMQService _rabbitMQService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(RabbitMQService rabbitMQService, ILogger<AccountController> logger)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    [HttpPost("Deposit")]
    public IActionResult OnDepositRequested([FromBody] object request)
    {
        try
        {
            _rabbitMQService.PublishMessage(request);
            _logger.LogInformation("Request forwarded to RabbitMQ");
            return Ok(new { message = "Request forwarded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error forwarding request to RabbitMQ");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
    
    [HttpPost("Withdraw")]
    public IActionResult OnWithdrawRequested([FromBody] object request)
    {
        try
        {
            _rabbitMQService.PublishMessage(request);
            _logger.LogInformation("Request forwarded to RabbitMQ");
            return Ok(new { message = "Request forwarded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error forwarding request to RabbitMQ");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

}