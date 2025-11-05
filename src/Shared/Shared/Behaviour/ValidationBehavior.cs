using FluentValidation;
using MediatR;
using Shared.CQRS;

namespace Shared.Behaviour
{
    public class ValidationBehavior<TRquest, TResponse>
        (IEnumerable<IValidator<TRquest>> validators)
        : IPipelineBehavior<TRquest, TResponse>
        where TRquest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRquest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRquest>(request);
            var validationResults = await Task.WhenAll(validators
                .Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
