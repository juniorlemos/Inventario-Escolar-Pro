using InventarioEscolar.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InventarioEscolar.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public long? SchoolId
        {
            get
            {
                var user = httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity!.IsAuthenticated)
                    return null; // <==== IMPORTANTE

                var claim = user.FindFirst("schoolId")?.Value;
                if (claim == null)
                    return null; // <==== NÃO lançar exceção

                if (long.TryParse(claim, out var schoolId))
                    return schoolId;

                return null;
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
                    return userId;

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

        public bool IsAuthenticated =>
            httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
    }
}
