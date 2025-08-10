using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetMovementRepository;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.AssetMovementCaseTest.Register
{
    public class RegisterAssetMovementCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldRegisterAssetMovement_WhenValidAndUserIsAuthenticated()
        {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(assetMovementDto);

            var asset = AssetBuilder.Build();
            asset.Id = assetMovementDto.AssetId;

            var roomFrom = RoomLocationBuilder.Build();
            roomFrom.Id = assetMovementDto.FromRoomId;
            roomFrom.Assets.Add(asset);

            var roomTo = RoomLocationBuilder.Build();
            roomTo.Id = assetMovementDto.ToRoomId;

            var currentUser = CreateCurrentUserService(true, assetMovementDto.Id);
            var validator = CreateValidator<AssetMovementDto>(isValid: true);

            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var roomReadOnlyRepository = CreateRoomLocationReadOnlyRepository(roomFrom, roomTo);

            var handler = CreateUseCase(
                validator,
                assetReadOnlyRepository,
                roomReadOnlyRepository,
                currentUser
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.AssetId.ShouldBe(assetMovementDto.AssetId);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenUserIsNotAuthenticated()
        {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(assetMovementDto);

            var currentUser = CreateCurrentUserService(false);
            var validator = CreateValidator<AssetMovementDto>(isValid: true);

            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true);
            var roomReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                validator,
                assetReadOnlyRepository,
                roomReadOnlyRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenFromRoomEqualsToRoom()
        {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            assetMovementDto.ToRoomId = assetMovementDto.FromRoomId;

            var command = new RegisterAssetMovementCommand(assetMovementDto);
            var currentUser = CreateCurrentUserService(true, assetMovementDto.Id);
            var validator = CreateValidator<AssetMovementDto>(isValid: true);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true);
            var roomReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                validator,
                assetReadOnlyRepository,
                roomReadOnlyRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSETMOVEMENT_SAME_ROOM);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenAssetNotFound()
        {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(assetMovementDto);

            var currentUser = CreateCurrentUserService(true, assetMovementDto.Id);
            var validator = CreateValidator<AssetMovementDto>(isValid: true);

            var assetRepository = CreateAssetReadOnlyRepository(false);
            var roomRepository = new RoomLocationReadOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                validator,
                assetRepository,
                roomRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }
        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenToRoomDoesNotExist()
       {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            var asset = AssetBuilder.Build();

            var roomLocationFrom = RoomLocationBuilder.Build();
            roomLocationFrom.Id = assetMovementDto.FromRoomId;

            var toRoomId = roomLocationFrom.Id + 1000; 

            assetMovementDto.ToRoomId = toRoomId;
            assetMovementDto.AssetId = asset.Id;

            var command = new RegisterAssetMovementCommand(assetMovementDto);
            var currentUser = CreateCurrentUserService(true, assetMovementDto.Id);
            var validator = CreateValidator<AssetMovementDto>(isValid: true);

            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);

            var roomReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomLocationFrom.Id, roomLocationFrom)
                .WithRoomLocationNotExist(toRoomId) 
                .Build();

            var handler = CreateUseCase(
                validator,
                assetReadOnlyRepository,
                roomReadOnlyRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenFromRoomDoesNotExist()
            {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            var roomLocationFromRoom = RoomLocationBuilder.Build();
            var asset = AssetBuilder.Build();

            assetMovementDto.AssetId = asset.Id;

            var command = new RegisterAssetMovementCommand(assetMovementDto);
            var currentUser = CreateCurrentUserService(true, assetMovementDto.Id);
            var validator = CreateValidator<AssetMovementDto>(isValid: true);

            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset).Build();

            var roomReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNotExist(assetMovementDto.ToRoomId).Build();

            var handler = CreateUseCase(
                validator,
                assetReadOnlyRepository,
                roomReadOnlyRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            var command = new RegisterAssetMovementCommand(assetMovementDto);

            var currentUser = CreateCurrentUserService(true, assetMovementDto.Id);
            var validator = CreateValidator<AssetMovementDto>(isValid: false, "Campo obrigatório");
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true);
            var roomReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                validator,
                assetReadOnlyRepository,
                roomReadOnlyRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldContain("Campo obrigatório");
        }

        private static IAssetReadOnlyRepository CreateAssetReadOnlyRepository(bool exists, Asset? asset = null)
        {
            var builder = new AssetReadOnlyRepositoryBuilder();
            return exists && asset != null
                ? builder.WithAssetExist(asset.Id, asset).Build()
                : builder.WithAssetNotFound(asset?.Id ?? 0).Build();
        }

        private static IRoomLocationReadOnlyRepository CreateRoomLocationReadOnlyRepository(RoomLocation from, RoomLocation to)
        {
            return new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(from.Id, from)
                .WithRoomLocationExist(to.Id, to)
                .Build();
        }

        private static RegisterAssetMovementCommandHandler CreateUseCase(
            IValidator<AssetMovementDto> validator,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            ICurrentUserService currentUser)
        {

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var assetUpdateRepository = new AssetUpdateOnlyRepositoryBuilder().Build();
            var roomLocationUpdateRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();
            var assetMovementWriteOnlyRepository = new AssetMovementWriteOnlyRepositoryBuilder().Build();

            return new RegisterAssetMovementCommandHandler(
                assetMovementWriteOnlyRepository,
                unitOfWork,
                validator,
                assetReadOnlyRepository,
                assetUpdateRepository,
                roomLocationUpdateRepository,
                roomLocationReadOnlyRepository,
                currentUser
            );
        }
    }
}