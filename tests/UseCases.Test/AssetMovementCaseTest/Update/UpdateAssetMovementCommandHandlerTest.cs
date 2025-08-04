using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetMovementRepository;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using CommonTestUtilities.Services;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Update;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
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
            roomFrom.Assets.Add(asset);  // asset está na sala de origem

            var roomTo = RoomLocationBuilder.Build();
            roomTo.Id = assetMovement.ToRoom.Id;
            roomTo.SchoolId = asset.SchoolId;
            // NÃO adiciona asset em roomTo.Assets, pois ele será movido para cá

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset)
                .Build();

            var assetMovementReadOnlyRepository = new AssetMovementReadOnlyRepositoryBuilder()
                .WithAssetMovementExist(assetMovement.Id, assetMovement)
                .Build();

            var roomLocationReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomFrom.Id, roomFrom)
                .WithRoomLocationExist(roomTo.Id, roomTo)
                .Build();

            var assetUpdateOnlyRepository = new AssetUpdateOnlyRepositoryBuilder().Build();
            var assetMovementUpdateOnlyRepository = new AssetMovementUpdateOnlyRepositoryBuilder().Build();
            var roomLocationUpdateOnlyRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(asset.SchoolId)
                .Build();

            var handler = CreateUseCase(
                unitOfWork,
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository,
                roomLocationUpdateOnlyRepository,
                assetMovementReadOnlyRepository,
                assetUpdateOnlyRepository,
                assetMovementUpdateOnlyRepository,
                currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);

            assetMovementUpdateOnlyRepository.Received(1).Update(assetMovement);
            assetUpdateOnlyRepository.Received(1).Update(asset);
            roomLocationUpdateOnlyRepository.Received(1).Update(roomFrom);
            roomLocationUpdateOnlyRepository.Received(1).Update(roomTo);
            await unitOfWork.Received(1).Commit();

            assetMovement.IsCanceled.ShouldBeTrue();
            assetMovement.CancelReason.ShouldBe(command.CancelReason);
            asset.RoomLocationId.ShouldBe(roomFrom.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetMovementDoesNotExist()
        {
            var command = new UpdateAssetMovementCommand(999, "Cancelamento");

            var assetMovementReadOnlyRepository = new AssetMovementReadOnlyRepositoryBuilder()
                .WithAssetMovementNotFound(command.Id)
                .Build();

            var handler = CreateUseCase(
                new UnitOfWorkBuilder().Build(),
                new AssetReadOnlyRepositoryBuilder().Build(),
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                new RoomLocationUpdateOnlyRepositoryBuilder().Build(),
                assetMovementReadOnlyRepository,
                new AssetUpdateOnlyRepositoryBuilder().Build(),
                new AssetMovementUpdateOnlyRepositoryBuilder().Build(),
                new CurrentUserServiceBuilder().IsAuthenticatedTrue().Build());

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSETMOVEMENT_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var assetMovementReadOnlyRepository = new AssetMovementReadOnlyRepositoryBuilder()
                .WithAssetMovementExist(assetMovement.Id, assetMovement)
                .Build();

            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetNotFound(assetMovement.Asset.Id)
                .Build();

            var handler = CreateUseCase(
                new UnitOfWorkBuilder().Build(),
                assetReadOnlyRepository,
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                new RoomLocationUpdateOnlyRepositoryBuilder().Build(),
                assetMovementReadOnlyRepository,
                new AssetUpdateOnlyRepositoryBuilder().Build(),
                new AssetMovementUpdateOnlyRepositoryBuilder().Build(),
                new CurrentUserServiceBuilder().IsAuthenticatedTrue().Build());

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenAssetDoesNotBelongToSchool()
        {
            var assetMovement = AssetMovementBuilder.Build();
            var command = new UpdateAssetMovementCommand(assetMovement.Id, "Cancelamento");

            var asset = AssetBuilder.Build();
            asset.Id = assetMovement.Asset.Id;
            asset.SchoolId = 999;

            var assetMovementReadOnlyRepository = new AssetMovementReadOnlyRepositoryBuilder()
                .WithAssetMovementExist(assetMovement.Id, assetMovement)
                .Build();

            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset)
                .Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(assetMovement.Asset.SchoolId) // diferente do asset
                .Build();

            var handler = CreateUseCase(
                new UnitOfWorkBuilder().Build(),
                assetReadOnlyRepository,
                new RoomLocationReadOnlyRepositoryBuilder().Build(),
                new RoomLocationUpdateOnlyRepositoryBuilder().Build(),
                assetMovementReadOnlyRepository,
                new AssetUpdateOnlyRepositoryBuilder().Build(),
                new AssetMovementUpdateOnlyRepositoryBuilder().Build(),
                currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);
        }

        // Você pode criar mais testes para roomFrom e roomTo pertencimento, etc., seguindo o padrão acima.

        private static UpdateAssetMovementCommandHandler CreateUseCase(
            IUnitOfWork unitOfWork,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository,
            IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository,
            IAssetUpdateOnlyRepository assetUpdateOnlyRepository,
            IAssetMovementUpdateOnlyRepository assetMovementUpdateOnlyRepository,
            ICurrentUserService currentUser)
        {
            return new UpdateAssetMovementCommandHandler(
                unitOfWork,
                assetReadOnlyRepository,
                roomLocationReadOnlyRepository,
                roomLocationUpdateOnlyRepository,
                assetMovementReadOnlyRepository,
                assetUpdateOnlyRepository,
                assetMovementUpdateOnlyRepository,
                currentUser);
        }
    }
}
