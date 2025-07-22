using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.Inventory
{
    public class InventoryReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public List<Asset> Assets { get; set; } = new();

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri).FontSize(10).FontColor(Colors.Grey.Darken2));

                page.Header().Element(ComposeHeader);
                page.Content().PaddingVertical(10).Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        void ComposeHeader(IContainer container)
        {
            var totalCategorias = Assets
                .Select(a => a.Category?.Name)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct()
                .Count();

            var totalLocais = Assets
                .Select(a => a.RoomLocation?.Name)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct()
                .Count();

            var totalDanificados = Assets
                .Count(a => a.Description?.ToLower().Contains("danificado") == true);

            var itensNovos = Assets
                .Count(a => a.CreatedOn >= DateTime.Today.AddDays(-30));

            container.Column(column =>
            {
                column.Item().Text("📘 Relatório de Inventário Patrimonial")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter()
                    .FontColor(Colors.Blue.Darken2);

                column.Item().PaddingVertical(5);

               
                column.Item().Row(row =>
                {
                    row.Spacing(25); 

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Text($"🏫 Escola: {SchoolName}")
                            .FontSize(12)
                            .FontColor(Colors.Grey.Darken2);

                        col.Item().Text($"🗓️ Gerado em: {GeneratedAt:dd/MM/yyyy HH:mm}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);

                        col.Item().Text($"📦 Total de Itens: {Assets.Count}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);

                        col.Item().Text($"📍 Locais: {totalLocais}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);
                    });

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Text($"🗂️ Categorias: {totalCategorias}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);

                        col.Item().Text($"🆕 Novos (últimos 30 dias): {itensNovos}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);

                        col.Item().Text($"⚠️ Danificados: {totalDanificados}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);
                    });
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);  // Nome
                    columns.ConstantColumn(50); // Código
                    columns.RelativeColumn(2);  // Local
                    columns.RelativeColumn(2);  // Categoria
                    columns.RelativeColumn(2);  // Status
                    columns.RelativeColumn(2);  // Entrada
                    columns.RelativeColumn(2);  // Descrição
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Blue.Lighten4).Padding(5).Text("Nome").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Blue.Lighten4).Padding(5).Text("Código").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Blue.Lighten4).Padding(5).Text("Local").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Blue.Lighten4).Padding(5).Text("Categoria").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Blue.Lighten4).Padding(5).AlignCenter().Text("Status").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Blue.Lighten4).Padding(5).AlignCenter().Text("Entrada").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Blue.Lighten4).Padding(5).Text("Descrição").SemiBold().FontColor(Colors.Black);

                });

                bool isEven = false;
                foreach (var asset in Assets)
                {
                    var background = isEven ? Colors.Grey.Lighten4 : Colors.White;
                    isEven = !isEven;

                    table.Cell().Background(background).Padding(5).Text(asset.Name ?? "-");
                    table.Cell().Background(background).Padding(5).Text(asset.PatrimonyCode.ToString());
                    table.Cell().Background(background).Padding(5).Text(asset.RoomLocation?.Name ?? "-");
                    table.Cell().Background(background).Padding(5).Text(asset.Category?.Name ?? "-");
                    table.Cell().Background(background).Padding(5).AlignCenter().Text(asset.Description ?? "-");
                    table.Cell().Background(background).Padding(5).AlignCenter().Text(asset.CreatedOn.ToString("dd/MM/yyyy"));
                    table.Cell().Background(background).Padding(5).Text(asset.Description ?? "-");
                }
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Página ").FontSize(9).FontColor(Colors.Blue.Darken2);
                text.CurrentPageNumber().FontSize(9).FontColor(Colors.Blue.Darken2);
                text.Span(" de ").FontSize(9).FontColor(Colors.Blue.Darken2);
                text.TotalPages().FontSize(9).FontColor(Colors.Blue.Darken2);
            });
        }
    }
}
