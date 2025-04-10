using Common.Session;

namespace SessionService.Service;

public class SessionMapService
{
    private object _lock = new();

    private readonly Dictionary<long, UserSession> _idSessionMap = new();
    
    private readonly Dictionary<string, UserSession> _deviceIdSessionMap = new();


    public Task<UserSession> Login(
        long accountId, string deviceId)
    {
        var userSession = new UserSession
        {
            Id = accountId,
            DeviceId = deviceId,
            LastReceivedAt = DateTime.UtcNow,
        };

        lock (_lock)
        {
            if (_idSessionMap.Remove(accountId, out var session))
            {
                _deviceIdSessionMap.Remove(session.DeviceId);
            }

            _idSessionMap[accountId] = userSession;
            _deviceIdSessionMap[deviceId] = userSession;
        }

        return Task.FromResult(userSession);
    }

    public Task Logout(long accountId)
    {
        lock (_lock)
        {
            if (_idSessionMap.Remove(accountId, out var session))
            {
                _deviceIdSessionMap.Remove(session.DeviceId);
            }
        }

        return Task.CompletedTask;
    }
}
