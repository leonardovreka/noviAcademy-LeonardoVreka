using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Decorators;

public class LoggingRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly ILogger<LoggingRequestHandler<TRequest, TResponse>> _logger;

    public LoggingRequestHandler(IRequestHandler<TRequest, TResponse> inner, ILogger<LoggingRequestHandler<TRequest, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);
        var response = await _inner.Handle(request, cancellationToken);
        _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
        return response;
    }
}