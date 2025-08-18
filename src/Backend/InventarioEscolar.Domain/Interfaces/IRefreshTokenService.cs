using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GenerateRefreshToken(ApplicationUser user);
        Task<RefreshToken?> GetByRefreshToken(string token);
    }
}
