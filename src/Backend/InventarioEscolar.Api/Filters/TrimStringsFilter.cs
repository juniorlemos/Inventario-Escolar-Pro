using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace InventarioEscolar.Api.Filters
{
    public class TrimStringsFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var stringProperties = argument.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

                foreach (var prop in stringProperties)
                {
                    var currentValue = prop.GetValue(argument) as string;
                    if (currentValue != null)
                    {
                        var trimmedValue = currentValue.Trim();
                        prop.SetValue(argument, trimmedValue);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
