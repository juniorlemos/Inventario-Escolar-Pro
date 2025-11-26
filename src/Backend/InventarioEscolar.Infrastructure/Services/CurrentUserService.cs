using InventarioEscolar.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InventarioEscolar.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public long SchoolId
        {
            get
            {
                var user = httpContextAccessor.HttpContext?.User
                    ?? throw new InvalidOperationException("Usuário não autenticado.");

                var claim = user.FindFirst("schoolId")?.Value
                    ?? throw new InvalidOperationException("Claim 'schoolId' não encontrada.");

                if (!long.TryParse(claim, out var schoolId))
                    throw new InvalidOperationException("SchoolId inválido.");

                return schoolId;
            }
        }

        public long? UserId
        {
            get
            {
                var user = httpContextAccessor.HttpContext?.User;
                if (user == null) return null;

                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (long.TryParse(userIdClaim, out var userId))
                {
                    return userId;
                }

                return null;
            }
        }

        public string? UserName
        {
            get
            {
                var user = httpContextAccessor.HttpContext?.User;

                return user?.FindFirst(JwtRegisteredClaimNames.Name)?.Value
                    ?? user?.FindFirst("name")?.Value
                    ?? user?.Identity?.Name;
            }
        }
        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    }
}
