using MediatR;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;

namespace ItemDashServer.Application.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

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