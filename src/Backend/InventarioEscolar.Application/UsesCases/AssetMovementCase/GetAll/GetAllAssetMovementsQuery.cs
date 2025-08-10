using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public record GetAllAssetMovementsQuery(int Page, int PageSize, bool? IsCanceled = null) : IRequest<PagedResult<AssetMovementDto>>;
}
