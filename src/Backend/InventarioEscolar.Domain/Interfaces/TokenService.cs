using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
