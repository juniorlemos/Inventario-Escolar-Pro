using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase
{
    public interface IInventoryReportGenerator
    {
        byte[] Generate(string schoolName, IEnumerable<Asset> assets, DateTime generatedAt);
    }
}
