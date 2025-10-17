using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll
{
    public class GetAllRoomLocationsQueryHandler(IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository)
       : IRequestHandler<GetAllRoomLocationsQuery, PagedResult<RoomLocationDto>>
    {
        public async Task<PagedResult<RoomLocationDto>> Handle(GetAllRoomLocationsQuery request, CancellationToken cancellationToken)
        {
            var pagedRoomLocations = await roomLocationReadOnlyRepository.GetAll(
                request.Page,
                request.PageSize,
                request.SearchTerm
            ) ?? PagedResult<RoomLocation>.Empty(request.Page, request.PageSize, request.SearchTerm);

            var dtoItems = pagedRoomLocations.Items
                                             .Select(RoomLocationMapper.ToDto)
                                             .ToList();

            return new PagedResult<RoomLocationDto>(
                dtoItems,
                pagedRoomLocations.TotalCount,
                pagedRoomLocations.Page,
                pagedRoomLocations.PageSize
            );
        }
    }
}
