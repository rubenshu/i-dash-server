using MediatR;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class RegisterUserCommandHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Users.Any(u => u.Username == request.Username))
            throw new InvalidOperationException("Username already exists.");

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var user = new User
        {
            Username = request.Username,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}