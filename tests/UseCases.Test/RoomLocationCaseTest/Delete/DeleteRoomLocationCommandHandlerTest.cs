using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using CommonTestUtilities.Services;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.RoomLocationCaseTest.Delete
{
    public class DeleteRoomLocationCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnUnit_WhenRoomLocationDeletedSuccessfully()
        {
            var roomLocation = RoomLocationBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(roomLocation.SchoolId)
                .Build();

            var readRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomLocation.Id, roomLocation)
                .Build();

            var deleteRepository = new RoomLocationDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(roomLocation.Id)
                .Build();

            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var handler = new DeleteRoomLocationCommandHandler(deleteRepository, readRepository, unitOfWork, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomLocationDoesNotExist()
        {
            var roomLocation = RoomLocationBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(roomLocation.SchoolId)
                .Build();

            var readRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNotFound(roomLocation.Id)
                .Build();

            var deleteRepository = new RoomLocationDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(roomLocation.Id)
                .Build();

            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var handler = new DeleteRoomLocationCommandHandler(deleteRepository, readRepository, unitOfWork, currentUser);

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenRoomLocationHasAssets()
        {
            var assets = AssetBuilder.BuildList(3);
            var roomLocations = RoomLocationBuilder.BuildList(10);
            roomLocations.ForEach(r => r.Assets = assets);

            var roomLocation = roomLocations.First();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(roomLocation.SchoolId)
                .Build();

            var readRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomLocation.Id, roomLocation)
                .Build();

            var deleteRepository = new RoomLocationDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(roomLocation.Id)
                .Build();

            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var handler = new DeleteRoomLocationCommandHandler(deleteRepository, readRepository, unitOfWork, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_HAS_ASSETS);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenRoomLocationNotBelongsToCurrentUserSchool()
        {
            var roomLocation = RoomLocationBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(roomLocation.SchoolId + 1) // diferente
                .Build();

            var readRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomLocation.Id, roomLocation)
                .Build();

            var deleteRepository = new RoomLocationDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(roomLocation.Id)
                .Build();

            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var handler = new DeleteRoomLocationCommandHandler(deleteRepository, readRepository, unitOfWork, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);
        }
    }
}
