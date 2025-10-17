using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Register
{
    public record RegisterAssetMovementCommand(AssetMovementDto AssetMovementDto) : IRequest<Unit>;
}