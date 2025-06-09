using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Commands;

public record RegisterUserCommand(string Username, string Password) : IRequest<Result<UserDto>>;