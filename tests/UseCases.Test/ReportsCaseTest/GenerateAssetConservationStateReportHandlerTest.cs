using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.ReportsCaseTest
{
    public class GenerateAssetConservationStateReportHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldGenerateReport_WhenQueryIsValid()
        {
            // Arrange
            var repository = Substitute.For<IAssetReportReadOnlyRepository>();
            var reportGenerator = Substitute.For<IAssetConservationStateReportGenerator>();
            var currentUserService = Substitute.For<ICurrentUserService>();
            var schoolReadOnlyRepository = Substitute.For<ISchoolReadOnlyRepository>();

            var handler = new GenerateAssetConservationStateReportHandler(
                repository,
                reportGenerator,
                currentUserService,
                schoolReadOnlyRepository
            );

            var query = new GenerateAssetConservationStateReportQuery();

            var fakeAssets = new List<Asset>
        {
            new Asset { Id = 1, Name = "Notebook" }
        };

            var fakeSchool = new School { Id = 1, Name = "Escola Municipal X" };

            currentUserService.SchoolId.Returns(fakeSchool.Id);
            repository.GetAllAssetReport().Returns(Task.FromResult<IEnumerable<Asset>>(fakeAssets));
            schoolReadOnlyRepository.GetById(fakeSchool.Id).Returns(Task.FromResult(fakeSchool));
            var expectedBytes = new byte[] { 1, 2, 3 };
            reportGenerator.Generate(fakeSchool.Name, fakeAssets, Arg.Any<DateTime>()).Returns(expectedBytes);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedBytes, result);
            await repository.Received(1).GetAllAssetReport();
            await schoolReadOnlyRepository.Received(1).GetById(fakeSchool.Id);
            reportGenerator.Received(1).Generate(fakeSchool.Name, fakeAssets, Arg.Any<DateTime>());
        }
    }
}
