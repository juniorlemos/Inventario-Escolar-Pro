using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Infrastructure.Reports.AssetByLocationCase
{
    public class AssetByLocationReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public List<Asset> Assets { get; set; } = [];

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text($"Gerado em {GeneratedAt:dd/MM/yyyy HH:mm}");
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().AlignCenter().Text("Relatório de Bens por Localização")
                    .FontSize(16).SemiBold().FontColor(Colors.Green.Medium);
                col.Item().AlignCenter().Text(SchoolName).FontSize(12);
                col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
            });
        }

        void ComposeContent(IContainer container)
        {
            var assetsByLocation = Assets
                .GroupBy(a => a.RoomLocation?.Name ?? "Sem Localização")
                .OrderBy(g => g.Key);

            container.Column(col =>
            {
                foreach (var group in assetsByLocation)
                {
                    col.Item().PaddingTop(10).Text(group.Key)
                        .FontSize(12).Bold().FontColor(Colors.Green.Darken2);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4); // Nome
                            columns.RelativeColumn(2); // Código Patrimônio
                            columns.RelativeColumn(2); // Categoria
                            columns.RelativeColumn(2); // Estado Conservação
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Nome").SemiBold();
                            header.Cell().Text("Código").SemiBold();
                            header.Cell().Text("Categoria").SemiBold();
                            header.Cell().Text("Estado").SemiBold();
                        });

                        foreach (var asset in group.OrderBy(a => a.Name))
                        {
                            table.Cell().Text(asset.Name);
                            table.Cell().Text(asset.PatrimonyCode?.ToString() ?? "-");
                            table.Cell().Text(asset.Category?.Name ?? "-");
                            table.Cell().Text(asset.ConservationState.ToString());
                        }
                    });
                }
            });
        }
    }
}
