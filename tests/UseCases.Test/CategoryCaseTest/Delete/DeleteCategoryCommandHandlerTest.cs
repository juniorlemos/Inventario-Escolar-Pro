using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.CategoryRepository;
using CommonTestUtilities.Services;
using InventarioEscolar.Application.UsesCases.CategoryCase.Delete;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.CategoryCaseTest.Delete
{
    public class DeleteCategoryCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenCategoryDeletedSuccessfully()
        {
            var category = CategoryBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(category.SchoolId)
                .Build();

            var categoryReadRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryExist(category.Id, category)
                .Build();

            var categoryDeleteRepository = new CategoryDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(category.Id)
                .Build();

            var command = new DeleteCategoryCommand(category.Id);

            var handler = new DeleteCategoryCommandHandler(
                categoryDeleteRepository, categoryReadRepository, unitOfWork, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var category = CategoryBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(category.SchoolId)
                .Build();

            var categoryReadRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryNotFound(category.Id)
                .Build();

            var categoryDeleteRepository = new CategoryDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(category.Id)
                .Build();

            var command = new DeleteCategoryCommand(category.Id);

            var handler = new DeleteCategoryCommandHandler(
                categoryDeleteRepository, categoryReadRepository, unitOfWork, currentUser);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenCategoryHasAssets()
        {
            var assets = AssetBuilder.BuildList(5);
            var categories = CategoryBuilder.BuildList(20);

            categories.ForEach(c => c.Assets = assets);
            var category = categories.First();
            

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(category.SchoolId)
                .Build();

            var categoryReadRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryExist(category.Id, category)
                .Build();

            var categoryDeleteRepository = new CategoryDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(category.Id)
                .Build();

            var command = new DeleteCategoryCommand(category.Id);

            var handler = new DeleteCategoryCommandHandler(
                categoryDeleteRepository, categoryReadRepository, unitOfWork, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_HAS_ASSETS);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenCategoryDoesNotBelongToCurrentUserSchool()
        {
            var category = CategoryBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder().Build();

            var categoryReadRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryExist(category.Id, category)
                .Build();

            var categoryDeleteRepository = new CategoryDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(category.Id)
                .Build();

            var command = new DeleteCategoryCommand(category.Id);

            var handler = new DeleteCategoryCommandHandler(
                categoryDeleteRepository, categoryReadRepository, unitOfWork, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);
        }
    }
}
