using InventarioEscolar.Application.Services.Interfaces;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;

            var user = await _userManager.FindByEmailAsync(r.Email);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            var result = await _signInManager.CheckPasswordSignInAsync(user, r.Password, false);
            if (!result.Succeeded)
                throw new Exception("Senha inválida");

            return _jwtTokenGenerator.GenerateToken(user);
        }
    }
}

