using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InventarioEscolar.Application.Services.AuthService.ForgotPassword
{
    public class ForgotPasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IEmailService emailService) : IRequestHandler<ForgotPasswordCommand,Unit>
    {
        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Request.Email)
                ?? throw new Exception(ResourceMessagesException.E_MAIL_NOT_FOUND);
            
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://seusite.com/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";

            var message = $"<p>Clique no link abaixo para redefinir sua senha:</p><p><a href='{resetLink}'>Redefinir senha</a></p>";
            await emailService.SendEmailAsync(user.Email, "Redefinir Senha", message);

            return Unit.Value;
        }
    }
}