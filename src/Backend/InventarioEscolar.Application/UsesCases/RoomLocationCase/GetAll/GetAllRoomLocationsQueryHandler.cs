using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll
{
    public class GetAllRoomLocationsQueryHandler : IRequestHandler<GetAllRoomLocationsQuery, PagedResult<RoomLocationDto>>
    {
        private readonly IRoomLocationReadOnlyRepository _roomLocationReadOnlyRepository;

        public GetAllRoomLocationsQueryHandler(IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository)
        {
            _roomLocationReadOnlyRepository = roomLocationReadOnlyRepository;
        }

        public async Task<PagedResult<RoomLocationDto>> Handle(GetAllRoomLocationsQuery request, CancellationToken cancellationToken)
        {
            var pagedRoomLocations = await _roomLocationReadOnlyRepository.GetAll(request.Page, request.PageSize)
                                      ?? PagedResult<RoomLocation>.Empty(request.Page, request.PageSize);

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
