using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll
{
    public class GetAllRoomLocationUseCase(IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository)
        : IGetAllRoomLocationUseCase
    {
        public async Task<PagedResult<RoomLocationDto>> Execute(int page, int pageSize)
        {
            var pagedRoomLocations = await roomLocationReadOnlyRepository.GetAll(page, pageSize)
                         ?? PagedResult<RoomLocation>.Empty(page, pageSize);

            var dtoItems = pagedRoomLocations.Items.Adapt<List<RoomLocationDto>>();

            return new PagedResult<RoomLocationDto>(
                dtoItems,
                pagedRoomLocations.TotalCount,
                pagedRoomLocations.Page,
                pagedRoomLocations.PageSize
            );
        }
    }
}
