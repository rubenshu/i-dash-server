using System.Diagnostics;
using System.Runtime.ExceptionServices;
using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Common.Handlers;

public abstract class AsyncQueryHandlerBase<TQuery, TResult>(ILogger logger) : IAsyncQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery
{
    private const int LONG_RUNNING_QUERY_WARNING_IN_MS = 1000;

    public ILogger Logger { get; } = logger;

    [DebuggerStepThrough]
    public async Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Debug(GetType(), "Entering Execute");
            var result = await DoExecute(query, cancellationToken);
            Logger.Debug(GetType(), "Exiting Execute. Total time {0} ms", stopwatch.ElapsedMilliseconds);
            if (stopwatch.ElapsedMilliseconds > LONG_RUNNING_QUERY_WARNING_IN_MS)
            {
                Logger.Warn(GetType(), "This query took a long time to complete. {0} ms.", stopwatch.ElapsedMilliseconds);
            }
            return result;
        }
        catch (Exception e)
        {
            switch (e)
            {
                case TaskCanceledException:
                    Logger.Info(e, e.Message);
                    return default!;
                case UnauthorizedAccessException:
                    break;
                default:
                    if (!e.Data.Contains("Handler"))
                        e.Data.Add("Handler", GetType().ToString());
                    Logger.Error(GetType(), e, e.Message);
                    break;
            }
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    protected abstract Task<TResult> DoExecute(TQuery query, CancellationToken cancellationToken);
}
