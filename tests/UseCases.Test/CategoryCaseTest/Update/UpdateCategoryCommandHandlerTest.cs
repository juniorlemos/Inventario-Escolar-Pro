using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.CategoryRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UploadCategoryDto>().WithValidResult().Build();
            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(category.SchoolId)
                .Build();

            var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryExist(category.Id, category)
                .Build();

            var categoryUpdateRepository = new CategoryUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(unitOfWork, validator, categoryReadOnlyRepository, categoryUpdateRepository, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            categoryUpdateRepository.Received(1).Update(category);
            await unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            var updateCategoryDto = UpdateCategoryDtoBuilder.Build();

            var id = 999;
            var command = new UpdateCategoryCommand(id, updateCategoryDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UploadCategoryDto>().WithValidResult().Build();
            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(1)
                .Build();

            var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryNotFound(id)
                .Build();

            var categoryUpdateRepository = new CategoryUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(unitOfWork, validator, categoryReadOnlyRepository, categoryUpdateRepository, currentUser);

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

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UploadCategoryDto>().WithValidResult().Build();
            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .Build();

            var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryExist(category.Id, category)
                .Build();

            var categoryUpdateRepository = new CategoryUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(unitOfWork, validator, categoryReadOnlyRepository, categoryUpdateRepository, currentUser);

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

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UploadCategoryDto>()
                .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
                .Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(category.SchoolId)
                .Build();

            var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
                .WithCategoryExist(category.Id, category)
                .Build();

            var categoryUpdateRepository = new CategoryUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(unitOfWork, validator, categoryReadOnlyRepository, categoryUpdateRepository, currentUser);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        private static UpdateCategoryCommandHandler CreateUseCase(
            IUnitOfWork unitOfWork,
            IValidator<UploadCategoryDto> validator,
            ICategoryReadOnlyRepository categoryReadOnlyRepository,
            ICategoryUpdateOnlyRepository categoryUpdateOnlyRepository,
            ICurrentUserService currentUser)
        {
            return new UpdateCategoryCommandHandler(
                unitOfWork,
                validator,
                categoryReadOnlyRepository,
                categoryUpdateOnlyRepository,
                currentUser);
        }
    }
}
