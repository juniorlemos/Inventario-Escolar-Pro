using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetAll
{
    public record GetAllAssetQuery(int Page, int PageSize) : IRequest<PagedResult<AssetDto>>;
}

