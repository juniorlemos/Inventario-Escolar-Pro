using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.AssetRepository;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using Shouldly;

namespace UseCases.Test.AssetCaseTest.GetAll
{
    public class GetAllAssetQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenAssetsExist()
        {
            const int page = 1;
            const int pageSize = 10;
            const int totalCount = 20;
            const int expectedItems = 10;

            var assets = AssetBuilder.BuildList(totalCount);
            var query = new GetAllAssetQuery(page, pageSize);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, assets, page, pageSize);
           
            var useCase = CreateUseCase(assetReadOnlyRepository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(expectedItems);
            result.TotalCount.ShouldBe(totalCount);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNull()
        {
            const int page = 1;
            const int pageSize = 10;

            var query = new GetAllAssetQuery(page, pageSize);
            IList<Asset>? assets = null;
            
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(false, assets, page, pageSize);

            var useCase = CreateUseCase(assetReadOnlyRepository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }
        private static IAssetReadOnlyRepository CreateAssetReadOnlyRepository(bool assetsExist, IList<Asset>? assets, int page, int pageSize)
        {
            var builder = new AssetReadOnlyRepositoryBuilder();
            return assetsExist
                ? builder.WithAssetsExist(assets!, page, pageSize).Build()
                : builder.WithGetAllReturningNull(page, pageSize).Build();
        }
        private static GetAllAssetQueryHandler CreateUseCase(
            IAssetReadOnlyRepository assetReadOnlyRepository)
        {
            return new GetAllAssetQueryHandler(assetReadOnlyRepository);
        }
    }
}