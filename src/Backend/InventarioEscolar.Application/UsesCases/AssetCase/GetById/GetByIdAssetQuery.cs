using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public record GetByIdAssetQuery(long AssetId) : IRequest<AssetDto>;
}
