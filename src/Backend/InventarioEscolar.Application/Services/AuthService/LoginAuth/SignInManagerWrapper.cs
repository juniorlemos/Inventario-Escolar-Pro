using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.Services.AuthService.LoginAuth
{
    public class SignInManagerWrapper : ISignInManagerWrapper
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInManagerWrapper(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            return _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }
    }
}
