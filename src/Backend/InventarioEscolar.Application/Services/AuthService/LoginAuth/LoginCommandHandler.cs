using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.Services.AuthService.LoginAuth
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISignInManagerWrapper _signInManagerWrapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            ISignInManagerWrapper signInManagerWrapper,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManagerWrapper = signInManagerWrapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;

            var user = await _userManager.FindByEmailAsync(r.Email);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            var result = await _signInManagerWrapper.CheckPasswordSignInAsync(user, r.Password, false);
            if (!result.Succeeded)
                throw new Exception("Senha inválida");

            return _jwtTokenGenerator.GenerateToken(user);
        }
    }
}
