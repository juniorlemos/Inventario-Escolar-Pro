using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.Inventory
{
    public class InventoryReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public IEnumerable<Asset> Assets { get; set; } 

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

            var totalDanificados = Assets.Count(a => a.ConservationState == ConservationState.Danificado);

            var totalIrrecuperavel = Assets.Count(a => a.ConservationState == ConservationState.Irrecuperavel);

            var itensNovos = Assets
                .Count(a => a.CreatedOn >= DateTime.Today.AddDays(-30));

            container.Column(column =>
            {
                column.Item().Text("📘 Relatório Patrimonial")
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
                            .FontSize(12)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"🗓️ Gerado em: {GeneratedAt:dd/MM/yyyy - HH:mm}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"📦 Total de Bens: {Assets.Count()}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"📍 Locais: {totalLocais}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();
                    });

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Text($"🗂️ Categorias: {totalCategorias}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"🆕 Novos (últimos 30 dias): {itensNovos}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();

                        col.Item().Text($"⚠️ Danificados: {totalDanificados}")
                            .FontSize(10)
                            .FontColor(Colors.Yellow.Darken1)
                            .Bold();

                        col.Item().Text($"❌ Irrecuperáveis: {totalIrrecuperavel}")
                            .FontSize(10)
                            .FontColor(Colors.Red.Darken1)
                            .Bold();
                    });
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            var padding = 2;

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(50); // Código
                    columns.RelativeColumn(3);  // Patrimônio
                    columns.RelativeColumn(2);  // Local
                    columns.RelativeColumn(2);  // Categoria
                    columns.RelativeColumn(2);  // Status
                    columns.RelativeColumn(2);  // Preço
                    columns.RelativeColumn(2);  // Descrição
                    columns.RelativeColumn(2);  // Entrada
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Código").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Patrimônio").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Local").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Categoria").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Estado").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Preço").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Descrição").SemiBold().FontColor(Colors.Black);
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Data de Cadastro").SemiBold().FontColor(Colors.Black);

                });

                bool isEven = false;
                foreach (var asset in Assets)
                {
                    var background = isEven ? Colors.Green.Lighten5 : Colors.White;
                    isEven = !isEven;

                    table.Cell().Background(background).Padding(padding).Text(asset.PatrimonyCode.ToString());
                    table.Cell().Background(background).Padding(padding).Text(asset.Name ?? "-");
                    table.Cell().Background(background).Padding(padding).Text(asset.RoomLocation?.Name ?? "-");
                    table.Cell().Background(background).Padding(padding).Text(asset.Category?.Name ?? "-");
                    table.Cell().Background(background).Padding(padding).Text(asset.ConservationState.ToString() ?? "-");
                    table.Cell().Background(background).Padding(padding).Text(asset.AcquisitionValue.HasValue ? asset.AcquisitionValue.Value.ToString("C2") : "-");
                    table.Cell().Background(background).Padding(padding).Text(asset.Description ?? "-");
                    table.Cell().Background(background).Padding(padding).Text(asset.CreatedOn.ToString("dd/MM/yyyy"));
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
