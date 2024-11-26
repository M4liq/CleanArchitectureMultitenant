using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Common.Extensions;

namespace WebApi.Common.Models;

public class ModelStateErrorResponse
{
    public List<string> Errors { get; }
    
    public ModelStateErrorResponse(ModelStateDictionary modelState)
    {
        Errors = modelState.GetFormattedErrors();
    }
}