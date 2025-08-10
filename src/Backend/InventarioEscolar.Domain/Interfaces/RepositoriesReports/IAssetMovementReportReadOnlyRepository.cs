using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.RepositoriesReports
{
    public interface IAssetMovementReportReadOnlyRepository
    {
        Task <IEnumerable<AssetMovement>> GetAllAssetMovementsReport();
        Task <IEnumerable<AssetMovement>> GetAllAssetCanceledMovementsReport();
    }
}