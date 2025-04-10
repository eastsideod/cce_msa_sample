
using Microsoft.AspNetCore.Mvc;
using SessionService.Service;

namespace AccountService.Controllers;

[ApiController]
public class SessionController
{
    private readonly ILogger<SessionController> _logger;
    private readonly SessionMapService _sessionMapService;

    public SessionController(
        ILogger<SessionController> logger,
        SessionMapService sessionMapService)
    {
        _logger = logger;
        _sessionMapService = sessionMapService;
    }

    [Route("/Login/{id?}/{deviceId?}")]
    [HttpGet]
    public async Task<string> Login(long accountId, string deviceId)
    {
        await _sessionMapService.Login(accountId, deviceId);

        return "ok";
    }

    [Route("/Logout/{id?}")]
    [HttpGet]
    public async Task<string> UnlockAccount(long accountId)
    {
        await _sessionMapService.Logout(accountId);

        return "ok";
    }
}
