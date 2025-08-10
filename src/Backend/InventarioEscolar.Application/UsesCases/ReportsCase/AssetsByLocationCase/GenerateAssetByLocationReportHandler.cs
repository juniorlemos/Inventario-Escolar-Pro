using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase
{
    public class GenerateAssetByLocationReportHandler(
        IAssetReportReadOnlyRepository repository,
        IAssetByLocationReportGenerator reportGenerator,
        ICurrentUserService currentUserService,
        ISchoolReadOnlyRepository schoolReadOnlyRepository)
                : IRequestHandler<GenerateAssetByLocationReportQuery, byte[]>
    {
        public async Task<byte[]> Handle(
            GenerateAssetByLocationReportQuery request,
            CancellationToken cancellationToken)
        {
            var assets = await repository.GetAllAssetReport();

            var schoolId = currentUserService.SchoolId;

            var schoolName = await schoolReadOnlyRepository.GetById(schoolId);
          
            return reportGenerator.Generate(schoolName?.Name, assets, DateTime.Now);
        }
    }
}
