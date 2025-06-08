namespace ItemDashServer.Application.Services;

public interface IAuthService
{
    string GenerateJwtToken(int userId, string username);
    bool IsPasswordComplex(string password);
}