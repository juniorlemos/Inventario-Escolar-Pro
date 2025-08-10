using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.CategoryRepository;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetById;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.CategoryCaseTest.GetById
{
    public class GetCategoryByIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnCategoryDto_WhenCategoryExists()
        {
            var category = CategoryBuilder.Build();

            var query = new GetCategoryByIdQuery(category.Id);
            var categoryReadOnlyRepository = CreateBuildCategoryReadRepository(true, category);

            var useCase = CreateUseCase(categoryReadOnlyRepository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(category.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var category = CategoryBuilder.Build();

            var query = new GetCategoryByIdQuery(category.Id);

            var categoryReadOnlyRepository = CreateBuildCategoryReadRepository(false, category);

            var useCase = CreateUseCase(categoryReadOnlyRepository);

            var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Handle(query, CancellationToken.None));
            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        }

        private static ICategoryReadOnlyRepository CreateBuildCategoryReadRepository(
            bool categoryAlreadyExists,
            Category category)
        {
            var categoryReadOnlyRepositoryBuilder = new CategoryReadOnlyRepositoryBuilder();
            return categoryAlreadyExists
                ? categoryReadOnlyRepositoryBuilder.WithCategoryExist(category.Id, category).Build()
                : categoryReadOnlyRepositoryBuilder.WithCategoryNotExist(category.Id).Build();
        }

        private static GetCategoryByIdQueryHandler CreateUseCase(
                        ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            return new GetCategoryByIdQueryHandler(categoryReadOnlyRepository);
        }
    }
}