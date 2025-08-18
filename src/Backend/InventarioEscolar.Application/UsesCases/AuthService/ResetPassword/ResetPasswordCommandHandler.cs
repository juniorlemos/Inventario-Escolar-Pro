using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace InventarioEscolar.Application.UsesCases.AuthService.ResetPassword
{
    public class ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<ResetPasswordCommand,Unit>
    {
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new Exception(ResourceMessagesException.USER_NOT_FOUND);
            
            var decodedToken = WebUtility.UrlDecode(request.Token);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            
            if (!result.Succeeded)
                throw new Exception("Erro ao redefinir a senha: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            return Unit.Value;
        }
    }
}