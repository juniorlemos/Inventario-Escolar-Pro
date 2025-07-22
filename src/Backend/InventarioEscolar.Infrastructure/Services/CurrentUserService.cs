using InventarioEscolar.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventarioEscolar.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? SchoolId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return null;

                var schoolIdClaim = user.FindFirst("schoolId")?.Value;
                if (long.TryParse(schoolIdClaim, out var schoolId))
                {
                    return schoolId;
                }

                return null;
            }
        }
    }
}