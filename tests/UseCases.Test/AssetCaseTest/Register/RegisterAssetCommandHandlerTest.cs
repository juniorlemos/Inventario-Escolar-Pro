using CommonTestUtilities.Dtos;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetRepository;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.AssetCaseTest.Register
{
    public class RegisterAssetCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldRegisterAsset_WhenAllFieldsAreValidAndUserIsAuthenticated()
        {
            var assetDto = AssetDtoBuilder.Build();
            var command = new RegisterAssetCommand(assetDto);

            var user = CreateCurrentUserService(true, assetDto.SchoolId);
            var validator = CreateValidator<AssetDto>(true);
            var assetReadRepository = CreateAssetReadOnlyRepository(false, assetDto.PatrimonyCode, user.SchoolId);

            var handler = CreateUseCase(validator, assetReadRepository, user);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.PatrimonyCode.ShouldBe(assetDto.PatrimonyCode);

        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenAssetDtoIsInvalid()
        {
            var assetDto = AssetDtoBuilder.Build();
            var command = new RegisterAssetCommand(assetDto);

            var user = CreateCurrentUserService(true, assetDto.SchoolId);
            var validator = CreateValidator<AssetDto>(false, ResourceMessagesException.NAME_EMPTY);
            var assetReadRepository = CreateAssetReadOnlyRepository(false, assetDto.PatrimonyCode, user.SchoolId);

            var handler = CreateUseCase( validator, assetReadRepository, user);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
        {
            var assetDto = AssetDtoBuilder.Build();
            var command = new RegisterAssetCommand(assetDto);

            var user = CreateCurrentUserService(false);
            var validator = CreateValidator<AssetDto>(true);
            var assetReadRepository = CreateAssetReadOnlyRepository(false, assetDto.PatrimonyCode, assetDto.SchoolId);

            var handler = CreateUseCase( validator, assetReadRepository, user);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.USER_NOT_AUTHENTICATED);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenPatrimonyCodeAlreadyExists()
        {
            var assetDto = AssetDtoBuilder.Build();
            var command = new RegisterAssetCommand(assetDto);

            var user = CreateCurrentUserService(true, assetDto.SchoolId);
            var validator = CreateValidator<AssetDto>(true);
            var assetReadRepository = CreateAssetReadOnlyRepository(true, assetDto.PatrimonyCode, user.SchoolId);

            var handler = CreateUseCase( validator, assetReadRepository, user);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.PATRIMONY_CODE_ALREADY_EXISTS_);
        }

        [Fact]
        public async Task Handle_ShouldCallInsertAndCommit_WhenAssetIsValid()
        {
            var assetDto = AssetDtoBuilder.Build();
            var command = new RegisterAssetCommand(assetDto);

            var user = CreateCurrentUserService(true, assetDto.SchoolId);
            var validator = CreateValidator<AssetDto>(true);
            var assetReadRepository = CreateAssetReadOnlyRepository(false, assetDto.PatrimonyCode, user.SchoolId);

            var handler = CreateUseCase( validator, assetReadRepository, user);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.PatrimonyCode.ShouldBe(assetDto.PatrimonyCode);
        }

        private static IAssetReadOnlyRepository CreateAssetReadOnlyRepository(bool exists, long? patrimonyCode, long schoolId)
        {
            var builder = new AssetReadOnlyRepositoryBuilder();

            return exists
                ? builder.WithAssetExistenceTrue(patrimonyCode, schoolId).Build()
                : builder.WithAssetExistenceFalse(patrimonyCode, schoolId).Build();
        }

        private static RegisterAssetCommandHandler CreateUseCase(
            IValidator<AssetDto> validator,
            IAssetReadOnlyRepository assetReadRepository,
            ICurrentUserService user)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            var assetWriteRepository = new AssetWriteOnlyRepositoryBuilder().Build();
           
            return new RegisterAssetCommandHandler(assetWriteRepository, unitOfWork, validator, assetReadRepository, user);
        }
    }
}