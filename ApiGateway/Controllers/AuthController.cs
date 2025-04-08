using Microsoft.AspNetCore.Mvc;
using Core.Settings;
using Core.Connector;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RabbitMQService _rabbitMQService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(RabbitMQService rabbitMQService, ILogger<AuthController> logger)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    [HttpPost("Login")]
    public IActionResult ProcessLogin([FromBody] object request)
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