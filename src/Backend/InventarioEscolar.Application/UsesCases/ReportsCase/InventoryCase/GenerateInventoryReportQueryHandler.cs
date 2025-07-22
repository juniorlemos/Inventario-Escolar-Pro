using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase
{
    public class GenerateInventoryReportQueryHandler : IRequestHandler<GenerateInventoryReportQuery, byte[]>
    {
        private readonly IAssetReadOnlyRepository _repository;
        private readonly IInventoryReportGenerator _reportGenerator;

        public GenerateInventoryReportQueryHandler(
            IAssetReadOnlyRepository repository,
            IInventoryReportGenerator reportGenerator)
        {
            _repository = repository;
            _reportGenerator = reportGenerator;
        }

        public async Task<byte[]> Handle(GenerateInventoryReportQuery request, CancellationToken cancellationToken)
        {
            var assets = await _repository.GetAllAssetsAsync();

            var schoolName = "Escola Vinculada";
            return _reportGenerator.Generate(schoolName, assets, DateTime.Now);
        }
    }
}
