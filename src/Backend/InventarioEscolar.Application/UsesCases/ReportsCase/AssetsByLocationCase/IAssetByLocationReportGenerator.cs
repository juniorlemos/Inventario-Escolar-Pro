using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase
{
    public interface IAssetByLocationReportGenerator
    {
        byte[] Generate(string? schoolName, IEnumerable<Asset> assets, DateTime generatedAt);
    }
}
