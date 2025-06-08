using MediatR;
using ItemDashServer.Application.Users.Repositories;
using AutoMapper;
using ItemDashServer.Application.Users.Queries;

namespace ItemDashServer.Application.Users.QueryHandlers;

public class GetUserByRefreshTokenQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByRefreshTokenQuery, UserDto?>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto?> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}