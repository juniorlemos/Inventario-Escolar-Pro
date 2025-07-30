using InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Infrastructure.Reports.AssetMovementsCase;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Infrastructure.Reports.AssetCanceledMovements
{
    public class AssetCanceledMovementsReportGenerator : IAssetCanceledMovementsReportGenerator
    {
        public byte[] Generate(string schoolName, IEnumerable<AssetMovement> movements, DateTime generatedAt)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = new AssetCanceledMovementsReportDocument
            {
                SchoolName = schoolName,
                Movements = movements,
                GeneratedAt = generatedAt
            };

            return document.GeneratePdf();
        }
    }
}
