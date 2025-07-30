using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase
{
    public interface IAssetByCategoryReportGenerator
    {
        byte[] Generate(string schoolName, IEnumerable<Asset> assets, DateTime generatedAt);
    }
}
