using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Repositories.RoomLocations
{
    public interface IRoomLocationReadOnlyRepository
    {
         Task<bool> ExistRoomLocationName(string roomLocation);
         Task<RoomLocation?> GetById(long roomlocationId);
         Task<PagedResult<RoomLocation>> GetAll(int page, int pageSize);
    }
}
