using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase
{
    public interface IAssetMovementsReportGenerator
    {
        byte[] Generate(string? schoolName, IEnumerable<AssetMovement> movements, DateTime generatedAt);
    }
}
