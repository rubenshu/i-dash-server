using Xunit;
using ItemDashServer.Application.Services;

namespace ItemDashServer.Api.Tests.Services;

public class InMemoryLoginRateLimiterTests
{
    [Fact]
    public async Task AllowAttemptAsync_AllowsFirstAttempts()
    {
        var limiter = new InMemoryLoginRateLimiter();
        for (int i = 0; i < 5; i++)
        {
            var allowed = await limiter.AllowAttemptAsync("user");
            Assert.True(allowed);
            await limiter.RegisterFailureAsync("user");
        }
    }

    [Fact]
    public async Task AllowAttemptAsync_DeniesAfterMaxFailures()
    {
        var limiter = new InMemoryLoginRateLimiter();
        for (int i = 0; i < 5; i++)
            await limiter.RegisterFailureAsync("user");
        var allowed = await limiter.AllowAttemptAsync("user");
        Assert.False(allowed);
    }

    [Fact]
    public async Task ResetFailuresAsync_AllowsAfterReset()
    {
        var limiter = new InMemoryLoginRateLimiter();
        for (int i = 0; i < 5; i++)
            await limiter.RegisterFailureAsync("user");
        await limiter.ResetFailuresAsync("user");
        var allowed = await limiter.AllowAttemptAsync("user");
        Assert.True(allowed);
    }
}
