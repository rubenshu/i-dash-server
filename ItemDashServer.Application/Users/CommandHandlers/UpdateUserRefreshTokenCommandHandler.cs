using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class UpdateUserRefreshTokenCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<UpdateUserRefreshTokenCommand>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task Handle(UpdateUserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(u => u.Id == request.UserId);
        if (user != null)
        {
            user.RefreshToken = request.RefreshToken;
            user.RefreshTokenExpiry = request.RefreshTokenExpiry;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}