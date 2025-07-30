using InventarioEscolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Domain.Interfaces.RepositoriesReports
{
    public interface IAssetReportReadOnlyRepository
    {
        Task<IEnumerable<Asset>> GetAllAssetReport();

    }
}
