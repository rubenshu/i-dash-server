using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Queries;

public record LoginUserQuery(string Username, string Password, string RefreshToken) : IRequest<Result<UserDto>>;