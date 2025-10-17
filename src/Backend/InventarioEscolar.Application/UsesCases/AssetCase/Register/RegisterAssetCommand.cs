using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Response;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Register
{
    public record RegisterAssetCommand(AssetDto AssetDto) : IRequest<CreatedAssetResponse>;
}
