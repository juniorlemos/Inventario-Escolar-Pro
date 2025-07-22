using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase
{
    public interface IAssetConservationStateReportGenerator
    {
        byte[] Generate(string schoolName, List<Asset> assets, DateTime generatedAt);
    }
}
