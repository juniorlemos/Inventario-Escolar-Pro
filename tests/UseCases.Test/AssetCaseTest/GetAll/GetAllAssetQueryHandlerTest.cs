using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.AssetRepository;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.AssetCaseTest.GetAll
{
    public class GetAllAssetQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenAssetsExist()
        {
            var assets = AssetBuilder.BuildList(20);
            var page = 1;
            var pageSize = 10;

            var query = new GetAllAssetQuery(page, pageSize);

            var repository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetsExist(assets, page, pageSize)
                .Build();

            var useCase = new GetAllAssetQueryHandler(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(10);
            result.TotalCount.ShouldBe(20);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNull()
        {
            var page = 1;
            var pageSize = 10;

            var query = new GetAllAssetQuery(page, pageSize);

            var repository = new AssetReadOnlyRepositoryBuilder()
                .WithGetAllReturningNull(page, pageSize)
                .Build();

            var useCase = new GetAllAssetQueryHandler(repository);
            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }
    }
}
