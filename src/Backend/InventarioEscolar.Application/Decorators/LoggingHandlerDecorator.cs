using InventarioEscolar.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventarioEscolar.Application.Decorators
{
    public class LoggingHandlerDecorator<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> inner,
        ILogger<LoggingHandlerDecorator<TRequest, TResponse>> logger,
        ICurrentUserService currentUser) : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId?.ToString() ?? "Anônimo";
            var requestName = typeof(TRequest).Name;

            logger.LogInformation("Usuário {UserId} iniciando {RequestName} com dados: {@Request}",
                userId, requestName, request);

            try
            {
                var response = await inner.Handle(request, cancellationToken);

                string safeResponse = LoggingHandlerDecorator<TRequest, TResponse>.ObterResumoSeguranca(response?.ToString());
                logger.LogInformation("Usuário {UserId} finalizou {RequestName} com parte da resposta: {Response}",
                    userId, requestName, safeResponse);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro no {RequestName} pelo usuário {UserId}", requestName, userId);
                throw;
            }
        }

        private static string ObterResumoSeguranca(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return "[resposta vazia]";

            if (valor.Length <= 10)
                return valor;

            return $"{valor[..5]}...{valor[^5..]}";
        }
    }
}