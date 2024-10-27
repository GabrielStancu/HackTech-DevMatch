using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Core.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest: notnull, IRequest<TResponse>
    where TResponse: notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[START] Handle Request = {Request} - Response = {Response} - RequestData = {RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();
        var timeTaken = timer.Elapsed;

        if (timeTaken.Seconds > 3)
        {
            _logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds", typeof(TRequest).Name, timeTaken.Seconds);
        }

        _logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, response);

        return response;
    }
}
