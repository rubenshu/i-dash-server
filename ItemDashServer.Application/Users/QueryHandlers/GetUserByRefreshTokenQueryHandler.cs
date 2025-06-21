using ItemDashServer.Application.Users.Repositories;
using AutoMapper;
using ItemDashServer.Application.Users.Queries;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Users.QueryHandlers;

public class GetUserByRefreshTokenQueryHandler(IUserRepository userRepository, IMapper mapper) : IGetUserByRefreshTokenQueryHandler
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<UserDto>> ExecuteAsync(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user == null) return Result<UserDto>.Failure("User not found for refresh token");
        return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
    }
}