using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Users.Commands;

public sealed record UpdateUserRefreshTokenCommand(
    int UserId,
    string? RefreshToken,
    DateTime? RefreshTokenExpiry
) : ICommand;