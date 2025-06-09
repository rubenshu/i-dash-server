using Xunit;
using ItemDashServer.Api.Services;

namespace ItemDashServer.Api.Tests.Services;

public class AuthServiceTests
{
    [Theory]
    [InlineData("Password1!", true)]
    [InlineData("password", false)]
    [InlineData("PASSWORD1!", false)]
    [InlineData("Password", false)]
    [InlineData("Password1", false)]
    [InlineData("Pass1!", false)]
    [InlineData("Password!", false)]
    public void IsPasswordComplex_Works(string password, bool expected)
    {
        var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
        var service = new AuthService(config);
        Assert.Equal(expected, service.IsPasswordComplex(password));
    }
}
