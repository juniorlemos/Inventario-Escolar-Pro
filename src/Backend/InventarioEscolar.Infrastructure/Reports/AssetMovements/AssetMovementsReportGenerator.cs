using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetMovementsCase
{
    public class AssetMovementsReportGenerator : IAssetMovementsReportGenerator
    {
        public byte[] Generate(string schoolName, List<AssetMovement> movements, DateTime generatedAt)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = new AssetMovementsReportDocument
            {
                SchoolName = schoolName,
                Movements = movements,
                GeneratedAt = generatedAt
            };

            return document.GeneratePdf();
        }
    }
}
