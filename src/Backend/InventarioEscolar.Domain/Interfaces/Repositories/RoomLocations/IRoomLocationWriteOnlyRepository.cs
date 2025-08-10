using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations
{
    public interface IRoomLocationWriteOnlyRepository
    {
        Task Insert(RoomLocation roomLocation);
    }
}