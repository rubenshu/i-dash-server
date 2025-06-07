namespace ItemDashServer.Application.Users;

public interface IAuthService
{
    string GenerateJwtToken(int userId, string username);
    bool IsPasswordComplex(string password);
}