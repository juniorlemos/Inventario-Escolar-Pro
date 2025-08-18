using MediatR;

namespace InventarioEscolar.Application.UsesCases.AuthService.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Unit>;
}