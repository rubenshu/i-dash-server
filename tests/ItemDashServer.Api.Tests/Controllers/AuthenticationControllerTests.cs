using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Api.Services;
using ItemDashServer.Application.Services;
using ItemDashServer.Application.Users.Queries;
using ItemDashServer.Application.Users;
using Microsoft.Extensions.Configuration;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Users.QueryHandlers;
using ItemDashServer.Application.Users.CommandHandlers;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Api.Tests.Controllers;

public class AuthenticationControllerTest
{
    private readonly Mock<ILoginUserQueryHandler> _loginUserHandler = new();
    private readonly Mock<IRegisterUserCommandHandler> _registerUserHandler = new();
    private readonly Mock<ILogger<AuthenticationController>> _loggerMock = new();
    private readonly Mock<ILoginRateLimiter> _rateLimiterMock = new();

    private static AuthService CreateAuthService()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"JwtSettings:Secret", "supersecretkey1234567890supersecretkey1234567890"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"},
            {"JwtSettings:ExpiryInMinutes", "60"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
        return new AuthService(configuration);
    }
    private AuthenticationController CreateController(AuthService? authService = null)
    {
        return new AuthenticationController(
        );
    }

    private static AuthenticationController.LoginRequest ValidLoginRequest =>
        new("testuser", "TestPass1!");

    private static UserDto GetUserDto() =>
        new() { Id = 1, Username = "testuser", Role = "User", Rights = ["Read", "Write"] };

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithTokenAndUser()
    {
        var userDto = GetUserDto();
        _rateLimiterMock.Setup(r => r.AllowAttemptAsync(It.IsAny<string>())).ReturnsAsync(true);
        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest, _loginUserHandler.Object, default);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.False(string.IsNullOrWhiteSpace(response.Token));
        Assert.Equal(userDto.Id, response.User.Id);
        Assert.Equal(userDto.Username, response.User.Username);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        _rateLimiterMock.Setup(r => r.AllowAttemptAsync(It.IsAny<string>())).ReturnsAsync(true);
        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest, _loginUserHandler.Object, default);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Login_ExceptionThrown_ReturnsInternalServerError()
    {
        _loginUserHandler.Setup(h => h.ExecuteAsync(It.IsAny<LoginUserQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("fail"));
        _rateLimiterMock.Setup(r => r.AllowAttemptAsync(It.IsAny<string>())).ReturnsAsync(true);
        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest, _loginUserHandler.Object, default);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error", objectResult.Value);
    }

    [Fact]
    public async Task Login_InvalidModelState_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Username", "Required");

        var result = await controller.Login(new AuthenticationController.LoginRequest(string.Empty, "pass"), _loginUserHandler.Object, default);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Register_Success_ReturnsOkWithUser()
    {
        var userDto = GetUserDto();
        _registerUserHandler.Setup(h => h.HandleAsync(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<UserDto>.Success(userDto));
        var controller = CreateController();

        var result = await controller.Register(ValidLoginRequest, _registerUserHandler.Object, default);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<UserDto>(okResult.Value);

        Assert.Equal(userDto.Id, response.Id);
        Assert.Equal(userDto.Username, response.Username);
    }

    [Fact]
    public async Task Register_UsernameExists_ReturnsBadRequest()
    {
        _registerUserHandler.Setup(h => h.HandleAsync(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Username already exists."));
        var controller = CreateController();
        var result = await controller.Register(ValidLoginRequest, _registerUserHandler.Object, default);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username already exists.", badRequest.Value);
    }

    [Fact]
    public async Task Register_ExceptionThrown_ReturnsInternalServerError()
    {
        _registerUserHandler.Setup(h => h.HandleAsync(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("fail"));
        var controller = CreateController();

        var result = await controller.Register(ValidLoginRequest, _registerUserHandler.Object, default);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error", objectResult.Value);
    }

    [Fact]
    public async Task Register_InvalidModelState_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Username", "Required");

        var result = await controller.Register(new AuthenticationController.LoginRequest(string.Empty, "pass"), _registerUserHandler.Object, default);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}