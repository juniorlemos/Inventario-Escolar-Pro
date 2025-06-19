using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class RoomLocationRepository(InventarioEscolarProDBContext dbContext) : IRoomLocationWriteOnlyRepository, IRoomLocationReadOnlyRepository
    {
        public async Task<bool> ExistRoomLocationName(string roomLocation)
        {
            return await dbContext.RoomLocations.AnyAsync(s => s.Name.ToUpper() == roomLocation.ToUpper());
        }
        public async Task Insert(RoomLocation roomLocation) => await dbContext.RoomLocations.AddAsync(roomLocation);
    }
}
