using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.AssetRepository;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.AssetCaseTest.GetById
{
    public class GetByIdAssetQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnAssetDto_WhenAssetExists()
        {
            var asset = AssetBuilder.Build();

            var query = new GetByIdAssetQuery(asset.Id);

            var repository = new AssetReadOnlyRepositoryBuilder()
                    .WithAssetExist(asset.Id, asset)
                    .Build();

            var useCase = new GetByIdAssetQueryHandler(repository);

            var result = await useCase.Handle(query, CancellationToken.None);


            result.ShouldNotBeNull();
            result.Id.ShouldBe(asset.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
            var asset = AssetBuilder.Build();

            var query = new GetByIdAssetQuery(asset.Id);

            var repository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetNotFound(asset.Id)
                .Build();

            var useCase = new GetByIdAssetQueryHandler(repository);

            var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Handle(query, CancellationToken.None));
            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }


    }
}
