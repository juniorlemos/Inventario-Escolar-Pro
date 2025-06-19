using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Register
{
    public interface IRegisterRoomLocationUseCase
    {
        Task<RoomLocationDto> Execute(RoomLocationDto roomLocationDto);
    }
}
