using Moq;
using Xunit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Api.Services;
using ItemDashServer.Application.Services;
using ItemDashServer.Application.Users.Queries;
using ItemDashServer.Application.Users;
using Microsoft.Extensions.Configuration;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Api.Tests.Controllers;

public class AuthenticationControllerTest
{
    private readonly Mock<IMediator> _mediatorMock = new();
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
            _mediatorMock.Object,
            authService ?? CreateAuthService(),
            _loggerMock.Object,
            _rateLimiterMock.Object
        );
    }

    private void SetupMediatorForLogin(bool success, UserDto? user = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
        }
        else
        {
            var result = success && user != null ? Result<UserDto>.Success(user) : Result<UserDto>.Failure("Invalid credentials");
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
        }
    }

    private void SetupMediatorForRegister(UserDto? user = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
        }
        else
        {
            var result = user != null ? Result<UserDto>.Success(user) : Result<UserDto>.Failure("Registration failed");
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
        }
    }

    private void AssertUserDto(object? value, UserDto expected)
    {
        var user = Assert.IsType<UserDto>(value);
        Assert.Equal(expected.Id, user.Id);
        Assert.Equal(expected.Username, user.Username);
    }

    private static AuthenticationController.LoginRequest ValidLoginRequest =>
        new("testuser", "TestPass1!");

    private static UserDto GetUserDto() =>
        new() { Id = 1, Username = "testuser" };

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithTokenAndUser()
    {
        var userDto = GetUserDto();
        SetupMediatorForLogin(true, userDto);
        _rateLimiterMock.Setup(r => r.AllowAttemptAsync(It.IsAny<string>())).ReturnsAsync(true);
        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.False(string.IsNullOrWhiteSpace(response.Token));
        Assert.Equal(userDto.Id, response.User.Id);
        Assert.Equal(userDto.Username, response.User.Username);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        SetupMediatorForLogin(false);
        _rateLimiterMock.Setup(r => r.AllowAttemptAsync(It.IsAny<string>())).ReturnsAsync(true);
        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task Login_ExceptionThrown_ReturnsInternalServerError()
    {
        SetupMediatorForLogin(false, exception: new Exception("fail"));
        _rateLimiterMock.Setup(r => r.AllowAttemptAsync(It.IsAny<string>())).ReturnsAsync(true);
        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error", objectResult.Value);
    }

    [Fact]
    public async Task Login_InvalidModelState_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Username", "Required");

        var result = await controller.Login(new AuthenticationController.LoginRequest(string.Empty, "pass"));

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Register_Success_ReturnsOkWithUser()
    {
        var userDto = GetUserDto();
        SetupMediatorForRegister(userDto);
        var controller = CreateController();

        var result = await controller.Register(ValidLoginRequest);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<UserDto>(okResult.Value);

        Assert.Equal(userDto.Id, response.Id);
        Assert.Equal(userDto.Username, response.Username);
    }

    [Fact]
    public async Task Register_UsernameExists_ReturnsBadRequest()
    {
        var userDto = GetUserDto();
        SetupMediatorForRegister(userDto, new InvalidOperationException("Username already exists."));
        var controller = CreateController();
        var result = await controller.Register(ValidLoginRequest);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Username already exists.", badRequest.Value);
    }

    [Fact]
    public async Task Register_ExceptionThrown_ReturnsInternalServerError()
    {
        SetupMediatorForRegister(null, new Exception("fail"));
        var controller = CreateController();

        var result = await controller.Register(ValidLoginRequest);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error", objectResult.Value);
    }

    [Fact]
    public async Task Register_InvalidModelState_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Username", "Required");

        var result = await controller.Register(new AuthenticationController.LoginRequest(string.Empty, "pass"));

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}