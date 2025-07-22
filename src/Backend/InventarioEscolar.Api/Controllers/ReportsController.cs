using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class ReportsController : InventarioApiBaseController
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventoryReport()
        {
            var reportPdfBytes = await _mediator.Send(new GenerateInventoryReportQuery());
            return File(reportPdfBytes, "application/pdf", "inventario.pdf");
        }

        [HttpGet("by-location")]
        public async Task<IActionResult> GetAssetsByLocationReport()
        {
            var reportPdfBytes = await _mediator.Send(new GenerateAssetByLocationReportQuery());
            return File(reportPdfBytes, "application/pdf", "patrimonio-por-setor.pdf");
        }

        [HttpGet("movements")]
        public async Task<IActionResult> GetAssetMovementsReport()
        {
            var reportPdfBytes = await _mediator.Send(new GenerateAssetMovementsReportQuery());
            return File(reportPdfBytes, "application/pdf", "movimentacoes.pdf");
        }

        [HttpGet("by-conservation")]
        public async Task<IActionResult> GetAssetsByConservationReport()
        {
            var reportPdfBytes = await _mediator.Send(new GenerateAssetConservationStateReportQuery());
            return File(reportPdfBytes, "application/pdf", "estado-de-conservacao.pdf");
        }

        [HttpGet("by-category")]
        public async Task<IActionResult> GetAssetsByCategoryReport()
        {
            var reportPdfBytes = await _mediator.Send(new GenerateAssetByCategoryReportQuery());
            return File(reportPdfBytes, "application/pdf", "patrimonio-por-categoria.pdf");
        }
    }
}