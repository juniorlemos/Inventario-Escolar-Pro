using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Enums;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetAll
{
    public record GetAllAssetQuery(int Page, int PageSize, string SearchTerm, ConservationState? ConservationState = null) : IRequest<PagedResult<AssetDto>>;
}