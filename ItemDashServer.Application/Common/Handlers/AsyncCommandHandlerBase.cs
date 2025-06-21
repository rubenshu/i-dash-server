using System.Diagnostics;
using System.Runtime.ExceptionServices;
using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Common.Handlers;

public abstract class AsyncCommandHandlerBase<TCommand, TResult>(ILogger logger, IUnitOfWork unitOfWork) : IAsyncCommandHandler<TCommand, TResult>
    where TCommand : class, ICommand
{
    private const int LONG_RUNNING_COMMAND_WARNING_IN_MS = 1000;

    protected bool DisableUnitOfWork { get; set; }
    public ILogger Logger { get; } = logger;
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    [DebuggerStepThrough]
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Debug(GetType(), "Entering Handle");
            var startedUnitOfWork = false;
            if (!DisableUnitOfWork)
            {
                // No Start() method, so just track that we want to commit/rollback
                startedUnitOfWork = true;
            }
            var result = await DoHandle(command, cancellationToken);
            if (startedUnitOfWork)
                await UnitOfWork.CommitAsync();
            Logger.Debug(GetType(), "Exiting Handle. Total time {0} ms", stopwatch.ElapsedMilliseconds);
            if (stopwatch.ElapsedMilliseconds > LONG_RUNNING_COMMAND_WARNING_IN_MS)
            {
                Logger.Warn(GetType(), "This command took a long time to complete. {0} ms.", stopwatch.ElapsedMilliseconds);
            }
            return result;
        }
        catch (Exception e)
        {
            if (!e.Data.Contains("Handler"))
                e.Data.Add("Handler", GetType().ToString());
            Logger.Error(GetType(), e, e.Message);
            if (!DisableUnitOfWork)
                UnitOfWork.Rollback();
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
        finally
        {
            UnitOfWork.Dispose();
        }
    }

    protected abstract Task<TResult> DoHandle(TCommand command, CancellationToken cancellationToken);
}

