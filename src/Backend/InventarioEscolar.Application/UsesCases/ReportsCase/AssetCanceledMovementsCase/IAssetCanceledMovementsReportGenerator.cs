using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase
{
    public interface IAssetCanceledMovementsReportGenerator
    {
        byte[] Generate(string? schoolName, IEnumerable<AssetMovement> movements, DateTime generatedAt);
    }
}
