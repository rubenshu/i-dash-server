using MediatR;

namespace ItemDashServer.Application.Users.Commands;

public record RefreshUserCommand(string RefreshToken) : IRequest<(bool Success, string? Token, string? RefreshToken, UserDto? User)>;