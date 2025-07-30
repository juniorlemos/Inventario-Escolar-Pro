using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.Services.AuthService.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand,Unit>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;
            var user = await _userManager.FindByEmailAsync(r.Email);
            if (user == null)
                throw new Exception("E-mail não encontrado.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://seusite.com/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";

            var message = $"<p>Clique no link abaixo para redefinir sua senha:</p><p><a href='{resetLink}'>Redefinir senha</a></p>";
            await _emailService.SendEmailAsync(user.Email, "Redefinir Senha", message);

            return Unit.Value;
        }
    }
}

