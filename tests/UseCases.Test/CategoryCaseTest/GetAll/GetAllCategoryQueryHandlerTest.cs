using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Repositories.CategoryRepository;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetAll;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.CategoryCaseTest.GetAll
{
    public class GetAllCategoryQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenCategoriesExist()
        {
            var categories = CategoryBuilder.BuildList(20);
            var page = 1;
            var pageSize = 10;

            var query = new GetAllCategoriesQuery(page, pageSize);

            var repository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoriesExist(categories, page, pageSize)
                .Build();

            var useCase = new GetAllCategoriesQueryHandler(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(10);
            result.TotalCount.ShouldBe(20);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNullCategories()

        {
            var page = 1;
            var pageSize = 10;

            var query = new GetAllCategoriesQuery(page, pageSize);

            var repository = new CategoryReadOnlyRepositoryBuilder()
                .WithGetAllReturningNull(page, pageSize)
                .Build();

            var useCase = new GetAllCategoriesQueryHandler(repository);
            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(0);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }
    }
}

