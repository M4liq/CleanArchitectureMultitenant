using System.Net;
using Application.Common.Interfaces.Core;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseResponse, new()
{
    private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;
    private readonly IDomainValidationHandler<TRequest> _validationHandler;
    
    public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TResponse>> logger,
        IDomainValidationHandler<TRequest> validationHandler)
    {
        _logger = logger;
        _validationHandler = validationHandler;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType();
        if (_validationHandler == null)
        {
            _logger.LogInformation("{Request} does not have a validation handler configured.", requestName);
            return await next();
        }

        var result = await _validationHandler.Validate(request);
        if (!result.IsSuccessful)
        {
            _logger.LogWarning("Validation failed for {Request}. Error: {Error}", requestName, result.ErrorMessages);
            return new TResponse {StatusCode = HttpStatusCode.BadRequest, Messages = result.ErrorMessages.ToList()};
        }

        _logger.LogInformation("Validation successful for {Request}.", requestName);
        return await next();
    }
}