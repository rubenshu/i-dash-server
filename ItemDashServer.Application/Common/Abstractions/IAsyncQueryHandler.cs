namespace ItemDashServer.Application.Common.Abstractions;

public interface IAsyncQueryHandler<in TQuery, TResult> : IHandler where TQuery : class, IQuery
{
    Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken);
}

