using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase
{
    public class GenerateAssetMovementsReportHandler : IRequestHandler<GenerateAssetMovementsReportQuery, byte[]>
    {
        private readonly IAssetMovementReadOnlyRepository _repository;
        private readonly IAssetMovementsReportGenerator _reportGenerator;

        public GenerateAssetMovementsReportHandler(
            IAssetMovementReadOnlyRepository repository,
            IAssetMovementsReportGenerator reportGenerator)
        {
            _repository = repository;
            _reportGenerator = reportGenerator;
        }

        public async Task<byte[]> Handle(GenerateAssetMovementsReportQuery request, CancellationToken cancellationToken)
        {
            var movements = await _repository.GetAllWithDetailsAsync();

            var schoolName = "Escola Vinculada"; // você pode substituir isso por algo dinâmico futuramente
            return _reportGenerator.Generate(schoolName, movements, DateTime.Now);
        }
    }
}

