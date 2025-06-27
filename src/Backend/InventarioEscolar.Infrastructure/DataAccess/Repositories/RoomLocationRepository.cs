using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Domain.Repositories.Schools;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class RoomLocationRepository(InventarioEscolarProDBContext dbContext) : IRoomLocationWriteOnlyRepository, IRoomLocationReadOnlyRepository, IRoomLocationUpdateOnlyRepository, IRoomLocationDeleteOnlyRepository
    {
        public void Update(RoomLocation roomLocation) => dbContext.RoomLocations.Update(roomLocation);

        public async Task<PagedResult<RoomLocation>> GetAll(int page, int pageSize)
        {
            var query = dbContext.RoomLocations
               .Select(r => new RoomLocation
               {
                   Id = r.Id,
                   Name = r.Name,
                   Description = r.Description,
                   Building = r.Building,
                   Assets = r.Assets
                         .Select(a => new Asset
                         {
                             Id = a.Id,
                             Name = a.Name
                         }).ToList()
               })
                .AsNoTracking();

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
        public async Task<bool> ExistRoomLocationName(string roomLocation)
        {
            return await dbContext.RoomLocations.AnyAsync(s => s.Name.ToUpper() == roomLocation.ToUpper());
        }

        public async Task<RoomLocation?> GetById(long roomlocationId)
        {
            return await dbContext.RoomLocations.Where(r => r.Id == roomlocationId)
                 .Select(r => new RoomLocation
                 {
                     Id = r.Id,
                     Name = r.Name,
                     Description = r.Description,
                     Building = r.Building,
                     SchoolId = r.SchoolId,
                     Assets = r.Assets
                         .Select(a => new Asset
                         {
                             Id = a.Id,
                             Name = a.Name
                         }).ToList()
                 })
                 .FirstOrDefaultAsync(roomlocation => roomlocation.Id == roomlocationId);
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
