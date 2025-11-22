using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Application.UsesCases.AuthService.LoginAuth
{
    public class LoginCommandHandler(
      UserManager<ApplicationUser> userManager,
      ISignInManagerWrapper signInManagerWrapper,
      ITokenService jwtTokenGenerator,
      IRefreshTokenService refreshTokenService)
      : IRequestHandler<LoginCommand, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
           .Include(u => u.School)
           .FirstOrDefaultAsync(u => u.Email == request.Request.Email);

            if (user == null)
                throw new UnauthorizedException("Usuário ou senha inválidos.");

            var result = await signInManagerWrapper.CheckPasswordSignInAsync(user, request.Request.Password, false);

            if (!result.Succeeded)
                throw new UnauthorizedException("Usuário ou senha inválidos.");

            var accessToken = jwtTokenGenerator.GenerateToken(user);

            var refreshToken = await refreshTokenService.GenerateRefreshToken(user);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}