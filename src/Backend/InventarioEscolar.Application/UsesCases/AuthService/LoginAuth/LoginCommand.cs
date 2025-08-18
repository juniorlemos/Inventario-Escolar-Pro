using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AuthService.LoginAuth
{
    public record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>;
}
