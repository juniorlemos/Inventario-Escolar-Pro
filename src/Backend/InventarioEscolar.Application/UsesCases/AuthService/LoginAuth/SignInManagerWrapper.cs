using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventarioEscolar.Application.UsesCases.AuthService.LoginAuth
{
    public class SignInManagerWrapper(SignInManager<ApplicationUser> signInManager) : ISignInManagerWrapper
    {
        public Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            return signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }
    }
}