namespace InventarioEscolar.Domain.Repositories.RoomLocations
{
    public interface IRoomLocationDeleteOnlyRepository
    {
       Task<bool> Delete(long roomLocationId);
    }
}
