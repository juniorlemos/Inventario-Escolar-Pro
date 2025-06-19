using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.RoomLocations
{
    public interface IRoomLocationWriteOnlyRepository
    {
        Task Insert(RoomLocation roomLocation);
    }
}
