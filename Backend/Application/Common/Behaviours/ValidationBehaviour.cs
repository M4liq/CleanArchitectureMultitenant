using System.Net;
using Application.Common.Core;
using Domain.Common.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseResponse, new()
{
    private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidationHandler<TRequest>> _validationHandlers;

    public ValidationBehaviour(
        ILogger<ValidationBehaviour<TRequest, TResponse>> logger,
        IEnumerable<IValidationHandler<TRequest>> validationHandlers)
    {
        _logger = logger;
        _validationHandlers = validationHandlers;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = request.GetType();
        var validationHandler = _validationHandlers.FirstOrDefault();

        if (validationHandler == null)
        {
            _logger.LogWarning("{Request} does not have a validation handler configured.", requestName);
            return await next();
        }

        var result = await validationHandler.Validate(request, ct);
        if (!result.IsSuccessful)
        {
            return new TResponse { StatusCode = HttpStatusCode.BadRequest, Messages = result.ErrorMessages.ToList() };
        }

        return await next();
    }
}