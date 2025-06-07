using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;
using ItemDashServer.Application.Users.Queries;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Users.QueryHandlers;

public class GetUserByRefreshTokenQueryHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<GetUserByRefreshTokenQuery, UserDto?>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto?> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}