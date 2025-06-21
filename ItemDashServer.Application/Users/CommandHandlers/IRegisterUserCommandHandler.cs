using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers
{
    public interface IRegisterUserCommandHandler : IAsyncCommandHandler<RegisterUserCommand, Result<UserDto>> { }
}
