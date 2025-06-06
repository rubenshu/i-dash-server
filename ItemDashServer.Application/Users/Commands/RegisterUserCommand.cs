using MediatR;

namespace ItemDashServer.Application.Users.Commands;

public record RegisterUserCommand(string Username, string Password) : IRequest<UserDto>;