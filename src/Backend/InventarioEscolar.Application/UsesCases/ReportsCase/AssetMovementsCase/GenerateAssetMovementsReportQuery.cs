using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase
{
    public record GenerateAssetMovementsReportQuery() : IRequest<byte[]>;
}
