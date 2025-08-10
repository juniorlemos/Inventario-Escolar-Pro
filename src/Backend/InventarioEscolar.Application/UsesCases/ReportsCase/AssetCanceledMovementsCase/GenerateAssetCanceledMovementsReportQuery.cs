using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase
{
    public record GenerateAssetCanceledMovementsReportQuery() : IRequest<byte[]>;
}
