using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.Security.Token
{
    public class RefreshTokenService(InventarioEscolarProDBContext context) : IRefreshTokenService
    {
        public async Task<RefreshToken> GenerateRefreshToken(ApplicationUser user)
        {
            var existingToken = await context.Set<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.UserId == user.Id);

            if (existingToken != null)
            {
                context.Set<RefreshToken>().Remove(existingToken);
            }

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            context.Set<RefreshToken>().Add(refreshToken);
            await context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken?> GetByRefreshToken(string token)
        {
            return await context.Set<RefreshToken>()
                .Include(r => r.User)
                    .ThenInclude(u => u.School)
                .FirstOrDefaultAsync(r => r.Token == token && r.ExpiresAt > DateTime.UtcNow);
        }
    }
}

