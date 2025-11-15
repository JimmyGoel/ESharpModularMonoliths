using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviour
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Start] Handling {RequestType} - Response {Request} requestData = {RequesstData} ",
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = Stopwatch.StartNew();
            var response = await next();
            timer.Stop();

            var timeTaken = timer.Elapsed.TotalSeconds;

            if (timeTaken > 3)
            {
                logger.LogWarning("[PERFORMANCE] {Request} took {TimeTaken}s to complete.",
                     typeof(TRequest).Name, timeTaken);
            }

            logger.LogInformation("[END] Handled {Request} => {Response} in {TimeTaken}s.",
               typeof(TRequest).Name, typeof(TResponse).Name, timeTaken);

            return response;
        }
    }
}
