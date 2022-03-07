using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace SpotifyExtension.Attributes
{
    public class XSSProtection : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach(var arg in context.ActionArguments)
            {
                if (arg.Value is not string)
                    continue;

                var value = arg.Value?.ToString();

                if (!string.IsNullOrEmpty(value))
                    context.ActionArguments[arg.Key] = Regex.Replace(value, "<.*?>", string.Empty);
            }
            await next();
        }
    }
}
