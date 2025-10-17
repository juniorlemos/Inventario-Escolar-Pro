using InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers;
[Authorize]
[Produces("application/pdf")]
public class ReportController(IMediator mediator) : InventarioApiBaseController
{
    private async Task<IActionResult> GenerateReport<TRequest>(TRequest query, string fileName) where TRequest : IRequest<byte[]>
    {
        var reportBytes = await mediator.Send(query);
        if (reportBytes == null || reportBytes.Length == 0)
            return NoContent();

        return File(reportBytes, "application/pdf", fileName);
    }

    [HttpGet("inventory")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetInventoryReport()
        => GenerateReport(new GenerateInventoryReportQuery(), "inventario.pdf");

    [HttpGet("by-location")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAssetsByLocationReport()
        => GenerateReport(new GenerateAssetByLocationReportQuery(), "patrimonio-por-setor.pdf");

    [HttpGet("movements")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAssetMovementsReport()
        => GenerateReport(new GenerateAssetMovementsReportQuery(), "movimentacoes.pdf");

    [HttpGet("canceled-movements")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAssetCanceledMovementsReport()
        => GenerateReport(new GenerateAssetCanceledMovementsReportQuery(), "movimentacoes-canceladas.pdf");

    [HttpGet("by-conservation")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAssetsByConservationReport()
        => GenerateReport(new GenerateAssetConservationStateReportQuery(), "estado-de-conservacao.pdf");

    [HttpGet("by-category")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> GetAssetsByCategoryReport()
        => GenerateReport(new GenerateAssetByCategoryReportQuery(), "patrimonio-por-categoria.pdf");
}
