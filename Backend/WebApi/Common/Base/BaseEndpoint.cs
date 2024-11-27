using Domain.Common.Base;
using FastEndpoints;

namespace WebApi.Common.Base;

public abstract class BaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : BaseResponse, new()
{
    public override void Configure()
    {
        AllowAnonymous();
    }

    protected IResult ApiResponse(TResponse response)
    {
        return Results.Json(response, statusCode: (int)response.StatusCode);
    }
}