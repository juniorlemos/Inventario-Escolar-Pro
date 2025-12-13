using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InventarioEscolar.Application.UsesCases.AuthService.ForgotPassword
{
    public class ForgotPasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IEmailService emailService) : IRequestHandler<ForgotPasswordCommand,Unit>
    {
        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Request.Email)
                ?? throw new NotFoundException(ResourceMessagesException.E_MAIL_NOT_FOUND);
            
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var frontendUrl = configuration["FRONTEND_URL"]
            ?? throw new BusinessException("FRONTEND_URL não configurada");

            var resetLink = $"{frontendUrl}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}";

            var message = $"<p>Clique no link abaixo para redefinir sua senha:</p><p><a href='{resetLink}'>Redefinir senha</a></p>";
            await emailService.SendEmailAsync(user.Email!, "Redefinir Senha", message);

            return Unit.Value;
        }
    }
}