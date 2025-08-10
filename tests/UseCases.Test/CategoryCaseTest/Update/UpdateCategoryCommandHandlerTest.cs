using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.CategoryRepository;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.CategoryCaseTest.Update
{
    public class UpdateCategoryCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldUpdateCategory_WhenAllFieldsAreValidAndUserIsAuthenticated()
        {
            var category = CategoryBuilder.Build();
            
            var updateCategoryDto = UpdateCategoryDtoBuilder.Build();

            var command = new UpdateCategoryCommand(category.Id, updateCategoryDto);

            var validator = CreateValidator<UpdateCategoryDto>(isValid: true);

            var userAuthenticated = CreateCurrentUserService(true, category.SchoolId);
            var categoryReadOnlyRepository = CreateBuildCategoryReadRepository(true, category);

            var handler = CreateUseCase( validator, userAuthenticated, categoryReadOnlyRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var category = CategoryBuilder.Build();

            var updateCategoryDto = UpdateCategoryDtoBuilder.Build();
             
            updateCategoryDto.Name = category.Name;
            updateCategoryDto.Description = category.Description!;

            var userAuthenticated = CreateCurrentUserService(true, category.SchoolId);

            var categoryReadOnlyRepository = CreateBuildCategoryReadRepository(false, category);
            var command = new UpdateCategoryCommand(category.Id, updateCategoryDto);

            var validator = CreateValidator<UpdateCategoryDto>(isValid: true);

            var handler = CreateUseCase( validator , userAuthenticated, categoryReadOnlyRepository);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenCategoryDoesNotBelongToSchool()
        {
            var category = CategoryBuilder.Build();
            var updateCategoryDto = UpdateCategoryDtoBuilder.Build();

            var command = new UpdateCategoryCommand(category.Id, updateCategoryDto);

            var validator = CreateValidator<UpdateCategoryDto>(isValid: true);
            var userAuthenticated = CreateCurrentUserService(false);

            var categoryReadOnlyRepository = CreateBuildCategoryReadRepository(true, category);
           
            var handler = CreateUseCase( validator, userAuthenticated, categoryReadOnlyRepository);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var category = CategoryBuilder.Build();
            var updateCategoryDto = UpdateCategoryDtoBuilder.Build();

            var command = new UpdateCategoryCommand(category.Id, updateCategoryDto);

            var validator = CreateValidator<UpdateCategoryDto>(isValid: false, ResourceMessagesException.NAME_EMPTY);

            var userAuthenticated = CreateCurrentUserService(true, category.SchoolId);

            var categoryReadOnlyRepository = CreateBuildCategoryReadRepository(true, category);
            var handler = CreateUseCase(validator, userAuthenticated, categoryReadOnlyRepository);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }
        private static ICategoryReadOnlyRepository CreateBuildCategoryReadRepository(bool categoryExists, Category category)
        {
            var builder = new CategoryReadOnlyRepositoryBuilder();

            return categoryExists
                ? builder.WithCategoryExist(category.Id, category).Build()
                : builder.WithCategoryNotExist(category.Id).Build();
        }
        
        private static UpdateCategoryCommandHandler CreateUseCase(
            IValidator<UpdateCategoryDto> validator,
            ICurrentUserService currentUser,
            ICategoryReadOnlyRepository categoryReadOnlyRepository
            )
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            var categoryUpdateRepository = new CategoryUpdateOnlyRepositoryBuilder().Build();

            return new UpdateCategoryCommandHandler(
                unitOfWork,
                validator,
                categoryReadOnlyRepository,
                categoryUpdateRepository,
                currentUser);
        }
    }
}