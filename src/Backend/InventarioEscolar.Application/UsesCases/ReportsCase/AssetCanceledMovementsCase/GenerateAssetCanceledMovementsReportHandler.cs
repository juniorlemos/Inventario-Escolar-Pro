using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase
{
    public class GenerateAssetCanceledMovementsReportHandler(
        IAssetMovementReportReadOnlyRepository repository,
        IAssetCanceledMovementsReportGenerator reportGenerator,
        ICurrentUserService currentUser,
        ISchoolReadOnlyRepository schoolReadOnlyRepository) : IRequestHandler<GenerateAssetCanceledMovementsReportQuery, byte[]>
    {
        public async Task<byte[]> Handle(GenerateAssetCanceledMovementsReportQuery request, CancellationToken cancellationToken)
        {
            var movements = await repository.GetAllAssetCanceledMovementsReport();

            var schoolId = currentUser.SchoolId;

            var schoolName = await schoolReadOnlyRepository.GetById(schoolId);
           
            return reportGenerator.Generate(schoolName?.Name , movements, DateTime.Now);
        }
    }
}