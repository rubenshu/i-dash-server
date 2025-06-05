using Xunit;
using Moq;
using Moq.Async;
using MediatR;
using Moq.Language.Flow;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Api.Services;
using ItemDashServer.Application.Users.Queries;
using ItemDashServer.Application.Users;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Configuration;

namespace ItemDashServer.Api.Tests;

public class AuthenticationControllerTest
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<ILogger<AuthenticationController>> _loggerMock = new();

    private static JwtTokenService CreateJwtTokenService()
    {
        var inMemorySettings = new Dictionary<string, string?>
    {
        {"JwtSettings:Secret", "supersecretkey1234567890supersecretkey1234567890"},
        {"JwtSettings:Issuer", "TestIssuer"},
        {"JwtSettings:Audience", "TestAudience"},
        {"JwtSettings:ExpiryInMinutes", "60"}
    };
        var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        return new JwtTokenService(configuration);
    }

    private AuthenticationController CreateController(JwtTokenService? jwtTokenService = null)
    {
        return new AuthenticationController(
            _mediatorMock.Object,
            jwtTokenService ?? CreateJwtTokenService(),
            _loggerMock.Object
        );
    }

    private static AuthenticationController.LoginRequest ValidLoginRequest =>
        new("testuser", "testpass");

    private static UserDto GetUserDto() =>
        new() { Id = 1, Username = "testuser" };

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithTokenAndUserr()
    {
        var userDto = GetUserDto();
        _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<(bool Success, UserDto? User)>((true, userDto)));

        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        dynamic response = okResult.Value!;
        Assert.False(string.IsNullOrWhiteSpace((string)response.Token));
        Assert.Equal(userDto.Id, (int)response.User.Id);
        Assert.Equal(userDto.Username, (string)response.User.Username);
    }

    [Fact]
    public void JwtTokenService_GeneratesToken()
    {
        var service = CreateJwtTokenService();
        var token = service.GenerateJwtToken("testuser");
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null));

        var controller = CreateController();

        var result = await controller.Login(ValidLoginRequest);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task Login_ExceptionThrown_ReturnsInternalServerError()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("fail"));

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
        _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userDto);

        var controller = CreateController();

        var result = await controller.Register(ValidLoginRequest);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userDto.Id, returnedUser.Id);
        Assert.Equal(userDto.Username, returnedUser.Username);
    }

    [Fact]
    public async Task Register_UsernameExists_ReturnsBadRequest()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Username already exists."));

        var controller = CreateController();

        var result = await controller.Register(ValidLoginRequest);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Username already exists.", badRequest.Value);
    }

    [Fact]
    public async Task Register_ExceptionThrown_ReturnsInternalServerError()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("fail"));

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

    // Minimal DTO for test context
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public required UserDto User { get; set; }
    }
}