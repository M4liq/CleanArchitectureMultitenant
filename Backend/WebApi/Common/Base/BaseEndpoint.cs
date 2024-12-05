using System.Net;
using Domain.Common.Base;
using FastEndpoints;

namespace WebApi.Common.Base;

public abstract class BaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : BaseResponse, new()
{
    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        var response = await ExecuteAsync(req, ct);
        await SendResponseAsync(response, ct);
    }

    protected abstract Task<TResponse> ExecuteAsync(TRequest req, CancellationToken ct);

    private async Task SendResponseAsync(TResponse response, CancellationToken ct)
    {
        if (response.StatusCode == HttpStatusCode.OK)
        {
            await SendAsync(response, (int)response.StatusCode, cancellation: ct);
        }
        else
        {
            await SendAsync(response, (int)response.StatusCode, cancellation: ct);
        }
    }
}