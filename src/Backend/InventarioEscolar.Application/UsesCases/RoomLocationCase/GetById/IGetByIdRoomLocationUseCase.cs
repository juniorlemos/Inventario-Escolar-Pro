using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById
{
    public interface IGetByIdRoomLocationUseCase
    {
        Task<RoomLocationDto> Execute(long roomLocationId);
    }
}
