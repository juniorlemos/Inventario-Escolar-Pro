using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class RoomLocationRepository(InventarioEscolarProDBContext dbContext) : IRoomLocationWriteOnlyRepository, IRoomLocationReadOnlyRepository, IRoomLocationUpdateOnlyRepository, IRoomLocationDeleteOnlyRepository
    {
        public void Update(RoomLocation roomLocation) => dbContext.RoomLocations.Update(roomLocation);

        public async Task<PagedResult<RoomLocation>> GetAll(int page, int pageSize, string? searchTerm)
        {
            var query = dbContext.RoomLocations
    .Include(r => r.Assets)
    .Include(r => r.School)
    .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.ToLower();

                query = query.Where(r =>
                    r.Name.ToLower().Contains(normalizedSearch) ||
                    (r.Building != null && r.Building.ToString().ToLower().Contains(normalizedSearch))
                );
            }

            var totalCount = await query.CountAsync();

            var items = new List<RoomLocation>();

            if (page > 0 && pageSize > 0)
            {
                items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                items = await query.ToListAsync();
            }

            return new PagedResult<RoomLocation>(items, totalCount, page, pageSize);
        }
        public async Task<bool> ExistRoomLocationName(string roomLocation, long? schoolId)
        {
            return await dbContext.RoomLocations.AnyAsync(s => s.Name.ToUpper() == roomLocation.ToUpper() && s.SchoolId == schoolId );
        }
        public async Task<RoomLocation?> GetById(long roomlocationId)
        {
            return await dbContext.RoomLocations
                .Include(r => r.School)
                .Include(r => r.Assets)
                .FirstOrDefaultAsync(r => r.Id == roomlocationId);
        }
        public async Task Insert(RoomLocation roomLocation) => await dbContext.RoomLocations.AddAsync(roomLocation);

        public async Task<bool> Delete(long roomLocationId)
        {
            var roomLocation = await dbContext.RoomLocations.FindAsync(roomLocationId);

            if (roomLocation == null)
                return false;

            roomLocation.Active = false;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}