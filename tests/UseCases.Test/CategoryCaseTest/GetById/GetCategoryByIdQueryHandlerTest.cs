using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.CategoryRepository;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetById;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.CategoryCaseTest.GetById
{
    public class GetCategoryByIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnCategoryDto_WhenCategoryExists()
        {
            var category = CategoryBuilder.Build();

            var query = new GetCategoryByIdQuery(category.Id);

            var repository = new CategoryReadOnlyRepositoryBuilder()
                    .WithCategoryExist(category.Id, category)
                    .Build();

            var useCase = new GetCategoryByIdQueryHandler(repository);

            var result = await useCase.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(category.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var category = CategoryBuilder.Build();

            var query = new GetCategoryByIdQuery(category.Id);

            var repository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryNotFound(category.Id)
                .Build();

            var useCase = new GetCategoryByIdQueryHandler(repository);

            var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Handle(query, CancellationToken.None));
            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        }


    }
}

 
