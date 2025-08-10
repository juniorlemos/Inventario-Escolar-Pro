using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetMovements
{
    public class AssetMovementsReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public IEnumerable<AssetMovement> Movements { get; set; }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri).FontSize(10).FontColor(Colors.Grey.Darken2));

                page.Header().ShowOnce().Element(ComposeHeader);
                page.Content().PaddingVertical(10).Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        void ComposeHeader(IContainer container)
        {
            var total = Movements.Count();
            var ultimos30 = Movements.Count(m => m.CreatedOn >= DateTime.Today.AddDays(-30));
            var canceladas = Movements.Count(m => m.IsCanceled);

            container.Column(column =>
            {
                column.Item().Text("📤 Relatório de Movimentação Patrimonial")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter()
                    .FontColor(Colors.Green.Darken2);

                column.Item().PaddingVertical(5);

                column.Item().Row(row =>
                {
                    row.Spacing(25);

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Text($"🏫 Escola: {SchoolName}")
                            .FontSize(12).FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"🗓️ Gerado em: {GeneratedAt:dd/MM/yyyy - HH:mm}")
                            .FontSize(10).FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"📦 Total de Movimentações: {total}")
                            .FontSize(10).FontColor(Colors.Grey.Darken2)
                            .Bold();
                    });

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Text($"🕒 Últimos 30 dias: {ultimos30}")
                            .FontSize(10).FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"❌ Canceladas: {canceladas}")
                            .FontSize(10).FontColor(Colors.Red.Darken2)
                            .Bold();
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
                    columns.RelativeColumn(2); // Código
                    columns.RelativeColumn(3); // Patrimônio
                    columns.RelativeColumn(2); // Origem
                    columns.RelativeColumn(2); // Destino
                    columns.RelativeColumn(2); // Responsável
                    columns.RelativeColumn(2); // Data
                   
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Código").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Patrimônio").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Origem").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Destino").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Responsável").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Data").SemiBold().FontColor(Colors.Black);
                });

                bool isEven = false;

                foreach (var movement in Movements)
                {
                    var bg = isEven ? Colors.Green.Lighten5 : Colors.White;
                    isEven = !isEven;

                    table.Cell().Background(bg).Padding(5).Text(movement.Asset?.PatrimonyCode?.ToString() ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.Asset?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.FromRoom?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.ToRoom?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.Responsible ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.MovedAt.ToString("dd/MM/yyyy") ?? "-");
                }
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Página ").FontSize(9).FontColor(Colors.Green.Darken2);
                text.CurrentPageNumber().FontSize(9).FontColor(Colors.Green.Darken2);
                text.Span(" de ").FontSize(9).FontColor(Colors.Green.Darken2);
                text.TotalPages().FontSize(9).FontColor(Colors.Green.Darken2);
            });
        }
    }
}