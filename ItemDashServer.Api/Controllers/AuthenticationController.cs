using AutoMapper;
using ItemDashServer.Application.Users;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using ItemDashServer.Application.Users.Queries;
using System.ComponentModel.DataAnnotations;
using ItemDashServer.Api.Services;

namespace ItemDashServer.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthenticationController(
    IMediator mediator,
    JwtTokenService jwtTokenService,
    ILogger<AuthenticationController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly JwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ILogger<AuthenticationController> _logger = logger;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var (success, userDto) = await _mediator.Send(new LoginUserQuery(request.Username, request.Password));
            if (!success || userDto == null)
                return Unauthorized();

            var token = _jwtTokenService.GenerateJwtToken(request.Username);
            return Ok(new LoginResponseDto { Token = token, User = userDto });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Username}", request.Username);
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userDto = await _mediator.Send(new RegisterUserCommand(request.Username, request.Password));
            return Ok(userDto);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed for user {Username}", request.Username);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user {Username}", request.Username);
            return StatusCode(500, "Internal server error");
        }
    }

    public record LoginRequest(
        [Required] string Username,
        [Required] string Password
    );
}