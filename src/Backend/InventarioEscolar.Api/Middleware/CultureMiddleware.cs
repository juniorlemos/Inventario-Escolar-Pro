using InventarioEscolar.Domain.Extension;
using System.Globalization;

namespace InventarioEscolar.Api.Middleware
{
    public class CultureMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            var cultureInfo = new CultureInfo("en");

            if (requestedCulture.NotEmpty()
                && supportedLanguages.Exists(c => c.Name.Equals(requestedCulture)))
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await next(context);
        }
    }
}
