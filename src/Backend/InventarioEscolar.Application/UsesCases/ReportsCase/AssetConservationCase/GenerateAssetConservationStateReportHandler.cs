using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase
{
    public class GenerateAssetConservationStateReportHandler
         : IRequestHandler<GenerateAssetConservationStateReportQuery, byte[]>
    {
        private readonly IAssetReadOnlyRepository _repository;
        private readonly IAssetConservationStateReportGenerator _reportGenerator;

        public GenerateAssetConservationStateReportHandler(
            IAssetReadOnlyRepository repository,
            IAssetConservationStateReportGenerator reportGenerator)
        {
            _repository = repository;
            _reportGenerator = reportGenerator;
        }

        public async Task<byte[]> Handle(GenerateAssetConservationStateReportQuery request, CancellationToken cancellationToken)
        {
            var assets = await _repository.GetAllAssetsAsync();

            var schoolName = "Escola Vinculada"; // Substituir futuramente por valor dinâmico
            return _reportGenerator.Generate(schoolName, assets, DateTime.Now);
        }
    }
}
