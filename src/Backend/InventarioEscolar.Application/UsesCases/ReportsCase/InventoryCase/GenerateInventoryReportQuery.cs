using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase
{
    public record GenerateInventoryReportQuery() : IRequest<byte[]>;
}
