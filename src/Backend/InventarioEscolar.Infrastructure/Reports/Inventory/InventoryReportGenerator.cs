using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.Inventory
{
    public class InventoryReportGenerator : IInventoryReportGenerator
    {
        public byte[] Generate(string? schoolName, IEnumerable<Asset> assets, DateTime generatedAt)
        {

            QuestPDF.Settings.License = LicenseType.Community;

            var document = new InventoryReportDocument
            {
                SchoolName = schoolName,
                Assets = assets,
                GeneratedAt = generatedAt
            };

            return document.GeneratePdf();
        }
    }
}

