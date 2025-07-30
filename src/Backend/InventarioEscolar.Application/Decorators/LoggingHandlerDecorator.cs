using InventarioEscolar.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.Decorators
{
    public class LoggingHandlerDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly ILogger<LoggingHandlerDecorator<TRequest, TResponse>> _logger;
        private readonly ICurrentUserService _currentUser;

        public LoggingHandlerDecorator(
            IRequestHandler<TRequest, TResponse> inner,
            ILogger<LoggingHandlerDecorator<TRequest, TResponse>> logger,
            ICurrentUserService currentUser)
        {
            _inner = inner;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId?.ToString() ?? "Anônimo";
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Usuário {UserId} iniciando {RequestName} com dados: {@Request}",
                userId, requestName, request);

            try
            {
                var response = await _inner.Handle(request, cancellationToken);

                string safeResponse = ObterResumoSeguranca(response?.ToString());
                _logger.LogInformation("Usuário {UserId} finalizou {RequestName} com parte da resposta: {Response}",
                    userId, requestName, safeResponse);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no {RequestName} pelo usuário {UserId}", requestName, userId);
                throw;
            }
        }

        private string ObterResumoSeguranca(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return "[resposta vazia]";

            if (valor.Length <= 10)
                return valor;

            return $"{valor.Substring(0, 5)}...{valor.Substring(valor.Length - 5)}";
        }
    }
}
