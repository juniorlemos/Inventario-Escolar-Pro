using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.CategoryRepository;
using CommonTestUtilities.Services;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.CategoryCase.Delete;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;

namespace UseCases.Test.CategoryCaseTest.Delete
{
    public class DeleteCategoryCommandHandlerTest
    { 
  
        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenCategoryDeletedSuccessfully()
        {
            var category = CategoryBuilder.Build();

            var command = new DeleteCategoryCommand(category.Id);
            var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(true, category);
            var categoryDeleteRepository = CreateBuildCategoryDeleteOnlyRepository(category.Id);

            var currentUser = new CurrentUserServiceBuilder().WithSchoolId(category.SchoolId).Build();
            var handler = CreateHandler(categoryDeleteRepository, categoryReadOnlyRepository, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var category = CategoryBuilder.Build();

            var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(false, category);
            var categoryDeleteRepository = CreateBuildCategoryDeleteOnlyRepository(category.Id);
            var currentUser = new CurrentUserServiceBuilder().WithSchoolId(category.SchoolId).Build();

            var command = new DeleteCategoryCommand(category.Id);
           
            var handler = CreateHandler(categoryDeleteRepository, categoryReadOnlyRepository, currentUser);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenCategoryHasAssets()
        {
            var category = CategoryBuilder.Build();
            category.Assets = AssetBuilder.BuildList(5);
            
            var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(true, category);
            var categoryDeleteRepository = CreateBuildCategoryDeleteOnlyRepository(category.Id);
            var currentUser = new CurrentUserServiceBuilder().WithSchoolId(category.SchoolId).Build();

            var command = new DeleteCategoryCommand(category.Id);

            var handler = CreateHandler(categoryDeleteRepository, categoryReadOnlyRepository, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_HAS_ASSETS);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenCategoryDoesNotBelongToCurrentUserSchool()
        {
            var category = CategoryBuilder.Build();
            var schoolId = SchoolBuilder.Build().Id + category.Id;

            var command = new DeleteCategoryCommand(category.Id);

            var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(true, category);
            var categoryDeleteRepository = CreateBuildCategoryDeleteOnlyRepository(category.Id);
            var currentUser = new CurrentUserServiceBuilder().WithSchoolId(schoolId).Build();

            var handler = CreateHandler(categoryDeleteRepository, categoryReadOnlyRepository, currentUser); 

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);
        }
      
        private static ICategoryDeleteOnlyRepository CreateBuildCategoryDeleteOnlyRepository(long categoryId)
        {
            return new CategoryDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(categoryId)
                .Build();
        }
        private static ICategoryReadOnlyRepository CreateBuildCategoryReadOnlyRepository(
            bool categoryAlreadyExists,
            Category category)
        {
            var categoryReadOnlyRepositoryBuilder = new CategoryReadOnlyRepositoryBuilder();
            return categoryAlreadyExists
                ? categoryReadOnlyRepositoryBuilder.WithCategoryExist(category.Id, category).Build()
                : categoryReadOnlyRepositoryBuilder.WithCategoryNotExist(category.Id).Build();
        }
        private static DeleteCategoryCommandHandler CreateHandler(
                                             ICategoryDeleteOnlyRepository categoryDeleteRepository,
                                             ICategoryReadOnlyRepository categoryReadOnlyRepository,
                                             ICurrentUserService currentUser 
                                             )
        {
          
            var unitOfWork = new UnitOfWorkBuilder().Build();

            return new DeleteCategoryCommandHandler(categoryDeleteRepository, categoryReadOnlyRepository, unitOfWork, currentUser);
        }
    }
}