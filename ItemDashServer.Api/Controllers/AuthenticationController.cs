using ItemDashServer.Application.Users.QueryHandlers;
using ItemDashServer.Application.Users.CommandHandlers;
using ItemDashServer.Application.Users.Queries;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ItemDashServer.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthenticationController()
 : ControllerBase
{

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] ILoginUserQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new LoginUserQuery(request.Username, request.Password, null);
        return Ok(await handler.ExecuteAsync(query, cancellationToken));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] LoginRequest request,
        [FromServices] IRegisterUserCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Username, request.Password);
        return Ok(await handler.HandleAsync(command, cancellationToken));
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshRequest request,
        [FromServices] IGetUserByRefreshTokenQueryHandler getUserHandler,
        [FromServices] IUpdateUserRefreshTokenCommandHandler updateUserHandler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByRefreshTokenQuery(request.RefreshToken);
        var userResult = await getUserHandler.ExecuteAsync(query, cancellationToken);
        var command = new UpdateUserRefreshTokenCommand(userResult is { Value: { Id: var id } } ? id : 0, null, null);
        return Ok(await updateUserHandler.HandleAsync(command, cancellationToken));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshRequest request,
        [FromServices] IRefreshUserCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RefreshUserCommand(request.RefreshToken);
        return Ok(await handler.HandleAsync(command, cancellationToken));
    }

    public record LoginRequest(
        [Required] string Username,
        [Required] string Password
    );
    public record RefreshRequest(string RefreshToken);
}