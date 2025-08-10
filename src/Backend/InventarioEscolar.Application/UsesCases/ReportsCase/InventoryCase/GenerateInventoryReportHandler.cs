using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase
{
    public class GenerateInventoryReportHandler(
        IAssetReportReadOnlyRepository repository,
        IInventoryReportGenerator reportGenerator,
        ICurrentUserService currentUser,
        ISchoolReadOnlyRepository schoolReadOnlyRepository) : IRequestHandler<GenerateInventoryReportQuery, byte[]>
    {
        public async Task<byte[]> Handle(GenerateInventoryReportQuery request, CancellationToken cancellationToken)
        {
            var assets = await repository.GetAllAssetReport();

            var schoolId = currentUser.SchoolId;

            var schoolName = await schoolReadOnlyRepository.GetById(schoolId);

            return reportGenerator.Generate(schoolName?.Name, assets, DateTime.Now);
        }
    }
}
