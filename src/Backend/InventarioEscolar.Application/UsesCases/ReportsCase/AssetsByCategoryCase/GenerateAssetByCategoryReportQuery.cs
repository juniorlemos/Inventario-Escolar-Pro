using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase
{
    public record GenerateAssetByCategoryReportQuery() : IRequest<byte[]>;
}
