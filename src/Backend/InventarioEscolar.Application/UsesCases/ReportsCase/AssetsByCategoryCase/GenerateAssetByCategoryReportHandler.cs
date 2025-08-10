using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase
{
    public class GenerateAssetByCategoryReportHandler(
        IAssetReportReadOnlyRepository repository,
        IAssetByCategoryReportGenerator reportGenerator,
        ICurrentUserService currentUser,
        ISchoolReadOnlyRepository schoolReadOnlyRepository)
                : IRequestHandler<GenerateAssetByCategoryReportQuery, byte[]>
    {
        public async Task<byte[]> Handle(
            GenerateAssetByCategoryReportQuery request,CancellationToken cancellationToken)
        {
            var assets = await repository.GetAllAssetReport();

            var schoolId = currentUser.SchoolId;

            var schoolName = await schoolReadOnlyRepository.GetById(schoolId);
            return reportGenerator.Generate(schoolName?.Name, assets, DateTime.Now);
        }
    }
}

