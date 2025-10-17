using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public record GetAllAssetMovementsQuery(int Page, int PageSize, string SearchTerm, bool? IsCanceled = null ) : IRequest<PagedResult<AssetMovementDto>>;
}
