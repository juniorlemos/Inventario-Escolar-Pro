using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public interface IUpdateRoomLocationUseCase
    {
        Task Execute(long id, UpdateRoomLocationDto roomLocationDto);
    }
}
