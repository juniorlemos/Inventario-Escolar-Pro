using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetByCategoryCase
{
    public class AssetByCategoryReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public List<Asset> Assets { get; set; } = [];
        public DateTime GeneratedAt { get; set; }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text($"Relatório gerado em {GeneratedAt:dd/MM/yyyy HH:mm}");
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Relatório de Ativos por Categoria")
                    .FontSize(18).SemiBold().FontColor(Colors.Green.Darken2);

                column.Item().Text(SchoolName)
                    .FontSize(14).FontColor(Colors.Black);

                column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Green.Darken3);
            });
        }

        void ComposeContent(IContainer container)
        {
            var grouped = Assets
                .GroupBy(a => a.Category?.Name ?? "Sem Categoria")
                .OrderBy(g => g.Key);

            container.Column(column =>
            {
                foreach (var group in grouped)
                {
                    column.Item()
                    .PaddingBottom(5)
                    .Text(group.Key)
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.Green.Darken1);
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Nome
                            columns.RelativeColumn(2); // Código
                            columns.RelativeColumn(3); // Localização
                            columns.RelativeColumn(2); // Valor
                            columns.RelativeColumn(2); // Estado
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Nome").SemiBold().FontSize(11);
                            header.Cell().Text("Código").SemiBold().FontSize(11);
                            header.Cell().Text("Local").SemiBold().FontSize(11);
                            header.Cell().Text("Valor").SemiBold().FontSize(11);
                            header.Cell().Text("Estado").SemiBold().FontSize(11);
                        });

                        foreach (var asset in group)
                        {
                            table.Cell().Text(asset.Name).FontSize(10);
                            table.Cell().Text(asset.PatrimonyCode?.ToString() ?? "-").FontSize(10);
                            table.Cell().Text(asset.RoomLocation?.Name ?? "-").FontSize(10);
                            table.Cell().Text(asset.AcquisitionValue?.ToString("C") ?? "-").FontSize(10);
                            table.Cell().Text(asset.ConservationState.ToString()).FontSize(10);
                        }
                    });

                    // Espaçamento entre categorias
                    column.Item().PaddingBottom(15);
                }
            });
        }
    }
}
