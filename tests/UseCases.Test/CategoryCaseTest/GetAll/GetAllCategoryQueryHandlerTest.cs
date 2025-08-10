using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.CategoryRepository;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetAll;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using Shouldly;

namespace UseCases.Test.CategoryCaseTest.GetAll
{
    public class GetAllCategoryQueryHandlerTest
    {

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenCategoriesExist()
        {
        const int pageSize = 10;
        const int page = 1;
        const int totalCount = 20;
        const int itens = 10;

            var categories = CategoryBuilder.BuildList(20);

            var query = new GetAllCategoriesQuery(page, pageSize);

            var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(categories, page, pageSize);
            var useCase = CreateUseCase(categoryReadOnlyRepository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(itens);
            result.TotalCount.ShouldBe(totalCount);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsNullCategories()
        {
            const int page = 1;
            const int pageSize = 10;
            const int expectedTotalCount = 0;

            var query = new GetAllCategoriesQuery(page, pageSize);

            bool categoriesAlreadyExists = false;

            var categories = (IList<Category>?)null;
            var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(categories, page, pageSize, categoriesAlreadyExists);

            var useCase = CreateUseCase(categoryReadOnlyRepository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Items.ShouldBeEmpty();
            result.TotalCount.ShouldBe(expectedTotalCount);
            result.Page.ShouldBe(page);
            result.PageSize.ShouldBe(pageSize);
        }

        private static ICategoryReadOnlyRepository CreateBuildCategoryReadOnlyRepository(
            IList<Category>? categories,
            int page,
            int pageSize,
            bool categoriesAlreadyExists = true
            )
        {
            var categoryReadOnlyRepositoryBuilder = new CategoryReadOnlyRepositoryBuilder();
            return categoriesAlreadyExists
                ? categoryReadOnlyRepositoryBuilder.WithCategoriesExist(categories!, page, pageSize).Build()
                : categoryReadOnlyRepositoryBuilder.WithGetAllReturningNull(page, pageSize).Build();
        }

        private static GetAllCategoriesQueryHandler CreateUseCase(
            ICategoryReadOnlyRepository categoryReadOnlyRepository
            )
        {
            return new GetAllCategoriesQueryHandler(categoryReadOnlyRepository);
        }
    }
}