using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase
{
    public class GenerateAssetByCategoryReportHandler
        : IRequestHandler<GenerateAssetByCategoryReportQuery, byte[]>
    {
        private readonly IAssetReadOnlyRepository _repository;
        private readonly IAssetByCategoryReportGenerator _reportGenerator;

        public GenerateAssetByCategoryReportHandler(
            IAssetReadOnlyRepository repository,
            IAssetByCategoryReportGenerator reportGenerator)
        {
            _repository = repository;
            _reportGenerator = reportGenerator;
        }

        public async Task<byte[]> Handle(
            GenerateAssetByCategoryReportQuery request,CancellationToken cancellationToken)
        {
            var assets = await _repository.GetAllWithCategoryAsync();

            var schoolName = "Escola Vinculada"; // TODO: tornar dinâmico (ICurrentUserService)
            return _reportGenerator.Generate(schoolName, assets, DateTime.Now);
        }
    }
}

