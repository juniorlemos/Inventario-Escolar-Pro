using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
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

namespace UseCases.Test.RoomLocationCaseTest.Update
{
    public class UpdateRoomLocationCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var updateRoomLocationDto = UpdateRoomLocationDtoBuilder.Build();

            var command = new UpdateRoomLocationCommand(roomLocation.Id, updateRoomLocationDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UpdateRoomLocationDto>().WithValidResult().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedFalse()
                .Build();

            var roomLocationReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomLocation.Id, roomLocation)
                .Build();

            var roomLocationUpdateRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateHandler(unitOfWork, validator, roomLocationReadOnlyRepository, roomLocationUpdateRepository, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);
        }

        [Fact]
        public async Task Handle_ShouldUpdateRoomLocation_WhenNameIsSameAsBefore()
        {
            var roomLocation = RoomLocationBuilder.Build();
            var updateRoomLocationDto = UpdateRoomLocationDtoBuilder.Build();
            updateRoomLocationDto.Name = roomLocation.Name;

            var command = new UpdateRoomLocationCommand(roomLocation.Id, updateRoomLocationDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UpdateRoomLocationDto>().WithValidResult().Build();
            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(roomLocation.SchoolId)
                .Build();

            var roomLocationReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationExist(roomLocation.Id, roomLocation)
                .Build();

            var roomLocationUpdateRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateHandler(unitOfWork, validator, roomLocationReadOnlyRepository, roomLocationUpdateRepository, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            roomLocationUpdateRepository.Received(1).Update(roomLocation);
            await unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenRoomLocationDtoIsNull()
        {
            var command = new UpdateRoomLocationCommand(1, null!);

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UpdateRoomLocationDto>()
                .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
                .Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(1)
                .Build();

            var roomLocationReadOnlyRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNotFound(1)
                .Build();

            var roomLocationUpdateRepository = new RoomLocationUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateHandler(unitOfWork, validator, roomLocationReadOnlyRepository, roomLocationUpdateRepository, currentUser);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        private static UpdateRoomLocationCommandHandler CreateHandler(
            IUnitOfWork unitOfWork,
            IValidator<UpdateRoomLocationDto> validator,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository,
            ICurrentUserService currentUserService)
        {
            return new UpdateRoomLocationCommandHandler(
                unitOfWork,
                validator,
                roomLocationReadOnlyRepository,
                roomLocationUpdateOnlyRepository,
                currentUserService);
        }
    }
}