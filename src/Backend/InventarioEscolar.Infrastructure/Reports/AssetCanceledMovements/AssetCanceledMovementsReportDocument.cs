using InventarioEscolar.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Infrastructure.Reports.AssetCanceledMovements
{
    public class AssetCanceledMovementsReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public IEnumerable<AssetMovement> Movements { get; set; }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            // Filtra só as movimentações canceladas
            var canceledMovements = Movements.Where(m => m.IsCanceled).ToList();

            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri).FontSize(10).FontColor(Colors.Grey.Darken2));

                page.Header().ShowOnce().Element(c => ComposeHeader(c, canceledMovements.Count));
                page.Content().PaddingVertical(10).Element(c => ComposeContent(c, canceledMovements));
                page.Footer().Element(ComposeFooter);
            });
        }

        void ComposeHeader(IContainer container, int totalCanceled)
        {
            container.Column(column =>
            {
                column.Item().Text("❌ Relatório de Movimentações Patrimoniais Canceladas")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter()
                    .FontColor(Colors.Red.Darken2);

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

                        col.Item().Text($"📦 Total de Movimentações Canceladas: {totalCanceled}")
                            .FontSize(10).FontColor(Colors.Grey.Darken2)
                            .Bold();
                    });

                    // Você pode adicionar outras métricas específicas aqui se quiser
                });
            });
        }

        void ComposeContent(IContainer container, List<AssetMovement> canceledMovements)
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
                    columns.RelativeColumn(2); // Data da Movimentação
                    columns.RelativeColumn(2); // Data do Cancelamento (opcional)
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Red.Lighten4).Padding(5).Text("Código").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Red.Lighten4).Padding(5).Text("Patrimônio").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Red.Lighten4).Padding(5).Text("Origem").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Red.Lighten4).Padding(5).Text("Destino").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Red.Lighten4).Padding(5).Text("Responsável").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Red.Lighten4).Padding(5).Text("Movimentação").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Red.Lighten4).Padding(5).Text("Cancelamento").SemiBold().FontColor(Colors.Black);
                });

                bool isEven = false;

                foreach (var movement in canceledMovements)
                {
                    var bg = isEven ? Colors.Green.Lighten5 : Colors.White;
                    isEven = !isEven;

                    table.Cell().Background(bg).Padding(5).Text(movement.Asset?.PatrimonyCode?.ToString() ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.Asset?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.FromRoom?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.ToRoom?.Name ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.Responsible ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.MovedAt.ToString("dd/MM/yyyy") ?? "-");
                    table.Cell().Background(bg).Padding(5).Text(movement.CanceledAt?.ToString("dd/MM/yyyy") ?? "-");
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
