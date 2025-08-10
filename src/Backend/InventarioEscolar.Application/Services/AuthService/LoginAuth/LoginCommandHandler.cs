using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InventarioEscolar.Application.Services.AuthService.LoginAuth
{
    public class LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        ISignInManagerWrapper signInManagerWrapper,
        IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, string>
    {
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByEmailAsync(request.Request.Email)
                ?? throw new Exception(ResourceMessagesException.INVALID_USERNAME_OR_PASSWORD);
            
            var result = await signInManagerWrapper.CheckPasswordSignInAsync(user, request.Request.Password, false);
            
            if (!result.Succeeded)
                throw new Exception(ResourceMessagesException.INVALID_USERNAME_OR_PASSWORD);

            return jwtTokenGenerator.GenerateToken(user);
        }
    }
}