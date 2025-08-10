using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Update
{
    public record UpdateAssetMovementCommand(long Id, string CancelReason) : IRequest<Unit>;
}
