namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete
{
    public interface IDeleteRoomLocationUseCase
    {
        Task Execute(long roomLocationId);
    }
}
