using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.RepositoriesReports
{
    public interface IAssetReportReadOnlyRepository
    {
        Task<IEnumerable<Asset>> GetAllAssetReport();
    }
}