using ItemDashServer.Domain.Entities;
using AutoMapper;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Services;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class RegisterUserCommandHandler(
    ILogger logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAuthService authService
) : AsyncCommandHandlerBase<RegisterUserCommand, Result<UserDto>>(logger, unitOfWork), IRegisterUserCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthService _authService = authService;

    protected override async Task<Result<UserDto>> DoHandle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (!_authService.IsPasswordComplex(request.Password))
            return Result<UserDto>.Failure("Password does not meet complexity requirements.");

        var existing = await _unitOfWork.Users.GetByUsernameAsync(request.Username, cancellationToken);
        if (existing != null)
            return Result<UserDto>.Failure("Username already exists.");

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var user = new User
        {
            Username = request.Username,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };
        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
    }
}