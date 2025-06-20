using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class UpdateUserRefreshTokenCommandHandler(IUnitOfWork unitOfWork) : IUpdateUserRefreshTokenCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(UpdateUserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return Result<bool>.Failure("User not found");

        user.RefreshToken = request.RefreshToken;
        user.RefreshTokenExpiry = request.RefreshTokenExpiry;
        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}