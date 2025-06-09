using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Commands;

public record UpdateUserRefreshTokenCommand(int UserId, string? RefreshToken, DateTime? RefreshTokenExpiry) : IRequest<Result<bool>>;