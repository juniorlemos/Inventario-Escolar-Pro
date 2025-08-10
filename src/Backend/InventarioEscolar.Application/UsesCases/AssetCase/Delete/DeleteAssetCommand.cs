using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Delete
{
    public record DeleteAssetCommand(long AssetId) : IRequest<Unit>;
}
