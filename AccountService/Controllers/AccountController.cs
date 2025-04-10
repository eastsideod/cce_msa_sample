using AccountService.AccountLock;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers;

[ApiController]
public class AccountController
{
    private readonly ILogger<AccountController> _logger;
    private readonly AccountLockService _accountLockService;

    public AccountController(
        ILogger<AccountController> logger, 
        AccountLockService accountLockService)
    {
        _logger = logger;
        _accountLockService = accountLockService;
    }

    [Route("/Account/Lock/{id?}")]
    [HttpGet]
    public Task<string> LockAccount(int accountId)
    {
        TaskCompletionSource<string> tcs = new ();

        _accountLockService.LockAsync(accountId, () =>
        {
            tcs.SetResult("ok");
        });

        return tcs.Task;
    }

    [Route("/Account/Unlock/{id?}")]
    [HttpGet]
    public string UnlockAccount(int accountId)
    {
        _accountLockService.Unlock(accountId);

        return "ok";
    }
}
