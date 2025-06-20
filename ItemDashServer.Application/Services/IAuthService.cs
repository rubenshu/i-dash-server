namespace ItemDashServer.Application.Services;

public interface IAuthService
{
    string GenerateJwtToken(int userId, string username, string role, List<string> rights);
    bool IsPasswordComplex(string password);
}