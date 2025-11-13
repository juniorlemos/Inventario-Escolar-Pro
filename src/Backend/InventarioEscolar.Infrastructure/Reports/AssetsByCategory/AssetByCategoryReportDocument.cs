using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventarioEscolar.Infrastructure.Reports.AssetsByCategory
{
    public class AssetByCategoryReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public IEnumerable<Asset> Assets { get; set; } = [];
        public DateTime GeneratedAt { get; set; }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Calibri));

                page.Header().ShowOnce().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        void ComposeHeader(IContainer container)
        {
            var totalLocais = Assets
                .Select(a => a.RoomLocation?.Name)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct()
                .Count();

            var totalCategorias = Assets
                .Select(a => a.Category?.Name)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct()
                .Count();

            var totalDanificados = Assets.Count(a => a.ConservationState == ConservationState.Danificado);
            var totalIrrecuperavel = Assets.Count(a => a.ConservationState == ConservationState.Irrecuperavel);

            var itensNovos = Assets
                .Count(a => a.CreatedOn >= DateTime.Today.AddDays(-30));

            container.Column(column =>
            {

                column.Item().Text("Relatório de Patrimônio por Categoria")
                    .FontSize(20).AlignCenter().Bold().FontColor(Colors.Green.Darken2);

                column.Item().PaddingVertical(5);


                column.Item().Row(row =>
                {
                    row.Spacing(25);

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Row(row2 =>
                        {
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("escola.png")).FitArea();
                            row2.Spacing(3);

                            row2.RelativeItem().Text($"Escola: {SchoolName}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();
                        });
                        col.Item().Row(row2 =>
                        {
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("calendario.png")).FitArea();
                            row2.Spacing(3);

                            row2.RelativeItem().Text($"Gerado em: {GeneratedAt:dd/MM/yyyy - HH:mm}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2)
                                .Bold();
                        });

                        col.Item().Row(row2 =>
                        {
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("itens.png")).FitArea();
                            row2.Spacing(3);

                            row2.RelativeItem().Text($"Total de Bens: {Assets.Count()}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();
                        });

                        col.Item().Row(row2 =>
                        {
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("categoria.png")).FitArea();
                            row2.Spacing(3);
                            row2.RelativeItem().Text($"Total de categorias: {totalCategorias}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();
                        });
                    });

                    row.RelativeItem(1).Column(col =>
                    {

                        col.Item().Row(row2 =>
                        {
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("30-dias.png")).FitArea();
                            row2.Spacing(3);
                            row2.RelativeItem().Text($"Novos bens (últimos 30 dias): {itensNovos}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();
                        });

                        col.Item().Row(row2 =>
                        {
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("danificado.png")).FitArea();
                            row2.Spacing(3);
                            row2.RelativeItem().Text($"Bens danificados: {totalDanificados}")
                            .FontSize(10)
                            .FontColor(Colors.Yellow.Darken1)
                            .Bold();
                        });
                        col.Item().Row(row2 =>
                        {
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("irrecuperavel.png")).FitArea();
                            row2.Spacing(3);
                            row2.RelativeItem().Text($"Bens irrecuperáveis: {totalIrrecuperavel}")
                            .FontSize(10)
                            .FontColor(Colors.Red.Darken1)
                            .Bold();
                        });
                    });
                });

                column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
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
                            columns.RelativeColumn(2); // Código
                            columns.RelativeColumn(3); // Nome
                            columns.RelativeColumn(2); // Série
                            columns.RelativeColumn(2); // Localização
                            columns.RelativeColumn(2); // Estado
                            columns.RelativeColumn(2); // Data de Cadastro

                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Green.Lighten3).Text("Código").SemiBold().FontSize(11);
                            header.Cell().Background(Colors.Green.Lighten3).Text("Patrimônio").SemiBold().FontSize(11);
                            header.Cell().Background(Colors.Green.Lighten3).Text("Série").SemiBold().FontSize(11);
                            header.Cell().Background(Colors.Green.Lighten3).Text("Local").SemiBold().FontSize(11);
                            header.Cell().Background(Colors.Green.Lighten3).Text("Estado").SemiBold().FontSize(11);
                            header.Cell().Background(Colors.Green.Lighten3).Text("Data de Cadastro").SemiBold().FontSize(11);
                        });

                        bool isEven = false;

                        foreach (var asset in group)
                        {
                            var bg = isEven ? Colors.Green.Lighten5 : Colors.White;
                            isEven = !isEven;

                            table.Cell().Background(bg).Text(asset.PatrimonyCode?.ToString() ?? "-");
                            table.Cell().Background(bg).Text(asset.Name);
                            table.Cell().Background(bg).Text(asset.SerieNumber?.ToString() ?? "-");
                            table.Cell().Background(bg).Text(asset.RoomLocation?.Name ?? "-");
                            table.Cell().Background(bg).Text(asset.ConservationState.ToString());
                            table.Cell().Background(bg).Text(asset.CreatedOn.ToString("dd/MM/yyyy"));
                        }

                    });
                    
                    // Espaçamento entre categorias
                    column.Item().PaddingBottom(15);

                    column.Item().PaddingTop(2).AlignRight().Text($"Total de itens: {group.Count()}")
                   .FontSize(10).Italic();
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
        private static string GetImagePath(string imageFileName)
        {
            return Path.Combine(AppContext.BaseDirectory, "Reports", "ImagensRelatorio", imageFileName);
        }
    }
}
