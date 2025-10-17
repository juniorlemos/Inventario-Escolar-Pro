using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetById
{
    public record GetByIdAssetMovementQuery(long AssetMovementId) : IRequest<AssetMovementDto>;    
}
