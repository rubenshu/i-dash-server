using MediatR;

namespace ItemDashServer.Application.Users;

public record RegisterUserCommand(string Username, string Password) : IRequest<UserDto>;