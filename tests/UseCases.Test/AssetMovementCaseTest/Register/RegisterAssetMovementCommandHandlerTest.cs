using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetMovementRepository;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.AssetMovementCaseTest.Register
{
    public class RegisterAssetMovementCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldRegisterAssetMovement_WhenValidAndUserIsAuthenticated()
        {
            var dto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(dto);

            var asset = AssetBuilder.Build();

            asset.Id = dto.AssetId;

            var roomFrom = RoomLocationBuilder.Build();

            roomFrom.Id = dto.FromRoomId;
            roomFrom.Assets.Add(asset);

            var roomTo = RoomLocationBuilder.Build();
            roomTo.Id = dto.ToRoomId;
            roomTo.Assets.Add(asset);


            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(dto.Id)
                .Build();

            var validator = new ValidatorBuilder<AssetMovementDto>()
                .WithValidResult()
                .Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(dto.AssetId, asset)
                .Build();

            var assetWriteOnlyRepository = new AssetUpdateOnlyRepositoryBuilder().Build();

            var roomReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(dto.FromRoomId, roomFrom)
                .WithRoomLocationExist(dto.ToRoomId, roomTo)
                .Build();

            var roomWriteOnlyRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();
            var assetMovementWriteOnlyRepository = new AssetMovementWriteOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                assetMovementWriteOnlyRepository,
                unitOfWork,
                validator,
                assetReadOnlyRepository,
                assetWriteOnlyRepository,
                roomWriteOnlyRepository,
                roomReadOnlyRepository,
                currentUser
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.AssetId.ShouldBe(dto.AssetId);

            await assetMovementWriteOnlyRepository.Received(1).Insert(Arg.Any<AssetMovement>());
             assetWriteOnlyRepository.Received(1).Update(asset);
             roomWriteOnlyRepository.Received(1).Update(roomFrom);
             roomWriteOnlyRepository.Received(1).Update(roomTo);
            await unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenUserIsNotAuthenticated()
        {
            var dto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(dto);

            var currentUser = new CurrentUserServiceBuilder().IsAuthenticatedFalse().Build();
            var validator = new ValidatorBuilder<AssetMovementDto>().WithValidResult().Build();

            var handler = CreateUseCase(
                new AssetMovementWriteOnlyRepositoryBuilder().Build(),
                new UnitOfWorkBuilder().Build(),
                validator,
                new AssetReadOnlyRepositoryBuilder().Build(),
                new AssetUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                currentUser
            );

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenFromRoomEqualsToRoom()
        {
            var dto = AssetMovementDtoBuilder.Build();
            dto.FromRoomId = dto.ToRoomId; 

            var command = new RegisterAssetMovementCommand(dto);

            var currentUser = new CurrentUserServiceBuilder().IsAuthenticatedTrue().Build();
            var validator = new ValidatorBuilder<AssetMovementDto>().WithValidResult().Build();

            var handler = CreateUseCase(
                new AssetMovementWriteOnlyRepositoryBuilder().Build(),
                new UnitOfWorkBuilder().Build(),
                validator,
                new AssetReadOnlyRepositoryBuilder().Build(),
                new AssetUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                currentUser
            );

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSETMOVEMENT_SAME_ROOM);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenAssetNotFound()
        {
            var dto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(dto);

            var currentUser = new CurrentUserServiceBuilder().IsAuthenticatedTrue().Build();
            var validator = new ValidatorBuilder<AssetMovementDto>().WithValidResult().Build();

            var assetRepository = new AssetReadOnlyRepositoryBuilder().Build();
            assetRepository.GetById(Arg.Any<long>()).Returns((Asset)null); 

            var handler = CreateUseCase(
                new AssetMovementWriteOnlyRepositoryBuilder().Build(),
                new UnitOfWorkBuilder().Build(),
                validator,
                assetRepository,
                new AssetUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                currentUser
            );

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var dto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(dto);

            var currentUser = new CurrentUserServiceBuilder().IsAuthenticatedTrue().Build();
            var validator = new ValidatorBuilder<AssetMovementDto>().WithInvalidResult("Campo obrigatório").Build();

            var handler = CreateUseCase(
                new AssetMovementWriteOnlyRepositoryBuilder().Build(),
                new UnitOfWorkBuilder().Build(),
                validator,
                new AssetReadOnlyRepositoryBuilder().Build(),
                new AssetUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationUpdateOnlyRepositoryBuilder().Build(),
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                currentUser
            );

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldContain("Campo obrigatório");
        }

        private static RegisterAssetMovementCommandHandler CreateUseCase(
            IAssetMovementWriteOnlyRepository assetMovementWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IValidator<AssetMovementDto> validator,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IAssetUpdateOnlyRepository assetWriteOnlyRepository,
            IRoomLocationUpdateOnlyRepository roomLocationWriteOnlyRepository,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            ICurrentUserService currentUser)
        {
            return new RegisterAssetMovementCommandHandler(
                assetMovementWriteOnlyRepository,
                unitOfWork,
                validator,
                assetReadOnlyRepository,
                assetWriteOnlyRepository,
                roomLocationWriteOnlyRepository,
                roomLocationReadOnlyRepository,
                currentUser
            );
        }
    }
}
