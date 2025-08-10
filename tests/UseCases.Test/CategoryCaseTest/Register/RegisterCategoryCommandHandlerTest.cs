using CommonTestUtilities.Dtos;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.CategoryRepository;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;


namespace UseCases.Test.CategoryCaseTest.Register
{
public class RegisterCategoryCommandHandlerTest
{
    [Fact]
    public async Task Handle_ShouldRegisterCategory_WhenValidAndUserIsAuthenticated()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

        var validator = CreateValidator<CategoryDto>(isValid: true);
        var userAuthenticated = CreateCurrentUserService(true, categoryDto.SchoolId);
        var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(false, categoryDto);
        
        var useCase = CreateUseCase(validator,userAuthenticated, categoryReadOnlyRepository);

        var result = await useCase.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(categoryDto.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenCategoryDtoIsInvalid()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

         var validator = CreateValidator<CategoryDto>(isValid: false, ResourceMessagesException.NAME_EMPTY);
         var userAuthenticated = CreateCurrentUserService(true, categoryDto.SchoolId);
         
         var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(false, categoryDto);
         
         var useCase = CreateUseCase( validator, userAuthenticated, categoryReadOnlyRepository);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(
            () => useCase.Handle(command, CancellationToken.None));

        exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
    }

    [Fact]
    public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

        var userAuthenticated = CreateCurrentUserService(false, categoryDto.SchoolId);
        var validator = CreateValidator<CategoryDto>(isValid: true);
        var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(false, categoryDto);

            var useCase = CreateUseCase (validator, userAuthenticated, categoryReadOnlyRepository);

        var exception = await Should.ThrowAsync<BusinessException>(
            () => useCase.Handle(command, CancellationToken.None));

        exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
    }

    [Fact]
    public async Task Handle_ShouldThrowDuplicateEntityException_WhenCategoryNameAlreadyExists()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

        var validator = CreateValidator<CategoryDto>(isValid: true);
        var authenticatedUser = CreateCurrentUserService(true, categoryDto.SchoolId);
        var categoryReadOnlyRepository = CreateBuildCategoryReadOnlyRepository(true, categoryDto);

        var useCase = CreateUseCase( validator, authenticatedUser, categoryReadOnlyRepository);

        var exception = await Should.ThrowAsync<DuplicateEntityException>(
            () => useCase.Handle(command, CancellationToken.None));

        exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NAME_ALREADY_EXISTS);
    }

        private static ICategoryReadOnlyRepository CreateBuildCategoryReadOnlyRepository(bool categoryAlreadyExists, CategoryDto categoryDto)
        {
            var builder = new CategoryReadOnlyRepositoryBuilder();

            return categoryAlreadyExists
                ? builder.WithCategoryNameExists(categoryDto.Name, categoryDto.SchoolId).Build()
                : builder.WithCategoryNameNotExists(categoryDto.Name, categoryDto.SchoolId).Build();
        }
        private static RegisterCategoryCommandHandler CreateUseCase(
            IValidator<CategoryDto> validator,
            ICurrentUserService currentUser,
            ICategoryReadOnlyRepository categoryReadOnlyRepository
            )
    {
            var unitOfWork = new UnitOfWorkBuilder().Build();

            var categoryWriteOnlyRepository = new CategoryWriteOnlyRepositoryBuilder().Build();

            return new RegisterCategoryCommandHandler(
            unitOfWork,
            validator,
            categoryReadOnlyRepository,
            categoryWriteOnlyRepository,
            currentUser);
    }
 }
}