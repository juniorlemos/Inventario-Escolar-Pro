using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.AssetMovementRepository;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.AssetMovementCaseTest.GetAll
{
    public class GetAllAssetMovementsQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenAssetMovementsExist()
        {
            var assetMovements = AssetMovementBuilder.BuildList(15);
            var page = 1;
            var pageSize = 10;
            var isCanceled = false;

            var query = new GetAllAssetMovementsQuery(page, pageSize, isCanceled);

            var repository = new AssetMovementReadOnlyRepositoryBuilder()
                .WithAssetMovementsExist(assetMovements, page, pageSize, isCanceled)
                .Build();

            var handler = new GetAllAssetMovementsQueryHandler(repository);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(10);
            result.TotalCount.ShouldBe(15);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNull()
        {
            var page = 1;
            var pageSize = 10;
            var isCanceled = false;

            var query = new GetAllAssetMovementsQuery(page, pageSize, isCanceled);

            var repository = new AssetMovementReadOnlyRepositoryBuilder()
                .WithGetAllReturningNull(page, pageSize, isCanceled)
                .Build();

            var handler = new GetAllAssetMovementsQueryHandler(repository);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }
    }
}
