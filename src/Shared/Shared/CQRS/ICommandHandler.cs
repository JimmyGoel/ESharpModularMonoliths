using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CQRS
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
