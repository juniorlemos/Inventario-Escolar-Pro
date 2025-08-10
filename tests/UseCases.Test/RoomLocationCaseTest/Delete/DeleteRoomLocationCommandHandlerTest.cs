using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using CommonTestUtilities.Services;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;

namespace UseCases.Test.RoomLocationCaseTest.Delete
{
    public class DeleteRoomLocationCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnUnit_WhenRoomLocationDeletedSuccessfully()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var readRepository = CreateRoomLocationReadOnlyRepository(true, roomLocation);
            var deleteRepository = CreateRoomLocationDeleteOnlyRepository(roomLocation.Id);
            var currentUser = CreateCurrentUserService(roomLocation.SchoolId);

            var handler = CreateHandler(readRepository, deleteRepository, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenRoomLocationDoesNotExist()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var readRepository = CreateRoomLocationReadOnlyRepository(false, roomLocation);
            var deleteRepository = CreateRoomLocationDeleteOnlyRepository(roomLocation.Id);
            var currentUser = CreateCurrentUserService(roomLocation.SchoolId);

            var handler = CreateHandler(readRepository, deleteRepository, currentUser);

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenRoomLocationHasAssets()
        {
            var roomLocation = RoomLocationBuilder.Build();
            roomLocation.Assets = AssetBuilder.BuildList(3);

            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var readRepository = CreateRoomLocationReadOnlyRepository(true, roomLocation);
            var deleteRepository = CreateRoomLocationDeleteOnlyRepository(roomLocation.Id);
            var currentUser = CreateCurrentUserService(roomLocation.SchoolId);

            var handler = CreateHandler(readRepository, deleteRepository, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_HAS_ASSETS);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenRoomLocationNotBelongsToCurrentUserSchool()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var schoolIdDifferent = roomLocation.SchoolId + 1;

            var command = new DeleteRoomLocationCommand(roomLocation.Id);

            var readRepository = CreateRoomLocationReadOnlyRepository(true, roomLocation);
            var deleteRepository = CreateRoomLocationDeleteOnlyRepository(roomLocation.Id);
            var currentUser = CreateCurrentUserService(schoolIdDifferent);

            var handler = CreateHandler(readRepository, deleteRepository, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);
        }

        private static IRoomLocationReadOnlyRepository CreateRoomLocationReadOnlyRepository(
            bool exists,
            RoomLocation roomLocation)
        {
            var builder = new RoomLocationReadOnlyRepositoryBuilder();

            return exists
                ? builder.WithRoomLocationExist(roomLocation.Id, roomLocation).Build()
                : builder.WithRoomLocationNotExist(roomLocation.Id).Build();
        }

        private static IRoomLocationDeleteOnlyRepository CreateRoomLocationDeleteOnlyRepository(long roomLocationId)
        {
            return new RoomLocationDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(roomLocationId)
                .Build();
        }

        private static ICurrentUserService CreateCurrentUserService(long schoolId)
        {
            return new CurrentUserServiceBuilder()
                .WithSchoolId(schoolId)
                .Build();
        }

        private static DeleteRoomLocationCommandHandler CreateHandler(
            IRoomLocationReadOnlyRepository readRepository,
            IRoomLocationDeleteOnlyRepository deleteRepository,
            ICurrentUserService currentUser)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            return new DeleteRoomLocationCommandHandler(deleteRepository, readRepository, unitOfWork, currentUser);
        }
    }
}