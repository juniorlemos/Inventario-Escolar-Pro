using InventarioEscolar.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventarioEscolar.Application.Services.Interfaces.Auth
{
    public interface ISignInManagerWrapper
    {
        Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure);
    }
}
