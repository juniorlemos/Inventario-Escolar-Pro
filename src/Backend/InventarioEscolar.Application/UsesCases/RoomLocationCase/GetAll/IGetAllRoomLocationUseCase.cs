using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll
{
    public interface IGetAllRoomLocationUseCase
    {
        Task<PagedResult<RoomLocationDto>> Execute(int page, int pagesize);

    }
}
