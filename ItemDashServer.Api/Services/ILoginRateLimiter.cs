namespace ItemDashServer.Api.Services;

public interface ILoginRateLimiter
{
    Task<bool> AllowAttemptAsync(string username);
    Task RegisterFailureAsync(string username);
    Task ResetFailuresAsync(string username);
}