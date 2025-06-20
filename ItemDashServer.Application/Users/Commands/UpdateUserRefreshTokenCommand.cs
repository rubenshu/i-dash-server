using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Commands;

public sealed record UpdateUserRefreshTokenCommand(
    int UserId,
    string? RefreshToken,
    DateTime? RefreshTokenExpiry
) : ICommand;