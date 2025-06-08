using ItemDashServer.Application.Users;
using ItemDashServer.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ItemDashServer.Application.Users.Queries;
using System.ComponentModel.DataAnnotations;
using ItemDashServer.Application.Users.Commands;
using System.Security.Cryptography;
using ItemDashServer.Api.Services;

namespace ItemDashServer.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthenticationController(
    IMediator mediator,
    IAuthService authService,
    ILogger<AuthenticationController> logger,
    ILoginRateLimiter rateLimiter) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthenticationController> _logger = logger;
    private readonly ILoginRateLimiter _rateLimiter = rateLimiter;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _rateLimiter.AllowAttemptAsync(request.Username))
            return StatusCode(429, "Too many login attempts. Please try again later.");

        try
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshExpiry = DateTime.UtcNow.AddDays(7);

            var (success, userDto) = await _mediator.Send(new LoginUserQuery(request.Username, request.Password, refreshToken));
            if (!success || userDto == null)
            {
                await _rateLimiter.RegisterFailureAsync(request.Username);
                return Unauthorized();
            }

            await _rateLimiter.ResetFailuresAsync(request.Username);

            var token = _authService.GenerateJwtToken(userDto.Id, userDto.Username);

            // For local/dev, return refresh token in response (in production, use HttpOnly cookie)
            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken,
                User = userDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_authService.IsPasswordComplex(request.Password))
            return BadRequest("Password does not meet complexity requirements.");

        try
        {
            var userDto = await _mediator.Send(new RegisterUserCommand(request.Username, request.Password));
            return Ok(userDto);
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Registration failed for user");
            return BadRequest("Registration failed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest("Refresh token required.");

        try
        {
            var user = await _mediator.Send(new GetUserByRefreshTokenQuery(request.RefreshToken));
            if (user == null)
                return Ok();

            await _mediator.Send(new UpdateUserRefreshTokenCommand(user.Id, null, null));
            _logger.LogInformation("User {UserId} logged out and refresh token invalidated.", user.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseDto>> Refresh([FromBody] RefreshRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest("Refresh token required.");

        try
        {
            var (success, token, refreshToken, userDto) = await _mediator.Send(new RefreshUserCommand(request.RefreshToken));
            if (!success || userDto == null)
                return Unauthorized();

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken,
                User = userDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, "Internal server error");
        }
    }

    public record LoginRequest(
        [Required] string Username,
        [Required] string Password
    );
    public record RefreshRequest(string RefreshToken);
}