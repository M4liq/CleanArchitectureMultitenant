using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApi.Common.Extensions;

public static class ModelStateDictionaryExtensions
{
    public static List<string> GetFormattedErrors(this ModelStateDictionary dictionary)
    {
        return dictionary.Values.SelectMany(_ => _.Errors.Select(__ => __.ErrorMessage)).ToList();
    }
}