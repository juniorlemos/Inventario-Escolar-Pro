using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
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
        private readonly IAssetReportReadOnlyRepository _repository;
        private readonly IInventoryReportGenerator _reportGenerator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;

        public GenerateInventoryReportQueryHandler(
            IAssetReportReadOnlyRepository repository,
            IInventoryReportGenerator reportGenerator,
            ICurrentUserService currentUser,
            ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {
            _repository = repository;
            _reportGenerator = reportGenerator;
            _currentUserService = currentUser;
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
        }

        public async Task<byte[]> Handle(GenerateInventoryReportQuery request, CancellationToken cancellationToken)
        {
            var assets = await _repository.GetAllAssetReport();

            var schoolId = _currentUserService.SchoolId;

            var schoolName = await _schoolReadOnlyRepository.GetById(schoolId);

            return _reportGenerator.Generate(schoolName.Name, assets, DateTime.Now);
        }
    }
}
