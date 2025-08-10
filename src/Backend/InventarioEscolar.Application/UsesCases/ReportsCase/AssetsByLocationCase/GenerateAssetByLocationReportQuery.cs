using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase
{
    public record GenerateAssetByLocationReportQuery() : IRequest<byte[]>;
}
