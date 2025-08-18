using InventarioEscolar.Communication.Request;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AuthService.RegisterAuth
{
    public record RegisterCommand(RegisterRequest Request) : IRequest<Unit>;
}
