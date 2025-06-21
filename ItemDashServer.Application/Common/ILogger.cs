namespace ItemDashServer.Application.Common;

public interface ILogger
{
    void Debug(Type type, string message, params object[] args);
    void Warn(Type type, string message, params object[] args);
    void Info(Exception ex, string message);
    void Error(Type type, Exception ex, string message);
}