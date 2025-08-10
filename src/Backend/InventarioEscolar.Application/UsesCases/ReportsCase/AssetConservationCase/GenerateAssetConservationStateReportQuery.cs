using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase
{
    public record GenerateAssetConservationStateReportQuery() : IRequest<byte[]>;
}