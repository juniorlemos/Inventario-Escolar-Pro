using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InventarioEscolar.Application.UsesCases.AuthService.ResetPassword
{
    public class ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<ResetPasswordCommand,Unit>
    {
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);

            var fixedToken = request.Token.Replace(' ', '+');

            var decodedToken = fixedToken;

            var result = await userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            
            if (!result.Succeeded)
                throw new BusinessException("Erro ao redefinir a senha: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            return Unit.Value;
        }
    }
}