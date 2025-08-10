using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetMovementRepository;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Update;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;

namespace UseCases.Test.AssetMovementCaseTest.Update
{
    public class UpdateAssetMovementCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldUpdateAssetMovement_WhenAllValidAndUserIsAuthenticated()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Motivo do cancelamento");

            var asset = AssetBuilder.Build();
            asset.Id = assetMovement.Asset.Id;
            asset.SchoolId = assetMovement.Asset.SchoolId;
            asset.RoomLocationId = assetMovement.FromRoom.Id;

            var roomFrom = RoomLocationBuilder.Build();
            roomFrom.Id = assetMovement.FromRoom.Id;
            roomFrom.SchoolId = asset.SchoolId;
            roomFrom.Assets.Add(asset);

            var roomTo = RoomLocationBuilder.Build();
            roomTo.Id = assetMovement.ToRoom.Id;
            roomTo.SchoolId = asset.SchoolId;

            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(true, assetMovement);
            var roomLocationReadOnlyRepository = CreateRoomLocationReadOnlyRepository(roomFrom, roomTo);
            var currentUserService = CreateCurrentUserService(true, asset.SchoolId);

            var handler = CreateUseCase(
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository,
                assetMovementReadOnlyRepository,
                currentUserService);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            assetMovement.IsCanceled.ShouldBeTrue();
            assetMovement.CancelReason.ShouldBe(command.CancelReason);
            asset.RoomLocationId.ShouldBe(roomFrom.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetMovementDoesNotExist()
        {
            var command = new UpdateAssetMovementCommand(999, "Cancelamento");

            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(false);
            var currentUserService = CreateCurrentUserService(true);
            var roomLocationReadRepository = new RoomLocationReadOnlyRepositoryBuilder().Build();
            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                assetReadOnlyRepository,
                roomLocationReadRepository,
                assetMovementReadOnlyRepository,
                currentUserService);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSETMOVEMENT_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(true, assetMovement);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(false, assetMovement.Asset);
            var currentUserService = CreateCurrentUserService(true);
            var roomLocationReadRepository = new RoomLocationReadOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                assetReadOnlyRepository,
                roomLocationReadRepository,
                assetMovementReadOnlyRepository,
                currentUserService);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenAssetDoesNotBelongToSchool()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var asset = AssetBuilder.Build();
            assetMovement.Asset = asset;

            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(true, assetMovement);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var currentUserService = CreateCurrentUserService(true, asset.Id +1); 

            var handler = CreateUseCase(
                assetReadOnlyRepository,
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                assetMovementReadOnlyRepository,
                currentUserService);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenRoomFromDoesNotBelongToSchool()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var roomFrom = RoomLocationBuilder.Build();
            var asset = AssetBuilder.Build();
            
            assetMovement.Asset = asset;
            assetMovement.AssetId = asset.Id;
            assetMovement.SchoolId = asset.SchoolId;

            assetMovement.FromRoom = roomFrom;

            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(true, assetMovement);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var currentUserService = CreateCurrentUserService(true, asset.SchoolId);

            var roomLocationReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomFrom.Id, roomFrom).Build();
              
            var handler = CreateUseCase(
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository, 
                assetMovementReadOnlyRepository,
                currentUserService);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenRoomToDoesNotBelongToSchool()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var roomFrom = RoomLocationBuilder.Build();
            var roomTo = RoomLocationBuilder.Build();
            var asset = AssetBuilder.Build();

            assetMovement.Asset = asset;
            assetMovement.AssetId = asset.Id;
            assetMovement.SchoolId = asset.SchoolId;

            assetMovement.FromRoom = roomFrom;
            assetMovement.FromRoomId = roomFrom.Id;

            roomFrom.SchoolId = assetMovement.SchoolId;

            assetMovement.ToRoom = roomTo;

            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(true, assetMovement);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var currentUserService = CreateCurrentUserService(true, asset.SchoolId);

            var roomLocationReadOnlyRepository = CreateRoomLocationReadOnlyRepository(roomFrom,roomTo);

            var handler = CreateUseCase(
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository,
                assetMovementReadOnlyRepository,
                currentUserService);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenFromRoomDoesNotExist()
        {
           
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var asset = AssetBuilder.Build();

            assetMovement.Asset = asset;
            assetMovement.AssetId = asset.Id;
            assetMovement.SchoolId = asset.SchoolId;

            assetMovement.FromRoom = RoomLocationBuilder.Build(); 

            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(true, assetMovement);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var currentUserService = CreateCurrentUserService(true, asset.SchoolId);

            var roomLocationReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository,
                assetMovementReadOnlyRepository,
                currentUserService);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenToRoomDoesNotExist()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var roomFrom = RoomLocationBuilder.Build();
            var asset = AssetBuilder.Build();

            assetMovement.Asset = asset;
            assetMovement.AssetId = asset.Id;
            assetMovement.SchoolId = asset.SchoolId;

            roomFrom.SchoolId = assetMovement.SchoolId;

            assetMovement.FromRoom = roomFrom;
            assetMovement.ToRoom = RoomLocationBuilder.Build(); 

            var assetMovementReadOnlyRepository = CreateAssetMovementReadOnlyRepository(true, assetMovement);
            var assetReadOnlyRepository = CreateAssetReadOnlyRepository(true, asset);
            var currentUserService = CreateCurrentUserService(true, asset.SchoolId);

            var roomLocationReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomFrom.Id, roomFrom)  
                .Build();                                     

            var handler = CreateUseCase(
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository,
                assetMovementReadOnlyRepository,
                currentUserService);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);
        }

        private static IAssetReadOnlyRepository CreateAssetReadOnlyRepository(bool exists, Asset asset)
        {
            var builder = new AssetReadOnlyRepositoryBuilder();
            return exists
                ? builder.WithAssetExist(asset.Id, asset).Build()
                : builder.WithAssetNotFound(asset.Id).Build();
        }

        private static IAssetMovementReadOnlyRepository CreateAssetMovementReadOnlyRepository(bool exists, AssetMovement? movement = null)
        {
            var builder = new AssetMovementReadOnlyRepositoryBuilder();
            return exists && movement != null
                ? builder.WithAssetMovementExist(movement.Id, movement).Build()
                : builder.WithAssetMovementNotFound(movement?.Id ?? 0).Build();
        }

        private static IRoomLocationReadOnlyRepository CreateRoomLocationReadOnlyRepository(RoomLocation roomFrom, RoomLocation roomTo)
        {
            return new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomFrom.Id, roomFrom)
                .WithRoomLocationExist(roomTo.Id, roomTo)
                .Build();
        }

        private static UpdateAssetMovementCommandHandler CreateUseCase(
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository,
            ICurrentUserService currentUser)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            var assetUpdateRepository = new AssetUpdateOnlyRepositoryBuilder().Build();
            var roomLocationUpdateRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();
            var assetMovementUpdateRepository = new AssetMovementUpdateOnlyRepositoryBuilder().Build();

            return new UpdateAssetMovementCommandHandler(
                unitOfWork,
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository,
                roomLocationUpdateRepository,
                assetMovementReadOnlyRepository,
                assetUpdateRepository,
                assetMovementUpdateRepository,
                currentUser);
        }
    }
}