using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
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

namespace UseCases.Test.AssetCaseTest.Update
{
    public class UpdateAssetCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldUpdateAsset_WhenAllFieldsAreValidAndUserIsAuthenticated()
        {
            var updateAssetDto = UpdateAssetDtoBuilder.Build();

            var asset = AssetBuilder.Build();

            var command = new UpdateAssetCommand(asset.Id, updateAssetDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(asset.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<UpdateAssetDto>()
                .WithValidResult()
                .Build();

            var assetUpdateRepository = new AssetUpdateOnlyRepositoryBuilder().Build();

            var assetReadRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset)
                .Build();

            var useCase = CreateUseCase(unitOfWork, validator, assetReadRepository, assetUpdateRepository, currentUser);

            var result = await useCase.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            assetUpdateRepository.Received(1).Update(asset);
            await unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
            var updateAssetDto = UpdateAssetDtoBuilder.Build();

            var asset = AssetBuilder.Build();
            var command = new UpdateAssetCommand(asset.Id, updateAssetDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(asset.SchoolId) 
                .Build();

            var validator = new ValidatorBuilder<UpdateAssetDto>()
                .WithValidResult()
                .Build();

            var assetUpdateRepository = new AssetUpdateOnlyRepositoryBuilder().Build();

            var assetReadRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetNotFound(asset.Id)
                .Build();

            var useCase = CreateUseCase(unitOfWork, validator, assetReadRepository, assetUpdateRepository, currentUser);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
        {
            var asset = AssetBuilder.Build();
            var updateAssetDto = UpdateAssetDtoBuilder.Build();

            var command = new UpdateAssetCommand(asset.Id, updateAssetDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedFalse()
                .Build();

            var validator = new ValidatorBuilder<UpdateAssetDto>()
                .WithValidResult()
                .Build();

            var assetUpdateRepository = new AssetUpdateOnlyRepositoryBuilder().Build();

            var assetReadRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset)
                .Build();

            var useCase = CreateUseCase(unitOfWork, validator, assetReadRepository, assetUpdateRepository, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenUpdateAssetDtoIsInvalid()
        {
            var asset = AssetBuilder.Build();
            var updateAssetDto = UpdateAssetDtoBuilder.Build();

            var command = new UpdateAssetCommand(asset.Id, updateAssetDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(asset.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<UpdateAssetDto>()
                .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
                .Build();

            var assetUpdateRepository = new AssetUpdateOnlyRepositoryBuilder().Build();

            var assetReadRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset)
                .Build();

            var useCase = CreateUseCase(unitOfWork, validator, assetReadRepository, assetUpdateRepository, currentUser);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
              () => useCase.Handle(command, CancellationToken.None));
            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        private static UpdateAssetCommandHandler CreateUseCase(
            IUnitOfWork unitOfWork,
            IValidator<UpdateAssetDto> validator,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IAssetUpdateOnlyRepository assetUpdateOnlyRepository,
            ICurrentUserService currentUser)
        {
            return new UpdateAssetCommandHandler(
                unitOfWork,
                validator,
                assetReadOnlyRepository,
                assetUpdateOnlyRepository,
                currentUser);
        }
    }   
    }
