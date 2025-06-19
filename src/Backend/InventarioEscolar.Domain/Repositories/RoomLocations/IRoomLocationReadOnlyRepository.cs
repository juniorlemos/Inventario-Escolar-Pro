using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.RoomLocations
{
    public interface IRoomLocationReadOnlyRepository
    {
         Task<bool> ExistRoomLocationName(string roomLocation);
    }
}
