using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase
{
    public interface IAssetConservationStateReportGenerator
    {
        byte[] Generate(string? schoolName, IEnumerable<Asset> assets, DateTime generatedAt);
    }
}
