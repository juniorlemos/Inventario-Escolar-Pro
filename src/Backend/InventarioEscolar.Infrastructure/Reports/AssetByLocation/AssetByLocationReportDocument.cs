using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace InventarioEscolar.Infrastructure.Reports.AssetByLocation
{
    public class AssetByLocationReportDocument : IDocument
    {
        public string SchoolName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public IEnumerable<Asset> Assets { get; set; } = [];

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

                column.Item().Text("Relatório de Patrimônio por Localização")
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
                            row2.ConstantItem(10).Height(10).Image(GetImagePath("local.png")).FitArea();
                            row2.Spacing(3);

                            row2.RelativeItem().Text($"Totais de locais: {totalLocais}")
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
    var padding = 1;

    var assetsByLocation = Assets
        .GroupBy(a => a.RoomLocation?.Name ?? "Sem Localização")
        .Select(g => new
        {
            LocationName = g.Key,
            Building = g.FirstOrDefault()?.RoomLocation?.Building ?? "Sede",
            Assets = g.ToList()
        })
        .OrderBy(g => g.LocationName);

    container.Column(col =>
    {
        foreach (var group in assetsByLocation)
        {
            col.Item().PaddingTop(10).Text($"{group.Building} - {group.LocationName}")
                .FontSize(12).Bold().FontColor(Colors.Green.Darken2);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // Código
                    columns.RelativeColumn(4); // Nome
                    columns.RelativeColumn(2); // Série
                    columns.RelativeColumn(2); // Categoria
                    columns.RelativeColumn(2); // Estado
                    columns.RelativeColumn(2); // Preço
                    columns.RelativeColumn(2); // Data de Cadastro
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Código").SemiBold();
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Patrimônio").SemiBold();
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Série").SemiBold();
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Categoria").SemiBold();
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Estado").SemiBold();
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Preço").SemiBold();
                    header.Cell().Background(Colors.Green.Lighten3).Padding(padding).Text("Data de Cadastro").SemiBold();
                });

                var assets = group.Assets.OrderBy(a => a.Name).ToList();

                for (int i = 0; i < assets.Count; i++)
                {
                    var asset = assets[i];
                    var bgColor = i % 2 == 0 ? Colors.Grey.Lighten3 : Colors.White;
                    var estado = asset.ConservationState.ToString();
                    var estadoColor = estado == "Ruim" ? Colors.Red.Medium : Colors.Black;

                    table.Cell().Background(bgColor).Padding(padding).Text(asset.PatrimonyCode.ToString() ?? "-");
                    table.Cell().Background(bgColor).Padding(padding).Text(asset.Name);
                    table.Cell().Background(bgColor).Padding(padding).Text(asset.SerieNumber?.ToString() ?? "-");
                    table.Cell().Background(bgColor).Padding(padding).Text(asset.Category?.Name ?? "-");
                    table.Cell().Background(bgColor).Padding(padding).Text(estado).FontColor(estadoColor);
                    table.Cell().Background(bgColor).Padding(padding).Text(asset.AcquisitionValue.HasValue ? asset.AcquisitionValue.Value.ToString("C2", new CultureInfo("pt-BR")) : "-");
                    table.Cell().Background(bgColor).Padding(padding).Text(asset.CreatedOn.ToString("dd/MM/yyyy"));
                }

                table.Cell().PaddingBottom(5); // opcional
            });

            // Total por sala
            col.Item().PaddingTop(2).AlignRight().Text($"Total de itens: {group.Assets.Count}")
                .FontSize(10).Italic();
        }

        // Total geral
        col.Item().PaddingTop(20).AlignRight()
            .Text($"Total geral de ativos: {Assets.Count()}")
            .FontSize(11).SemiBold();
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
