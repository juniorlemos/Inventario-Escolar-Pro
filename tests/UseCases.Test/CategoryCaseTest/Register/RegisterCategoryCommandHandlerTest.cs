using CommonTestUtilities.Dtos;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.CategoryRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.CategoryCaseTest.Register
{
public class RegisterCategoryCommandHandlerTest
{
    [Fact]
    public async Task Handle_ShouldRegisterCategory_WhenValidAndUserIsAuthenticated()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

        var unitOfWork = new UnitOfWorkBuilder().Build();

        var currentUser = new CurrentUserServiceBuilder()
            .IsAuthenticatedTrue()
            .WithSchoolId(categoryDto.SchoolId)
            .Build();

        var validator = new ValidatorBuilder<CategoryDto>()
            .WithValidResult()
            .Build();

        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
            .WithCategoryNameNotExists(categoryDto.Name, currentUser.SchoolId)
            .Build();

        var categoryWriteOnlyRepository = new CategoryWriteOnlyRepositoryBuilder().Build();

        var useCase = CreateUseCase(
            unitOfWork,
            validator,
            categoryReadOnlyRepository,
            categoryWriteOnlyRepository,
            currentUser
        );

        var result = await useCase.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(categoryDto.Name);

        await categoryWriteOnlyRepository.Received(1).Insert(Arg.Any<Category>());
        await unitOfWork.Received(1).Commit();
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenCategoryDtoIsInvalid()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

        var unitOfWork = new UnitOfWorkBuilder().Build();

        var currentUser = new CurrentUserServiceBuilder()
            .IsAuthenticatedTrue()
            .WithSchoolId(categoryDto.SchoolId)
            .Build();

        var validator = new ValidatorBuilder<CategoryDto>()
            .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
            .Build();

        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
            .WithCategoryNameNotExists(categoryDto.Name, currentUser.SchoolId)
            .Build();

        var categoryWriteOnlyRepository = new CategoryWriteOnlyRepositoryBuilder().Build();

        var useCase = CreateUseCase(unitOfWork, validator, categoryReadOnlyRepository, categoryWriteOnlyRepository, currentUser);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(
            () => useCase.Handle(command, CancellationToken.None));

        exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
    }

    [Fact]
    public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

        var unitOfWork = new UnitOfWorkBuilder().Build();

        var currentUser = new CurrentUserServiceBuilder()
            .IsAuthenticatedFalse()
            .Build();

        var validator = new ValidatorBuilder<CategoryDto>()
            .WithValidResult()
            .Build();

        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
            .WithCategoryNameNotExists(categoryDto.Name, categoryDto.SchoolId)
            .Build();

        var categoryWriteOnlyRepository = new CategoryWriteOnlyRepositoryBuilder().Build();

        var useCase = CreateUseCase(unitOfWork, validator, categoryReadOnlyRepository, categoryWriteOnlyRepository, currentUser);

        var exception = await Should.ThrowAsync<BusinessException>(
            () => useCase.Handle(command, CancellationToken.None));

        exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
    }

    [Fact]
    public async Task Handle_ShouldThrowDuplicateEntityException_WhenCategoryNameAlreadyExists()
    {
        var categoryDto = CategoryDtoBuilder.Build();
        var command = new RegisterCategoryCommand(categoryDto);

        var unitOfWork = new UnitOfWorkBuilder().Build();

        var currentUser = new CurrentUserServiceBuilder()
            .IsAuthenticatedTrue()
            .WithSchoolId(categoryDto.SchoolId)
            .Build();

        var validator = new ValidatorBuilder<CategoryDto>()
            .WithValidResult()
            .Build();

        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
            .WithCategoryNameExists(categoryDto.Name, currentUser.SchoolId)
            .Build();

        var categoryWriteOnlyRepository = new CategoryWriteOnlyRepositoryBuilder().Build();

        var useCase = CreateUseCase(unitOfWork, validator, categoryReadOnlyRepository, categoryWriteOnlyRepository, currentUser);

        var exception = await Should.ThrowAsync<DuplicateEntityException>(
            () => useCase.Handle(command, CancellationToken.None));

        exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_NAME_ALREADY_EXISTS);
    }

    private static RegisterCategoryCommandHandler CreateUseCase(
        IUnitOfWork unitOfWork,
        IValidator<CategoryDto> validator,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ICategoryWriteOnlyRepository categoryWriteOnlyRepository,
        ICurrentUserService currentUser)
    {
        return new RegisterCategoryCommandHandler(
            unitOfWork,
            validator,
            categoryReadOnlyRepository,
            categoryWriteOnlyRepository,
            currentUser);
    }
 }
}