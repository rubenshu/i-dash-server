using System;
using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Api.Services;

public class ConsoleLogger : ItemDashServer.Application.Common.Abstractions.ILogger
{
    public void Debug(Type type, string message, params object[] args) =>
        Console.WriteLine($"DEBUG [{type.Name}]: {string.Format(message, args)}");
    public void Warn(Type type, string message, params object[] args) =>
        Console.WriteLine($"WARN [{type.Name}]: {string.Format(message, args)}");
    public void Info(Exception ex, string message) =>
        Console.WriteLine($"INFO: {message} Exception: {ex}");
    public void Error(Type type, Exception ex, string message) =>
        Console.WriteLine($"ERROR [{type.Name}]: {message} Exception: {ex}");
}
