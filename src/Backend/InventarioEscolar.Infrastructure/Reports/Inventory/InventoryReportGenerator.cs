using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Infrastructure.Reports.Inventory;
using QuestPDF.Fluent;
using QuestPDF;
using QuestPDF.Infrastructure;

public class InventoryReportGenerator : IInventoryReportGenerator
    {
        public byte[] Generate(string schoolName, IEnumerable<Asset> assets, DateTime generatedAt)
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

