using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetAll
{
    public class GetAllSchoolQueryHandler(ISchoolReadOnlyRepository schoolReadOnlyRepository) : IRequestHandler<GetAllSchoolQuery, PagedResult<SchoolDto>>
    {
        public async Task<PagedResult<SchoolDto>> Handle(GetAllSchoolQuery request, CancellationToken cancellationToken)
        {
            var pagedSchools = await schoolReadOnlyRepository.GetAll(request.Page, request.PageSize)
                              ?? PagedResult<School>.Empty(request.Page, request.PageSize);

            var dtoItems = pagedSchools.Items.Adapt<List<SchoolDto>>();

            return new PagedResult<SchoolDto>(
                dtoItems,
                pagedSchools.TotalCount,
                pagedSchools.Page,
                pagedSchools.PageSize
            );
        }
    }
}