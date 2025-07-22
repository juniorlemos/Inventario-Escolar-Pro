using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetAll
{
    public class GetAllSchoolQueryHandler : IRequestHandler<GetAllSchoolQuery, PagedResult<SchoolDto>>
    {
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;

        public GetAllSchoolQueryHandler(ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
        }

        public async Task<PagedResult<SchoolDto>> Handle(GetAllSchoolQuery request, CancellationToken cancellationToken)
        {
            var pagedSchools = await _schoolReadOnlyRepository.GetAll(request.Page, request.PageSize)
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
