using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}
