using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Common;
using AutoMapper;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class UpdateUserRefreshTokenCommandHandler(ILogger logger, IUnitOfWork unitOfWork) : AsyncCommandHandlerBase<UpdateUserRefreshTokenCommand, Result<bool>>(logger, unitOfWork), IUpdateUserRefreshTokenCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    protected override async Task<Result<bool>> DoHandle(UpdateUserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return Result<bool>.Failure("User not found");

        user.RefreshToken = request.RefreshToken;
        user.RefreshTokenExpiry = request.RefreshTokenExpiry;
        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        try
        {
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
        return Result<bool>.Success(true);
    }
}