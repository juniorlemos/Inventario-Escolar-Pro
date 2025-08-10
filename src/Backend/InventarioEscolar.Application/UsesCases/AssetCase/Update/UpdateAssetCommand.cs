using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public record UpdateAssetCommand(long Id, UpdateAssetDto AssetDto) : IRequest<Unit>;
}
