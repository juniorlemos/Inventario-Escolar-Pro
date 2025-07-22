namespace InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations
{
    public interface IRoomLocationDeleteOnlyRepository
    {
       Task<bool> Delete(long roomLocationId);
    }
}
