using MediatR;

namespace ItemDashServer.Application.Users.Queries;

public record LoginUserQuery(string Username, string Password, string RefreshToken) : IRequest<(bool Success, UserDto? User)>;