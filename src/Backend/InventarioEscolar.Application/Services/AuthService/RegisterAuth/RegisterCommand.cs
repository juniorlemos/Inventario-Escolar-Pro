using InventarioEscolar.Communication.Request;
using MediatR;

namespace InventarioEscolar.Application.Services.AuthService.RegisterAuth
{
    public record RegisterCommand(RegisterRequest Request) : IRequest<Unit>;
}
