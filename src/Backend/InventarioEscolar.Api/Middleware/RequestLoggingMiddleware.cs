using Serilog;
using System.Security.Claims;
using System.Text;

namespace InventarioEscolar.Api.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            var userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                : "Usuário não autenticado";

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido";
            var method = context.Request.Method;
            var path = context.Request.Path;

            string body = "";
            if (context.Request.ContentLength > 0 && context.Request.ContentType != null &&
                context.Request.ContentType.Contains("application/json"))
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(
                    context.Request.Body, Encoding.UTF8, leaveOpen: true);
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            Log.Information("Requisição recebida: {Method} {Path} | IP: {IP} | Usuário: {User} | Corpo: {Body}",
                method, path, ip, userId, Truncate(body, 1000));

            await next(context);
        }

        private string Truncate(string value, int maxLength) =>
            string.IsNullOrEmpty(value) ? "" : value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}