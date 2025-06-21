using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.RoomLocations
{
    public interface IRoomLocationUpdateOnlyRepository
    {
       void Update(RoomLocation roomLocation);
    }
}
