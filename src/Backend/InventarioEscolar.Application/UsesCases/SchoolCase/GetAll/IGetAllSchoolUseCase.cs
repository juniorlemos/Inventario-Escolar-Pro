using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetAll
{
    public interface IGetAllSchoolUseCase
    {
        Task<PagedResult<SchoolDto>> Execute(int page, int pagesize);
    }
}
