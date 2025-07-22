using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase
{
    public class GenerateAssetByLocationReportHandler
        : IRequestHandler<GenerateAssetByLocationReportQuery, byte[]>
    {
        private readonly IAssetReadOnlyRepository _repository;
        private readonly IAssetByLocationReportGenerator _reportGenerator;

        public GenerateAssetByLocationReportHandler(
            IAssetReadOnlyRepository repository,
            IAssetByLocationReportGenerator reportGenerator)
        {
            _repository = repository;
            _reportGenerator = reportGenerator;
        }

        public async Task<byte[]> Handle(
            GenerateAssetByLocationReportQuery request,
            CancellationToken cancellationToken)
        {
            var assets = await _repository.GetAllAssetsWithLocationAsync();

            var schoolName = "Escola Vinculada"; // Pode ser substituído por ICurrentUserService futuramente
            return _reportGenerator.Generate(schoolName, assets, DateTime.Now);
        }
    }
}
