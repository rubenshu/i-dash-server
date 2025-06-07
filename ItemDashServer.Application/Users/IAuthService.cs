namespace ItemDashServer.Application.Users;

public interface IAuthService
{
    string GenerateJwtToken(int userId, string username, string refreshToken);
}