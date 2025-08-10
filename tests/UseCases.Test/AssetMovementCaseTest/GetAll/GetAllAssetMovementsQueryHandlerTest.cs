using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.AssetMovementRepository;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using Shouldly;

namespace UseCases.Test.AssetMovementCaseTest.GetAll
{
    public class GetAllAssetMovementsQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenAssetMovementsExist()
        {
            const int page = 1;
            const int pageSize = 10;
            const int totalCount = 15;
            const bool isCanceled = false;
            const int expectedItems = 10;

            var assetMovements = AssetMovementBuilder.BuildList(totalCount);
            var query = new GetAllAssetMovementsQuery(page, pageSize, isCanceled);
            var assetMovementsRepository = CreateRepository(assetMovements, page, pageSize, isCanceled);
           
            var useCase = CreateUseCase(assetMovementsRepository);

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
            const bool isCanceled = false;

            var query = new GetAllAssetMovementsQuery(page, pageSize, isCanceled);
            IList<AssetMovement>? assetMovements = null;
            var assetMovementsAlreadyExist = false;

            var assetMovementsRepository = CreateRepository(assetMovements, page, pageSize, isCanceled, assetMovementsAlreadyExist);
            var useCase = CreateUseCase(assetMovementsRepository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        private static IAssetMovementReadOnlyRepository CreateRepository(
            IList<AssetMovement>? assetMovements,
            int page,
            int pageSize,
            bool isCanceled,
            bool assetMovementsAlreadyExist = true)
        {
            var builder = new AssetMovementReadOnlyRepositoryBuilder();
            return assetMovementsAlreadyExist
                ? builder.WithAssetMovementsExist(assetMovements!, page, pageSize, isCanceled).Build()
                : builder.WithGetAllReturningNull(page, pageSize, isCanceled).Build();
        }
        private static GetAllAssetMovementsQueryHandler CreateUseCase(
            IAssetMovementReadOnlyRepository assetMovements
            )
        {
            return new GetAllAssetMovementsQueryHandler(assetMovements);
        }
    }
}