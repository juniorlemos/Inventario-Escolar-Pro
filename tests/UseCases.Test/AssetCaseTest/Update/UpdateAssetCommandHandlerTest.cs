using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetRepository;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.AssetCaseTest.Update
{
    public class UpdateAssetCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldUpdateAsset_WhenAllFieldsAreValidAndUserIsAuthenticated()
        {
            var asset = AssetBuilder.Build();
            var UpdateAssetDto = UpdateAssetDtoBuilder.Build();
            var command = new UpdateAssetCommand(asset.Id, UpdateAssetDto);

            var user = CreateCurrentUserService(true, asset.SchoolId);

            var validator = CreateValidator<UpdateAssetDto>(isValid: true);

            var assetReadRepository = CreateAssetReadOnlyRepository(true, asset);

            var handler = CreateUseCase(validator, assetReadRepository, user);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
            var asset = AssetBuilder.Build();
            var UpdateAssetDto = UpdateAssetDtoBuilder.Build();
            var command = new UpdateAssetCommand(asset.Id, UpdateAssetDto);

            var user = CreateCurrentUserService(true, asset.SchoolId);
            var validator = CreateValidator<UpdateAssetDto>(isValid: true);
            var assetReadRepository = CreateAssetReadOnlyRepository(false, asset);

            var handler = CreateUseCase(validator, assetReadRepository, user);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
        {
            var asset = AssetBuilder.Build();
            var UpdateAssetDto = UpdateAssetDtoBuilder.Build();
            var command = new UpdateAssetCommand(asset.Id, UpdateAssetDto);

            var user = CreateCurrentUserService(false);
            var validator = CreateValidator<UpdateAssetDto>(isValid: true);
            var assetReadRepository = CreateAssetReadOnlyRepository(true, asset);

            var handler = CreateUseCase( validator, assetReadRepository, user);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenUpdateAssetDtoIsInvalid()
        {
            var asset = AssetBuilder.Build();
            var UpdateAssetDto = UpdateAssetDtoBuilder.Build();
            var command = new UpdateAssetCommand(asset.Id, UpdateAssetDto);

            var user = CreateCurrentUserService(true, asset.SchoolId);
            var validator = CreateValidator<UpdateAssetDto>(isValid: false, ResourceMessagesException.NAME_EMPTY);
            var assetReadRepository = CreateAssetReadOnlyRepository(true, asset);

            var handler = CreateUseCase(validator, assetReadRepository, user);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        private static IAssetReadOnlyRepository CreateAssetReadOnlyRepository(bool exists, Asset asset)
        {
            var builder = new AssetReadOnlyRepositoryBuilder();

            return exists
                ? builder.WithAssetExist(asset.Id, asset).Build()
                : builder.WithAssetNotFound(asset.Id).Build();
        }

        private static UpdateAssetCommandHandler CreateUseCase(
            IValidator<UpdateAssetDto> validator,
            IAssetReadOnlyRepository assetReadRepository,
            ICurrentUserService user)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            var assetUpdateRepository = new AssetUpdateOnlyRepositoryBuilder().Build();

            return new UpdateAssetCommandHandler(unitOfWork, validator, assetReadRepository, assetUpdateRepository, user);
        }
    }
}