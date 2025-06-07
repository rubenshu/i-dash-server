using MediatR;

namespace ItemDashServer.Application.Users.Commands;

public record UpdateUserRefreshTokenCommand(int UserId, string? RefreshToken, DateTime? RefreshTokenExpiry) : IRequest;