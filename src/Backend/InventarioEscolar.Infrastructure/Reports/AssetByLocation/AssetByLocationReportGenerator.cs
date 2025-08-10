using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase;
using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetByLocation
{
    public class AssetByLocationReportGenerator : IAssetByLocationReportGenerator
    {
        public byte[] Generate(string? schoolName, IEnumerable<Asset> assets, DateTime generatedAt)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = new AssetByLocationReportDocument
            {
                SchoolName = schoolName,
                Assets = assets,
                GeneratedAt = generatedAt
            };

            return document.GeneratePdf();
        }
    }
}