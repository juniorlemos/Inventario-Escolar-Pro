using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
namespace InventarioEscolar.Infrastructure.Reports.AssetConservation
{
    public class AssetConservationStateReportGenerator : IAssetConservationStateReportGenerator
    {
        public byte[] Generate(string schoolName, List<Domain.Entities.Asset> assets, DateTime generatedAt)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var document = new AssetConservationStateReportDocument
            {
                SchoolName = schoolName,
                Assets = assets,
                GeneratedAt = generatedAt
            };
            return document.GeneratePdf();
        }
    }
}
