using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
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
        private readonly IAssetMovementReportReadOnlyRepository _repository;
        private readonly IAssetMovementsReportGenerator _reportGenerator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;


        public GenerateAssetMovementsReportHandler(
            IAssetMovementReportReadOnlyRepository repository,
            IAssetMovementsReportGenerator reportGenerator,
            ICurrentUserService currentUser,
            ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {
            _repository = repository;
            _reportGenerator = reportGenerator;
            _currentUserService = currentUser;
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
        }

        public async Task<byte[]> Handle(GenerateAssetMovementsReportQuery request, CancellationToken cancellationToken)
        {
            var movements = await _repository.GetAllAssetMovementsReport();

            var schoolId = _currentUserService.SchoolId;

            var schoolName = await _schoolReadOnlyRepository.GetById(schoolId);

            return _reportGenerator.Generate(schoolName.Name, movements, DateTime.Now);
        }
    }
}

