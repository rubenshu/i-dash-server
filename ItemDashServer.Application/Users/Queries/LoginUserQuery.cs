using MediatR;

namespace ItemDashServer.Application.Users.Queries;

public record LoginUserQuery(string Username, string Password) : IRequest<(bool Success, UserDto? User)>;