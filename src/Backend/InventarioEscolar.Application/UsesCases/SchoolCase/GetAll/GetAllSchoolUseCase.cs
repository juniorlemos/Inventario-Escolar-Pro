using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.Schools;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetAll
{
    public class GetAllSchoolUseCase(ISchoolReadOnlyRepository schoolReadOnlyRepository)
        : IGetAllSchoolUseCase
    {
        public async Task<PagedResult<SchoolDto>> Execute(int page, int pageSize)
        {
            var pagedSchools = await schoolReadOnlyRepository.GetAll(page, pageSize)
                         ?? PagedResult<School>.Empty(page, pageSize);

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
