using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public record GetByIdAssetQuery(long AssetId) : IRequest<AssetDto>;
}
