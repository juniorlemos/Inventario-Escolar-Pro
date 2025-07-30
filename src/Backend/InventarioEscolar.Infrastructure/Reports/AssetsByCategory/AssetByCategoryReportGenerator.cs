using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetByCategoryCase
{
    public class AssetByCategoryReportGenerator : IAssetByCategoryReportGenerator
    {
        public byte[] Generate(string schoolName, IEnumerable<Domain.Entities.Asset> assets, DateTime generatedAt)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = new AssetByCategoryReportDocument
            {
                SchoolName = schoolName,
                Assets = assets,
                GeneratedAt = generatedAt
            };

            return document.GeneratePdf();
        }
    }
}
