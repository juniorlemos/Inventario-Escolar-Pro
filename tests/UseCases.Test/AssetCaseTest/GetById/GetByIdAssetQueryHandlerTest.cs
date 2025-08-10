using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.AssetRepository;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.AssetCaseTest.GetById
{
    public class GetByIdAssetQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnAssetDto_WhenAssetExists()
        {
            var asset = AssetBuilder.Build();
            var query = new GetByIdAssetQuery(asset.Id);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var useCase = CreateUseCase(assetReadOnlyRepository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(asset.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
            
            var asset = AssetBuilder.Build();
            var query = new GetByIdAssetQuery(asset.Id);
            
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(false, asset);
            var useCase = CreateUseCase(assetReadOnlyRepository);

            
            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                useCase.Handle(query, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        private static IAssetReadOnlyRepository CreateAssetReadOnlyRepository(bool assetAlreadyExists, Asset asset)
        {
            var assetReadOnlyRepositoryBuilder = new AssetReadOnlyRepositoryBuilder();
            return assetAlreadyExists
                ? assetReadOnlyRepositoryBuilder.WithAssetExist(asset.Id, asset).Build()
                : assetReadOnlyRepositoryBuilder.WithAssetNotFound(asset.Id).Build();
        }
        private static GetByIdAssetQueryHandler CreateUseCase(IAssetReadOnlyRepository assetReadRepository)
        {
            return new GetByIdAssetQueryHandler(assetReadRepository);
        }
    }
}