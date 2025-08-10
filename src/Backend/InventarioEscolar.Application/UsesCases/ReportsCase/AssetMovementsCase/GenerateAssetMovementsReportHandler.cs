using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase
{
    public class GenerateAssetMovementsReportHandler(
        IAssetMovementReportReadOnlyRepository repository,
        IAssetMovementsReportGenerator reportGenerator,
        ICurrentUserService currentUser,
        ISchoolReadOnlyRepository schoolReadOnlyRepository) : IRequestHandler<GenerateAssetMovementsReportQuery, byte[]>
    {
        public async Task<byte[]> Handle(GenerateAssetMovementsReportQuery request, CancellationToken cancellationToken)
        {
            var movements = await repository.GetAllAssetMovementsReport();

            var schoolId = currentUser.SchoolId;

            var schoolName = await schoolReadOnlyRepository.GetById(schoolId);

            return reportGenerator.Generate(schoolName?.Name, movements, DateTime.Now);
        }
    }
}

