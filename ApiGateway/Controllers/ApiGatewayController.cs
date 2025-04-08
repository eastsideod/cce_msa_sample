using Microsoft.AspNetCore.Mvc;
using Core.Settings;
using Core.Connector;
using Core.Services.Rpc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiGatewayController : ControllerBase
{
    private readonly RpcServerService _rpcService;
    private readonly ILogger<ApiGatewayController> _logger;

    public ApiGatewayController(RpcServerService rabbitMQService, ILogger<ApiGatewayController> logger)
    {
        _rpcService = rabbitMQService;
        _logger = logger;
    }

    [HttpPost("forward")]
    public IActionResult ForwardRequest([FromBody] object request)
    {
        try
        {
            _rpcService.PublishMessage(request);
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