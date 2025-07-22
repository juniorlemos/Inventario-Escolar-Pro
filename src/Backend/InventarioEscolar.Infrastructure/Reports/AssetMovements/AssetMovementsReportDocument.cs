using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetMovementsCase
{
    public class AssetMovementsReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public List<AssetMovement> Movements { get; set; } = new();

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
            var total = Movements.Count;
            var ultimos30 = Movements.Count(m => m.CanceledAt >= DateTime.Today.AddDays(-30));
            var canceladas = Movements.Count(m => m.IsCanceled);

            container.Column(column =>
            {
                column.Item().Text("📤 Relatório de Movimentações de Bens")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter()
                    .FontColor(Colors.Indigo.Darken2);

                column.Item().PaddingVertical(5);

                column.Item().Row(row =>
                {
                    row.Spacing(25);

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Text($"🏫 Escola: {SchoolName}")
                            .FontSize(12).FontColor(Colors.Grey.Darken2);

                        col.Item().Text($"🗓️ Gerado em: {GeneratedAt:dd/MM/yyyy HH:mm}")
                            .FontSize(10).FontColor(Colors.Grey.Medium);

                        col.Item().Text($"📦 Total de Movimentações: {total}")
                            .FontSize(10).FontColor(Colors.Grey.Medium);
                    });

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Text($"🕒 Últimos 30 dias: {ultimos30}")
                            .FontSize(10).FontColor(Colors.Grey.Medium);

                        col.Item().Text($"❌ Canceladas: {canceladas}")
                            .FontSize(10).FontColor(Colors.Red.Medium);
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
                    columns.RelativeColumn(3); // Bem
                    columns.RelativeColumn(2); // Origem
                    columns.RelativeColumn(2); // Destino
                    columns.RelativeColumn(2); // Responsável
                    columns.RelativeColumn(2); // Data
                    columns.RelativeColumn(3); // Motivo
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Indigo.Lighten4).Padding(5).Text("Bem").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Indigo.Lighten4).Padding(5).Text("Origem").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Indigo.Lighten4).Padding(5).Text("Destino").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Indigo.Lighten4).Padding(5).Text("Responsável").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Indigo.Lighten4).Padding(5).Text("Data").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Indigo.Lighten4).Padding(5).Text("Observações").SemiBold().FontColor(Colors.Black);
                });

                bool isEven = false;

                foreach (var movement in Movements)
                {
                    var bg = isEven ? Colors.Grey.Lighten4 : Colors.White;
                    isEven = !isEven;

                    table.Cell().Background(bg).Padding(5).Text(movement.Asset?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.FromRoom?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.ToRoom?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.Responsible ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.CanceledAt?.ToString("dd/MM/yyyy") ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.CancelReason ?? "-");
                }
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Página ").FontSize(9).FontColor(Colors.Indigo.Darken2);
                text.CurrentPageNumber().FontSize(9).FontColor(Colors.Indigo.Darken2);
                text.Span(" de ").FontSize(9).FontColor(Colors.Indigo.Darken2);
                text.TotalPages().FontSize(9).FontColor(Colors.Indigo.Darken2);
            });
        }
    }
}
