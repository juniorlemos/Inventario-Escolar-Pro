using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Domain.Interfaces.RepositoriesReports
{
    public interface IAssetMovementReportReadOnlyRepository
    {
        Task <IEnumerable<AssetMovement>> GetAllAssetMovementsReport();
        Task <IEnumerable<AssetMovement>> GetAllAssetCanceledMovementsReport();
    }
}
