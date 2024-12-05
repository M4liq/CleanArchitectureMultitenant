using Infrastructure.Services.Core;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware;

public class LanguageMiddleware : IMiddleware 
{
    private const string LanguageHeaderName = "X-Requested-Language";
    private readonly ICurrentLanguage _languageService;

    public LanguageMiddleware(ICurrentLanguage languageService)
    {
        _languageService = languageService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers.TryGetValue(LanguageHeaderName, out var requestedLanguage))
        {
            _languageService.SetCurrentLanguage(requestedLanguage);
        }

        await next(context);
    }
}