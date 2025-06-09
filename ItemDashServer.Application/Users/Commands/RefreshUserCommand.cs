using MediatR;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Users.CommandHandlers; // For RefreshUserResultDto

namespace ItemDashServer.Application.Users.Commands;

public record RefreshUserCommand(string RefreshToken) : IRequest<Result<RefreshUserResultDto>>;