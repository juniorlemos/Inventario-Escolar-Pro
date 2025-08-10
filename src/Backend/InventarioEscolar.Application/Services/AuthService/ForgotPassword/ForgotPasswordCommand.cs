using InventarioEscolar.Communication.Request;
using MediatR;

namespace InventarioEscolar.Application.Services.AuthService.ForgotPassword
{
    public record ForgotPasswordCommand(ForgotPasswordRequest Request) : IRequest<Unit>;
}
