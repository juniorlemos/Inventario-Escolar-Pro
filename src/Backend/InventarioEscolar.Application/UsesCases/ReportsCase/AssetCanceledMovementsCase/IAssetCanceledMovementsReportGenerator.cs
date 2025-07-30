using InventarioEscolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase
{
    public interface IAssetCanceledMovementsReportGenerator
    {
        byte[] Generate(string schoolName, IEnumerable<AssetMovement> movements, DateTime generatedAt);
    }
}
