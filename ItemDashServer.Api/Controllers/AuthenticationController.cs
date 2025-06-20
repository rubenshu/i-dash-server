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

            var result = await _mediator.Send(new LoginUserQuery(request.Username, request.Password, refreshToken));
            if (!result.IsSuccess || result.Value == null)
            {
                await _rateLimiter.RegisterFailureAsync(request.Username);
                return Unauthorized();
            }

            await _rateLimiter.ResetFailuresAsync(request.Username);

            var userDto = result.Value;
            var token = _authService.GenerateJwtToken(userDto.Id, userDto.Username, userDto.Role, userDto.Rights);

            var response = new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                User = userDto
            };
            return Ok(response);
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
            var result = await _mediator.Send(new RegisterUserCommand(request.Username, request.Password));
            if (!result.IsSuccess || result.Value == null)
                return BadRequest(result.Error ?? "Registration failed.");
            return Ok(result.Value);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed for user");
            return BadRequest(ex.Message);
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
            var userResult = await _mediator.Send(new GetUserByRefreshTokenQuery(request.RefreshToken));
            if (!userResult.IsSuccess || userResult.Value == null)
                return Ok();

            await _mediator.Send(new UpdateUserRefreshTokenCommand(userResult.Value.Id, null, null));
            _logger.LogInformation("User {UserId} logged out and refresh token invalidated.", userResult.Value.Id);
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
            var result = await _mediator.Send(new RefreshUserCommand(request.RefreshToken));
            if (!result.IsSuccess || result.Value == null)
                return Unauthorized();

            return Ok(new
            {
                Token = result.Value.Token,
                RefreshToken = result.Value.RefreshToken,
                User = result.Value.User
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