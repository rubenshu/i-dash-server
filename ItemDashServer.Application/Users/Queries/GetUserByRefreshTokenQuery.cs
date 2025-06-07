using MediatR;

namespace ItemDashServer.Application.Users.Queries;

public record GetUserByRefreshTokenQuery(string RefreshToken) : IRequest<UserDto?>;