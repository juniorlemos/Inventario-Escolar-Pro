using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations
{
    public interface IRoomLocationUpdateOnlyRepository
    {
       void Update(RoomLocation roomLocation);
    }
}
