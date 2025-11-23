using MediatR;

namespace Shared.Contract.CQRS
{
    public interface ICommandHandler<in TCommand>
        : ICommandHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>
    { }
    public interface ICommandHandler<in TCommand, TResonse>
        : IRequestHandler<TCommand, TResonse>
        where TCommand : ICommand<TResonse>
        where TResonse : notnull
    { }
}
