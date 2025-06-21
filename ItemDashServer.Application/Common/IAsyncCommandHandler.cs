namespace ItemDashServer.Application.Common
{
    public interface IAsyncCommandHandler<in TCommand, TResult> : IHandler where TCommand : class, ICommand
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}
