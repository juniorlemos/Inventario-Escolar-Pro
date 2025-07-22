using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations
{
    public interface IRoomLocationReadOnlyRepository
    {
         Task<bool> ExistRoomLocationName(string roomLocation, long? schoolId);
         Task<RoomLocation?> GetById(long roomlocationId);
         Task<PagedResult<RoomLocation>> GetAll(int page, int pageSize);
    }
}
