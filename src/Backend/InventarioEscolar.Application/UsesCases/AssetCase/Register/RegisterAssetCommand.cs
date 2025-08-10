using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Register
{
    public record RegisterAssetCommand(AssetDto AssetDto) : IRequest<AssetDto>;
}
