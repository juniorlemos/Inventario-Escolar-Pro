using InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase;
using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetCanceledMovements
{
    public class AssetCanceledMovementsReportGenerator : IAssetCanceledMovementsReportGenerator
    {
        public byte[] Generate(string? schoolName, IEnumerable<AssetMovement> movements, DateTime generatedAt)
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
