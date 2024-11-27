using System.Net;
using Domain.Common.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Common.Models;

namespace WebApi.Common.Filters;

public class ResponseMappingFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult cqrsResult && cqrsResult.Value is BaseResponse cqrsResponse && cqrsResponse.StatusCode != HttpStatusCode.OK)
        {
            context.Result = new ObjectResult(new
                {messages = cqrsResponse.Messages, StatusCode = (int) cqrsResponse.StatusCode});
        }

        if (context.Result is ObjectResult modelValidationResult &&
            modelValidationResult.Value is ModelStateErrorResponse modelStateErrorResponse)
        {
            context.Result = new ObjectResult(new
                {messages = modelStateErrorResponse.Errors, StatusCode = (int) HttpStatusCode.BadRequest});
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
}