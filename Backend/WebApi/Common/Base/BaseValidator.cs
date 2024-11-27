using Application.Common.Core;
using FastEndpoints;
namespace WebApi.Common.Base;

public abstract class BaseValidator<T> : Validator<T>
{
    protected readonly IRequestErrorManager ErrorService;

    protected BaseValidator(IRequestErrorManager errorService)
    {
        ErrorService = errorService;
    }

    protected string GetErrorMessage(IRequestError error)
    {
        return ErrorService.GetErrorMessage(error);
    }
}