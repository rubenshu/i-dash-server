namespace ItemDashServer.Application.Services;

public class InMemoryLoginRateLimiter : ILoginRateLimiter
{
    private static readonly Dictionary<string, (int Failures, DateTime LastAttempt)> _attempts = new();
    private const int MaxFailures = 5;
    private static readonly TimeSpan LockoutTime = TimeSpan.FromMinutes(5);

    public Task<bool> AllowAttemptAsync(string username)
    {
        lock (_attempts)
        {
            if (_attempts.TryGetValue(username, out var entry))
            {
                if (entry.Failures >= MaxFailures && DateTime.UtcNow - entry.LastAttempt < LockoutTime)
                    return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }

    public Task RegisterFailureAsync(string username)
    {
        lock (_attempts)
        {
            if (_attempts.TryGetValue(username, out var entry))
                _attempts[username] = (entry.Failures + 1, DateTime.UtcNow);
            else
                _attempts[username] = (1, DateTime.UtcNow);
        }
        return Task.CompletedTask;
    }

    public Task ResetFailuresAsync(string username)
    {
        lock (_attempts)
        {
            _attempts.Remove(username);
        }
        return Task.CompletedTask;
    }
}