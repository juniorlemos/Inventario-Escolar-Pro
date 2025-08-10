using MediatR;

namespace InventarioEscolar.Application.Services.AuthService.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Unit>;
}