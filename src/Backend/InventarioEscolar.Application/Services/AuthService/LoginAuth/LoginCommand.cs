using InventarioEscolar.Communication.Request;
using MediatR;

namespace InventarioEscolar.Application.Services.AuthService.LoginAuth
{
    public record LoginCommand(LoginRequest Request) : IRequest<string>;
}
