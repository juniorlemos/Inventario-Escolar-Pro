using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
